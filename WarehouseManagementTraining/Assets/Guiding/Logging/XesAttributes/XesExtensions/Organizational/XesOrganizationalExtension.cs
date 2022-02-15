using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class XesOrganizationalExtension : XesExtension
{
    protected XesOrganizationalExtension(XesString value)
    {
        type = "string";
        prefix = "org";
        this.value = value;

    }
}
