using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerChoosingTutorial : MonoBehaviour
{
    // public field
    #region publicScripts
    [Header("SCRIPTS")]
    public ObjectTriggeredEvents objectTriggeredEvents;
    #endregion

    #region publicObjects
    [Header("TARGET ITEMS")]
    public GameObject prefabTarget1;
    public GameObject prefabTarget2;
    public Transform target1_p;
    public Transform target2_p;
    #endregion

    #region publicAudios
    [Header("AUDIOS")]
    public AudioSource audioSource_voice;
    public AudioClip audioClip_0, audioClip_1, audioClip_2, audioClip_3, audioClip_4, audioClip_5, audioClip_6, audioClip_7;
    #endregion

    #region publicTimers
    [Header("TIMERS")]

    [Tooltip("timer1 is the timer before 'welcome clip'")]
    [SerializeField] private float timer1 = 0;

    [Tooltip("intervalTimer is the timer between 'welcome clip' and 'ready-touch clip'")]
    [SerializeField] private float intervalTimer = 0;

    [Tooltip("timer2 is the timer between 'goodJob clip' and 'tryAnotherRound clip' ")]
    [SerializeField] private float timer2 = 0;

    [Tooltip("timer3 is the timer of the frequency of voice prompt")]
    [SerializeField] private float timer3 = 0;

    [Tooltip("timer4 is the timer of the frequency of visual prompt")]
    [SerializeField] private float timer4 = 0;
    #endregion

    // private field
    #region privateTimers
    private float _timer1 = 0;
    private float _intervalTimer = 0;
    private float _timer2 = 0;
    private float _timer3 = 0;
    private float _timer4 = 0;
    #endregion

    private int count = 0;

    #region private bool variables
    private bool hadPlay0 = false;
    private bool hadPlay1 = false;
    private bool hadPlay2 = false;
    private bool hadPlay3 = false;
    private bool hadPlay4 = false;
    private bool hadPlay5 = false;
    private bool hadPlay6 = false;
    private bool hadPlay7 = false;

    private bool hadStarted = false;
    private bool hadFinished = false;
    private bool roundHadFinished = false;
    private bool answered = false;
    private bool answerCorrected = false;
    private bool generated = false;
    private bool startTrail = false;
    #endregion

    #region privateRegion objects
    private GameObject target1InScene;
    private GameObject target2InScene;
    //private GameObject target3InScene;
    #endregion

    #region traceObject Generate
    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject prefabTrailObject;
    private Transform controllerTransform;
    #endregion



    private void Awake()
    {
        ControllerEvents.OnControllerSource += UpdateOrigin;
    }
    // Start is called before the first frame update
    void Start()
    {
        _timer1 = timer1;
        _intervalTimer = intervalTimer;
        _timer2 = timer2;
        _timer3 = timer3;
        _timer4 = timer4;

        timeBtwShots = startTimeBtwShots;
        //controllerTransform = pointerEvents.CurrentOrigin;
    }

    // Update is called once per frame
    void Update()
    {
        // timers
        welcomeTimer();
        IntervalTimer();
        repeatPromptTimer1();
        repeatPromptTimer2();
        rightAnswerTimer();
        
        //prompts before answer
        SetupQuestion();
        AnswerIsRightEventHandler();
        AnswerIsWrongEventHandler();
        RepeatPrompt1();
        RepeatPrompt2();
        
        // had chose the answer
        if (answered && answerCorrected)
        {
            AnswerIsRight();
        }
        else if (answered && !answerCorrected)
        {
            AnswerIsWrong();
        }
    }

    private void OnDestroy()
    {
        ControllerEvents.OnControllerSource -= UpdateOrigin;
    }

    private void UpdateOrigin(OVRInput.Controller controller, GameObject controllerObject)
    {
        controllerTransform = controllerObject.transform;
    }

    private void SetupQuestion()
    {
        // play wellcome clip
        if (_timer1 <= 0)
        {
            PlayAudioClip_0();
        }

        // play touchHorse clip
        if (_intervalTimer <= 0 && roundHadFinished && count < 3)
        {
            PlayAudioClip_1();
        }

        // generate items ramdomly
        if (hadPlay1 && roundHadFinished && !generated)
        {
            GenerateTargets();
            generated = true;
            roundHadFinished = false;
        }

    }

    // definition of right and wrong
    private void AnswerIsRightEventHandler()
    {
        if (objectTriggeredEvents.hadTriggeredTarget && objectTriggeredEvents.hadTriggeredRightTarget)
        {
            answered = true;
            answerCorrected = true;
        }
    }

    private void AnswerIsWrongEventHandler()
    {
        if (objectTriggeredEvents.hadTriggeredTarget && !objectTriggeredEvents.hadTriggeredRightTarget)
        {
            answered = true;
            answerCorrected = false;
        }
    }
   
    // results of answer
    // 1. time is up
    private void RepeatPrompt1()
    {
        int playRandom = Random.Range(1, 4);

        if (_timer3 <= 0)
        {
            if(playRandom == 1)
            {
                PlayAudioClip_4();
            }else if (playRandom == 2 || playRandom == 3)
            {
                PlayAudioClip_7();
            }
            startTrail = true;   
        }

        if (hadPlay4 || hadPlay7)
        {
            _timer3 = timer3;
            hadPlay1 = false;
            hadPlay2 = false;
            hadPlay3 = false;
            hadPlay4 = false;
            hadPlay5 = false;
            hadPlay6 = false;
            hadPlay7 = false;
            answered = false;
            answerCorrected = false;
            objectTriggeredEvents.hadTriggeredTarget = false;
            objectTriggeredEvents.hadTriggeredRightTarget = false;
            generated = true;
            roundHadFinished = false;
        }
    }

    private void RepeatPrompt2()
    {
        if (_timer4 != timer4 && _timer4 > 0)
        {
            GenerateTrailObject();
            
        } else if (_timer4 <= 0)
        {
            startTrail = false;
        }
    }

    // 2. answer is right
    private void AnswerIsRight()
    {
        PlayAudioClip_2();
        if (_timer2 <= 0 && hadPlay2)
        {
            if (count < 3)
            {
                PlayAudioClip_3();
            }

            if (count >= 3)
            {
                PlayAudioClip_6();
            }

            Destroy(target1InScene);
            Destroy(target2InScene);
            _timer2 = timer2;
        }

        if (hadPlay3 || hadPlay6)
        {
            _timer3 = timer3;
            hadPlay1 = false;
            hadPlay2 = false;
            hadPlay3 = false;
            hadPlay4 = false;
            hadPlay5 = false;
            hadPlay6 = false;
            hadPlay7 = false;
            answered = false;
            answerCorrected = false;
            objectTriggeredEvents.hadTriggeredTarget = false;
            objectTriggeredEvents.hadTriggeredRightTarget = false;
            generated = false;
            roundHadFinished = true;
        }
    }

    // 3. answer is wrong
    private void AnswerIsWrong()
    {
        PlayAudioClip_5();

        if (hadPlay5)
        {
            _timer3 = timer3;
            hadPlay1 = false;
            hadPlay2 = false;
            hadPlay3 = false;
            hadPlay4 = false;
            hadPlay5 = false;
            hadPlay6 = false;
            hadPlay7 = false;
            answered = false;
            answerCorrected = false;
            objectTriggeredEvents.hadTriggeredTarget = false;
            objectTriggeredEvents.hadTriggeredRightTarget = false;
            generated = true;
            roundHadFinished = false;
        }
    }

    // generate objects
    // generate reminder trace
    private void GenerateTrailObject()
    {
        if(timeBtwShots <= 0)
        {
            Instantiate(prefabTrailObject, controllerTransform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        } else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    // generate targets
    private void GenerateTargets()
    {
        int positions = Random.Range(1, 3);

        if (positions == 1)
        {
            target1InScene = Instantiate(prefabTarget1, target1_p.position, Quaternion.AngleAxis(130, Vector3.up));
            target2InScene = Instantiate(prefabTarget2, target2_p.position, Quaternion.AngleAxis(230, Vector3.up));
        }
        else if (positions == 2)
        {
            target1InScene = Instantiate(prefabTarget1, target2_p.position, Quaternion.AngleAxis(230, Vector3.up));
            target2InScene = Instantiate(prefabTarget2, target1_p.position, Quaternion.AngleAxis(130, Vector3.up));
        }
    }

    // setup timers
    // timer before "Welcome-clip"
    private void welcomeTimer()
    {
        if (hadPlay0)
        {
            _timer1 = timer1;
        } else
        {
            _timer1 -= Time.deltaTime;
        }  
    }

    // timer between "Welcome-clip" and "Ready? Touch..."
    private void IntervalTimer()
    {
        if (!audioSource_voice.isPlaying && roundHadFinished)
        {
            _intervalTimer -= Time.deltaTime;
        }
        else
        {
            _intervalTimer = intervalTimer;
        }
    }

    // timer of the frequency of voice prompt
    private void repeatPromptTimer1()
    {
        if (generated && !answered)
        {
            _timer3 -= Time.deltaTime;
        }
    }

    // timer of the frequency of trail prompt
    private void repeatPromptTimer2()
    {
        if (startTrail)
        {
            _timer4 -= Time.deltaTime;
        } else
        {
            _timer4 = timer4;
        }
    }

    private void rightAnswerTimer()
    {
        if (answerCorrected && answered && !audioSource_voice.isPlaying)
        {
            _timer2 -= Time.deltaTime;
        }
    }

    // play AudioClips
    private void PlayAudioClip_0()
    {
        if (!hadPlay0 && !hadStarted && !answered)
        {
            audioSource_voice.PlayOneShot(audioClip_0);
            hadStarted = true;
            hadFinished = false;
            hadPlay0 = true;
            roundHadFinished = true;
        }
        else if (!audioSource_voice.isPlaying)
        {
            hadFinished = true;
            hadStarted = false;
        }
    }

    private void PlayAudioClip_1()
    {
        if (!hadPlay1 && !hadStarted && !answered)
        {
            audioSource_voice.PlayOneShot(audioClip_1);
            hadStarted = true;
            hadFinished = false;
            hadPlay1 = true;
        }
        else if (!audioSource_voice.isPlaying)
        {
            hadFinished = true;
            hadStarted = false;
        }
    }

    private void PlayAudioClip_2()
    {
        if (!objectTriggeredEvents.audioSource_click.isPlaying && !hadPlay2 && !hadStarted && answered)
        {
            //Debug.Log("Start playing Clip_2!");
            audioSource_voice.PlayOneShot(audioClip_2);
            count = count + 1;
            hadStarted = true;
            hadFinished = false;
            hadPlay2 = true;
        }
        else if (!audioSource_voice.isPlaying)
        {
            hadFinished = true;
            hadStarted = false;
        }
    }

    private void PlayAudioClip_3()
    {
        if (!hadPlay3 && !hadStarted)
        {
            //Debug.Log("Start playing Clip_3!");
            audioSource_voice.PlayOneShot(audioClip_3);
            hadStarted = true;
            hadFinished = false;
            hadPlay3 = true;
        }
        else if (!audioSource_voice.isPlaying)
        {
            hadFinished = true;
            hadStarted = false;
        }
    }

    private void PlayAudioClip_4()
    {
        if (!hadPlay4 && !hadStarted && !answered)
        {
            //Debug.Log("Start playing Clip_4!");
            audioSource_voice.PlayOneShot(audioClip_4);
            hadStarted = true;
            hadFinished = false;
            hadPlay4 = true;
        }
        else if (!audioSource_voice.isPlaying)
        {
            hadFinished = false;
            hadStarted = false;
        }
    }

    private void PlayAudioClip_5()
    {
        if (!objectTriggeredEvents.audioSource_click.isPlaying && !hadPlay5 && !hadStarted && answered)
        {
            //Debug.Log("Start playing Clip_5!");
            audioSource_voice.PlayOneShot(audioClip_5);
            hadStarted = true;
            hadFinished = false;
            hadPlay5 = true;
        }
        else if (!audioSource_voice.isPlaying)
        {
            hadFinished = false;
            hadStarted = false;
        }
    }

    private void PlayAudioClip_6()
    {
        if (!objectTriggeredEvents.audioSource_click.isPlaying && !hadPlay6 && !hadStarted && answered)
        {
            //Debug.Log("Start playing Clip_5!");
            audioSource_voice.PlayOneShot(audioClip_6);
            hadStarted = true;
            hadFinished = false;
            hadPlay6 = true;
        }
        else if (!audioSource_voice.isPlaying)
        {
            hadFinished = true;
            hadStarted = false;
        }
    }

    private void PlayAudioClip_7()
    {
        if (!hadPlay7 && !hadStarted && !answered)
        {
            //Debug.Log("Start playing Clip_7!");
            audioSource_voice.PlayOneShot(audioClip_7);
            hadStarted = true;
            hadFinished = false;
            hadPlay7 = true;
        }
        else if (!audioSource_voice.isPlaying)
        {
            hadFinished = false;
            hadStarted = false;
        }
    }
}
