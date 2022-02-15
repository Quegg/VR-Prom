using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XesAttributes;

public class XesTimestamp : XesExtension
{
    public XesTimestamp(XesDate value)
    {
        type = "date";
        prefix = "time";
        key = "timestamp";
        this.value = value;

    }
}
