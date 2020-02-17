using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointerEvents : MonoBehaviour
{
    public float Distance = 10.0f;
    public LineRenderer LineRenderer = null;

    public LayerMask EverythingMask = 0;
    public LayerMask InteractableMask = 0;

    public static UnityAction<Vector3, GameObject> OnPointerUpdate = null;
    public static UnityAction<GameObject> OnPointerUpdateForObject = null;
    

    private Transform CurrentOrigin = null;
    private Vector3 endPosition;
    private GameObject currentObject = null;

    private void Awake()
    {
        ControllerEvents.OnControllerSource += UpdateOrigin;
    }

    private void OnDestroy()
    {
        ControllerEvents.OnControllerSource -= UpdateOrigin;
    }

    private void Start()
    {
        SetLineColor();
    }

    private void Update()
    {
        Vector3 hitPoint = UpdateLine();//current position of endPosition
        currentObject = UpdatePointerStatus();//current gameObject being hitted
           
        //sent out OnPointerUpdate
        if (OnPointerUpdate != null)
            OnPointerUpdate(hitPoint, currentObject);

        //sent out OnPointerUpdateForObject
        if (OnPointerUpdateForObject != null)
            OnPointerUpdateForObject(currentObject);
    }

    private Vector3 UpdateLine()
    {
        //Create ray
        RaycastHit hit = CreateRaycast(EverythingMask);

        //if didn't hit (default end)
        endPosition = CurrentOrigin.position + (CurrentOrigin.forward * Distance);

        //if hit
        if (hit.collider != null)
            endPosition = hit.point;

        //set position
        LineRenderer.SetPosition(0, CurrentOrigin.position);
        LineRenderer.SetPosition(1, endPosition);

        return endPosition;
    }

    //return gameObject in the "Interactable" layer which are navigating elements
    public GameObject UpdatePointerStatus()
    {
        //create ray
        RaycastHit hit = CreateRaycast(InteractableMask);

        //check hit
        if (hit.collider)
            return hit.collider.gameObject;

        //return
        return null;
    }

    private void UpdateOrigin(OVRInput.Controller controller, GameObject controllerObject)
    {
        //Set origin of pointer
        CurrentOrigin = controllerObject.transform;
        //Is the laser visible?
    }

    private RaycastHit CreateRaycast(int layer)
    {
        RaycastHit hit;
        Ray ray = new Ray(CurrentOrigin.position, CurrentOrigin.forward);
        Physics.Raycast(ray, out hit, Distance, layer);
        return hit;
    }

    private void SetLineColor()
    {
        if (!LineRenderer)
            return;

        Color endColor = Color.white;
        endColor.a = 0.0f;
        LineRenderer.endColor = endColor;
    }
}
