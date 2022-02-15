using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class MyLaserPointer : SteamVR_LaserPointer
{
    
    
    public override void OnPointerClick(PointerEventArgs e)
    {
        base.OnPointerClick(e);
        if (e.target.gameObject.CompareTag("VR_Button"))
        {
            
            e.target.gameObject.GetComponent<Button>().onClick.Invoke();
            e.target.gameObject.GetComponent<Image>().color =
                e.target.gameObject.GetComponent<Button>().colors.pressedColor;
            //Debug.Log("Button Pressed outside");
        }
    }
    
    

    public override void OnPointerIn(PointerEventArgs e)
    {
        base.OnPointerIn(e);
        if (e.target.gameObject.CompareTag("VR_Button"))
        {


            e.target.gameObject.GetComponent<Image>().color =
                e.target.gameObject.GetComponent<Button>().colors.highlightedColor;
            //Debug.Log("Button Pressed outside");
        }
    }

    public override void OnPointerOut(PointerEventArgs e)
    {
        base.OnPointerOut(e);
        
        if (e.target.gameObject.CompareTag("VR_Button"))
        {


            e.target.gameObject.GetComponent<Image>().color =
                e.target.gameObject.GetComponent<Button>().colors.normalColor;
            //Debug.Log("Button Pressed outside");
        }
    }
    
    
}
