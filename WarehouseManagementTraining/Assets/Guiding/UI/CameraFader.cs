using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CameraFader : MonoBehaviour
{
    public float secondsToFadeIn;
    private void Start()
    {
        Debug.Log("Start");
        SteamVR_Fade.Start(Color.black,0);
        StartCoroutine(DelayToFadeInCamera());
    }

    IEnumerator DelayToFadeInCamera()
    {
        yield return new WaitForSeconds(0.5f);
        //yield return new WaitForSeconds(secondsToFadeIn);
        Debug.Log("Clear");
        SteamVR_Fade.Start(Color.clear, 2f);
    }

    [ContextMenu("NoFade")]
    public void NoFade()
    {
        SteamVR_Fade.Start(Color.clear, 0);
    }
}
