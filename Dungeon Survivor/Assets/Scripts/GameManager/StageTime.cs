using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTime : MonoBehaviour

{
    public float time;
    TimerUI timerUI;
    // Start is called before the first frame update
    void Awake()
    {
        timerUI = FindObjectOfType<TimerUI>();
        if(timerUI == null)
        {
            Debug.LogError("Null timer UI");
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (timerUI == null)
        {
            Debug.LogError("It is null in Update");
        }
        time += Time.deltaTime;
        timerUI.UpdateTime(time);
        
    }
}
