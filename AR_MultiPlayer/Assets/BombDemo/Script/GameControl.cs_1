using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameVal
{
    public static GameControl gc;
}

public class GameControl : MonoBehaviour
{
    public int score;
    public int scorePerExplosion = 300;


    private void Awake()
    {
        GameVal.gc = this;
    }
    void Start()
    {
        Init();
    }
    public void Init()
    {
        score = 0;
    }
    public void AddScore()
    {
        score += scorePerExplosion;
        Debug.Log("Score now: " + score);
    }
}

