using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICntrl : MonoBehaviour
{
    [SerializeField] private DirBtnMgr dirBtnMgr;
    [SerializeField] private MsgDialogCntrl msgDialogCntrl;
    [SerializeField] private LevelCntrl levelCntrl;
    [SerializeField] private UndoCntrl undoCntrl;

    public void DisplayMsg(string title, string message, string buttonTxt) 
    {
        msgDialogCntrl.Display(title, message, buttonTxt);
    }

    public void Undo(string move) => dirBtnMgr.Undo(move);

    public void AddDirBtnCnt(string direction) => dirBtnMgr.AddDirBtnCnt(direction);

    public void CreateDirBtns() => dirBtnMgr.CreateDirBtns();

    public void SetLevel(int level) => levelCntrl.SetLevel(level);

    public int GetDirBtnCnt() => dirBtnMgr.GetDirBtnCnt();

    public void WonGame() => levelCntrl.WonGame();

    public void StartNewGame() => dirBtnMgr.StartNewGame();
}
