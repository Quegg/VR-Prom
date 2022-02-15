using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumpadInputField : MonoBehaviour
{
    public int textFieldLength;
    private TextMeshProUGUI textField;

    private LastUserId lastUserId;
    // Start is called before the first frame update
    void OnEnable()
    { 
        textField = GetComponent<TextMeshProUGUI>();
        lastUserId = FindObjectOfType<LastUserId>();
        if (lastUserId is null)
        {
            GameObject obj = new GameObject("LastUserId");
            lastUserId = obj.AddComponent<LastUserId>();
            DontDestroyOnLoad(lastUserId);
            textField.text = "";
        }
        else
        {
            textField.text = lastUserId.lastId;
        }
        
        
    }

    public string GetId()
    {
        lastUserId.lastId = textField.text;
        return textField.text;
    }

    public void AddNumber(string number)
    {
        if (textField.text.Length < textFieldLength)
        {
            textField.text += number;
        }
        
    }

    public void Clear()
    {
        textField.text = "";
    }
}
