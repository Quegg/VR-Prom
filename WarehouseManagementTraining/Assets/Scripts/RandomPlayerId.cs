using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Random PlayerID input field
/// </summary>
public class RandomPlayerId : MonoBehaviour
{

    private TextMeshProUGUI inputField;

    private LastUserId lastUserId;
    // Start is called before the first frame update
 
    // Start is called before the first frame update
    
    
    void Start()
    {
        inputField=GetComponent<TextMeshProUGUI>();
        string value = Random.Range(0, 999999).ToString();
        string padding = "";
        if (value.Length < 6)
        {
            int counter = 6 - value.Length;
            while (counter>0)
            {
                padding = padding + "0";
                counter--;
            }
        }

        inputField.text = padding + value;
        if (lastUserId is null)
        {
            GameObject obj = new GameObject("LastUserId");
            lastUserId = obj.AddComponent<LastUserId>();
            DontDestroyOnLoad(lastUserId);
        }
    }
    

    public string GetUserId()
    {
        lastUserId.lastId=inputField.text;
        return inputField.text;
    }

    
}