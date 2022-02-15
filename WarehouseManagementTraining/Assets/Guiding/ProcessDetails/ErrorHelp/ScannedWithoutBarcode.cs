using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;

namespace Guiding.ProcessDetails.ErrorHelp
{
    public class ScannedWithoutBarcode : MonoBehaviour, IErrorHelp {
        
        
        private string className="ScannedWithoutBarcode";
        private GuidingController myGuidingController;
        [SerializeField] private int numberOfMistakesBeforeFirstHelp;
        private int mistakesHappened = 0;
        
        //Return the class' name
        public string GetName()
        {
            return className;
        }
        
        
        //Show Help, if this error occurs. Else Help for current task is shown
        public void ShowHelp()
        {
            //myGuidingController.ShowTextHelp("Aim at an item's barcode to scan it",className);
        }
        
        
        //Is help offered in this moment?
        public bool HelpAvailable()
        {
            return false;
            mistakesHappened++;
            return (mistakesHappened > numberOfMistakesBeforeFirstHelp);
        }
        
        
        
        //Gets called by the GuidingController when loading the class);
        public void Initialize(GuidingController guidingController)
        {
            myGuidingController = guidingController;
            
        }
    }
}
