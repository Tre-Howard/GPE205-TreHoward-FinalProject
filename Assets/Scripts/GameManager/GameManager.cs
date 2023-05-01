using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public MapGenerator mapGenerator;

    public List<PlayerController> players;
    public List<AIController> ai;
    public int howManyAI;
    private int howManyAICache;

    //public int howManyPlayers; // no longer need these, singleplayer game
    //private int howManyPlayersCache;

    public List<EnemySpawner> enemySpawners;

    public List<GameObject> aiControllerList;
    public List<GameObject> playerPawnPrefabs; //list of prefabs, should only be 1 but open to adding Sentries/turrets and such as "playerobjects"
    public List<GameObject> enemyPawnPrefabs;


    public enum MenuState { TitleScreen, MainMenu, OptionsScreen, InGameOptionsScreen, CreditsScreen, Gameplay, GameOver }; // TURN THIS INTO FSM
    public MenuState currentMenu;
    //game states
    public GameObject TitleScreenStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsScreenStateObject;
    public GameObject InGameOptionsScreenStateObject;
    public GameObject CreditsScreenStateObject;
    public GameObject GameplayStateObject;
    public GameObject GameOverScreenStateObject;

    //audio/sound variables
    public AudioSource audioSource;
    public AudioMixer audioMixer;

    public AudioClip[] backgroundMusic; //reduce this to exactly what you need
    public AudioClip[] audioClips;

    private bool checkInstanceOnLoad = true;

    // Awake is called when the object is first created - before even Start can run!
    private void Awake()
    {
        if (checkInstanceOnLoad == true)
        {
            // If the instance doesn't exist yet...
            CheckInstance();
            checkInstanceOnLoad = false;
        }       

        audioSource = GetComponent<AudioSource>();
        audioMixer.SetFloat("masterVolume", 20.0f); //sets master to 20db

        currentMenu = MenuState.TitleScreen;
        MenuFSM();
    }

    private void Start()
    {
        audioSource.Stop();
        audioSource.clip = backgroundMusic[0];
        audioSource.loop = true;
        audioSource.Play();

        //mapGenerator.OnStartGameButtonPress();
        //FindSpawners();
        //SpawnPlayers();


        //StartCoroutine(StartGameCoroutine());

        ActivateAllStates();
        GetAllStates();
        ActivateTitleScreen();
    }

    private void Update()
    {

    }

    public void MenuFSM()
    {
        switch (currentMenu)
        {
            case MenuState.TitleScreen:
                if (currentMenu == MenuState.TitleScreen)
                {
                    //GetAllStates();                    
                    ActivateTitleScreen();
                }
                break;

            case MenuState.MainMenu:
                if (currentMenu == MenuState.MainMenu)
                {
                    ActivateMainMenuScreen();
                }
                break;

            case MenuState.Gameplay:
                if (currentMenu == MenuState.Gameplay)
                {
                    ActivateGamePlayScreen();
                }
                break;

            case MenuState.OptionsScreen:
                if (currentMenu == MenuState.OptionsScreen)
                {
                    ActivateOptionsScreen();
                }
                break;

            case MenuState.InGameOptionsScreen:
                if (currentMenu == MenuState.InGameOptionsScreen)
                {
                    ActivateInGameOptionsScreen();   
                }
                break;

            case MenuState.CreditsScreen:
                if (currentMenu == MenuState.CreditsScreen)
                {
                    ActivateCreditsScreen();   
                }
                break;

            case MenuState.GameOver:
                if (currentMenu == MenuState.GameOver)
                {
                    ActivateGameOver();
                }
                break;

        }
    }

    public void ChangeState(MenuState menuOption)
    {
        currentMenu = menuOption;
        MenuFSM();
    }


    // GameState/MainMenu Related Code ---------------------------------------------------------
    public void DeactivateAllStates()
    {
        //deactivates all game states
        TitleScreenStateObject.SetActive(false);
        MainMenuStateObject.SetActive(false);
        OptionsScreenStateObject.SetActive(false);
        CreditsScreenStateObject.SetActive(false);
        GameplayStateObject.SetActive(false);
        GameOverScreenStateObject.SetActive(false);
        InGameOptionsScreenStateObject.SetActive(false);
    }

    public void ActivateAllStates()
    {
        //deactivates all game states
        TitleScreenStateObject.SetActive(true);
        MainMenuStateObject.SetActive(true);
        OptionsScreenStateObject.SetActive(true);
        CreditsScreenStateObject.SetActive(true);
        GameplayStateObject.SetActive(true);
        GameOverScreenStateObject.SetActive(true);
        InGameOptionsScreenStateObject.SetActive(true);

    }

    public void GetAllStates()
    {
        TitleScreenStateObject = GameObject.Find("TitleScreen");
        MainMenuStateObject = GameObject.Find("MainMenu");
        OptionsScreenStateObject = GameObject.Find("Options");
        InGameOptionsScreenStateObject = GameObject.Find("InGameOptions");
        GameplayStateObject = GameObject.Find("GamePlay");
        GameOverScreenStateObject = GameObject.Find("GameOver");
        CreditsScreenStateObject = GameObject.Find("Credits");
    }

    public void ActivateTitleScreen()
    {
        //deactivate all other states first
        DeactivateAllStates();
        //activate title screen
        TitleScreenStateObject.SetActive(true);
        //extra code if u need for this function after titlescreen activates, if needed

    }

    public void ActivateMainMenuScreen()
    {
        //deactivate all other states first
        DeactivateAllStates();
        //activate main menu screen

        MainMenuStateObject.SetActive(true);
        //extra code if u need for this function after titlescreen activates, if needed

    }

    public void ActivateMainMusic()
    {
        audioSource.Stop();
        audioSource.clip = backgroundMusic[0];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void ActivateOptionsScreen()
    {
        //deactivate all other states first
        DeactivateAllStates();
        //activate options screen
        OptionsScreenStateObject.SetActive(true);
    }

    public void ActivateInGameOptionsScreen()
    {
        //deactivate all other states first
        DeactivateAllStates();
        //activate options screen
        InGameOptionsScreenStateObject.SetActive(true);
    }

    public void ActivateCreditsScreen()
    {
        //deactivate all other states first
        DeactivateAllStates();
        //activate credits screen
        CreditsScreenStateObject.SetActive(true);
    }

    public void ActivateGamePlayScreen()
    {
        //deactivate all other states first
        DeactivateAllStates();
        //activate gameplay screen/level
        GameplayStateObject.SetActive(true);
        //extra code if u need for this function after titlescreen activates, if needed

        // extra code here will need to remove existing level, ai's, pickups, players, essentially everything, generate a new level,
        // reset all values (score and health and lives) to their start values, spawn players/enemies, start game again
    }

    public void ActivateGameOver()
    {
        GameOverScreenStateObject.SetActive(true);
    }

    public void StartGame()
    {
        DeactivateAllStates();

        audioSource.Stop();
        audioSource.clip = backgroundMusic[1];
        audioSource.loop = true;
        audioSource.Play();

        mapGenerator.OnStartGameButtonPress();
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(1.0f); // Wait for 1 second before finding spawners
        //mapGenerator.OnStartGameButtonPress();
        FindSpawners();
        SpawnPlayers();
    }

    // GamePlay Related Code ---------------------------------------------------------
    /*public void SpawnPlayers()
    {
        howManyAICache = 0;

        // Spawn the TankPawn object
        GameObject newPawnObj = Instantiate(playerPawnPrefab, tankSpawners[i].transform.position, tankSpawners[i].transform.rotation) as GameObject;

        // Spawn the PlayerController object as a child of the TankPawn object
        GameObject newPlayerObj = Instantiate(playerControllerPrefab, newPawnObj.transform.position, newPawnObj.transform.rotation, newPawnObj.transform) as GameObject;

        // Get the PlayerController and Pawn components
        Controller newController = newPlayerObj.GetComponent<Controller>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        // Connect the Controller to the Pawn
        newController.pawn = newPawn;

        newController.score = 0;

        for (int i = 0; i < tankSpawners.Count; i++) // change tankSpawners to shapeSpawners
        {            
            if (howManyAICache < howManyAI) //ai players, fills in after humans
            {
                // Spawn the TankPawn object without a PlayerController child
                GameObject newPawnObj = Instantiate(playerPawnPrefab[Random.Range(0, tankPawnPrefabs.Count)], tankSpawners[i].transform.position, tankSpawners[i].transform.rotation) as GameObject;
                howManyAICache++;

                Controller newController = newPawnObj.GetComponent<Controller>();
                newController.score = 0;
            }
        }
    }


    // THIS WILL BE DIFFERENT, only do this if player picks up lives during gameplay, change code to spawn on self's location
    public void RespawnPlayer(int lives) // possibly feed OnDestroy to this (float) and save the score, respawn them with said score 4/19/23
    {
        if (tankSpawners?.Count == 0)
        {
            return;
        }

        int i = Random.Range(0, tankSpawners.Count);
        // Spawn the TankPawn object
        GameObject newPawnObj = Instantiate(tankPawnPrefab, tankSpawners[i].transform.position, tankSpawners[i].transform.rotation) as GameObject;

        // Spawn the PlayerController object as a child of the TankPawn object
        GameObject newPlayerObj = Instantiate(playerControllerPrefab, newPawnObj.transform.position, newPawnObj.transform.rotation, newPawnObj.transform) as GameObject;

        // Get the PlayerController and Pawn components
        Controller newController = newPlayerObj.GetComponent<Controller>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        // Connect the Controller to the Pawn
        newController.pawn = newPawn;
        newController.lives = lives;
    }

    // needs work, this should play BEFORE destroying at 0 health
    public void DeathScreen()
    {
        SceneManager.LoadScene("Main");
        GetAllStates();
        DeactivateAllStates();
        ActivateGameOver();
    }
    */

    public void SpawnPlayers()
    {
        howManyAICache = 0;

        // Spawn the TankPawn object for the player
        EnemySpawner playerSpawnPoint = enemySpawners[Random.Range(0, enemySpawners.Count)];
        GameObject newPawnObj = Instantiate(playerPawnPrefab, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation) as GameObject;

        // Spawn the PlayerController object as a child of the TankPawn object
        GameObject newPlayerObj = Instantiate(playerControllerPrefab, newPawnObj.transform.position, newPawnObj.transform.rotation, newPawnObj.transform) as GameObject;

        // Get the PlayerController and Pawn components
        Controller newController = newPlayerObj.GetComponent<Controller>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        // Connect the Controller to the Pawn
        newController.pawn = newPawn;

        // Set the score to 0
        newController.score = 0;

        // Spawn the AI players
        int enemySpawnerIndex = 0;
        while (howManyAICache < howManyAI && enemySpawnerIndex < enemySpawners.Count)
        {
            EnemySpawner enemySpawnPoint = enemySpawners[enemySpawnerIndex];
            if (CheckSpawnLocation(enemySpawnPoint))
            {
                // Spawn the TankPawn object without a PlayerController child
                GameObject newEnemyPawnObj = Instantiate(enemyPawnPrefabs[Random.Range(0, enemyPawnPrefabs.Count)], enemySpawnPoint.transform.position, enemySpawnPoint.transform.rotation) as GameObject;
                howManyAICache++;
            }
            enemySpawnerIndex++;
            if (enemySpawnerIndex == enemySpawners.Count)
            {
                enemySpawnerIndex = 0;
            }
        }
    }

    public void RespawnAI() // possibly feed OnDestroy to this (float) and save the score, respawn them with said score 4/19/23
    {
        EnemySpawner validSpawner = null;

        // Find a valid spawn location
        while (validSpawner == null)
        {
            int i = Random.Range(0, enemySpawners.Count);
            if (CheckSpawnLocation(enemySpawners[i]))
            {
                validSpawner = enemySpawners[i];
            }
        }

        // Spawn the new enemy if a valid spawner was found
        if (validSpawner != null)
        {
            GameObject newPawnObj = Instantiate(enemyPawnPrefabs[Random.Range(0, enemyPawnPrefabs.Count)], validSpawner.transform.position, validSpawner.transform.rotation) as GameObject;
        }
    }

    public void FindSpawners() // change tankSpawner to shapeSpawner, edit code to make sure spawn is outside 20f from playercontroller
    {
        EnemySpawner[] enemySpawnerArray = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner enemySpawner in enemySpawnerArray)
        {
            enemySpawners.Add(enemySpawner);
        }

        System.Random random = new System.Random();
        int n = enemySpawners.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            EnemySpawner temp = enemySpawners[k];
            enemySpawners[k] = enemySpawners[n];
            enemySpawners[n] = temp;
        }
    }

    public bool CheckSpawnLocation(EnemySpawner spawner)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            // playerController not found, allow respawn at any location
            return true;
        }

        float distance = Vector3.Distance(player.transform.position, spawner.transform.position);

        if (distance < 40f || distance > 150f)
        {
            // Spawn location is too close to player, disallow respawn
            return false;
        }

        // Spawn location is valid
        return true;
    }


    private void CheckInstance() // no change, this provides static connecting throughout game using Instance for all objects
    {
        if (instance == null)
        {
            // This is the instance
            instance = this;
            //Don't destroy it if we load a new scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Otherwise, there is already an instance, so destroy this gameObject
            Destroy(gameObject);
        }
    }

    public GameObject playerPawnPrefab; //used for spawning my pawn prefab setup
    public GameObject playerControllerPrefab; //used for spawning my controller
}
