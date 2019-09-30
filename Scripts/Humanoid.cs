using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animation))]

public class Humanoid : MonoBehaviour {

    NavMeshAgent agent;
    Transform target;
    Transform targetCar;
    Animation anim;

    public float speed;
    public float sightDistance;
    public float sightDistanceCar;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Bench").transform; //sets bench as target

        targetCar = GameObject.FindGameObjectWithTag("Car").transform; //sets anything with car tag as targets

        anim = GetComponent<Animation>();

        StartCoroutine(UpdatePath());
    }

    void Update()
    {

        if (Vector3.Distance(transform.position, targetCar.position) < sightDistanceCar) //if the car is in distance
        {
            agent.Stop(); //stop the humanoid
            anim.Play("Look"); //stop the animation
            StartCoroutine(Wait()); //make it wait 2 seconds
        }
        else
        {
            agent.Resume(); //humanoid carries on walking
            anim.Play("Walking"); //resume the animation
        }

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
    }


    IEnumerator UpdatePath() //goes through this loop and repeats it depending on the refresh rate, better performance
    {
        float refreshRate = 0.25f;

        while (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z); 
            agent.SetDestination(targetPosition); //sets destination as bench position
            yield return new WaitForSeconds(refreshRate);
        }

        if (Vector3.Distance(transform.position, target.position) < sightDistance) //if bench is within sight distance
        {
            var relativePos = target.position - transform.position;

            var rotation = Quaternion.LookRotation(relativePos);

            transform.rotation = rotation; //Humanoid rotates towards target (bench)
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

    }
}

//CODE FOR OLD WAYPOINTS FOR MODEL

//public Transform pathHolder;
//public float speed = 10f;
//public float pathWaitTime = 0.3f;


//Vector3[] waypoints = new Vector3[pathHolder.childCount];
//for (int i = 0; i < waypoints.Length; i++)
//{
//    waypoints[i] = pathHolder.GetChild(i).position;
//    waypoints[i] = new Vector3(waypoints[i].x, waypoints[i].y, waypoints[i].z); //keeps humanoid above ground
//}

//StartCoroutine(FollowPath(waypoints));


//IEnumerator FollowPath(Vector3[] waypoints)
//{
//    transform.position = waypoints[0];

//    int targetWaypointIndex = 1;
//    Vector3 targetWaypoint = waypoints[targetWaypointIndex];
//    transform.LookAt(targetWaypoint);

//    while (true)
//    {
//        var relativePos = targetWaypoint - transform.position;
//        var rotation = Quaternion.LookRotation(relativePos);

//        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime); //moves the humanoid to it's target waypoint from current position
//        if (transform.position == targetWaypoint) //if it reaches waypoint
//        {
//            targetWaypointIndex = (targetWaypointIndex + 1); 
//            targetWaypoint = waypoints[targetWaypointIndex];

//            yield return new WaitForSeconds(pathWaitTime);
//        }
//        yield return null;
//    }
//}

//void OnDrawGizmos()
//{
//    Vector3 startPosition = pathHolder.GetChild(0).position;
//    Vector3 previousPosition = startPosition;

//    foreach (Transform waypoint in pathHolder)
//    {
//        Gizmos.DrawSphere(waypoint.position, 0.5f); //draws spheres at waypoint positions
//        Gizmos.DrawLine(previousPosition, waypoint.position); //puts lines in between them
//        previousPosition = waypoint.position; //makes previous point the waypoint position
//    }
//}
