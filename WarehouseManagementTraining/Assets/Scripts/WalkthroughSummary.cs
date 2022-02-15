using System;
using System.Collections.Generic;

[Serializable]
public class WalkthroughSummary
{
    public string userId;
    public long durationSeconds;
    public int numberOfWrongExecutedTasks;
    public int numberOfErrorsWithoutForkliftHit;
    public int numberOfForkliftHits;
    
    //1=positive, -1= negative, 
    public List<SectionFeedback> feedback;
}