using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class playerEvent : MonoBehaviour
{
    #region Events
    public static UnityAction<OVRInput.Controller, GameObject> OnControllerSource = null;

    #endregion

    #region Anchors
    public GameObject LeftAnchor;
    public GameObject RightAnchor;
    public GameObject HeadAnchor;
    #endregion

    #region Input
    private Dictionary<OVRInput.Controller, GameObject> ControllerSets = null;
    private OVRInput.Controller InputSource = OVRInput.Controller.None;//detect where our input come from last
    private OVRInput.Controller Controller = OVRInput.Controller.None;//detect whatever the active controller state is
    private bool InputActive = true;//detect whether the headset is currently attached to somebody's face
    #endregion

    private bool changeController = true;

    private void Awake()
    {
        OVRManager.HMDMounted += PlayerFound;
        OVRManager.HMDUnmounted += PlayerLost;

        ControllerSets = CreateControllerSets();
    }

    private void OnDestroy()
    {
        OVRManager.HMDMounted -= PlayerFound;
        OVRManager.HMDUnmounted -= PlayerLost;
    }

    private void Update()
    {
        //check for active input(if the user has the headset on)
        if (!InputActive)
            return;

        //check if a controller exists
        CheckForController();

        //check for input source
        CheckControllerInput();

        //check for actural input

    }

    private void CheckForController()
    {
        OVRInput.Controller controllerCheck = Controller;

        //right controller
        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
            controllerCheck = OVRInput.Controller.RTouch;

        //left controller
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch))
            controllerCheck = OVRInput.Controller.LTouch;

        //left & right controller
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch) ||
           OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
        {
            if (!changeController)
            {
                controllerCheck = OVRInput.Controller.LTouch;
            }
            else if (changeController)
            {
                controllerCheck = OVRInput.Controller.RTouch;
            }      
        }

        // Update
        Controller = UpdateSource(controllerCheck, Controller);

    }

    private void CheckControllerInput()
    {
        OVRInput.Controller controllerInputCheck = InputSource;

        if (OVRInput.GetDown(OVRInput.Button.Any, OVRInput.Controller.LTouch))
        {
            changeController = false;
        }
        else if (OVRInput.GetDown(OVRInput.Button.Any, OVRInput.Controller.RTouch))
        {
            changeController = true;
        }

        controllerInputCheck = OVRInput.GetActiveController();
    
        //Update
        InputSource = UpdateSource(controllerInputCheck, InputSource);
    }

    private OVRInput.Controller UpdateSource(OVRInput.Controller check, OVRInput.Controller previous)
    {
        //if values are the same, return
        if (check == previous)
            return previous;

        //get controller object
        GameObject controllerObject = null;
        ControllerSets.TryGetValue(check, out controllerObject);

        //sent out event
        if (OnControllerSource != null)
            OnControllerSource(check, controllerObject);
        
        return check;
    }

    private void PlayerFound()
    {
        InputActive = true;
    }

    private void PlayerLost()
    {
        InputActive = false;
    }

    private Dictionary<OVRInput.Controller, GameObject> CreateControllerSets()
    {
        Dictionary<OVRInput.Controller, GameObject> newSets = new Dictionary<OVRInput.Controller, GameObject>()
        {
            {OVRInput.Controller.LTouch, LeftAnchor},
            {OVRInput.Controller.RTouch, RightAnchor}
        };
        return newSets;
    }
}
