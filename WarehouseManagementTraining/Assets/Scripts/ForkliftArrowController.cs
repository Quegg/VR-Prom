using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkliftArrowController : MonoBehaviour
{
    public GameObject arrows;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        arrows.SetActive(false);
    }

    [ContextMenu("StartAnimation")]
    public void StartAnimation()
    {
        arrows.SetActive(true);
        animator.SetTrigger("Start");
    }

    
    [ContextMenu("StopAnimation")]
    public void EndAnimation()
    {
        animator.SetTrigger("End");
        arrows.SetActive(false);

    }
}
