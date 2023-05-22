using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DirBtnCntrl : MonoBehaviour
{
    [SerializeField] private TMP_Text directionTxt;
    [SerializeField] private TMP_Text countTxt;

    private int selectCount = 0;

    public void PlayersMove() 
    {
        GameManager.Instance.PlayersMove(directionTxt.text);

        countTxt.text = (--selectCount).ToString();
    }

    public void Initialize(string direction, Sprite sprite, int count) 
    {
        // Set the button text
        directionTxt.text = direction;

        // Set the sprite of the button image
        GetComponent<Image>().sprite = sprite;

        // Initialize the count of this direction
        countTxt.text = count.ToString();
        selectCount = count;

    }
}
