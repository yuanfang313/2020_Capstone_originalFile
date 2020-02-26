using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerChoosing01 : MonoBehaviour
{
    public ObjectTriggeredEvents objectTriggeredEvents;
    public GameObject prefabTarget1;
    public GameObject prefabTarget2;
    //public GameObject prefabTarget3;
    public Transform target1_p;
    public Transform target2_p;
    //public Transform target3_p;

    public AudioSource audioSource_voice;
    public AudioClip audioClip_0;
    public AudioClip audioClip_1;
    public AudioClip audioClip_2;
    public AudioClip audioClip_3;
    public AudioClip audioClip_4;
    public AudioClip audioClip_5;
    public AudioClip audioClip_6;

    private float speed = 0.1f;
    private float timer1 = 0;
    private float timer2 = 0;
    private float timer3 = 0;
    private float timer4 = 0;
    private float timer5 = 0;
    private float timer6 = 0;
    private float intervalTimer = 0;

    private int number = 0;
    private int number2 = 0;
    private int count = 0;

    private bool hadPlay0 = false;
    private bool hadPlay1 = false;
    private bool hadPlay2 = false;
    private bool hadPlay3 = false;
    private bool hadPlay4 = false;
    private bool hadPlay5 = false;
    private bool hadPlay6 = false;

    private bool hadStarted = false;
    private bool hadFinished = false;
    private bool roundHadFinished = false;
    private bool answered = false;
    private bool answerCorrected = false;
    private bool generated = false;


    private GameObject target1InScene;
    private GameObject target2InScene;
    //private GameObject target3InScene;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // timers
        promptTimer();
        rightAnswerTimer();
        wrongAnswerTimer();
        roundTimer();
        IntervalTimer();

        //prompts before answer
        SetupQuestion();
        AnswerIsRightEventHandler();
        AnswerIsWrongEventHandler();
        TimeIsUp();

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
        if (timer1 > 6)
        {
            PlayAudioClip_0();
        }

        // play touchHorse clip
        if (intervalTimer > 6 && roundHadFinished && count < 3)
        {
            PlayAudioClip_1();
        }

        // generate items ramdomly
        if (hadPlay1 && hadFinished && !generated)
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
    private void TimeIsUp()
    {
        if (number2 >= 8)
        {
            PlayAudioClip_4();
        }

        if (number2 >= 15)
        {
            PlayAudioClip_3();
            Destroy(target1InScene);
            Destroy(target2InScene);
        }

        if (timer6 >= 30)
        {
            timer1 = 0;
            timer2 = 0;
            timer3 = 0;
            timer4 = 0;
            timer5 = 0;
            timer6 = 0;
            intervalTimer = 0;
            number = 0;
            number2 = 0;
            hadPlay1 = false;
            hadPlay5 = false;
            hadPlay3 = false;
            hadPlay4 = false;
            hadPlay5 = false;
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
        if (timer2 >= 18)
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
        }

        if (timer2 >= 30)
        {
            timer1 = 0;
            timer2 = 0;
            timer3 = 0;
            timer4 = 0;
            timer5 = 0;
            timer6 = 0;
            intervalTimer = 0;
            number = 0;
            number2 = 0;
            hadPlay1 = false;
            hadPlay2 = false;
            hadPlay3 = false;
            hadPlay4 = false;
            hadPlay5 = false;
            hadPlay6 = false;
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
        if (number >= 7 && !answerCorrected)
        {
            PlayAudioClip_3();
            Destroy(target1InScene);
            Destroy(target2InScene);
        }

        if (timer4 >= 15)
        {
            timer1 = 0;
            timer2 = 0;
            timer3 = 0;
            timer4 = 0;
            timer5 = 0;
            timer6 = 0;
            intervalTimer = 0;
            number = 0;
            number2 = 0;
            hadPlay1 = false;
            hadPlay2 = false;
            hadPlay3 = false;
            hadPlay4 = false;
            hadPlay5 = false;
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
        int positions = Random.Range(1, 3);

        if (positions == 1)
        {
            target1InScene = Instantiate(prefabTarget1, target1_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target2InScene = Instantiate(prefabTarget2, target2_p.position, Quaternion.AngleAxis(90, Vector3.up));
        }
        else if (positions == 2)
        {
            target1InScene = Instantiate(prefabTarget1, target2_p.position, Quaternion.AngleAxis(90, Vector3.up));
            target2InScene = Instantiate(prefabTarget2, target1_p.position, Quaternion.AngleAxis(90, Vector3.up));
        }
    }

    // setup timers
    private void promptTimer()
    {
        if (hadPlay0)
        {
            timer1 = 0;
        }
        timer1 += speed;
    }

    private void roundTimer()
    {
        if (generated && !answered)
        {
            timer5 = timer5 + speed / 6;
            number2 = (int)timer5;
            //TimerCount.text = number2.ToString();
        }
        if (number2 >= 15)
        {
            timer6 = timer6 + speed;
        }
    }

    private void rightAnswerTimer()
    {
        if (answerCorrected && answered)
        {
            timer2 = timer2 + speed;
        }
    }

    private void wrongAnswerTimer()
    {
        if (!answerCorrected && answered)
        {

            timer3 = timer3 + speed / 6;
            number = (int)timer3;
            //TimerCount.text = number.ToString();

            if (number >= 7 && !answerCorrected)
            {
                timer4 = timer4 + speed;
            }
        }
    }

    private void IntervalTimer()
    {
        if (!audioSource_voice.isPlaying && roundHadFinished)
        {
            intervalTimer += speed;
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
            hadFinished = true;
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
}
