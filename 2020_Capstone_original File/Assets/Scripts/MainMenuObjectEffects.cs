using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuObjectEffects : MonoBehaviour
{
    public Pointer Pointer;
    public Transform cube1;
    public Transform cube2;
    public Transform cube3;
    public AudioSource audioSource_hover;
    
    private Vector3 originalScale = new Vector3(0.5f, 0.5f, 0.5f);
    bool hadHit = false;


    private void Awake()
    {
        Pointer.OnPointerUpdateForObject += UpdateCube;
        Pointer.OnPointerUpdateForObject += UpdateSound_hover;
        
    }

    private void OnDestroy()
    {
        Pointer.OnPointerUpdateForObject -= UpdateCube;
        Pointer.OnPointerUpdateForObject -= UpdateSound_hover;
        //Pointer.OnPointerUpdate -= LoadScenes;
    }

    private void Update()
    {
       //if (OVRInput.GetActiveController() == OVRInput.Controller.None)
            //return;
    }

    private Transform CheckCube()
    {
        Transform cubeTransform = Pointer.UpdatePointerStatus().GetComponent<Transform>();
        return cubeTransform;
    }

    //change the scale of MainMenu Interactable Object
    private void UpdateCube(GameObject hitObject)
    {
        if (hitObject)
        {
            CheckCube().localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        else
        {
            cube1.localScale = originalScale;
            cube2.localScale = originalScale;
            cube3.localScale = originalScale;
        }
    }

    //sound when the pointer hover over the MainMenu Interactable Object 
    private void UpdateSound_hover(GameObject hitObject)
    {
        if (hitObject && !hadHit)
        {
            audioSource_hover.Play();
            hadHit = true;
        }
        else if (!hitObject)
        {
            hadHit = false;
        }
    }
}
