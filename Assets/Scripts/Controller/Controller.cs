using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

[System.Serializable]

public class Controller : MonoBehaviour
{

    public Pawn pawn; // variable to reference pawn parent class
    public float score;
    public int lives; // might not use

    public Canvas playerInterfaceCanvas;
    public TextMeshProUGUI scoreVariableText;

    // Start is called before the first frame update
    public virtual void Start()
    {
        // when I get to scoring
        
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            return; // skip the rest of the method if not a PlayerController
        }

        playerInterfaceCanvas = transform.parent.Find("PlayerInterface").GetComponent<Canvas>();
        scoreVariableText = playerInterfaceCanvas.transform.Find("ScoreVariable").GetComponent<TextMeshProUGUI>();

        if (scoreVariableText == null)
        {
            return;
        }
        else if (playerInterfaceCanvas == null)
        {
            return;
        }
        else
        {
            Debug.Log("Found the scorevariable and canvas");
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void UpdateScore()
    {

    }


}
