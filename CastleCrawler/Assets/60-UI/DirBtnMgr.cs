using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirBtnMgr : MonoBehaviour
{
    [SerializeField] private GameObject buttonColorPreFab;
    [SerializeField] private GameData gameData;
    [SerializeField] private Transform parent;

    private Dictionary<string, int> dirBtnCntDict = null;
    private List<string> moveList = null;
    private Dictionary<string, DirBtnCntrl> btnCntrlsDict = null;

    private void Awake() 
    {
        moveList = new List<string>();
        dirBtnCntDict = new Dictionary<string, int>();
        btnCntrlsDict = new Dictionary<string, DirBtnCntrl>();
    }

    public void AddDirBtnCnt(string direction)
    {
        if (dirBtnCntDict.TryGetValue(direction, out int count))
        {
            dirBtnCntDict[direction] = ++count;
        } 
        else 
        {
            dirBtnCntDict[direction] = ++count;
            moveList.Add(direction);
        }
    }

    public void Undo(string move)
    {
        if (btnCntrlsDict.TryGetValue(move, out DirBtnCntrl button))
        {
            button.Undo();
        }
    }

    public void CreateDirBtns()
    {
        int color = 0;

        foreach(string move in moveList)
        {
            GameObject go = Instantiate(buttonColorPreFab, parent);
            DirBtnCntrl button = go.GetComponent<DirBtnCntrl>();

            if (dirBtnCntDict.TryGetValue(move, out int count))
            {
                button.Initialize(move, gameData.gameColors[color++], count);
                btnCntrlsDict[move] = button;
            }       
        }
    }
}
