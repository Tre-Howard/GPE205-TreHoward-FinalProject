using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

using Toggle = UnityEngine.UI.Toggle;

public class ButtonPress : MonoBehaviour
{
    public GameObject motdToggleButton;

    public void Start()
    {
        

    }

    public void StartGame()
    {
        if (GameManager.instance != null && GameManager.instance.audioClips.Length > 0)
        {
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.audioClips[0]);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.StartGame();
        }
    }

    public void ChangeToMainMenu()
    {
        if (GameManager.instance != null && GameManager.instance.audioClips.Length > 0)
        {
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.audioClips[0]);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.ChangeState(GameManager.MenuState.MainMenu);
        }
    }

    public void ChangeToTitleScreen()
    {
        if (GameManager.instance != null && GameManager.instance.audioClips.Length > 0)
        {
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.audioClips[0]);
        }

        if (GameManager.instance != null)
        {            
            GameManager.instance.ActivateTitleScreen();
        }
    }

    public void ChangeToOptionsScreen()
    {
        if (GameManager.instance != null && GameManager.instance.audioClips.Length > 0)
        {
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.audioClips[0]);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.ActivateOptionsScreen();
        }
    }

    public void ChangeToCreditsScreen()
    {
        if (GameManager.instance != null && GameManager.instance.audioClips.Length > 0)
        {
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.audioClips[0]);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.ActivateCreditsScreen();
        }
    }

    public void ChangeToGamePlayScreen()
    {
        if (GameManager.instance != null && GameManager.instance.audioClips.Length > 0)
        {
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.audioClips[0]);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.ActivateGamePlayScreen();
        }
    }

    public void GameOverRestart()
    {
        if (GameManager.instance != null && GameManager.instance.audioClips.Length > 0)
        {
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.audioClips[0]);
        }

        if (GameManager.instance != null)
        {
            //GameManager.instance.ActivateAllStates();
            GameManager.instance.audioSource.Stop();
            //GameManager.instance.ActivateMainMusic();
            SceneManager.LoadScene("Main");
            //GameManager.instance.GetAllStates();
            //GameManager.instance.DeactivateAllStates();
            GameManager.instance.ActivateMainMenuScreen();
            
        }
    }

    public void GameOverExit()
    {
        ExitGame();
    }

    public void ExitGame()
    {
        if (GameManager.instance != null)
        {
            Application.Quit(); // does not work with editor/player
            UnityEditor.EditorApplication.isPlaying = false; // typically u need to remove this if game is official, this is just for editor
        }
    }
}
