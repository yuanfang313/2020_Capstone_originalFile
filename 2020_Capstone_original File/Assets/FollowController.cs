using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowController : MonoBehaviour
{
    public float speed;
   
    public float stopingDistance;
    public float retreatDistance;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject projectile;
    private Transform controller;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("controller").transform;
        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    { 

        /*if (Vector3.Distance(transform.position, controller.position) > stopingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, controller.position, speed * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, controller.position) < stopingDistance 
            && Vector3.Distance(transform.position, controller.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector3.Distance(transform.position, controller.position) < retreatDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, controller.position, -speed * Time.deltaTime);
        }*/

        if(timeBtwShots <= 0)
        {
            Instantiate(projectile, controller.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        } else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
