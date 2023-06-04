using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private Transform boardParent;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material[] stepMaterials;

    private int width;
    private int height;

    private int level;

    private GameBoard gameBoard = null;

    private MoveMgr moveMgr = null;

    private int colorSwitch = 0;
    private int moveStep = 0;

    private GameTileCntrl lastTile;
    private GameTileCntrl startingTile;
    private GameTileCntrl prevTile;

    private TileColRow currentTile;

    private Stack<TrackMove> playersMoves = null;

    void Start()
    {
        width = gameData.width;
        height = gameData.height;
        level = gameData.level;
        moveMgr = gameData.moveMgr;

        gameBoard = new GameBoard(gameData, boardParent, width, height);

        StartNewGame(false);
    }

    public void StartNewGame(bool deleteGameBoard)
    {
        if (deleteGameBoard)
        {
            gameBoard.DestoryGameBoard();
        }

        moveStep = 0;

        playersMoves = new Stack<TrackMove>();

        gameBoard.CreateGameBoard();

        CreatePuzzle();

        GameManager.Instance.SetLevel(level);
    }

    public bool OnPlayersMove(string move) 
    {
        Stack<GameTileCntrl> moveStack = new Stack<GameTileCntrl>();
        TileColRow startMove = new TileColRow(currentTile);
        bool goodMove = true;
        string errorMsg = "";

        colorSwitch = 1 - colorSwitch;

        for (int i = 0; (i < move.Length) && goodMove; i++)
        {             
            Step step = moveMgr.GetStep(move.Substring(i, 1));

            currentTile.Add(step);

            GameTileCntrl activeTile = gameBoard.GetTileCntrl(currentTile);

            if (activeTile != null)
            {
                if (!activeTile.HasBeenPlayed())
                {
                    activeTile.SetToPlayed(stepMaterials[colorSwitch]);
                    moveStack.Push(activeTile);
                }
                else
                { 
                    errorMsg = "Tile is already occupied.";
                    goodMove = false;
                }
            }
            else
            {
                errorMsg = "That move takes you off the board.";
                goodMove = false;
            }
        }

        if (goodMove) {
            MakeGoodMove(move, moveStack, startMove);
        } else {
            GameManager.Instance.DisplayMsg("Sorry", errorMsg, "Ok");
            currentTile = startMove;

            foreach (GameTileCntrl tile in moveStack) 
            {
                tile.SetToOpen();
            }
        }

        return (goodMove);                
    }

    public void MakeGoodMove(string move, Stack<GameTileCntrl> moveStack, TileColRow startMove)
    {
        int dirBtnCnt = GameManager.Instance.GetDirBtnCnt();

        prevTile = gameBoard.GetTileCntrl(currentTile);

        if ((dirBtnCnt == 0) && (prevTile.IsLastTile()))
        {
            GameManager.Instance.WonGame();
            GameManager.Instance.DisplayMsg("Congradulations", "You Won!", "Ok");
        } else {
            prevTile.CreateStepTile(++moveStep);

            playersMoves.Push(new TrackMove(move, moveStack, startMove));
        }
    }

    public void CheckWinner()
    {
        int dirBtnCnt = GameManager.Instance.GetDirBtnCnt();

        if ((dirBtnCnt == 0) && (prevTile.IsLastTile()))
        {
            GameManager.Instance.DisplayMsg("Congradulations", "You Won!", "Ok");
            GameManager.Instance.WonGame();
        }
    }

    public void UnDoLastMove()
    {
        GameTileCntrl firstTile = null;

        if (playersMoves.Count > 0)
        {
            TrackMove trackMove = playersMoves.Pop();

            GameManager.Instance.Undo(trackMove.Move);

            Stack<GameTileCntrl> move = trackMove.MoveStack;

            foreach(GameTileCntrl tile in move)
            {
                if (firstTile == null)
                {
                    firstTile = tile;
                }

                tile.SetToOpen();
            }

            firstTile.RemoveStepTile();

            moveStep--;

            currentTile = trackMove.startMove;
        }
    }

    private void CreatePuzzle()
    {
        int numberOfMoves = 0;
        int tries = 0;
        
        startingTile = GetStartingPoint();
        
        prevTile = startingTile;
        lastTile = null;

        currentTile = startingTile.GetColRow();

        TileColRow point = startingTile.GetColRow();

        GameTileCntrl firstMoveTile = startingTile;         
        firstMoveTile.SetAsGamePath();

        while((numberOfMoves < level) && (tries++ < 20))
        {
            Queue<GameTileCntrl> stepQueue = new Queue<GameTileCntrl>();
            TileColRow moveStartPoint = new TileColRow(point);
            string move = moveMgr.PickMove();
            bool moveIsValid = true;

            Debug.Log($"Move: {move}");

            for (int i = 0; (i < move.Length) && moveIsValid; i++)
            {
                point.Add(moveMgr.GetStep(move.Substring(i, 1)));

                GameTileCntrl tileCntrl = gameBoard.GetTileCntrl(point); 

                if ((tileCntrl != null) && (!tileCntrl.IsGamePath()))
                {
                    stepQueue.Enqueue(tileCntrl);
                    lastTile = tileCntrl;
                } else {              
                    moveIsValid = false;
                    Debug.Log("Invalid: ");
                }
            }

            if (moveIsValid) 
            {
                numberOfMoves++;
                GameManager.Instance.AddDirBtnCnt(move);

                while (stepQueue.Count != 0)
                {
                    GameTileCntrl tileCntrl = stepQueue.Dequeue();
                    tileCntrl.SetAsGamePath();
                }
            } else {
                point = moveStartPoint;
            }
        }

        GameManager.Instance.CreateDirBtns();

        lastTile.SetLastTile(level);
    }

    private GameTileCntrl GetStartingPoint()
    {
        TileColRow colRow = new TileColRow(width / 2, height / 2);

        GameTileCntrl tileCntrl = gameBoard.GetTileCntrl(colRow);
        tileCntrl.SetStartTile(moveStep);

        return (tileCntrl);
    }

    private class TrackMove
    {
        public Stack<GameTileCntrl> MoveStack { set; get; }
        public string Move { set; get; }
        public TileColRow startMove { set; get; }

        public TrackMove(string move, Stack<GameTileCntrl> moveStack, TileColRow startMove)
        {
            this.Move = move;
            this.MoveStack = moveStack;
            this.startMove = startMove;
        }
    }

    private class GameBoard
    {
        private GameData gameData;

        private int width;
        private int height;

        private GameObject gameTilePreFab;

        private float tileSpacing;

        private Transform parent;

        private GameTileCntrl[,] gameBoard = null;

        public GameBoard(GameData gameData, Transform parent, int width, int height)
        {
            this.gameData = gameData;
            this.width = gameData.width;
            this.height = gameData.height;
            this.tileSpacing = gameData.tileSpacing;
            this.gameTilePreFab = gameData.gameTilePreFab;
            this.parent = parent;
        }

        public void CreateGameBoard()
        {
            gameBoard = new GameTileCntrl[width, height];

            for (int col = 0; col < width; col++) 
            {
                for (int row = 0; row < height; row++)
                {
                    gameBoard[col, row] = CreateGameTile(col, row);
                }
            }
        }

        public void DestoryGameBoard()
        {
            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    GameTileCntrl cntrl = gameBoard[col, row];

                    if (cntrl != null)
                    { 
                        cntrl.RemoveStepTile();
                        Destroy(cntrl.gameObject);
                    }
                }
            }
        }

        public GameTileCntrl GetTileCntrl(TileColRow colRow)
        {
            int col = colRow.Col;
            int row = colRow.Row;
            GameTileCntrl cntrl = null;

            if ((col >= 0) && (col < width) && (row >= 0) && (row < height))
            {
                cntrl = gameBoard[col, row];
            }

            return(cntrl);
        }

        private GameTileCntrl CreateGameTile(int col, int row)
        {
            Vector3 position = new Vector3(col * tileSpacing, 0.0f, row * tileSpacing);

            GameObject go = Instantiate(gameTilePreFab, position, Quaternion.identity);
            go.transform.parent = parent;

            GameTileCntrl tileCntrl = go.transform.GetComponent<GameTileCntrl>();
            tileCntrl.SetColRow(col, row);

            return(tileCntrl);
        }
    } 
}

public class TileColRow
{
    public int Col { get; set; }
    public int Row { get; set; }

    public TileColRow(int col, int row)
    {
        this.Col = col;
        this.Row = row;
    }

    public Vector3 GetPosition() 
    {
        return(new Vector3(Col * 5.0f, 0.0f, Row * 5.0f));
    }

    public TileColRow(TileColRow colRow)
    {
        Col = colRow.Col;
        Row = colRow.Row;
    }

    public void Add(Step step)
    {
        Col += step.Col;
        Row += step.Row;
    }

    public void PrintIt(string msg)
    {
        Debug.Log($"{msg} - Tile ColRow: ({Col},{Row})");
    }
}
