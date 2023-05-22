using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepNumberCntrl : MonoBehaviour
{
    [SerializeField] private GameObject numbers;

    private int active = 0;

    public void SetActive(int number)
    {
        numbers.transform.GetChild(active).gameObject.SetActive(false);
        numbers.transform.GetChild(number).gameObject.SetActive(true);
        active = number;
    }
}
