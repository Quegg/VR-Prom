using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesInt : XesAttribute
{
    private int value;

    public XesInt(int value)
    {
        this.value = value;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
