using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesString : XesAttribute
{
    private string value;

    public XesString(string value)
    {
        this.value = value;
    }
    
    public override string ToString()
    {
        return value.ToString();
    }
}
