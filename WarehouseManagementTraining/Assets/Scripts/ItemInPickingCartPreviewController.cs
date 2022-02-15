using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInPickingCartPreviewController : MonoBehaviour
{
    public GameObject fragile;
    public GameObject robust;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        fragile.SetActive(false);
        robust.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("StartAnimationFragile")]
    public void StartAnimationFragile()
    {
        fragile.SetActive(true);
        animator.SetTrigger("Start");
        
    }
    
    [ContextMenu("StartAnimationRobust")]
    public void StartAnimationRobust()
    {
        robust.SetActive(true);
        animator.SetTrigger("Start");
        
    }

    [ContextMenu("EndAnimation")]
    public void EndAnimation()
    {
        animator.SetTrigger("End");
        fragile.SetActive(false);
        robust.SetActive(false);
        
        
    }
}
