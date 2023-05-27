using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileCntrl : MonoBehaviour
{
    private TileState state = TileState.OPEN;

    public bool OpenForMapping() => (state == TileState.OPEN);
    public void SetAsMappedPath() => state = TileState.MAPPED;
    public bool OpenForTracking() => (state == TileState.OPEN) || (state == TileState.MAPPED);
    public void SetAsTracked() => state = TileState.TRACKED;

    public Vector3 GetPosition() => gameObject.transform.position;

    public void SetMaterial(Material material)
    {
        transform.GetChild(0).GetComponent<Renderer>().material = material;
    }

    private enum TileState 
    {
        OPEN,
        MAPPED,
        TRACKED
    }
}
