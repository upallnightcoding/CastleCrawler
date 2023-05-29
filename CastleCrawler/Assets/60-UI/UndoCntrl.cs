using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoCntrl : MonoBehaviour
{
    public void OnUndo()
    {
        GameManager.Instance.OnUndo();
    }
}
