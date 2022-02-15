using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserManagement
{
    string GetUserId();

    void EventOccured(XesEvent userEvent, bool executedAtRightTime);
    
    void StartTrace();
    void EndTrace();
}
