using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBasicAIController : AIController
{
    public Pawn pawnTakeDamage;
    public float attackRange = 1;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update(); 
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //if (health.currentHealth <= health.maxHealth / 2f)
        //{
        //    ChangeState(AIState.Flee);
        //}
    }

    public override void MakeDecisions() // makes decisions, changed to virtual for personalities, changes based on state requirements
    { // ALL OF THIS NEEDS TO CHANGE, make generic, personalities after
        switch (currentState)
        {            
            case AIState.Chase: // when moving to chase state, mark tank, chase them, shoot them until outside of 10 range               
                if (target == null)
                {
                    return;
                }

                if (IsDistanceLessThan(target, attackRange))
                {
                    //Controller controller = GetComponent<Controller>(); 
                    Melee();
                }
                else
                {
                    Seek(target);
                }
                break;

            case AIState.Flee: // run away if <= half life     - will update this later if i have time            
                Flee();
                // Check for transitions
                if (!IsDistanceLessThan(target, 20))
                {
                    ChangeState(AIState.Chase);
                }
                break;
        }
    }
}
