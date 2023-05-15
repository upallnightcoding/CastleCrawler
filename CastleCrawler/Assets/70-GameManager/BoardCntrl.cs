using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private Transform board;

    private int width;
    private int height;

    // Start is called before the first frame update
    void Start()
    {
        width = gameData.width;
        height = gameData.height;

        DrawBoard();
    }

    private void DrawBoard()
    {
        GameObject blankTilePreFab = gameData.blankTilePreFab;

        for (int col = 0; col < width; col++) 
        {
            for (int row = 0; row < height; row++)
            {
                Vector3 position = new Vector3(col * 5.0f, 0.0f, row * 5.0f);

                GameObject go = Instantiate(blankTilePreFab, position, Quaternion.identity);
                go.transform.parent = board;
                go.name = $"Blank Tile: ({col},{row})";
            }
        }
    }

    
}
