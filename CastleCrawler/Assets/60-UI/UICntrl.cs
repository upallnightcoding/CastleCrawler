using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICntrl : MonoBehaviour
{
    [SerializeField] private DirBtnMgr dirBtnMgr;

    public void AddDirBtnCnt(string direction) => dirBtnMgr.AddDirBtnCnt(direction);

    public void CreateDirBtns() => dirBtnMgr.CreateDirBtns();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
