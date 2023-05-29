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

    private bool mapped = false;

    public void SetColRow(int col, int row)
    {
        Col = col;
        Row = row;
    }

    public TileColRow GetColRow() 
    {
        return(new TileColRow(Col, Row));
    }

    public void CreateMappedTile() 
    {
        SetAsMappedPath();
        SetMaterial(gameData.markedMaterial);
    }

    public void CreateStartingPointTile() 
    {
        SetMaterial(gameData.startingPointMaterial);
    }

    public void CreateCastleTile()
    {
        GameObject go = Instantiate(gameData.castlePreFab, GetPosition(), Quaternion.identity);
        SetMaterial(gameData.endPointMaterial);
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

    public bool OpenForMapping() => (state == TileState.OPEN);
    public void SetAsMappedPath() => state = TileState.MAPPED;
    public bool OpenForTracking() => (state == TileState.OPEN) || (state == TileState.MAPPED);
    public void SetAsTracked() => state = TileState.TRACKED;

    public Vector3 GetPosition() => gameObject.transform.position;

    private enum TileState 
    {
        OPEN,
        MAPPED,
        TRACKED
    }
}
