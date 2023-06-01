using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Castle Crawler/GameData")]
public class GameData : ScriptableObject
{
    [Header("Game Attributes")]
    public int width;
    public int height;

    public float tileSpacing;

    public int level;
    public int attemptPercent;

    [Header("Game PreFabs")]
    public GameObject gameTilePreFab;
    public GameObject castlePreFab;

    public Sprite[] gameColors;

    public GameObject stepNumberPreFab;

    [Header("Materials")]
    public Material startingPointMaterial;
    public Material endPointMaterial;
    public Material openMaterial;

    [Header("Moves")]
    public MoveMgr moveMgr;
}
