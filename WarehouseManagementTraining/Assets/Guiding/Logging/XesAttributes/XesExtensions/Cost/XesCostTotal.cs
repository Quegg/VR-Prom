using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesCostTotal : XesCostExtension
{
    public XesCostTotal(XesFloat value) : base(value)
    {
        type = "float";
        key = "total";
    }
}
