using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        door = GameObject.Find("Door").GetComponent<Door>();
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

        if (score == 3 && !doorOpened)
        {
            door.OpenDoor();
            doorOpened = true;
        }

        if (playerHealth == 0)
        {
            // INSERT GAME OVER
            Debug.Log("GAME OVER");
        }
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
}
