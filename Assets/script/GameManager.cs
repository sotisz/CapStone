using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string gameState = "playing";
    public static int currentLevel = 0;
    private void Update()
    {
        if (gameState == "playing")
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}