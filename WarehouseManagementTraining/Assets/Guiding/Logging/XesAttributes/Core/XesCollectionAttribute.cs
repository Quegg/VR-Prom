using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesCollectionAttribute : XesAttribute
{
    private string name;
    private XesAttribute value;

    public string Name
    {
        get => name;
        set => name = value;
    }

    public XesAttribute Value
    {
        get => value;
        set => this.value = value;
    }
}
