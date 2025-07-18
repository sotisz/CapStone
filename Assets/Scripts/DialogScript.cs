using System;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    
    private void Start()
    {
        GameManager.gameState = "paused";
        GameObject.Find("Canvas").SetActive(true);
    }
}
