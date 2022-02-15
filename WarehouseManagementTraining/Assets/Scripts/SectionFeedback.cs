using System;
using System.Collections.Generic;

[Serializable]
public class SectionFeedback
{
    public string sectionName;
    public List<int> feedback;


    public SectionFeedback(string sectionName, int firstValue)
    {
        feedback = new List<int>();
        feedback.Add(firstValue);
        this.sectionName = sectionName;
    }

    public float GetFeedbackSum()
    {
        float value = 0;
        foreach (var f in feedback)
        {
            value += f;
        }

        return value;
    }

}
