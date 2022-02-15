using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesContainer:XesAttribute
{
    private List<XesCollectionAttribute> attributes;

    public List<XesCollectionAttribute> Attributes
    {
        get => attributes;
        set => attributes = value;
    }

    public XesContainer()
    {
        attributes = new List<XesCollectionAttribute>();
    }
}
