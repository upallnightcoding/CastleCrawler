using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoveMgr
{
    public Direction[] directions;
    public String[] moves;
}

[Serializable]
public class Direction 
{
    public String name;
    public Step[] step;
}

[Serializable]
public class Step
{
    public int col;
    public int row;
}
