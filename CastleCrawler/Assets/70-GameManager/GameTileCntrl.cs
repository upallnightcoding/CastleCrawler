using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    public int Col { set; get; }
    public int Row { set; get; }

    private TileState state = TileState.OPEN;

    private GameObject stepTile = null;

    private bool gamePath = false;
    private bool bomb = false;
    private bool lastTile = false;

    public void SetColRow(int col, int row)
    {
        Col = col;
        Row = row;
    }

    public TileColRow GetColRow() 
    {
        return(new TileColRow(Col, Row));
    }

    public void CreateStepTile(int step) 
    {
        stepTile = Instantiate(gameData.stepNumberPreFab, GetPosition(), Quaternion.identity);
        StepNumberCntrl stepNumberCntrl = stepTile.GetComponent<StepNumberCntrl>();
        stepNumberCntrl.SetActive(step);
    }

    public void SetStartTile(int level)
    {
        CreateStepTile(level);
        SetAsGamePath();
        SetMaterial(gameData.startingPointMaterial);
    }

    public void SetLastTile(int level)
    {
        lastTile = true;

        CreateStepTile(level);
        SetAsGamePath();
        SetMaterial(gameData.endPointMaterial);
    }

    public void RemoveStepTile()
    {
        if (stepTile != null)
        {
            Destroy(stepTile);
        }
    }

    private void SetMaterial(Material material)
    {
        transform.GetChild(0).GetComponent<Renderer>().material = material;
    }
    
    public Vector3 GetPosition() => gameObject.transform.position;

    public bool HasBeenPlayed() => (state == TileState.PLAYED);
    public void SetToPlayed(Material material)
    {
        state = TileState.PLAYED;
        SetMaterial(material);
    }

    public bool IsOpen() => (state == TileState.OPEN);
    public void SetToOpen()
    {
        state = TileState.OPEN;
        SetMaterial(gameData.openMaterial);
    }

    public bool IsGamePath() => gamePath;
    public void SetAsGamePath() => gamePath = true;

    public bool IsBomb() => bomb;
    public bool IsLastTile() => lastTile;

    private enum TileState 
    {
        OPEN,
        PLAYED
    }
}
