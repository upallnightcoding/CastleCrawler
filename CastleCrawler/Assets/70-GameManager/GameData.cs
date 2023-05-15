using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Castle Crawler/GameData")]
public class GameData : ScriptableObject
{
    [Header("Game Attributes")]
    public int width;
    public int height;

    [Header("Game PreFabs")]
    public GameObject blankTilePreFab;

    [Header("Moves")]
    public MoveMgr moveMgr;
}