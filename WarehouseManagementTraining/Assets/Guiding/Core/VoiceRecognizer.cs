using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class VoiceRecognizer
{
    private Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    private KeywordRecognizer _keywordRecognizer;
    private GuidingController guidingController;


    

    public VoiceRecognizer(GuidingController guidingController)
    {
        this.guidingController = guidingController;
        keywordActions.Add("yes",Yes);
        keywordActions.Add("no",No);
        
      
        _keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += OnKeyWordsRecognized;

    }

    public void StartListening()
    {
        _keywordRecognizer.Start();
    }

    public void EndListening()
    {
        _keywordRecognizer.Stop();
    }
    

    private void OnKeyWordsRecognized(PhraseRecognizedEventArgs args)
    {
        
        keywordActions[args.text].Invoke();
    }

    private void Yes()
    {
        guidingController.FeedbackResult(true);
    }

    private void No()
    {
        guidingController.FeedbackResult(false);
    }

    public void Deactivate()
    {
        _keywordRecognizer.OnPhraseRecognized -= OnKeyWordsRecognized;
        _keywordRecognizer.Dispose();
        _keywordRecognizer.Dispose();
    }

    
}