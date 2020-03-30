﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    //public GameObject prefabTarget2;
    public Transform target1_p;
    public Transform target2_p;
    public Transform bubbleTransformAnchor;

    public GameObject ringEffect_0;
    public GameObject ringEffect_1;
    public GameObject ringEffect_2;
    public GameObject bubbleParticles;
    public Text voicePCounter_t;
    public Text visualPCounter_t;
    public Text visualPTimer_f;
    public Text visualPTimer_d;
    #endregion

    #region publicAudios
    [Header("AUDIOS")]
    public AudioSource audioSource_voice;
    public AudioClip audioClip_0, audioClip_1, audioClip_2, audioClip_3, audioClip_6, audioClip_7, audioClip_8;
    #endregion

    #region publicTimers
    [Header("TIMERS")]

    [Tooltip("timer1 is the timer before 'welcome clip'")]
    [SerializeField] private float timer1 = 0;

    [Tooltip("intervalTimer is the timer between 'welcome clip' and 'ready-touch clip'")]
    [SerializeField] private float intervalTimer = 0;

    [Tooltip("timer2 is the timer between 'goodJob clip' and 'tryAnotherRound clip' ")]
    [SerializeField] private float timer2 = 0;

    [Tooltip("voicePTimer1 is the first timer of the frequency of voice prompt")]
    [SerializeField] private float voicePTimer1 = 0;

    [Tooltip("voicePTimer2 is the second timer of the frequency of voice prompt")]
    [SerializeField] private float voicePTimer2 = 0;

    [Tooltip("visualPromptTimer1_f is the timer before the first visual prompt appearing")]
    [SerializeField] private float visualPromptTimer1_f = 0;

    [Tooltip("visualPromptTimer2_f is the timer between the visual prompts")]
    [SerializeField] private float visualPromptTimer2_f = 0;

    [Tooltip("visualPromptTimer1_d is the first timer of the duration of the visual prompts")]
    [SerializeField] private float visualPromptTimer1_d = 0;

    [Tooltip("visualPromptTimer2_d is the second timer of the duration of the visual prompts")]
    [SerializeField] private float visualPromptTimer2_d = 0;
    #endregion

    // private field
    #region privateTimers
    private float _timer1 = 0;
    private float _intervalTimer = 0;
    private float _timer2 = 0;
    private float voicePTempTimer = 0;
    private float visualPTempTimer_f = 0;
    private float visualPTempTimer_d = 0;
    #endregion

    private int count = 0;

    // vr ui debugging
    private int voicePromptCounter = 0;
    private int visualPromptCounter = 0;
    private int int_visualPTempTimer_f = 0;
    private int int_visualPTempTimer_d = 0;

    
    #region private bool variables
    private bool hadPlay0 = false;
    private bool hadPlay1 = false;
    private bool hadPlay2 = false;
    private bool hadPlay3 = false;
    private bool hadPlay6 = false;
    private bool hadPlay7 = false;
    private bool hadPlay8 = false;

    private bool hadStarted = false;
    private bool hadFinished = false;
    private bool roundHadFinished = false;
    private bool answered = false;
    private bool answerCorrected = false;
    private bool generated = false;
    private bool startTrail = false;
    private bool startRing0 = false;
    private bool startRing1 = false;
    private bool startRing2 = false;
    private bool startPrompts = false;
    private bool startBubbles = false;
    #endregion

    #region privateRegion objects
    private GameObject target1InScene;
    private GameObject ringInScene0;
    private GameObject ringInScene1;
    private GameObject ringInScene2;
    private GameObject trailsInScene;
    private GameObject bubbleInScene;
    private Transform rightAnswerTransformForRings;
    private ParticleSystem[] ringParticleInScene0;
    private ParticleSystem ringParticleInScene1;
    private ParticleSystem ringParticleInScene2;
    private Animator deerAnimator;
    //private GameObject target2InScene;
    #endregion

    #region trailObject Generate
    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject prefabTrailObject;
    private List<GameObject> trailGameObject = new List<GameObject>();
    private Transform controllerTransform;
    private OVRInput.Controller controllerMask;
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

        timeBtwShots = startTimeBtwShots;

        voicePTempTimer = voicePTimer1;
        visualPTempTimer_f = visualPromptTimer1_f;
        visualPTempTimer_d = visualPromptTimer1_d;
        PrintVoicePrompts();
        PrintVisualPrompts();
        PrintVisualPrompts_f();
        PrintVisualPrompts_d();
    }

    // Update is called once per frame
    void Update()
    {
        // timers
        welcomeTimer();
        IntervalTimer();
        voicePromptTimer();
        visualPromptTimer_f();
        visualPromptTimer_d();
        rightAnswerTimer();
        
        //prompts before answer
        SetupQuestion();
        AnswerIsRightEventHandler();
        AnswerIsWrongEventHandler();
        voicePrompt();
        visualPrompt();
        
        // had chose the answer
        if (answered && answerCorrected)
            AnswerIsRight();
        else if (answered && !answerCorrected)
            return;
    }

    private void OnDestroy()
    {
        ControllerEvents.OnControllerSource -= UpdateOrigin;
    }

    private void UpdateOrigin(OVRInput.Controller controller, GameObject controllerObject)
    {
        controllerTransform = controllerObject.transform;
        controllerMask = controller;
    }

    private void SetupQuestion()
    {
        // play wellcome clip
        if (_timer1 <= 0)
            PlayAudioClip_0();

        // play touchHorse clip
        if (_intervalTimer <= 0 && roundHadFinished && count < 5)
            PlayAudioClip_1();

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
   
    /* results of answer */
    // 1. prompts
    // 1-1. voice prompts
    private void voicePrompt()
    {
        if (voicePTempTimer <= 0)
            PlayAudioClip_7();


        if (hadPlay7)
        {
            voicePromptCounter = voicePromptCounter + 1;
            PrintVoicePrompts();

            if (voicePromptCounter <= 5)
                voicePTempTimer = voicePTimer1;
            else if (voicePromptCounter > 5)
                voicePTempTimer = voicePTimer2;
            
            hadPlay1 = false;
            hadPlay2 = false;
            hadPlay3 = false;
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

    private void voicePromptTimer()
    {
        if (generated && !answered && !audioSource_voice.isPlaying)
            voicePTempTimer -= Time.deltaTime;
    }

    // 1-2. visual prompts
    private void visualPrompt()
    {
        // when the interval time ends
        if (visualPTempTimer_f <= 0 && !startPrompts && !answered)
        {
            switch (visualPromptCounter)
            {
                case 0:
                    // add ring1
                    GenerateRingObject1();
                    visualPTempTimer_f = visualPromptTimer2_f;
                    PrintVisualPrompts_f();
                    startPrompts = true;
                    break;
                case 1:
                    // add ring2 & play ring1 
                    GenerateRingObject2();
                    ringParticleInScene1.Play();
                    visualPTempTimer_f = visualPromptTimer2_f;
                    PrintVisualPrompts_f();
                    startPrompts = true;
                    break;
                case 2:
                    // add animation + play ring1 & ring2
                    ringParticleInScene1.Play();
                    ringParticleInScene2.Play();
                    deerAnimator.SetBool("isWalking", true);
                    visualPTempTimer_f = visualPromptTimer2_f;
                    PrintVisualPrompts_f();
                    startPrompts = true;
                    break;
                case 3:
                    ringParticleInScene1.Play();
                    ringParticleInScene2.Play();
                    deerAnimator.SetBool("isWalking", true);
                    visualPTempTimer_f = visualPromptTimer2_f;
                    PrintVisualPrompts_f();
                    startPrompts = true;
                    startTrail = true;
                    break;
                default:
                    ringParticleInScene1.Play();
                    ringParticleInScene2.Play();
                    deerAnimator.SetBool("isWalking", true);
                    visualPTempTimer_f = visualPromptTimer2_f;
                    PrintVisualPrompts_f();
                    startPrompts = true;
                    startTrail = true;
                    break;
            }
        }

        if (startTrail)
            GenerateTrailObject();
        else if (!startTrail)
            OVRInput.SetControllerVibration(0, 0, controllerMask);

        // when the visual prompts end
        if (visualPTempTimer_d <= 0 && startPrompts && !answered)
        {
            if (ringParticleInScene1 != null)
                ringParticleInScene1.Stop();

            if (ringParticleInScene2 != null)
                ringParticleInScene2.Stop();
            
            if(deerAnimator != null)
                deerAnimator.SetBool("isWalking", false);

            if (visualPromptCounter < 4)
            {
                visualPTempTimer_d = visualPromptTimer1_d;
                PrintVisualPrompts_d();

            } else if (visualPromptCounter >= 4)
            {
                visualPTempTimer_d = visualPromptTimer2_d;
                PrintVisualPrompts_d();
            }

            visualPromptCounter = visualPromptCounter + 1;
            PrintVisualPrompts();
            startTrail = false;
            startPrompts = false;
        }
    }

    private void visualPromptTimer_f()
    {
        if(generated && !answered && !startPrompts)
        {
            visualPTempTimer_f -= Time.deltaTime;
            PrintVisualPrompts_f();
        }
    }

    private void visualPromptTimer_d()
    {
        if (generated && !answered && startPrompts)
        {
            visualPTempTimer_d -= Time.deltaTime;
            PrintVisualPrompts_d();
        }
    }

    // 2. answer is right
    private void AnswerIsRight()
    {
        PlayAudioClip_2();
        rightAnswerEffects();

        if (_timer2 <= 0 && hadPlay2)
        {
            if (count < 5)
                PlayAudioClip_3();

            if (count == 5)
                PlayAudioClip_6();
        }

        if (hadPlay3 || hadPlay6)
            cleanField();
    }

    private void cleanField()
    {
        deerAnimator.SetBool("isWalking", false);
        OVRInput.SetControllerVibration(0, 0, controllerMask);

        if (ringInScene0 != null)
            Destroy(ringInScene0);

        if (ringInScene1 != null)
            Destroy(ringInScene1);

        if (ringInScene2 != null)
            Destroy(ringInScene2);

        if (bubbleInScene != null)
            Destroy(bubbleInScene);

        Destroy(target1InScene);

        _timer2 = timer2;

        voicePTempTimer = voicePTimer1;
        // visual frequency timer == the value when a round begin
        visualPTempTimer_f = visualPromptTimer1_f;
        PrintVisualPrompts_f();
        // visual duration timer == the value when a round begin
        visualPTempTimer_d = visualPromptTimer1_d;
        PrintVisualPrompts_d();
        // the counter of visualPrompts & voicePrompts
        visualPromptCounter = 0;
        PrintVisualPrompts();
        voicePromptCounter = 0;
        PrintVoicePrompts();

        hadPlay1 = false;
        hadPlay2 = false;
        hadPlay3 = false;
        hadPlay6 = false;
        hadPlay7 = false;
        hadPlay8 = false;
        startRing1 = false;
        startRing2 = false;
        startRing0 = false;
        startTrail = false;
        startPrompts = false;
        startBubbles = false;
        answered = false;
        answerCorrected = false;
        objectTriggeredEvents.hadTriggeredTarget = false;
        objectTriggeredEvents.hadTriggeredRightTarget = false;
        generated = false;
        roundHadFinished = true;
    }

    private void rightAnswerEffects()
    {
        if (hadPlay2)
            PlayAudioClip_8();

        GenerateBubbles();

        if (ringInScene1 != null || ringInScene2 != null)
            ringParticleInScene1.Stop();

        if(ringInScene2 != null)
            ringParticleInScene2.Stop();

        if (startTrail)
            startTrail = false;

        if (deerAnimator != null)
            deerAnimator.SetBool("isWalking", true);

        GenerateRingObject0();
    }

    /* generate objects */
    // generate targets
    private void GenerateTargets()
    {
        int positions = Random.Range(1, 3);

        if (positions == 1)
        {
            target1InScene = Instantiate(prefabTarget1, target1_p.position, Quaternion.AngleAxis(230, Vector3.up));
            rightAnswerTransformForRings = GameObject.FindGameObjectWithTag("rightAnswerPosition_rings").transform;
        }
        else if (positions == 2)
        {
            target1InScene = Instantiate(prefabTarget1, target2_p.position, Quaternion.AngleAxis(130, Vector3.up));
            rightAnswerTransformForRings = GameObject.FindGameObjectWithTag("rightAnswerPosition_rings").transform;
        }

        deerAnimator = target1InScene.GetComponent<Animator>();
    }

    // generate trails
    private void GenerateTrailObject()
    {
        if(timeBtwShots <= 0)
        {
            trailsInScene = Instantiate(prefabTrailObject, controllerTransform.position, Quaternion.identity);
            OVRInput.SetControllerVibration(0.5f, 0.5f, controllerMask);
            timeBtwShots = startTimeBtwShots;  
        } else
        {
            timeBtwShots -= Time.deltaTime;
            OVRInput.SetControllerVibration(0.1f, 0.1f, controllerMask);
        } 
    }
    //generate ring0
    private void GenerateRingObject0()
    {
        if (!startRing0)
        {
            ringInScene0 = Instantiate(ringEffect_0, rightAnswerTransformForRings.position, Quaternion.AngleAxis(0, Vector3.left));
            ringParticleInScene0 = ringInScene0.GetComponentsInChildren<ParticleSystem>();
            startRing0 = true;
        }
    }
    // generate ring1
    private void GenerateRingObject1()
    {
        if (!startRing1)
        {
            ringInScene1 = Instantiate(ringEffect_1, rightAnswerTransformForRings.position, Quaternion.AngleAxis(-90, Vector3.left));
            ringParticleInScene1 = ringInScene1.GetComponent<ParticleSystem>();
            startRing1 = true;
        }
    }
    // generate ring2
    private void GenerateRingObject2()
    {
        if (!startRing2)
        {
            ringInScene2 = Instantiate(ringEffect_2, rightAnswerTransformForRings.position, Quaternion.AngleAxis(-90, Vector3.left));
            ringParticleInScene2 = ringInScene2.GetComponent<ParticleSystem>();
            startRing2 = true;
        }
    }
    // generate bubbles
    private void GenerateBubbles()
    {
        if (!startBubbles)
        {
            bubbleInScene = Instantiate(bubbleParticles, bubbleTransformAnchor.position, Quaternion.AngleAxis(-90, Vector3.left));
            startBubbles = true;
        }
    }

    /* setup timers */
    // timer before "Welcome-clip"
    private void welcomeTimer()
    {
        if (hadPlay0)
            _timer1 = timer1;
        else
            _timer1 -= Time.deltaTime;
    }

    // timer between "Welcome-clip" and "Ready? Touch..."
    private void IntervalTimer()
    {
        if (!audioSource_voice.isPlaying && roundHadFinished)
            _intervalTimer -= Time.deltaTime;
        else
            _intervalTimer = intervalTimer;
    }

    private void rightAnswerTimer()
    {
        if (answerCorrected && answered && !audioSource_voice.isPlaying && !objectTriggeredEvents.audioSource_click.isPlaying)
            _timer2 -= Time.deltaTime;
    }

    /* printing data */
    private void PrintVoicePrompts()
    {
        voicePCounter_t.text = voicePromptCounter.ToString();
    }

    private void PrintVisualPrompts()
    {
        visualPCounter_t.text = visualPromptCounter.ToString();
    }

    private void PrintVisualPrompts_f()
    {
        //int_visualPTempTimer_f = (int)visualPTempTimer_f;
        visualPTimer_f.text = visualPTempTimer_f.ToString("F1");
    }

    private void PrintVisualPrompts_d()
    {
        //int_visualPTempTimer_d = (int)visualPTempTimer_d;
        visualPTimer_d.text = visualPTempTimer_d.ToString("F1");
    }

    /* play AudioClips */
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
        if (!objectTriggeredEvents.audioSource_click.isPlaying && !hadPlay2 && !hadStarted)
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

    private void PlayAudioClip_8()
    {
        if (!objectTriggeredEvents.audioSource_click.isPlaying && !hadPlay8 && !hadStarted)
        {
            //Debug.Log("Start playing Clip_7!");
            audioSource_voice.PlayOneShot(audioClip_8);
            hadStarted = true;
            hadFinished = false;
            hadPlay8 = true;
        }
        else if (!audioSource_voice.isPlaying)
        {
            hadFinished = false;
            hadStarted = false;
        }
    }
}
