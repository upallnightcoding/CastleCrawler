using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameCntrl : MonoBehaviour
{
    public void StartNewGame()
    {
        GameManager.Instance.StartNewGame();
    }
}
