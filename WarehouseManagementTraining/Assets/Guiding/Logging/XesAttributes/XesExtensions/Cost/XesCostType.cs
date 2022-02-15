using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesCostType : XesCostExtension
{
    public XesCostType(XesString value) : base(value)
    {
        type = "string";
        key = "type";
    }
}
