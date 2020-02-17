using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticuleEvents : MonoBehaviour
{
    public SpriteRenderer CircleRenderer;
    public Sprite OpenSprite;
    public Sprite CloseSprite;

    private Camera Camera = null;

    private void Awake()
    {
       PointerEvents.OnPointerUpdate += UpdateSprite;
        Camera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(Camera.gameObject.transform);
    }

    private void OnDestroy()
    {
        PointerEvents.OnPointerUpdate -= UpdateSprite;
    }

    // sprite effects
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
