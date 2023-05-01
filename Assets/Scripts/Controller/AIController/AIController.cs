using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class AIController : Controller
{
    public Health health;


    public enum AIState { Guard, Chase, Flee, Patrol, Idle, Escort }; // all the states for the ai and the variables representing them
    private float lastStateChangeTime; // no idea what this is for, never used it
    public AIState currentState; // allows the ai to change states

    public GameObject target;
    public float fleeDistance;
    public float hearingDistance;
    public float fieldOfView;
    protected bool canSeeBool;
    protected bool canHearBool;

    public Transform[] waypoints; // variables for the waypoint system used by patrol, guard, and escort
    public float waypointStopDistance; // how far away from the waypoint before it "counts" and move to next waypoint
    private int currentWaypoint = 0; // need to initialize first waypoint (or initialize the first of every array with something)
    public enum PatrolLoop { Loop, Stop }; // two options for patrol, loop or not
    public PatrolLoop patrolLoop;
    //public Boolean isLooping;

    // Start is called before the first frame update
    public override void Start() // get health component, change ai immediately to idle, target nearest tank (to avoid null, find tank can change this during)
    {
        if (GameManager.instance != null)
        {
            // And it tracks the player(s)
            if (GameManager.instance.ai != null)
            {
                // Register with the GameManager
                GameManager.instance.ai.Add(this);
            }
        }
        base.Start();
        health = GetComponent<Health>();
        TargetNearestPlayer();
    }

    // Update is called once per frame
    public override void Update() // make decisions per frame
    {
        base.Update();
        MakeDecisions();    
    }

    public virtual void FixedUpdate() // fixed update for other stuff in the personalities, if needed
    {
        // add code here - if player does not exist, do nothing
        TargetNearestPlayer();
    }

    public virtual void MakeDecisions() // makes decisions, changed to virtual for personalities, changes based on state requirements
    { // ALL OF THIS NEEDS TO CHANGE, make generic, personalities after
        switch (currentState)
        {
            //change this to always seek for basic units, seek up til a range for ranged units, explode within 2f if exploder
            case AIState.Idle: // idle, do nothing, as long as nothing is near them and they dont have waypoints
                if (IsDistanceLessThan(target, 10))
                {
                    ChangeState(AIState.Chase);
                }
                else
                {
                    if (waypoints == null) // idle
                    {
                        DoIdleState();
                    }
                    else if (waypoints.Length >= 1) // guard, same waypoint system, but only having 1 on the list
                    {
                        ChangeState(AIState.Guard);
                    }
                    else // patrol, same waypoint system, but having more than 1 on the list
                    {
                        ChangeState(AIState.Patrol);
                    }
                }
                break;
            case AIState.Patrol: // patrol back and forth between waypoints
                Patrol();
                break;
            case AIState.Chase: // when moving to chase state, mark tank, chase them, shoot them until outside of 10 range
                // Do work
                TargetNearestPlayer();
                DoChaseState();
                Shoot();
                // Check for transitions
                if (!IsDistanceLessThan(target, 10))
                {
                    ChangeState(AIState.Idle);
                }
                break;
            case AIState.Flee: // run away if <= half life
                // Do work 
                Flee();
                // Check for transitions
                if (!IsDistanceLessThan(target, 20))
                {
                    ChangeState(AIState.Idle);
                }
                break;
            case AIState.Guard: // will guard 1 waypoint
                Guard(); //reusing patrol code to have tank "Guard" this location
                break;
            case AIState.Escort: // will follow the targets allyFollowTransform (right side of prefab)
                Escort();
                break;
        }
    }
    public virtual void ChangeState(AIState newState) // allows ai to change states based on each state
    {
        // Change the current state
        currentState = newState;
        // Save the time when we changed states
        lastStateChangeTime = Time.time; // <-- useless, never used it, might delete it later

    }

    protected virtual void DoChaseState() // allows you to chase/seek without switching states 
    {
        Seek(target);
    }
    protected virtual void DoIdleState() // makes ai do nothing to represent idle, allows you do idle without switching states
    {
        // do nothing
    }
    public bool IsDistanceLessThan(GameObject target, float distance) // checks range of this vs target, returns result
    {
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected virtual void DoAttackState() // allows to attack without swapping states
    {
        // Chase
        Seek(target);
        // Shoot
        Shoot();
    }
    protected void Flee() // if below half life, will run away, this is part 1 of the code that takes the differences between the two and reverses it
    {

        // Find the Vector to our target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;
        // Find the Vector away from our target by multiplying by -1
        Vector3 vectorAwayFromTarget = -vectorToTarget;
        // Find the vector we would travel down in order to flee
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance;

        // Seek the point that is "fleeVector" away from our current position
        FleeInvert(fleeVector);
        //Seek(pawn.transform.position + fleeVector);

    }
    protected void Guard() // will always go back to this waypoint even after chase, requires 1 waypoint only
    {
        if (IsDistanceLessThan(target, 10))
        {
            ChangeState(AIState.Chase);
        }
        else
        {
            if (waypoints.Length > currentWaypoint)
            {
                // Then seek that waypoint
                Seek(waypoints[currentWaypoint]);
                // If we are close enough, then increment to next waypoint
            }
        }
    }
    protected void Escort() // follows playertank as long as they are close enough
    {
        if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypointStopDistance) // stops following if within 2 of the transform on the player tank
        {
            DoIdleState();
        }
        else if (IsDistanceLessThan(target, 10)) // if within 10 of playertank, go to the allytransform spot on tank, then above code when within range
        {
            Seek(waypoints[currentWaypoint]);
        }
    }
    protected void Patrol() // two codes, both patrol multiple waypoints but difference is if you want to repeat the patrolling
    {

        switch (patrolLoop)
        {
            case PatrolLoop.Loop:
                if (IsDistanceLessThan(target, 10)) // during patrol, if enemy comes near, chase
                {
                    ChangeState(AIState.Chase);
                }
                else
                {
                    if (waypoints.Length > currentWaypoint) // no enemies, continue patrolling
                    {
                        // Then seek that waypoint
                        Seek(waypoints[currentWaypoint]);
                        // If we are close enough, then increment to next waypoint
                        if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypointStopDistance)
                        {
                            currentWaypoint++;
                        }
                    }
                    else
                    {
                        RestartPatrol();
                    }
                }
                break;
            case PatrolLoop.Stop:
                if (IsDistanceLessThan(target, 10))
                {
                    ChangeState(AIState.Chase);
                }
                else
                {
                    if (waypoints.Length > currentWaypoint)
                    {
                        // Then seek that waypoint
                        Seek(waypoints[currentWaypoint]);
                        // If we are close enough, then increment to next waypoint
                        if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypointStopDistance)
                        {
                            currentWaypoint++;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                break;
        }
    }
    protected void RestartPatrol() // on loop for Patrol(), will reset it to 0  so it can do it again
    {
        // Set the index to 0
        currentWaypoint = 0;
    }
    protected bool IsHasTarget() // checks to see if you have a target
    {
        // return true if we have a target, false if we don't
        return (target != null); // by doing null, it will always return something as long as its not null
    }


    // ---- below is non-protected functions

    public void Shoot() // tell the pawn (player or ai) to shoot
    {
        // Tell the pawn to shoot
        pawn.Shoot();

    }

    public void Melee()
    {
        pawn.Melee();
    }
    public void FleeInvert(Vector3 fleeVector) // inverts the flee code to make it closer = run farther, far = run shorter
    {
        float targetDistance = Vector3.Distance(target.transform.position, pawn.transform.position);
        float percentOfFleeDistance = targetDistance / fleeDistance;
        percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);
        float flippedPercentOfFleeDistance = 1 - percentOfFleeDistance;
        Vector3 adjustedFleeVector = fleeVector * flippedPercentOfFleeDistance;
        Seek(pawn.transform.position + adjustedFleeVector);
    }
    public void Seek(GameObject target) // moves the pawn (player or AI) forward target
    {
        // RotateTowards the Funciton
        pawn.RotateTowards(target.transform.position);
        // Move Forward
        pawn.MoveForward();
    }
    public void Seek(Pawn targetPawn) // never used it, never worked during schoolwork, keeping it up
    {
        Seek(targetPawn.transform);
    }
    public void Seek(Transform targetTransform) // get the transform of the pawn/gameobject ur trying to target
    {
        // Seek the position of our target Transform
        Seek(targetTransform.position);

    }
    public void Seek(Vector3 targetPosition) // get the position of the pawn/gameobject target from their transform, moves toward it
    {
        // RotateTowards the Function
        pawn.RotateTowards(targetPosition);
        // Move Forward
        pawn.MoveForward();
    }
    public void TargetPlayerOne() // target first player in the gamemanager
    {
        // If the GameManager exists
        if (GameManager.instance != null)
        {
            // And the array of players exists
            if (GameManager.instance.players != null)
            {
                // And there are players in it
                if (GameManager.instance.players.Count > 0)
                {
                    //Then target the gameObject of the pawn of the first player controller in the list
                    target = GameManager.instance.players[0].pawn.gameObject;
                }
            }
        }
    }

    // update: might remove this, might just use TargetPlayerOne instead
    protected void TargetNearestPlayer() // gets all of the pawn tanks in the map, then ignores itself while filtering through the closest target and apply to variable
    {
        // Find the first PlayerController instance
        PlayerController player = FindObjectOfType<PlayerController>();

        // Set the target to the player's pawn
        if (player != null)
        {
            target = player.pawn.gameObject;
        }
        else
        {
            return;
        }
    }




    /*public bool CanHear(GameObject target) // NO IDEA -- losing points
    {
        // Get the target's NoiseMaker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();
        // If they don't have one, they can't make noise, so return false
        if (noiseMaker == null)
        {
            canHearBool = false;
            return false;
        }
        // If they are making 0 noise, they also can't be heard
        if (noiseMaker.volumeDistance <= 0)
        {
            canHearBool = false;
            return false;
        }

        // If they are making noise, add the volumeDistance in the noisemaker to the hearingDistance of this AI
        float totalDistance = noiseMaker.volumeDistance + hearingDistance;
        // If the distance between our pawn and target is closer than this...
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
        {
            // then we can hear the target
            canHearBool = true;
            return true;

        }
        else
        {
            // Otherwise, we are too far away to hear them
            canHearBool = false;
            return false;
        }
    }



    protected bool CanSee(GameObject target)
    {
        // Check if target is within the hearing distance
        if (IsDistanceLessThan(target, hearingDistance))
        {
            // Calculate direction of the target
            Vector3 directionToTarget = target.transform.position - transform.position;

            // Check if target is within the field of view
            if (Vector3.Angle(transform.forward, directionToTarget) < fieldOfView / 2f)
            {
                // Check if there are any obstacles between the AI and the target
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, directionToTarget.magnitude);

                if (hit.collider != null && hit.collider.gameObject != target)
                {
                    // If there is an obstacle, then the AI cannot see the target
                    canSeeBool = false;
                }

                // If there are no obstacles, then the AI can see the target
                canSeeBool = true;
                return true;
            }
        }
        canSeeBool = false;
        // If the target is not within the hearing distance or field of view, then the AI cannot see the target
        return false;
    }*/

}