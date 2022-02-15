using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesCostCurrency : XesCostExtension
{
    public XesCostCurrency(XesString value) : base(value)
    {
        key = "currency";
        type = "string";
    }
}
