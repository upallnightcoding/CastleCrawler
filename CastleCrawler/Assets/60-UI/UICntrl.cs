using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICntrl : MonoBehaviour
{
    [SerializeField] private DirBtnMgr dirBtnMgr;
    [SerializeField] private MsgDialogCntrl msgDialogCntrl;

    public void AddDirBtnCnt(string direction) => dirBtnMgr.AddDirBtnCnt(direction);

    public void CreateDirBtns() => dirBtnMgr.CreateDirBtns();

    public void DisplayMsg(string title, string message, string buttonTxt) => 
        msgDialogCntrl.Display(title, message, buttonTxt);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
