using System;
using System.Collections;
using System.Collections.Generic;
using PMLogging;
using UnityEngine;

public class TeleportLoggingHelper : MonoBehaviour
{
    private XesLogger logger;

    public GameObject player;
    private void Start()
    {
        logger=XesLogger.GetLogger;
        //item = GetComponent<Item>();
    }

    public void TeleportStart()
    {
        //logger.WriteEvent(new TeleportEvent(player.transform.position,DateTime.Now,XesLifecycleTransition.start));
    }

    public void TeleportEnd()
    {
        //logger.WriteEvent(new TeleportEvent(player.transform.position,DateTime.Now,XesLifecycleTransition.complete));
    }
}
