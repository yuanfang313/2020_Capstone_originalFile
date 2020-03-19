using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerChoosing01_old : MonoBehaviour
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
    public AudioClip audioClip_0, audioClip_1, audioClip_2, audioClip_3, audioClip_4, audioClip_5, audioClip_6, audioClip_7, audioClip_8;
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

    // publicUI
    #region publicUI
    [Header("UI")]
    public Text scoreOfSession1;
    public Text scoreOfSession2;
    public Text timerCountDown;
    public Text numOfRound;
    #endregion



    // private field
    // private UI
    #region privateUI
    private string strTimerRound;
    private string score1;
    private string score2;
    #endregion
    #region privateTimers
    private float _timer1 = 0;
    private float _intervalTimer = 0;
    private float _timer2 = 0;
    private float _timer3 = 0;
    private float _timerRound = 0;
    #endregion
    private int countRightAnswer1 = 0;
    private int countRightAnswer2 = 0;
    private int count = 1;

    #region private bool variables
    private bool hadPlay0 = false;
    private bool hadPlay1 = false;
    private bool hadPlay2 = false;
    private bool hadPlay3 = false;
    private bool hadPlay4 = false;
    private bool hadPlay5 = false;
    private bool hadPlay6 = false;
    private bool hadPlay7 = false;
    private bool hadPlay8 = false;

    private bool hadStarted = false;
    private bool hadFinished = false;
    private bool roundHadFinished = false;
    private bool answered = false;
    private bool answerCorrected = false;
    private bool generated = false;
    private bool hadPassed = false;
    #endregion

    #region privateRegion objects
    private GameObject target1InScene;
    private GameObject target2InScene;
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        _timer1 = timer1;
        _intervalTimer = intervalTimer;
        _timer2 = timer2;
        _timer3 = timer3;
        _timerRound = timerRound;

        //UI
        timerCountDown.text = ((int)timerRound).ToString();
        scoreOfSession1.text = "0";
        scoreOfSession2.text = "0";
        numOfRound.text = "1";

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

        //count & print rounds
        if (count <= 10)
        {
            numOfRound.text = count.ToString();
        }

        // count & print rounds of right answer
        if(count <= 5)
        {
            scoreOfSession1.text = countRightAnswer1.ToString();
        } else if (count > 5)
        {
            scoreOfSession2.text = countRightAnswer2.ToString();
        }
        
        // definition of right & wrong
        if (answered && answerCorrected)
        {
            AnswerIsRight();
        }
        else if (answered && !answerCorrected)
        {
            AnswerIsWrong();
        }

        // definition of passed & !passed
        if (countRightAnswer1 >= 4 && countRightAnswer2 >= 4)
        {
            hadPassed = true;
        }
        else if (countRightAnswer1 < 4 || countRightAnswer2 < 4)
        {
            hadPassed = false;
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
        if (_intervalTimer <= 0 && roundHadFinished && count <= 10)
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
        if (_timer3 <= 0 && _timerRound > 0)
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

        if(hadPlay4 || hadPlay7)
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
            if (count < 10)
            {
                PlayAudioClip_3();
            }
            else if (count == 10)
            {
                EndLevel();
            }
                
            Destroy(target1InScene);
            Destroy(target2InScene);
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
            hadPlay8 = false;
            answered = false;
            answerCorrected = false;
            objectTriggeredEvents.hadTriggeredTarget = false;
            objectTriggeredEvents.hadTriggeredRightTarget = false;
            generated = false;
            roundHadFinished = true;
            hadPassed = false;
            count = count + 1;
        }
    }

    private void AnswerIsRight()
    {
        PlayAudioClip_2();
        if (_timer2 <= 0 && hadPlay2)
        {
            if (count < 10)
            {
                PlayAudioClip_3();
            } else if (count == 10)
            {
                EndLevel();
            }

            Destroy(target1InScene);
            Destroy(target2InScene);
            _timer2 = timer2;
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
            hadPlay8 = false;
            answered = false;
            answerCorrected = false;
            objectTriggeredEvents.hadTriggeredTarget = false;
            objectTriggeredEvents.hadTriggeredRightTarget = false;
            generated = false;
            roundHadFinished = true;
            hadPassed = false;
            count = count + 1;
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
            if (count < 10)
            {
                PlayAudioClip_3();
            }
            else if (count == 10)
            {
                EndLevel();
            }
            Destroy(target1InScene);
            Destroy(target2InScene);
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
            hadPlay8 = false;
            answered = false;
            answerCorrected = false;
            objectTriggeredEvents.hadTriggeredTarget = false;
            objectTriggeredEvents.hadTriggeredRightTarget = false;
            generated = false;
            roundHadFinished = true;
            hadPassed = false;
            count = count + 1;
        }
    }

    private void EndLevel()
    {
            if (hadPassed)
            {
                PlayAudioClip_8();
            }
            else if (!hadPassed)
            {
                PlayAudioClip_6();
            }

        if(hadPlay6 || hadPlay8)
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
            hadPlay8 = false;
            answered = false;
            answerCorrected = false;
            objectTriggeredEvents.hadTriggeredTarget = false;
            objectTriggeredEvents.hadTriggeredRightTarget = false;
            generated = false;
            hadPassed = false;
            roundHadFinished = true;
            count = count + 1;
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

    private void repeatPromptTimer()
    {
        if (generated && !answered)
        {
            _timer3 -= Time.deltaTime;
        }
    }

    private void roundTimer()
    {
        int int_timerRound;

        if (hadPlay1 && !answerCorrected)
        {
            _timerRound -= Time.deltaTime;    
        } else if (_timerRound <= 0)
        {
            _timerRound = timerRound;
        }

        // print timer
        if (_timerRound <= 0)
        {
           int_timerRound = 0;
        } else
        {
           int_timerRound = (int)_timerRound;
        }

        strTimerRound = int_timerRound.ToString();
        timerCountDown.text = strTimerRound;
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
            if (count <= 5)
            {
                countRightAnswer1 = countRightAnswer1 + 1;
            } else if (count > 5)
            {
                countRightAnswer2 = countRightAnswer2 + 1;
            }
            
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
        if (!hadPlay6 && !hadStarted)
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

    private void PlayAudioClip_8()
    {
        if (!hadPlay8 && !hadStarted && answered)
        {
            //Debug.Log("Start playing Clip_6!");
            audioSource_voice.PlayOneShot(audioClip_8);
            hadStarted = true;
            hadFinished = false;
            hadPlay8 = true;
        }
        else if (!audioSource_voice.isPlaying)
        {
            hadFinished = true;
            hadStarted = false;
        }
    }
}
