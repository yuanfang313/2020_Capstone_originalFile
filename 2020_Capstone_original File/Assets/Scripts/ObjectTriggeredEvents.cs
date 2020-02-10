using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectTriggeredEvents : MonoBehaviour
{
    public AudioSource audioSource_click;
   
    private string tagOfHittedObject = null;
    private OVRInput.Controller currentController;
    private bool triggerDown = false;
    private bool hadPlay = false;
    private bool startLoadScene = false;
    private bool startTimer = false;
    private float timer = 0.0f;

    private void Awake()
    {
        PointerEvents.OnPointerUpdateForObject += LoadScenes;
        PointerEvents.OnPointerUpdateForObject += UpdateSound_click;
        ControllerEvents.OnControllerSource += UpdateController;
    }

    private void OnDestroy()
    {
        PointerEvents.OnPointerUpdateForObject -= LoadScenes;
        PointerEvents.OnPointerUpdateForObject -= UpdateSound_click;
        ControllerEvents.OnControllerSource -= UpdateController;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
           OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            triggerDown = true;
        }
        // check for Trigger up
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) ||
            OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            triggerDown = false;
        }

        Timer();
    }

    private void Timer()
    {
        if (startTimer)
        {
            timer = timer + 0.1f;
        }

        if (timer > 3.5f)
        {
            startLoadScene = true;
            timer = 0.0f;
        }
    }

    private void UpdateController(OVRInput.Controller controller, GameObject controllerObject)
    {
        currentController = controller;
    }

    private void LoadScenes(GameObject hitObject)
    {
        tagOfHittedObject = hitObject.tag;

        if (hitObject && triggerDown)
        {
            startTimer = true;
        }

        if (tagOfHittedObject == "tutorial" && startLoadScene)
        {
            SceneManager.LoadScene("Tutorial");
            startLoadScene = false;
        }
        else if (tagOfHittedObject == "level1" && startLoadScene)
        {
            SceneManager.LoadScene("Level1");
            startLoadScene = false;
        }
        else if (tagOfHittedObject == "level2" && startLoadScene)
        {
            SceneManager.LoadScene("Level2");
            startLoadScene = false;
        }
        else if (tagOfHittedObject == "backToMainMenu" && startLoadScene)
        {
            SceneManager.LoadScene("Main Menu");
            startLoadScene = false;
        }
    }

    private void UpdateSound_click(GameObject hitObject)
    {
        if(hitObject && triggerDown & !hadPlay)
        {
            audioSource_click.Play();
            OVRInput.SetControllerVibration(0.1f, 0.1f, currentController);
            hadPlay = true;
        }
        else if(!triggerDown)
        {
            hadPlay = false;
            OVRInput.SetControllerVibration(0f, 0f, currentController);
        }
    }
}
