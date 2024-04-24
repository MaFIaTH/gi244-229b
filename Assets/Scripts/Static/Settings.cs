using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatchDuration
{
    Short = 180,
    Medium = 600,
    Long = 1200
}

public enum GameResult
{
    Win,
    Lose,
    Draw
}
public static class Settings
{
    public static string currentScene;

    public static Nations mySide = Nations.Britain;
    public static Nations EnemySide = Nations.France;
    public static MatchDuration matchDuration = MatchDuration.Short;
    public static GameResult gameResult;
    public static int myAssets;
    public static int enemyAssets;

    public static void SelectSide(int side)
    {
        switch (side)
        {
            case 0:
                mySide = Nations.Britain;
                EnemySide = Nations.France;
                break;
            case 1:
                mySide = Nations.France;
                EnemySide = Nations.Britain;
                break;
        }
        Debug.Log("My side: " + mySide + " Enemy side: " + EnemySide);
    }
    
    public static void SelectDuration(int duration)
    {
        switch (duration)
        {
            case 0:
                matchDuration = MatchDuration.Short;
                break;
            case 1:
                matchDuration = MatchDuration.Medium;
                break;
            case 2:
                matchDuration = MatchDuration.Long;
                break;
        }
        Debug.Log("Match duration: " + matchDuration);
    }

}
