using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Animate : MonoBehaviour
{
    Animator animator;
    [HideInInspector]
    public float xInput;

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("xVelocity", xInput);
    }
    public void SetAnimate(GameObject animObject)
    {
        animator = animObject.GetComponentInChildren<Animator>();

    }
    //  create vector 
    void CreateVector()
    {
        Vector3 position = new Vector3(0, 0, 0);
        
    }
    void CreateRandomValue()
    {
        float randomValue = Random.Range(0f, 100f);
        
    }
}
