using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static string currentScene;

    public static Nations mySide;
    public static Nations EnemySide;

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

}
