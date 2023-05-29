using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private Transform boardParent;
    [SerializeField] private Material markedMaterial;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material[] stepMaterials;
    //[SerializeField] private Material startingPointMaterial;
    //[SerializeField] private Material endPointMaterial;

    private int width;
    private int height;

    private int level;

    private bool mapSw = false;

    private GameBoard gameBoard = null;

    private MoveMgr moveMgr = null;

    private int colorSwitch = 0;
    private int moveStep = 0;

    private GameTileCntrl lastTile;
    private GameTileCntrl startingTile;
    private TileColRow currentTile;
    private GameTileCntrl prevTile;

    private int GetRandom(int n) => Random.Range(0, n);

    private Stack<TrackMove> playersMoves = null;

    // Start is called before the first frame update
    void Start()
    {
        width = gameData.width;
        height = gameData.height;
        level = gameData.level;

        gameBoard = new GameBoard(gameData, boardParent, width, height);

        playersMoves = new Stack<TrackMove>();

        moveMgr = gameData.moveMgr;

        CreatePuzzle();

        GameManager.Instance.SetLevel(level);
    }

    public bool PlayersMove(string move) 
    {
        Stack<GameTileCntrl> moveStack = new Stack<GameTileCntrl>();
        TileColRow startMove = new TileColRow(currentTile);
        string errorMsg = "";
        bool goodMove = true;

        colorSwitch = 1 - colorSwitch;

        for (int i = 0; (i < move.Length) && goodMove; i++)
        {             
            Step step = moveMgr.GetStep(move.Substring(i, 1));

            currentTile.Add(step);

            GameTileCntrl activeTile = gameBoard.GetTileCntrl(currentTile);

            if (activeTile != null)
            {
                if (activeTile.OpenForTracking())
                {
                    activeTile.SetMaterial(stepMaterials[colorSwitch]);
                    activeTile.SetAsTracked();
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
            prevTile = gameBoard.GetTileCntrl(currentTile);
            prevTile.CreateStepTile(++moveStep);

            playersMoves.Push(new TrackMove(move, moveStack, startMove));
        } else {
            GameManager.Instance.DisplayMsg("Sorry", errorMsg, "Ok");
            currentTile = startMove;

            foreach (GameTileCntrl tile in moveStack) 
            {
                tile.SetMaterial(markedMaterial);
                tile.SetAsMappedPath();
            }
        }

        return (goodMove);                
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

                tile.SetMaterial(markedMaterial);
                tile.SetAsMappedPath();
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
        startingTile.CreateStepTile(moveStep);
        prevTile = startingTile;
        lastTile = null;

        currentTile = startingTile.GetColRow();

        TileColRow point = startingTile.GetColRow();

        GameTileCntrl firstMoveTile = startingTile;         
        firstMoveTile.SetAsMappedPath();

        Debug.Log($"Level {level}");
        
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

                if ((tileCntrl != null) && (tileCntrl.OpenForMapping()))
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
                    tileCntrl.CreateMappedTile();
                }
            } else {
                point = moveStartPoint;
            }
        }

        GameManager.Instance.CreateDirBtns();

        Debug.Log($"Castle {gameData.castlePreFab}");
        Debug.Log($"Last Tile {lastTile.GetPosition()}");

        lastTile.CreateCastleTile();
    }

    private GameTileCntrl GetStartingPoint()
    {
        TileColRow colRow = new TileColRow(width / 2, height / 2);

        GameTileCntrl tileCntrl = gameBoard.GetTileCntrl(colRow); 
        tileCntrl.CreateStartingPointTile();

        return(tileCntrl);
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

            gameBoard = new GameTileCntrl[width, height];

            CreateGameBoard();
        }

        private void CreateGameBoard()
        {
            for (int col = 0; col < width; col++) 
            {
                for (int row = 0; row < height; row++)
                {
                    gameBoard[col, row] = CreateGameTile(col, row);
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
