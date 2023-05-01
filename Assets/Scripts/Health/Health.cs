using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AIController;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public Pawn owner;

    Powerup powerup;
    private Image healthBarImage;
    private TextMeshProUGUI healthBarText;



    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // sets current health to max health on start

        // find the canvas/healthbar
        Transform canvas = transform.Find("HealthBarCanvas");
        if (canvas != null)
        {
            healthBarImage = canvas.Find("HealthBarUI").GetComponent<Image>();
        }
        else
        {
            return;
        }
        //healthBarImage = canvas.Find("HealthBarUI").GetComponent<Image>();
        healthBarText = canvas.Find("CurrentHealthTextUI").GetComponent<TextMeshProUGUI>();

        healthBarImage.fillAmount = currentHealth / maxHealth;
        healthBarText.text = currentHealth.ToString();

    }

    // Update is called once per frame
    void Update()
    {

        //currentHealth = Mathf.Clamp (currentHealth, 0, maxHealth); //maybe here? if not probably in takedamage
    }

    public void TakeDamage(float amount, Pawn source) // take damage, takes current health and reduces by damage value
    {
        currentHealth = currentHealth - amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Transform canvas = transform.Find("HealthBarCanvas");
        if (canvas != null)
        {
            //update healthbar fill amount
            healthBarImage.fillAmount = currentHealth / maxHealth;
            healthBarText.text = currentHealth.ToString();
        }

        if (currentHealth <= 0) // if below 0, destroy this pawn
        {
            if (source == null)
            {
                return; 
            }

            PowerupManager powerupManager = source.GetComponent<PowerupManager>();
            PowerupScore powerup = new PowerupScore();
            powerup.scoreToAdd = 250;

            if (powerupManager != null)
            {
                //add powerup
                powerupManager.Add(powerup);
            }          
            Die(source);            
        }
        else
        {
            owner = source;
        }
    }
    //Die(source);

    private IEnumerator DestroyEverything(float delay)
    {
        yield return new WaitForSeconds(delay);
        //gameObject.GetComponent<Controller>().enabled = true;
    }

    public void Heal(float amount, Pawn source) // opposite of take damage, will take current health and add the amount, then clamp it so it doesnt go above max
    {
        currentHealth = currentHealth + amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //Debug.Log("My current health is: " + currentHealth);

        //update healthbar fill amount
        //healthBar.fillAmount = currentHealth / maxHealth;

    }

    public void Die(Pawn source) // destroy object associated with this component
    {

        // this section needs redone, Die() should cause the gameover screen to appear IF dead, then everything else
        // possibly wont do lives since its a time survival game

        Controller controller = this.GetComponentInChildren<Controller>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            if (GetComponent<AIController>())
            {
                gameManager.RespawnAI();
            }
            else
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.ChangeState(GameManager.MenuState.GameOver);
                    Destroy(this.gameObject);
                }
                return;            
            }
        }
        Destroy(this.gameObject);
    }

    public void PermHealth(float amount, Pawn source)
    {
        currentHealth = currentHealth + amount;
        maxHealth = maxHealth + amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}
