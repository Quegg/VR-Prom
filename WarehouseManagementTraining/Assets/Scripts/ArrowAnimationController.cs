using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimationController : MonoBehaviour
{
    private List<Animator> animators;
    // Start is called before the first frame update
    void Start()
    {
        animators=new List<Animator>();
        foreach (var anim in GetComponentsInChildren<Animator>())
        {
            animators.Add(anim);
            anim.gameObject.SetActive(false);
        }
        
    }

    [ContextMenu("StartAnimation")]
    public void StartAnimation()
    {
        foreach (var animator in animators)
        {
            animator.gameObject.SetActive(true);
            animator.SetTrigger("Start");
        }
    }
    
    
    [ContextMenu("EndAnimation")]
    public void EndAnimation()
    {
        foreach (var animator in animators)
        {
            animator.SetTrigger("End");
            animator.gameObject.SetActive(false);
            
        }
    }
}
