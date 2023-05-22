using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private Transform boardParent;
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material[] stepMaterials;

    private int width;
    private int height;

    private int level = 6;

    private GameTileCntrl[,] gameBoard = null;

    private MoveMgr moveMgr = null;

    private int colorSwitch = 0;
    private int moveCount = 0;

    private GameTileCntrl lastTile;
    private TileColRow startingPoint;
    private TileColRow activeColRow;
    private TileColRow prevColRow;

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
        colorSwitch = 1 - colorSwitch;

        for (int i = 0; i < move.Length; i++)
        {
            Step step = moveMgr.GetStep(move.Substring(i, 1));

            activeColRow.Add(step);

             GameTileCntrl tile = GetTileCntrl(activeColRow); 

             if (tile != null) 
             {
                tile.SetMaterial(stepMaterials[colorSwitch]);
             }
        }

        prevColRow = activeColRow;

        CreateStep(activeColRow, ++moveCount);
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

        CreateStep(startingPoint, moveCount);
        
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
                    tileCntrl.MarkAsVisited();
                    tileCntrl.SetMaterial(whiteMaterial);
                }
            } else {
                point = moveStartPoint;
            }
        }

        GameManager.Instance.CreateDirBtns();

        GameObject go = Instantiate(gameData.castlePreFab, lastTile.GetPosition(), Quaternion.identity);
    }

    private void CreateStep(TileColRow point, int step)
    {
        GameObject go = Instantiate(gameData.stepNumber, point.GetPosition(), Quaternion.identity);
        StepNumberCntrl cntrl = go.GetComponent<StepNumberCntrl>();
        cntrl.SetActive(step);
    }

     private TileColRow GetStartingPoint()
    {
        int col = width / 2;
        int row = height / 2;

        TileColRow startingPoint = new TileColRow(col, row);
        GameTileCntrl tileCntrl = GetTileCntrl(startingPoint); 
        Destroy(tileCntrl.gameObject);

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
        //Vector3 position = new Vector3(col * 5.0f, 0.0f, row * 5.0f);

        TileColRow point = new TileColRow(col, row);

        GameObject go = Instantiate(preFab, point.GetPosition(), Quaternion.identity);
        go.transform.parent = boardParent;

        GameTileCntrl tileCntrl = go.transform.GetComponent<GameTileCntrl>();
        Debug.Log($"Tile Cntrl: {tileCntrl}");

        gameBoard[col, row] = tileCntrl;

        return(tileCntrl);
    }

    private void SetColor(TileColRow colRow, Material material)
    {
        GameTileCntrl tileCntrl = GetTileCntrl(colRow);

        if (tileCntrl != null)
        {
            tileCntrl.SetMaterial(material);
        }
    }

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

    private int GetRandom(int n)
    {
        return(Random.Range(0, n));
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
