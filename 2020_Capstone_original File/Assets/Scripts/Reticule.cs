using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticule : MonoBehaviour
{
    public Pointer Pointer;
    public SpriteRenderer CircleRenderer; 
    public Sprite OpenSprite;
    public Sprite CloseSprite;

    private Camera Camera = null;


    private void Awake()
    {
        Pointer.OnPointerUpdate += UpdateSprite;
        Camera = Camera.main;   
    }

    private void Update()
    {
        transform.LookAt(Camera.gameObject.transform);
    }

    private void OnDestroy()
    {
        Pointer.OnPointerUpdate -= UpdateSprite;
    }

    //sprite effects
    private void UpdateSprite(Vector3 point, GameObject hitObject)
    {
        transform.position = point;
        if (hitObject)
        {
            CircleRenderer.sprite = CloseSprite;
        }
        else
        {
            CircleRenderer.sprite = OpenSprite;
        }
    }







}
