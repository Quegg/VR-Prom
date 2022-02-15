using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigItemInOrder
{
    public string id;

    public int orderNumberIntern;

    public int orderNumberExtern;

    public bool isInPlace=false;

    public int palletPlaceID;

    private BigItem myBigItemScript;

    public BigItem BigItemScript => myBigItemScript;

    public BigItemInOrder(string id, int orderNumberIntern, int orderNumberExtern, BigItem bigItemScript)
    {
        this.id = id;
        this.orderNumberIntern = orderNumberIntern;
        this.orderNumberExtern = orderNumberExtern;
        myBigItemScript = bigItemScript;

    }

    public void SetPalletPlaceId(int ppID)
    {
        palletPlaceID = ppID;
        myBigItemScript.palletPlaceID = ppID;
    }
    
    
}
