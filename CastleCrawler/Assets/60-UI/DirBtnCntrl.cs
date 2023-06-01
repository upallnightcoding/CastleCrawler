using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DirBtnCntrl : MonoBehaviour
{
    [SerializeField] private TMP_Text directionTxt;
    [SerializeField] private TMP_Text countTxt;
    [SerializeField] private Sprite buttonDisabled;

    private int selectCount = 0;
    private Sprite originalSprite;
    private bool disableButton = false;

    public int GetSelectCount() => selectCount;

    public void OnPlayersMove() 
    {
        if (!disableButton)
        {
            bool goodMove = GameManager.Instance.OnPlayersMove(directionTxt.text);

            if (goodMove)
            {
                countTxt.text = (--selectCount).ToString();

                if (selectCount == 0) 
                {
                    GetComponent<Image>().sprite = buttonDisabled;    
                    disableButton = true;
                }

                GameManager.Instance.CheckWinner();
            }
        } else {
            GameManager.Instance.DisplayMsg("Sorry", "No more turns for this move.", "Ok");
        }
    }

    public void Undo()
    {
        if (selectCount == 0)
        {
            GetComponent<Image>().sprite = originalSprite;
            disableButton = false;
        }

        countTxt.text = (++selectCount).ToString();
    }

    public void Initialize(string direction, Sprite sprite, int count) 
    {
        // Set the button text
        directionTxt.text = direction;

        // Set the sprite of the button image
        GetComponent<Image>().sprite = sprite;

        // Cash the original sprite
        originalSprite = sprite;

        // Initialize the count of this direction
        countTxt.text = count.ToString();
        selectCount = count;

    }
}
