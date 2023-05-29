using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UICntrl uiCntrl;
    [SerializeField] private BoardCntrl boardCntrl;


    public void DisplayMsg(string title, string message, string buttonTxt)
    {
        uiCntrl.DisplayMsg(title, message, buttonTxt);
    }

    private void Awake() 
    {
        Instance = this;    
    }

    public void Undo(string move) => uiCntrl.Undo(move);

    public void SetLevel(int level) => uiCntrl.SetLevel(level);

    public static GameManager Instance { get; set; }

    public void AddDirBtnCnt(string direction) => uiCntrl.AddDirBtnCnt(direction);

    public void CreateDirBtns() => uiCntrl.CreateDirBtns();

    public bool OnPlayersMove(string move) => boardCntrl.PlayersMove(move);

    public void OnUndo() => boardCntrl.UnDoLastMove();
}