using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("TEst")]
    public void TestTrimming()
    {
        string namer = "1Hallo144223DiesIst144223Ein57Test144223";
        Regex rx = new Regex(@"(\d)*$",RegexOptions.Compiled | RegexOptions.IgnoreCase);

        string nameNew = namer.Substring(0, namer.Length - rx.Match(namer).Length);
        Debug.Log(nameNew);
    }
}
