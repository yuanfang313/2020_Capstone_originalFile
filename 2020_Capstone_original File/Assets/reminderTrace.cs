﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reminderTrace : MonoBehaviour
{
    public float speed;
    private Transform rightAnswer;

    private Vector3 target;
    
    // Start is called before the first frame update
    void Start()
    {
        rightAnswer = GameObject.FindGameObjectWithTag("rightAnswer").transform;
        target = new Vector3(rightAnswer.position.x, rightAnswer.position.y, rightAnswer.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, rightAnswer.position, speed * Time.deltaTime);
        if (transform.position.x == target.x && transform.position.y == target.y && transform.position.z == target.z)
        {
            DestroyReminderObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "rightAnswer")
        {
            DestroyReminderObject();
        }
    }
    private void DestroyReminderObject()
    {
        Destroy(gameObject);
    }
}
