using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerController : Controller
{
    // KeyCodes, or keyboard inputs from player, are here
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode rotateClockwiseKey;
    public KeyCode rotateCounterClockwiseKey;
    public KeyCode primaryShoot;
    public KeyCode secondaryAttack;
    public KeyCode changeGears;


    // Start is called before the first frame update
    public override void Start() // this is the gamemanager, basically saying if there is an instance of this gamemanager up when the level is created
                                 // and players list exist, add this playercontroller (multiplayer), otherwise ignore
    {
        // If we have a GameManager
        if (GameManager.instance != null)
        {
            // And it tracks the player(s)
            if (GameManager.instance.players != null)
            {
                // Register with the GameManager
                GameManager.instance.players.Add(this);
            }
        }
        // Run the Start() function from the parent (base) class
        base.Start();
    }

    // Update is called once per frame
    public override void Update() // update inputs from keyboard, parent update function
    {
        ProcessInputs();
        base.Update();
    }

    public void ProcessInputs() // this is where all the keyboard presses go, most reference pawn's parent class and then applies the public
                                // variable (mostly movement and rotation)
    {
        if (Input.GetKey(moveForwardKey))
        {
            pawn.MoveForward();
        }

        if (Input.GetKey(moveBackwardKey))
        {
            pawn.MoveBackward();
        }

        if (Input.GetKey(rotateClockwiseKey))
        {
            pawn.RotateClockwise();
        }

        if (Input.GetKey(rotateCounterClockwiseKey))
        {
            pawn.RotateCounterClockwise();
        }

        if (Input.GetKey(primaryShoot)) // function to shoot with cooldown
        {
            pawn.Shoot();            
        }

        if(Input.GetKeyDown(secondaryAttack))
        {
            pawn.Melee();
        }

        if (Input.GetKeyDown(changeGears)) // function to change moveSpeed by either half, normal, or double
        {
            //pawn.ChangeGears();
        }
    }

    public void OnDestroy() // if destroyed AND there is a gamemanager instance up AND there is a list of players, remove me - otherwise ignore
    {
        // If we have a GameManager
        if (GameManager.instance != null)
        {
            // And it tracks the player(s)
            if (GameManager.instance.players != null)
            {
                // Deregister with the GameManager
                GameManager.instance.players.Remove(this);
            }
        }
    }

    public override void UpdateScore()
    {
        if (scoreVariableText != null)
        {
            scoreVariableText.text = "Score: " + score.ToString();
        }
        else
        {
            Debug.Log("ScoreVariable text is null!");
        }
    }
}
