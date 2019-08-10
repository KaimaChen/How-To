using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject GameOverPanel;

    public static LevelManager Instance
    {
        get; private set;
    }

    public bool IsEnd
    {
        get; private set;
    }

    void Awake()
    {
        Instance = this;

        IsEnd = false;
        GameOverPanel.SetActive(false);
    }

    public void SetEnd()
    {
        IsEnd = true;
        GameOverPanel.SetActive(true);
    }
}
