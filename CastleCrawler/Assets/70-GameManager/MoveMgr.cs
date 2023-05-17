using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoveMgr
{
    public String[] moves;

    private Step EAST = new Step(1, 0);
    private Step WEST = new Step(-1, 0);
    private Step NORTH = new Step(0, 1);
    private Step SOUTH = new Step(0, -1);

    public string PickMove() 
    {
        return(moves[GetRandom(moves.Length)]);
    }

    public Step GetStep(string direction) 
    {
        Step step = null;

        switch(direction)
        {
            case "N" : 
                step = NORTH;
                break;
            case "S" :
                step = SOUTH;
                break;
            case "E" :
                step = EAST;
                break;
            case "W" :
                step = WEST;
                break;
        }

        return(step);
    }

    private int GetRandom(int n)
    {
        return(UnityEngine.Random.Range(0, n));
    }
}

[Serializable]
public class Step
{
    public int Col { get; set; }
    public int Row { get; set; }

    public Step(int col, int row) 
    {
        this.Col = col;
        this.Row = row;
    }

    public void PrintIt()
    {
        Debug.Log($"Step: ({Col},{Row})");
    }
}
