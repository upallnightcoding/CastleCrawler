using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCntrl : MonoBehaviour
{
    [SerializeField] private TMP_Text levelTxt;

    public void SetLevel(int level) => levelTxt.text = level.ToString();
}
