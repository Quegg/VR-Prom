using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class XesConceptExtension : XesExtension
{
    
    protected XesConceptExtension(XesString value)
    {
        type = "string";
        prefix = "concept";
        this.value = value;

    }
}
