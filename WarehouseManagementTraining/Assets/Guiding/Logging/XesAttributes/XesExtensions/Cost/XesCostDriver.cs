using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesCostDriver : XesCostExtension
{
    public XesCostDriver(XesString value) : base(value)
    {
        type = "string";
        key = "driver";
    }
}
