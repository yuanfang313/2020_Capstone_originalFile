using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public AudioSource audioSource_click;

    private string tagOfhitObject = null;
    private bool triggerDown = false;
    private bool hadPlay = false;
    private bool startTimer = false;
    //private bool hadhit = false;
    private bool startLoadScene = false;
    private float timer = 0.0f;

    private void Awake()
    {
        Pointer.OnPointerUpdateForObject += LoadScenes;
        //Pointer.OnPointerUpdateForObject += hadHit;
        Pointer.OnPointerUpdateForObject += UpdateSound_click;
        //playerEvent.onTriggerDown += PressTrigger;
        //playerEvent.onTriggerUp += ReleaseTrigger;

    }

    private void OnDestroy()
    {
        Pointer.OnPointerUpdateForObject -= LoadScenes;
        //Pointer.OnPointerUpdateForObject -= hadHit;
        Pointer.OnPointerUpdateForObject -= UpdateSound_click;
        //playerEvent.onTriggerDown -= PressTrigger;
        //playerEvent.onTriggerUp -= ReleaseTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        //if (OVRInput.GetActiveController() == OVRInput.Controller.None)
        //return;
        

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)||
           OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            triggerDown = true;
        }

        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger)||
           OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            triggerDown = false;
        }

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

   /*private void hadHit(GameObject hitObject)
    {
        if (hitObject)
        {
            hadhit = true;
        }    
        else if (!hitObject)
        {
            hadhit = false;
        }
            
    }*/

    private void LoadScenes(GameObject hitObject)
    {
       
        tagOfhitObject = hitObject.tag;

        if (hitObject && triggerDown)
        {
            startTimer = true;
        }

        if (tagOfhitObject == "tutorial" && startLoadScene)
        {
            SceneManager.LoadScene("Tutorial");
            startLoadScene = false;
        }
        else if (tagOfhitObject == "level1" && startLoadScene)
        {
            SceneManager.LoadScene("Level1");
            startLoadScene = false;
        }
        else if (tagOfhitObject == "level2" && startLoadScene)
        {
            SceneManager.LoadScene("Level2");
            startLoadScene = false;
        }
        else if (tagOfhitObject == "backToMainMenu" && startLoadScene)
        {
            SceneManager.LoadScene("Main Menu");
            startLoadScene = false;
        }
    }

    private void UpdateSound_click(GameObject hitObject)
    {
        if (hitObject && triggerDown & !hadPlay)
        {
            audioSource_click.Play();
            hadPlay = true;
        }
        else if(!triggerDown)
        {
            hadPlay = false;
        }
    }

   

}
