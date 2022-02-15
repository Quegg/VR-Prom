using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesBoolean : XesAttribute
{
   private bool value;

   public XesBoolean(bool value)
   {
      this.value = value;
   }

   public override string ToString()
   {
      if (value)
         return "true";
      else
         return "false";
   }
}
