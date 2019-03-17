using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBehaviour : MonoBehaviour {

    public enum BackgroundColor {Blue, Red, Green, Orange, Yellow};
    [SerializeField]
    private BackgroundColor myColor;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if(myColor == BackgroundColor.Blue)
        {
            animator.SetBool("Blue", true);
        }
        else if (myColor == BackgroundColor.Red)
        {
            animator.SetBool("Red", true);
        }
        else if (myColor == BackgroundColor.Green)
        {
            animator.SetBool("Green", true);
        }
        else if (myColor == BackgroundColor.Orange)
        {
            animator.SetBool("Orange", true);
        }
        else if (myColor == BackgroundColor.Yellow)
        {
            animator.SetBool("Yellow", true);
        }
    }

}
