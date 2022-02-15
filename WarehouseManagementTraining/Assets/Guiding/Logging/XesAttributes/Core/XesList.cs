using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesList:XesAttribute
{
    private List<XesCollectionAttribute> attributes;

    public List<XesCollectionAttribute> Attributes
    {
        get => attributes;
        set => attributes = value;
    }
    
    public XesList()
    {
        attributes = new List<XesCollectionAttribute>();
    }
}
