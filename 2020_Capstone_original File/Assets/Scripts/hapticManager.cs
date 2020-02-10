using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hapticManager : MonoBehaviour
{
    public static hapticManager singleton;
    
    // Start is called before the first frame update
    void Start()
    {
        if (singleton && singleton != this)
        {
            Destroy(this);
        }  
        else
        {
            singleton = this;
        }     
    }

    public void Vibration(AudioClip vibrationAudio, OVRInput.Controller controller)
    {
        OVRHapticsClip clip = new OVRHapticsClip(vibrationAudio);
        if(controller == OVRInput.Controller.LTouch)
        {
            OVRHaptics.LeftChannel.Preempt(clip);
        }
        else if (controller == OVRInput.Controller.RTouch)
        {
            OVRHaptics.RightChannel.Preempt(clip);
        }
    }
}
