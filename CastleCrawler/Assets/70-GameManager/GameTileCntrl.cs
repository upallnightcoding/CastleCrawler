using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    public int Col { set; get; }
    public int Row { set; get; }

    private TileState state = TileState.OPEN;
    private TileState revertState = TileState.OPEN;

    private GameObject stepTile = null;

    private bool gamePath = false;
    private bool bomb = false;
    private bool castleTile = false;

    public void SetColRow(int col, int row)
    {
        Col = col;
        Row = row;
    }

    public TileColRow GetColRow() 
    {
        return(new TileColRow(Col, Row));
    }

    public void CreateStartingPointTile() 
    {
        SetMaterial(gameData.startingPointMaterial);
    }

    public void CreateCastleTile()
    {
        GameObject go = Instantiate(gameData.castlePreFab, GetPosition(), Quaternion.identity);
        SetMaterial(gameData.endPointMaterial);
        castleTile = true;
    }

    public void CreateStepTile(int step) 
    {
        stepTile = Instantiate(gameData.stepNumberPreFab, GetPosition(), Quaternion.identity);
        StepNumberCntrl stepNumberCntrl = stepTile.GetComponent<StepNumberCntrl>();
        stepNumberCntrl.SetActive(step);
    }

    public void RemoveStepTile()
    {
        Destroy(stepTile);
    }

    public void SetMaterial(Material material)
    {
        transform.GetChild(0).GetComponent<Renderer>().material = material;
    }
    
    public Vector3 GetPosition() => gameObject.transform.position;

    public bool HasBeenPlayed() => (state == TileState.PLAYED);
    public void SetToPlayed() => state = TileState.PLAYED;

    public bool IsOpen() => (state == TileState.OPEN);
    public void SetToOpen()
    {
        state = TileState.OPEN;
        SetMaterial(gameData.openMaterial);
    }

    public bool IsGamePath() => gamePath;
    public void SetAsGamePath() => gamePath = true;

    public bool IsBomb() => bomb;

    public bool IsCastleTile() => castleTile;

    private enum TileState 
    {
        OPEN,
        PLAYED
    }
}
