﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerChoosing : MonoBehaviour
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
    public GameObject prefabTarget3;
    public Transform target1_p;
    public Transform target2_p;
    public Transform target3_p;
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

    [Tooltip("timerRound is the timer of a round")]
    [SerializeField] private float timerRound = 0;
    #endregion

    // private field
    #region privateTimers
    private float _timer1 = 0;
    private float _intervalTimer = 0;
    private float _timer2 = 0;
    private float _timer3 = 0;
    private float _timerRound = 0;
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
    #endregion

    #region privateRegion objects
    private GameObject target1InScene;
    private GameObject target2InScene;
    private GameObject target3InScene;
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        _timer1 = timer1;
        _intervalTimer = intervalTimer;
        _timer2 = timer2;
        _timer3 = timer3;
        _timerRound = timerRound;

    }

    // Update is called once per frame
    void Update()
    {
        // timers
        welcomeTimer();
        IntervalTimer();
        rightAnswerTimer();
        repeatPromptTimer();
        roundTimer();

        //prompts before answer
        SetupQuestion();
        AnswerIsRightEventHandler();
        AnswerIsWrongEventHandler();
        repeatPrompt();

        if (answered && answerCorrected)
        {
            AnswerIsRight();
        }
        else if (answered && !answerCorrected)
        {
            AnswerIsWrong();
        }
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
    private void repeatPrompt()
    {
        if (_timer3 <= 0)
        {
            int playRandom = Random.Range(1, 4);

            if (playRandom == 1)
            {
                PlayAudioClip_4();
            }
            else if (playRandom == 2 || playRandom == 3)
            {
                PlayAudioClip_7();
            }
        }

        if (hadPlay4 || hadPlay7)
        {
            _timer3 = timer3;
            hadPlay3 = false;
            hadPlay4 = false;
            hadPlay7 = false;
            answered = false;
            answerCorrected = false;
            objectTriggeredEvents.hadTriggeredTarget = false;
            objectTriggeredEvents.hadTriggeredRightTarget = false;
            generated = true;
            roundHadFinished = false;
        }

        if (_timerRound <= 0)
        {
            PlayAudioClip_3();
            Destroy(target1InScene);
            Destroy(target2InScene);
            Destroy(target3InScene);
        }

        if (hadPlay3)
        {
            _intervalTimer = intervalTimer;
            _timer2 = timer2;
            _timer3 = timer3;
            _timerRound = timerRound;
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
            Destroy(target3InScene);
            _timer2 = timer2;
        }

        if (hadPlay3 || hadPlay6)
        {
            _intervalTimer = intervalTimer;
            _timer2 = timer2;
            _timer3 = timer3;
            _timerRound = timerRound;
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

    private void AnswerIsWrong()
    {
        PlayAudioClip_5();

        if (hadPlay5)
        {
            _timer3 = timer3;
            hadPlay3 = false;
            hadPlay4 = false;
            hadPlay5 = false;
            hadPlay7 = false;
            answered = false;
            answerCorrected = false;
            objectTriggeredEvents.hadTriggeredTarget = false;
            objectTriggeredEvents.hadTriggeredRightTarget = false;
            generated = true;
            roundHadFinished = false;
        }

        if (_timerRound <= 0)
        {
            PlayAudioClip_3();
            Destroy(target1InScene);
            Destroy(target2InScene);
            Destroy(target3InScene);
        }

        if (hadPlay3)
        {
            _intervalTimer = intervalTimer;
            _timer2 = timer2;
            _timer3 = timer3;
            _timerRound = timerRound;
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

    // generate targets
    private void GenerateTargets()
    {
        int positions = Random.Range(1,7);

        if(positions == 1)
        {
            target1InScene = Instantiate(prefabTarget1, target1_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target2InScene = Instantiate(prefabTarget2, target2_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target3InScene = Instantiate(prefabTarget3, target3_p.position, Quaternion.AngleAxis(90, Vector3.up));
        } else if (positions == 2)
        {
            target1InScene = Instantiate(prefabTarget1, target2_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target2InScene = Instantiate(prefabTarget2, target1_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target3InScene = Instantiate(prefabTarget3, target3_p.position, Quaternion.AngleAxis(90, Vector3.up));
        } else if (positions ==3)
        {
            target1InScene = Instantiate(prefabTarget1, target2_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target2InScene = Instantiate(prefabTarget2, target3_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target3InScene = Instantiate(prefabTarget3, target1_p.position, Quaternion.AngleAxis(90, Vector3.up));
        } else if (positions == 4)
        {
            target1InScene = Instantiate(prefabTarget1, target1_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target2InScene = Instantiate(prefabTarget2, target3_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target3InScene = Instantiate(prefabTarget3, target2_p.position, Quaternion.AngleAxis(90, Vector3.up));
        } else if (positions == 5)
        {
            target1InScene = Instantiate(prefabTarget1, target3_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target2InScene = Instantiate(prefabTarget2, target1_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target3InScene = Instantiate(prefabTarget3, target2_p.position, Quaternion.AngleAxis(90, Vector3.up));
        } else if (positions == 6)
        {
            target1InScene = Instantiate(prefabTarget1, target3_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target2InScene = Instantiate(prefabTarget2, target2_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target3InScene = Instantiate(prefabTarget3, target1_p.position, Quaternion.AngleAxis(90, Vector3.up));
        }
    }

    // setup timers
    // timer before "Welcome-clip"
    private void welcomeTimer()
    {
        if (hadPlay0)
        {
            _timer1 = timer1;
        }
        else
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

    private void repeatPromptTimer()
    {
        if (generated && !answered)
        {
            _timer3 -= Time.deltaTime;
        }
    }

    private void roundTimer()
    {
        if (hadPlay1 && !answerCorrected)
        {
            _timerRound -= Time.deltaTime;
        }
        else if (answerCorrected || _timerRound <= 0)
        {
            _timerRound = timerRound;
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
        if (!hadPlay0 && !hadStarted && answered == false)
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
        if (!hadPlay1 && !hadStarted && answered == false)
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
        if (!objectTriggeredEvents.audioSource_click.isPlaying && !hadPlay5 && !hadStarted && answered == true)
        {
            //Debug.Log("Start playing Clip_5!");
            audioSource_voice.PlayOneShot(audioClip_5);
            hadStarted = true;
            hadFinished = false;
            hadPlay5 = true;
        }
        else if (!audioSource_voice.isPlaying)
        {
            hadFinished = true;
            hadStarted = false;
        }
    }

    private void PlayAudioClip_6()
    {
        if (!hadPlay6 && !hadStarted && answered)
        {
            //Debug.Log("Start playing Clip_6!");
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
