using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesSemanticExtension : XesExtension
{
    public XesSemanticExtension(XesString value)
    {
        type = "string";
        prefix = "semantic";
        key = "modelReference";
        this.value = value;

    }
}
