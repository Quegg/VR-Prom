using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesCostAmount : XesCostExtension
{
    public XesCostAmount(XesFloat value) : base(value)
    {
        type = "float";
        key = "amount";
    }
}
