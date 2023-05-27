using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirBtnMgr : MonoBehaviour
{
    [SerializeField] private GameObject buttonColorPreFab;
    [SerializeField] private GameData gameData;
    [SerializeField] private Transform parent;

    private Dictionary<string, int> dirBtnCnt = null;
    private List<string> dirList = null;

    private void Awake() 
    {
        dirList = new List<string>();
        dirBtnCnt = new Dictionary<string, int>();
    }

    public void AddDirBtnCnt(string direction)
    {
        int count = 0; 

        if (dirBtnCnt.TryGetValue(direction, out count))
        {
            dirBtnCnt[direction] = ++count;
        } 
        else 
        {
            dirBtnCnt[direction] = ++count;
            dirList.Add(direction);
        }
    }

    public void CreateDirBtns()
    {
        int count = 0;
        int color = 0;

        foreach(string direction in dirList)
        {
            GameObject go = Instantiate(buttonColorPreFab, parent);
            DirBtnCntrl btnCntrl = go.GetComponent<DirBtnCntrl>();

            if (dirBtnCnt.TryGetValue(direction, out count))
            {
                btnCntrl.Initialize(direction, gameData.gameColors[color++], count);
            }       
        }
    }
}
