using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelCntrl : MonoBehaviour
{
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private Image[] starsWon;

    private int starCount = 0;
    private int level = 0;

    public void Awake()
    {
        Init();
    }

    public void WonGame()
    {
        if (starCount < 3)
        {
            starsWon[starCount++].enabled = true;
        } 
        else
        {
            Init();

            levelTxt.text = (++level).ToString();
        }
    }

    public void SetLevel(int level)
    {
        this.level = level;
        levelTxt.text = level.ToString();
    }

    private void Init()
    {
        starsWon[0].enabled = false;
        starsWon[1].enabled = false;
        starsWon[2].enabled = false;

        starCount = 0;
    }
}
