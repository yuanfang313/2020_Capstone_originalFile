using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightEvents : MonoBehaviour
{
    //public GameObject highlightCollider;
    public AudioSource audio_target;
    //private MeshRenderer renderer;
    //private bool showCollider = true;
    private bool hadHit = false;


    private void Awake()
    {
        PointerEvents.OnPointerUpdateForTarget += UpdateSound_target;
    }

    private void Start()
    {
        //renderer = highlightCollider.GetComponent<MeshRenderer>();
        //renderer.enabled = !showCollider;
    }

    private void OnDestroy()
    {
        PointerEvents.OnPointerUpdateForTarget -= UpdateSound_target;
    }

    private void UpdateSound_target(GameObject hitTarget)
    {
        if (hitTarget && !hadHit)
        {
            audio_target.Play();
            hadHit = true;
        }
        else if (!hitTarget)
        {
            hadHit = false;
        }
    }


   /* private void ShowObject (GameObject hitTarget)
    {
       if (hitTarget)
        {
           renderer.enabled = showCollider;
        } else if (!hitTarget)
        {
           renderer.enabled = !showCollider;
        }
    }*/


}
