using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStartButton : MonoBehaviour
{
    private MenuControllerInitializer menuControllerInitializer;

    private NumpadInputField inputField;
    // Start is called before the first frame update
    void OnEnable()
    {
        inputField = FindObjectOfType<NumpadInputField>();
        menuControllerInitializer = FindObjectOfType<MenuControllerInitializer>();
    }

    public void OnClick()
    {
        string userId;
        if (!inputField.GetId().Equals(""))
        {
            userId = inputField.GetId();
        }
        else
        {
            userId=FindObjectOfType<RandomPlayerId>().GetUserId();
        }
        menuControllerInitializer.Load(userId);
    }
}
