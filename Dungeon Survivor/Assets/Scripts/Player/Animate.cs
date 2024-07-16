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
}
