using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class OutlineController : MonoBehaviour
{
    private Outline outline;

    private float outlineMaxWidth;

    private float currentWidth;
    private float animationDuration;
    private float percent;
    private bool gettingBig;
    private bool isAnimating;
    private bool gettingSmall;
    private bool stay;

    private float timer;

    public bool isDeactivated;

    public bool useOutlineValues;

    private float defaultWidth;

    private Color defaultColor;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        defaultColor = outline.OutlineColor;
        defaultWidth = outline.OutlineWidth;

    }

    [ContextMenu("INitialize")]
    public void Initialize()
    {
        outline = GetComponent<Outline>();
        currentWidth = 0;
        outline.enabled = false;
        
    }

    /// <summary>
    /// Activate the outline
    /// </summary>
    /// <param name="outlineMaxWidth"></param>
    /// <param name="animationDurationSeconds"></param>
    /// <param name="color"></param>
    /// <exception cref="Exception"></exception>
    public void StartOutlineAnimation(float outlineMaxWidth, float animationDurationSeconds, Color color)
    {
        
        if(isDeactivated)
            throw new Exception("Outline Deactivated.  Activate in Inspector");
        outline.enabled = true;
        
        isAnimating = true;
        gettingBig = true;

        if (!useOutlineValues)
        {
            this.outlineMaxWidth = outlineMaxWidth;
            this.animationDuration = animationDurationSeconds;
            outline.OutlineColor = color;
        }
        else
        {
            this.outlineMaxWidth = defaultWidth;
            this.animationDuration = 5;
        }
    }

    [ContextMenu("hide")]
    public void HideOutline()
    {
        if(isDeactivated)
            throw new Exception("Outline Deactivated.  Activate in Inspector");
        isAnimating = false;
        gettingBig = false;
        gettingSmall = false;
        currentWidth = 0;
        outlineMaxWidth = 0;
        outline.enabled = false;
    }

    public void ShowOutlineStatic(float outlineWidth, Color color)
    {
        if(isDeactivated)
            throw new Exception("Outline Deactivated.  Activate in Inspector");
        outline.enabled = true;
        outline.OutlineWidth = outlineWidth;
        outline.OutlineColor = color;
    }

    [ContextMenu("test static")]
    public void TestStatic()
    {
        if(isDeactivated)
            throw new Exception("Outline Deactivated.  Activate in Inspector");
        ShowOutlineStatic(10, Color.white);
    }
    
    [ContextMenu("test animation")]
    public void StartOutlineAnimation()
    {
        if(isDeactivated)
            throw new Exception("Outline Deactivated.  Activate in Inspector");
        StartOutlineAnimation(10,5, Color.white);
    }

    /// <summary>
    /// Let the outline get bigger and smaller by time
    /// </summary>
    void Update()
    {
        if (!isDeactivated)
        {
            if (isAnimating)
            {
                if (gettingBig)
                {
                    if (timer <= animationDuration)
                    {
                        // basic timer
                        timer += Time.deltaTime;
                        // percent is a 0-1 float showing the percentage of time that has passed on our timer!
                        percent = timer / animationDuration;
                        // multiply the percentage to the difference of our two positions
                        // and add to the start
                        currentWidth = outlineMaxWidth * percent;
                    }
                    else
                    {
                        gettingBig = false;
                        gettingSmall = true;
                    }
                }
                else if (gettingSmall)
                {
                    if (timer > 0)
                    {
                        // basic timer
                        timer -= Time.deltaTime;
                        // percent is a 0-1 float showing the percentage of time that has passed on our timer!
                        percent = timer / animationDuration;
                        // multiply the percentage to the difference of our two positions
                        // and add to the start
                        currentWidth = outlineMaxWidth * percent;
                    }
                    else
                    {
                        gettingBig = true;
                        gettingSmall = false;
                    }
                }

                outline.OutlineWidth = currentWidth;

            }
        }

    }
    
}