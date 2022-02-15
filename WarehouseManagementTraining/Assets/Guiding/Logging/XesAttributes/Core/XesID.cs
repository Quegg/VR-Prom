using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesID : XesAttribute
{
    private string value;

    public XesID(string value)
    {
        this.value = value;
    }
    
    public override string ToString()
    {
        return value.ToString();
    }

    
}
