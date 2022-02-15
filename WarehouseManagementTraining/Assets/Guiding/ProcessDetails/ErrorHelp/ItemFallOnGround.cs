using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;

namespace Guiding.ProcessDetails.ErrorHelp
{
    public class ItemFallOnGround : MonoBehaviour, IErrorHelp {
        
        
        private string className="ItemFallOnGround";
        private GuidingController myGuidingController;
        
        //Return the class' name
        public string GetName()
        {
            return className;
        }
        
        
        //Show Help, if this error occurs. Else Help for current task is shown
        public void ShowHelp()
        {
            throw new NotImplementedException();
        }
        
        
        //Is help offered in this moment?
        public bool HelpAvailable()
        {
            return false;
        }
        
        
        
        //Gets called by the GuidingController when loading the class);
        public void Initialize(GuidingController guidingController)
        {
            myGuidingController = guidingController;
            
        }
    }
}
