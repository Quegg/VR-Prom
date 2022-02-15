using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class XesLifecycleExtension :XesExtension
{
    protected XesLifecycleExtension(XesString value)
    {
        type = "string";
        prefix = "lifecycle";
        this.value = value;

    }
}
