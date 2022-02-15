using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class XesFloat : XesAttribute
{
    private float value;

    public XesFloat(float value)
    {
        this.value = value;
    }

    public override string ToString()
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
}
