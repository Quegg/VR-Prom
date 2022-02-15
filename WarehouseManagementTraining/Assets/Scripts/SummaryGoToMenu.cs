using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryGoToMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMenu()
    {
        FindObjectOfType<MenuController>().GoToMenu();
    }
}
