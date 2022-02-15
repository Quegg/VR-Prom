using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumpadNumber : MonoBehaviour
{
    public bool isClear;
    public string value;
    public NumpadInputField inputField;
    
    

    public void OnClick()
    {
        if (isClear)
        {
            inputField.Clear();
        }
        else
        {
            inputField.AddNumber(value);    
        }
        
    }


}
