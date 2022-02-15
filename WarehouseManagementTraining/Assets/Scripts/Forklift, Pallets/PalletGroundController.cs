using System;
using System.Collections;
using System.Collections.Generic;
using Guiding.LoggingEvents;
using Orders;
using PMLogging;
using TMPro;
using UnityEngine;
using Valve.Newtonsoft.Json.Utilities;

public class PalletGroundController : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer groundMarker;
    public Color markerEmpty;
    public Color markerPartly;
    private Color markerPartlyParticles;
    private Color markerEmptyParticles;
    public Color markerCompletely;
    private Color markerCompletelyParticles;
    public float transparencyRatio;

    private TextMeshProUGUI description;
    

    private ParticleSystem particles;
    private List<BigItem> palletsOnPlace;


    public string itemId;

    //is set by OrderManager after Instantiation
    [HideInInspector] 
    public OrderManager orderManager;
    //public int orderNumberExtern;

    public GameObject visuals;
    
    //is set by ordermanager after instantiation
    [HideInInspector] 
    public int orderNumberIntern;
    [HideInInspector]
    public int orderNumberExtern;
    [HideInInspector] 
    public int palletPlaceID;

    private GuidingController guidingController;

    public TextMeshPro palletPlaceNumberSign;

    public int palletPlaceNumber;
    void Start()
    {
        palletsOnPlace=new List<BigItem>();
        guidingController=FindObjectOfType<GuidingController>();
        groundMarker = GetComponentInChildren<SpriteRenderer>();
        groundMarker.color = markerEmpty;

        description = GetComponentInChildren<TextMeshProUGUI>();
        //description.text=name
        particles = GetComponentInChildren<ParticleSystem>();
        //particles.gameObject.SetActive(false);
        markerPartlyParticles= new Color(markerPartly.r,markerPartly.g,markerPartly.b,markerPartly.a/transparencyRatio);
        markerCompletelyParticles= new Color(markerCompletely.r,markerCompletely.g,markerCompletely.b,markerCompletely.a/transparencyRatio);
        markerEmptyParticles= new Color(markerEmpty.r,markerEmpty.g,markerEmpty.b,markerEmpty.a/transparencyRatio);
        palletPlaceNumberSign.text = palletPlaceNumber.ToString();
        
        
        var main = particles.main;
        main.startColor = markerEmptyParticles;
        //visuals.SetActive(false);
    }

    // Update is called once per frame
    
    public enum MarkerState
    {
        Empty, Partly,Completely
    }

    /// <summary>
    /// Changes the apperience of the pallet placed based on the given state
    /// </summary>
    /// <param name="state"></param>
    public void SetMarkerState(MarkerState state)
    {
        if (state == MarkerState.Empty)
        {
            groundMarker.color = markerEmpty;
            var main = particles.main;
            main.startColor = markerEmptyParticles;
            //particles.gameObject.SetActive(false);
        }
        else if(state== MarkerState.Partly)
        {
            groundMarker.color = markerPartly;
            var main = particles.main;
            main.startColor = markerPartlyParticles;
            //particles.gameObject.SetActive(true);
        }
        else if (state == MarkerState.Completely)
        {
            groundMarker.color = markerCompletely;
            var main = particles.main;
            main.startColor = markerCompletelyParticles;
            //particles.gameObject.SetActive(true);
            
        }
    }

     /// <summary>
     /// Inform the orderManager, that a pallet entered this pallet playce
     /// </summary>
     /// <param name="bigItemScript"></param>
    public void PalletEntered(BigItem bigItemScript)
    {
        if(bigItemScript.id==itemId)
            orderManager.UpdateBigItem(bigItemScript,true, orderNumberIntern, palletPlaceID);

        if (!palletsOnPlace.Contains(bigItemScript))
        {
            palletsOnPlace.Add(bigItemScript);
            
            guidingController.UserEvent(new PlaceBigItemOnPalletPlace(orderNumberExtern, bigItemScript.GetId(),(bigItemScript.palletPlaceID==palletPlaceID),DateTime.Now, XesLifecycleTransition.complete));
        }

        //Debug.Log("Pallet entered: "+bigItemScript.GetId());
    }

     /// <summary>
     /// Inform the orderManger, that the pallet left the palletPlace
     /// </summary>
     /// <param name="bigItemScript"></param>
    public void PalletLeft(BigItem bigItemScript)
    {
        if (!(orderManager is null))
        {
            orderManager.UpdateBigItem(bigItemScript, false, orderNumberIntern, palletPlaceID);
        }
        //Debug.Log("Pallet Left: "+bigItemScript.GetId());
        if (palletsOnPlace.Contains(bigItemScript))
        {
            guidingController.UserEvent(new PalletLeftPalletPlace(orderNumberExtern, bigItemScript.GetId(), DateTime.Now, XesLifecycleTransition.complete));


            palletsOnPlace.Remove(bigItemScript);
        }
    }
}

   

