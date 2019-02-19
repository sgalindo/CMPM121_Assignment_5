using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Salvador Galindo
 * sagalind
 * CMPM 121 - Assignment 5
 * GameManager.cs - Game manager that keeps track of collectibles, player health, and win/lose states.
 */
public class GameManager : MonoBehaviour
{
    private bool paused = false;
    private GameObject player;
    private PlayerMovement playerScript;
    private Door door;
    private int score = 0;
    [SerializeField] private int requiredScore = 3;
    private bool doorOpened = false;
    [SerializeField] private int playerHealth = 3;
    private Zombie zombie;
    private Canvas ui;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        door = GameObject.Find("Door").GetComponent<Door>();
        zombie = GameObject.Find("Zombie").GetComponent<Zombie>();
        ui = GameObject.Find("PlayerUI").GetComponent<Canvas>();

        ui.transform.Find("WinText").GetComponent<Text>().enabled = false;
        ui.transform.Find("LoseText").GetComponent<Text>().enabled = false;
        ui.transform.Find("RestartText").GetComponent<Text>().enabled = false;
        ui.transform.Find("SlideText").GetComponent<Text>().enabled = true;
        ui.transform.Find("HealthText").GetComponent<Text>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Pause button input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            Cursor.lockState = (paused == false) ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = (paused == false) ? 1f : 0f;
            playerScript.paused = paused;
        }

        if (score == requiredScore && !doorOpened)
        {
            door.OpenDoor();
            doorOpened = true;
            GameObject.Find("Zombie").GetComponent<Zombie>().enabled = true;
        }

        if (playerHealth == 0)
        {
            GameOver();
        }

        if (zombie.isDead == true)
        {
            Win();
        }

        if (gameOver && Input.GetKeyDown(KeyCode.R)) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        UpdateHealthText();
    }

    public int GetScore()
    {
        return score;
    }

    // Increase player's score by num
    public void IncreaseScore(int num)
    {
        score += num;
    }

    public int GetHealth()
    {
        return playerHealth;
    }

    public void DecreaseHealth(int damage)
    {
        playerHealth -= damage;
    }

    private void UpdateHealthText()
    {
        ui.transform.Find("HealthText").GetComponent<Text>().text = "Health: " + playerHealth;
    }

    private void GameOver()
    {
        player.GetComponent<Animator>().SetBool("IsGameOver", true);
        playerScript.enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;

        ui.transform.Find("LoseText").GetComponent<Text>().enabled = true;
        ui.transform.Find("RestartText").GetComponent<Text>().enabled = true;
        ui.transform.Find("SlideText").GetComponent<Text>().enabled = false;
        ui.transform.Find("HealthText").GetComponent<Text>().enabled = false;

        gameOver = true;
    }

    private void Win()
    {
        player.GetComponent<Animator>().SetBool("IsWin", true);
        playerScript.enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;

        ui.transform.Find("WinText").GetComponent<Text>().enabled = true;
        ui.transform.Find("RestartText").GetComponent<Text>().enabled = true;
        ui.transform.Find("SlideText").GetComponent<Text>().enabled = false;
        ui.transform.Find("HealthText").GetComponent<Text>().enabled = false;

        gameOver = true;
    }
}
