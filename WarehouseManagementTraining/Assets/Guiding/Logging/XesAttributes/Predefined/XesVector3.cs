using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesVector3 : XesCustomAttribute
{
    public XesFloat x;
    public XesFloat y;
    public XesFloat z;

    public XesVector3(Vector3 value)
    {
        x = new XesFloat(value.x);
        y = new XesFloat(value.y);
        z = new XesFloat(value.z);
    }
}
