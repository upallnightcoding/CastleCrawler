using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileCntrl : MonoBehaviour
{
    private TileState state = TileState.OPEN;

    public bool IsOpen() => state == TileState.OPEN;
    public void MakeAsVisited() => state = TileState.VISTED;

    public void SetMaterial(Material material)
    {
        GetComponent<Renderer>().material = material;
    }

    public Vector3 GetPosition() 
    {
        return(gameObject.transform.position);
    }

    private enum TileState 
    {
        OPEN,
        VISTED
    }
}
