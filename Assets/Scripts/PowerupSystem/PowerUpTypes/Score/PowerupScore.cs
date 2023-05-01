using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class PowerupScore : Powerup // this allows you to look at the powerup manager and just apply this as if it was a regular
                                    // powerup ability, however, this is a way to grant points without reworking the code
{
    public float scoreToAdd; // new variable to store the score to add


    public override void Apply(PowerupManager target)
    {
        // add score to the player
        Controller controller = target.GetComponentInChildren<Controller>();
        if (controller != null)
        {
            controller.score += scoreToAdd;
            controller.UpdateScore();
        }

    }

    public override void Remove(PowerupManager target)
    {
        // remove health changes
    }
}
