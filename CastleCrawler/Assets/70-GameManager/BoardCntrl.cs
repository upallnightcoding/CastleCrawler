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

    private void CreatePuzzle()
    {
        int numberOfMoves = 0;
        int tries = 0;
        TileColRow startPoint = GetRandomTile();
        TileColRow point = startPoint;
        //Stack<TileColRow> moveStack = new Stack<TileColRow>();
        SetColor(startPoint, blackMaterial);
        Debug.Log("Starting Point ...");
        startPoint.PrintIt();

        while((numberOfMoves < 3) && (tries++ < 20))
        {
            Queue<GameTileCntrl> stepQueue = new Queue<GameTileCntrl>();
            string move = moveMgr.PickMove();
            bool moveIsValid = true;
            TileColRow moveStartPoint = new TileColRow(point);
            int tracker = 0;

            Debug.Log($"Move: {move}");

            for (int i = 0; (i < move.Length) && moveIsValid; i++)
            {
                Step step = moveMgr.GetStep(move.Substring(i, 1));
                step.PrintIt();

                point.Add(step);

                point.PrintIt();

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
                    tileCntrl.MakeAsVisited();
                    tileCntrl.SetMaterial(whiteMaterial);

                    if (++tracker == 1) {
                        position = tileCntrl.GetPosition();
                        Instantiate(trackerPF, position, Quaternion.identity);
                    } else {
                        Debug.DrawLine(position, tileCntrl.GetPosition(), Color.red);
                        position = tileCntrl.GetPosition();
                        Instantiate(trackerPF, position, Quaternion.identity);
                    }
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
                Vector3 position = new Vector3(col * 5.0f, 0.0f, row * 5.0f);

                GameObject go = Instantiate(gameTilePreFab, position, Quaternion.identity);
                go.transform.parent = boardParent;
                go.name = $"Blank Tile: ({col},{row})";

                gameBoard[col, row] = go.transform.GetChild(0).GetComponent<GameTileCntrl>();
            }
        }
    }

    private void SetColor(TileColRow colRow, Material material)
    {
        GameTileCntrl tileCntrl = GetTileCntrl(colRow);
        colRow.PrintIt();
        Debug.Log($"Control: {tileCntrl}");

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

        public void PrintIt()
        {
            Debug.Log($"Tile ColRow: ({Col},{Row})");
        }
    }
    
}
