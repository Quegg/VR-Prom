using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderInShippingBox : MonoBehaviour
{
    //[SerializeField] private GameObject textObj;
    [SerializeField] private GameObject greyPanel;
    
    private TextMeshProUGUI textID;
    private TextMeshProUGUI textAmount;
    private ShowOrderItemsShippingBox parent;
    private string mId;

    [Header("Counts down automatically when activated")]
    public bool autoCount;

    private void Start()
    {
        parent = GetComponentInParent<ShowOrderItemsShippingBox>();

    }
   

    private void Awake()
    {
        TextMeshProUGUI[] guiTexts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var guiText in guiTexts)
        {
            if (guiText.gameObject.name=="textId")
            {
                textID = guiText;
            }
            else
            {
                textAmount = guiText;
            }
        }
    }

    public void SetText(string id, string amount)
    {
        mId = id;
        textID.text = id + ":";
        textAmount.text = amount;
    }
    
    public void UpdateAmount(string amount)
    {
        if(autoCount)
            textAmount.text = amount;
    }

    public void setGrey(bool value)
    {
        if(autoCount)
            greyPanel.SetActive(value);
    }
    
}
