using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MsgDialogCntrl : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text message;
    [SerializeField] private TMP_Text buttonTxt;

    public void Display(string title, string message, string buttonTxt) 
    {
        this.title.text = title;
        this.message.text = message;
        this.buttonTxt.text = buttonTxt;

        gameObject.SetActive(true);
    }
}
