using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followWayPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    private SpriteRenderer sprite;

    private float speed = 2f;
    public bool parar;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        parar = false;
    }

    private void Update()
    {
        if (!parar) { 
        //si la distancia es menor que .1 cambia el waypoint hacia el que se mueve
            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
            {
                sprite.flipX = !sprite.flipX;
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
        }
    }
}
