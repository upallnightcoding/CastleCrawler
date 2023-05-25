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
    [SerializeField] private Material startingPointMaterial;
    [SerializeField] private Material endPointMaterial;

    private int width;
    private int height;

    private int level = 6;

    private bool mapSw = false;

    private GameTileCntrl[,] gameBoard = null;

    private MoveMgr moveMgr = null;

    private int colorSwitch = 0;
    private int moveCount = 0;

    private GameTileCntrl lastTile;
    private TileColRow startingPoint;
    private TileColRow activeColRow;
    private TileColRow prevColRow;

    private int GetRandom(int n) => Random.Range(0, n);

    // Start is called before the first frame update
    void Start()
    {
        width = gameData.width;
        height = gameData.height;

        gameBoard = new GameTileCntrl[width, height];

        moveMgr = gameData.moveMgr;

        DrawBoard();

        CreatePuzzle();
    }

    public void PlayersMove(string move) 
    {
        bool goodMove = true;
        Stack<GameTileCntrl> moveStack = new Stack<GameTileCntrl>();
        colorSwitch = 1 - colorSwitch;

        for (int i = 0; (i < move.Length) && goodMove; i++)
        {
            Step step = moveMgr.GetStep(move.Substring(i, 1));

            activeColRow.Add(step);

             GameTileCntrl tile = GetTileCntrl(activeColRow); 

             if (tile != null) 
             {
                if (tile.IsOpen()) 
                {
                    tile.SetMaterial(stepMaterials[colorSwitch]);
                    moveStack.Push(tile);
                } else {
                    GameManager.Instance.DisplayMsg("Sorry", "Tile is already occupied.", "Ok");
                    goodMove = false;    
                }
             } else {
                GameManager.Instance.DisplayMsg("Sorry", "That move takes you off the board.", "Ok");
                goodMove = false;
             }
        }

        if (goodMove) {
            prevColRow = activeColRow;
            CreateStepTile(activeColRow, ++moveCount);
        } else {
            foreach(GameTileCntrl tile in moveStack) 
            {
                tile.SetMaterial(markedMaterial);
            }
        }
    }

    private void CreatePuzzle()
    {
        int numberOfMoves = 0;
        int tries = 0;
        
        startingPoint = GetStartingPoint();
        activeColRow = new TileColRow(startingPoint);
        prevColRow = new TileColRow(startingPoint);
        lastTile = null;

        TileColRow point = new TileColRow(startingPoint);

        CreateStepTile(startingPoint, moveCount);

        GameTileCntrl startingTile = GetTileCntrl(startingPoint); 
        startingTile.MarkAsPath();
        
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

                GameTileCntrl tileCntrl = GetTileCntrl(point); 

                if ((tileCntrl != null) && (tileCntrl.IsOpen()))
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
                    tileCntrl.MarkAsPath();
                    tileCntrl.SetMaterial(markedMaterial);
                }
            } else {
                point = moveStartPoint;
            }
        }

        GameManager.Instance.CreateDirBtns();

        GameObject go = Instantiate(gameData.castlePreFab, lastTile.GetPosition(), Quaternion.identity);
        lastTile.SetMaterial(endPointMaterial);
    }

    private void CreateStepTile(TileColRow point, int step)
    {
        GameObject go = Instantiate(gameData.stepNumber, point.GetPosition(), Quaternion.identity);
        StepNumberCntrl stepNumberCntrl = go.GetComponent<StepNumberCntrl>();
        stepNumberCntrl.SetActive(step);
    }

    private TileColRow GetStartingPoint()
    {
        int col = width / 2;
        int row = height / 2;

        TileColRow startingPoint = new TileColRow(col, row);
        GameTileCntrl tileCntrl = GetTileCntrl(startingPoint); 
        tileCntrl.SetMaterial(startingPointMaterial);

        return(startingPoint);
    }

    private void DrawBoard()
    {
        GameObject gameTilePreFab = gameData.gameTilePreFab;

        for (int col = 0; col < width; col++) 
        {
            for (int row = 0; row < height; row++)
            {
                CreateTile(gameTilePreFab, col, row);
            }
        }
    }

    private GameTileCntrl CreateTile(GameObject preFab, int col, int row)
    {
        TileColRow point = new TileColRow(col, row);

        GameObject go = Instantiate(preFab, point.GetPosition(), Quaternion.identity);
        go.transform.parent = boardParent;

        GameTileCntrl tileCntrl = go.transform.GetComponent<GameTileCntrl>();

        gameBoard[col, row] = tileCntrl;

        return(tileCntrl);
    }

    // private void SetColor(TileColRow colRow, Material material)
    // {
    //     GameTileCntrl tileCntrl = GetTileCntrl(colRow);

    //     if (tileCntrl != null)
    //     {
    //         tileCntrl.SetMaterial(material);
    //     }
    // }

    private GameTileCntrl GetTileCntrl(TileColRow colRow) 
    {
        GameTileCntrl cntrl = null;
        int col = colRow.Col;
        int row = colRow.Row;

        if ((col >= 0) && (col < width) && (row >= 0) && (row < height))
        {
            cntrl = gameBoard[col, row];
        }

        return(cntrl);
    }

    private TileColRow GetRandomTile()
    {
        return(new TileColRow(GetRandom(width), GetRandom(height)));
    }

    private class TileColRow
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
    
}
