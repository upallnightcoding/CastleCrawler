using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UICntrl uiCntrl;
    [SerializeField] private BoardCntrl boardCntrl;

    public static GameManager Instance { get; set; }

    public void AddDirBtnCnt(string direction) => uiCntrl.AddDirBtnCnt(direction);

    public void CreateDirBtns() => uiCntrl.CreateDirBtns();

    public void PlayersMove(string move) => boardCntrl.PlayersMove(move);

    private void Awake() 
    {
        Instance = this;    
    }
}
