using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public bool isStop;

    public void Awake()
    {
        isStop = false;
    }

    public void Start()
    {
        GameManager.Instance.resumeButton = this;
    }

    public void Toggle()
    {
        if (isStop)
        {
            Time.timeScale = 1f;
            isStop = false;
        }
        else
        {
            Time.timeScale = 0f;
            isStop = true;
        }
    }
}
