using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Guiding.LoggingEvents;
using UnityEngine;
using Random = UnityEngine.Random;

public class UserManagement : MonoBehaviour,IUserManagement
{
    private string userId="";
    private int numberOfErrors;
    private int numberOfForkliftHitsObject;
    private int numberOfWrongTasks;
    private List<SectionFeedback> feedback;

    private DateTime startTime;

    public string UserId
    {
        get => userId;
        set => userId = value;
    }

    public string GetUserId()
    {
        if (userId.Equals(""))
        {
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

            return padding + value;
        }
        return userId;
    }
    
    
    

    //normalEventButWrong means that a "normal" event occured (no feedback or error) but it was not the next event to execute
    //and we treat it like a special kind of error
    public void EventOccured(XesEvent userEvent, bool executedAtRightTime)
    {
        if (userEvent is PositiveFeedback)
        {
            string sectionName = ((PositiveFeedback) userEvent).sectionName.ToString();
            SectionFeedback thisFeedback =
                GetSectionFeedbackByName(sectionName);
            if (thisFeedback is null)
            {
                feedback.Add(new SectionFeedback(sectionName,1));
            }
            else
            {
                thisFeedback.feedback.Add(1);
            }
            
        }
        else if(userEvent is NegativeFeedback)
        {
            string sectionName = ((NegativeFeedback) userEvent).sectionName.ToString();
            SectionFeedback thisFeedback =
                GetSectionFeedbackByName(sectionName);
            if (thisFeedback is null)
            {
                feedback.Add(new SectionFeedback(sectionName,-1));
            }
            else
            {
                thisFeedback.feedback.Add(-1);
            }
        }
        
        else if (userEvent is ForkliftHitsObject)
        {
            numberOfForkliftHitsObject++;
        }
        else
        {
            //Searchj for the isError Field and get its value   
            bool isError=false;
            FieldInfo[] fields = userEvent.GetType().GetFields();
            foreach (var field in fields)
            {
                if (field.Name == "isError")
                {
                    isError = (bool) field.GetValue(userEvent);
                    break;
                }
            }

            if (isError)
            {
                numberOfErrors++;
            }

        }

        if (!executedAtRightTime)
        {
            numberOfWrongTasks++;
        }
    }
    

    [ContextMenu("Serialization test")]
    public void SerializeTest()
    {
        feedback.Add(new SectionFeedback("test",5));
        EndTrace();
        StartTrace();
    }

    public void StartTrace()
    {
        startTime=DateTime.Now;
        if (feedback is null)
        {
            feedback=new List<SectionFeedback>();
        }
        else
        {
            feedback.Clear();
        }
        
        //throw new System.NotImplementedException();
    }

    public void EndTrace()
    {
        
    }

    public WalkthroughSummary CreateSummary()
    {
        //create new Summary object and sets the data to serialize later
        WalkthroughSummary summary=new WalkthroughSummary();
        summary.userId = userId;
        DateTime dateTime = DateTime.Now;
        TimeSpan duration = dateTime - startTime;
        summary.durationSeconds = (long)duration.TotalSeconds;
        summary.numberOfForkliftHits = numberOfForkliftHitsObject;
        summary.numberOfErrorsWithoutForkliftHit = numberOfErrors;
        summary.feedback = feedback;
        summary.numberOfWrongExecutedTasks = numberOfWrongTasks;
        
        string filename = "user_" + summary.userId + "_" + dateTime.Day + "." + dateTime.Month + "." + dateTime.Year +
                          "_" + dateTime.Hour + "." + dateTime.Minute + "." + dateTime.Second+".json";
        
        //serialize the summary object
        string json = JsonUtility.ToJson(summary);
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath,"summaries")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath,"summaries"));
        }
        File.WriteAllText(Path.Combine(Application.persistentDataPath,"summaries",filename),json);
        
        
        return summary;
    }

    private SectionFeedback GetSectionFeedbackByName(string sectionName)
    {
        foreach (var f in feedback)
        {
            if (f.sectionName.Equals(sectionName))
            {
                return f;
            }
        }

        return null;
    }
    
}
