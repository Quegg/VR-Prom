using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BigItem : Item
{
    
    [SerializeField]
    private string nameShown= "undefined";

    [HideInInspector] public bool isInPlace;

    //[HideInInspector]
    public int palletPlaceID;

    //public int correspondingIdExtern;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TextMeshPro>().text = nameShown;
    }

    public string GetId()
    {
        return id;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
