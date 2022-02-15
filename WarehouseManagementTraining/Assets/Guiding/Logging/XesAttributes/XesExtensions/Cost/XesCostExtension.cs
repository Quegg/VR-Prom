using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class XesCostExtension : XesExtension
{
    protected XesCostExtension(XesAttribute value)
    {
        prefix = "cost";
        this.value = value;

    }
}
