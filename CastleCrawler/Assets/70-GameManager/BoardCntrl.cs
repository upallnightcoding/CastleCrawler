using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private Transform boardParent;
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private GameObject trackerPF;

    private int width;
    private int height;

    private int level = 3;

    private GameTileCntrl[,] gameBoard = null;

    private MoveMgr moveMgr = null;


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

    private TileColRow GetStartingPoint()
    {
        int col = width / 2;
        int row = height / 2;
        TileColRow startingPoint = new TileColRow(col, row);
        GameTileCntrl tileCntrl = GetTileCntrl(startingPoint); 
        Destroy(tileCntrl.gameObject);

        Debug.Log($"Knights: {gameData.knightsPreFab}");

        GameTileCntrl newTile = CreateTile(gameData.knightsPreFab, col, row);
        newTile.MarkAsVisited();

        //tileCntrl.MakeAsVisited();

        //SetColor(startingPoint, blackMaterial);

        //startingPoint.PrintIt("Starting Point");

        return(startingPoint);
    }

    private void CreatePuzzle()
    {
        int numberOfMoves = 0;
        int tries = 0;
        TileColRow startingPoint = GetStartingPoint();
        TileColRow point = new TileColRow(startingPoint);
        
        while((numberOfMoves < 4) && (tries++ < 20))
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
                } else {
                    moveIsValid = false;
                    Debug.Log("Invalid: ");
                }
            }

            if (moveIsValid) 
            {
                numberOfMoves++;
                Vector3 position = new Vector3();

                while (stepQueue.Count != 0)
                {
                    GameTileCntrl tileCntrl = stepQueue.Dequeue();
                    tileCntrl.MarkAsVisited();
                    tileCntrl.SetMaterial(whiteMaterial);

                    position = tileCntrl.GetPosition();
                    Instantiate(trackerPF, position, Quaternion.identity);
                }
            } else {
                point = moveStartPoint;
            }
        }
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
        Vector3 position = new Vector3(col * 5.0f, 0.0f, row * 5.0f);

        GameObject go = Instantiate(preFab, position, Quaternion.identity);
        go.transform.parent = boardParent;

        GameTileCntrl tileCntrl = go.transform.GetChild(0).GetComponent<GameTileCntrl>();

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
