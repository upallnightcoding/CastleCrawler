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

    public bool OnPlayersMove(string move) => boardCntrl.PlayersMove(move);
    public void OnUndo() => boardCntrl.UnDoLastMove();
    public void CheckWinner() => boardCntrl.CheckWinner();

    public int GetDirBtnCnt() => uiCntrl.GetDirBtnCnt();
    public void CreateDirBtns() => uiCntrl.CreateDirBtns();
    public void AddDirBtnCnt(string direction) => uiCntrl.AddDirBtnCnt(direction);

    public void WonGame() => uiCntrl.WonGame();
}