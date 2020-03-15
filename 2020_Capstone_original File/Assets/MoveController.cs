using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private Rigidbody rbController;
    public float speed;
    private Vector3 moveVelocity;


    void Start()
    {
        rbController = GetComponent<Rigidbody>();
    }


    void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveVelocity = moveInput.normalized * speed;
    }
    private void FixedUpdate()
    {
        rbController.MovePosition(rbController.position + moveVelocity * Time.deltaTime);
    }
}
