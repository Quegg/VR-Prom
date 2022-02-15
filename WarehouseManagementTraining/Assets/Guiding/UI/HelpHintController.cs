using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HelpHintController : MonoBehaviour
{
    //public HUDController hudController;
    public float delayToDisableButtonHints;
    public GameObject hint;
    public GameObject feedbackSavedObject;

    public AudioClip helpAvailableSound;
    public AudioClip feedbackSavedSound;

    private AudioSource hintAudio;

    private Animator animator;

    private GuidingController guidingController;
    private Player player;

    public GuidingController GuidingController
    {
        get => guidingController;
        set => guidingController = value;
    }

    public SteamVR_Action_Boolean toggleHelp;

    public SteamVR_Input_Sources inputSource;

    private bool isShowingNotification;
    
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<Player>();
        animator = GetComponent<Animator>();
        hintAudio = GetComponent<AudioSource>();
        
    }

    
    private void OnDestroy()
    {
        toggleHelp.RemoveOnStateDownListener(OnToggleHelpDown,inputSource);
    }

    private void Awake()
    {
        toggleHelp.AddOnStateDownListener(OnToggleHelpDown,inputSource);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// show the exclamation mark and play a sound to 
    /// </summary>
    [ContextMenu("Show")]
    public void ShowNotification()
    {
        ControllerButtonHints.ShowTextHint(player.leftHand,toggleHelp,"Show");
        StartCoroutine(DelayForDisablingButtonHints());
        isShowingNotification = true;
        hintAudio.clip = helpAvailableSound;
        hintAudio.Play();
        hint.SetActive(true);
        animator.SetTrigger("StartAnimation");
        
    }
    
    /// <summary>
    /// hide the notification
    /// </summary>
    [ContextMenu("Hide")]
    public void HideNotification()
    {
        Debug.Log("Hided");
        ControllerButtonHints.HideTextHint(player.leftHand, toggleHelp);
        StopCoroutine(DelayForDisablingButtonHints());
        animator.SetTrigger("EndAnimation");
        hint.SetActive(false);
        
    }

    IEnumerator DelayForDisablingButtonHints()
    {
        yield return new WaitForSeconds(delayToDisableButtonHints);
        ControllerButtonHints.HideTextHint(player.leftHand,toggleHelp);
        animator.SetTrigger("EndAnimation");
        hint.SetActive(false);
    }
    
    /// <summary>
    /// User pressed the button to show help
    /// </summary>
    /// <param name="fromAction"></param>
    /// <param name="fromSource"></param>
    public void OnToggleHelpDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (isShowingNotification)
        {
            isShowingNotification = false;
            HideNotification();
            
        }
        guidingController.UserRequestedShowHelp();
    }
    
    /// <summary>
    /// show notification, that the feedback was saved
    /// </summary>
    [ContextMenu("Feedback Saved")]
    public void ShowFeedbackSaved()
    {
        hintAudio.clip = feedbackSavedSound;
        hintAudio.Play();
        animator.SetTrigger("FeedbackSaved");
        //feedbackSavedObject.
    }

   

    
    
}

