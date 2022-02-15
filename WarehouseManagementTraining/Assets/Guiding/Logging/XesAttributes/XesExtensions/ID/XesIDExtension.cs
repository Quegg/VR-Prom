using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesIDExtension : XesExtension
{
    public XesIDExtension(XesID value)
    {
        type = "id";
        prefix = "identity";
        key = "id";
        this.value = value;

    }
}
