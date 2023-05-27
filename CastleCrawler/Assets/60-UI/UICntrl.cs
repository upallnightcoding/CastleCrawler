using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICntrl : MonoBehaviour
{
    [SerializeField] private DirBtnMgr dirBtnMgr;
    [SerializeField] private MsgDialogCntrl msgDialogCntrl;
    [SerializeField] private LevelCntrl levelCntrl;

    public void DisplayMsg(string title, string message, string buttonTxt) 
    {
        msgDialogCntrl.Display(title, message, buttonTxt);
    }

    public void AddDirBtnCnt(string direction) => dirBtnMgr.AddDirBtnCnt(direction);

    public void CreateDirBtns() => dirBtnMgr.CreateDirBtns();

    public void SetLevel(int level) => levelCntrl.SetLevel(level);

}
