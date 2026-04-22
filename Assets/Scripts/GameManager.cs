using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyOnePrefab;
    public GameObject enemyTwoLWPrefab;
    public GameObject cloudPrefab;
    public GameObject gameOverMenu;
    public GameObject powerupPrefab;
    public GameObject coinPrefab;
    public GameObject audioPlayer;

    public AudioClip powerupSound;
    public AudioClip powerdownSound;
    public AudioClip coin;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI powerupText;

    public float horizontalScreenSize;
    public float verticalScreenSize;


    public int score;

    private bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        horizontalScreenSize = 10f;
        verticalScreenSize = 6.5f;
        score = 0;
        gameOver = false;
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        CreateSky();
        InvokeRepeating("CreateEnemy", 1, 3);
        InvokeRepeating("CreateEnemy1", 1, 6);
        StartCoroutine(SpawnPowerup());
        StartCoroutine(SpawnCoin());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }
    }


    void CreateEnemy()
    {
        Instantiate(enemyOnePrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize) * 0.9f, verticalScreenSize, 0), Quaternion.Euler(180, 0, 0));
        
    }
    void CreateEnemy1()
    {
        Instantiate(enemyTwoLWPrefab, new Vector3(horizontalScreenSize, Random.Range(-verticalScreenSize, verticalScreenSize) * 0.9f, 0), Quaternion.Euler(180, 0, 90));
    }


    void CreateSky()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(cloudPrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize), Random.Range(-verticalScreenSize, verticalScreenSize), 0), Quaternion.identity);
        }
    }


    void CreatePowerup()
    {
        Instantiate(powerupPrefab, new Vector3(Random.Range(-horizontalScreenSize * 0.8f, horizontalScreenSize * 0.8f), Random.Range(-3.25f, 0), 0), Quaternion.identity);
    }

    void CreateCoin()
    {
        Instantiate(coinPrefab, new Vector3(Random.Range(-horizontalScreenSize * 0.8f, horizontalScreenSize * 0.8f), Random.Range(-3.25f, 0), 0), Quaternion.identity);
    }

    IEnumerator SpawnPowerup()
    {
        float spawnTime = Random.Range(3, 5); 
        yield return new WaitForSeconds(spawnTime); 
        CreatePowerup();
        StartCoroutine(SpawnPowerup());
    }

    IEnumerator SpawnCoin()
    {
        float spawnTime = Random.Range(2, 4);
        yield return new WaitForSeconds(spawnTime);
        CreateCoin();
        StartCoroutine(SpawnCoin());
    }


    public void PlaySound(int whichSound)
    {
        switch (whichSound)
        {
            case 1:
                audioPlayer.GetComponent<AudioSource>().PlayOneShot(powerupSound);
                break;
            case 2:
                audioPlayer.GetComponent<AudioSource>().PlayOneShot(powerdownSound);
                break;
            case 3:
                audioPlayer.GetComponent<AudioSource>().PlayOneShot(coin);
                break;
        }
    }


    public void ManagePowerupText(int powerupType)
    {
        switch (powerupType)
        {
            case 1:
                powerupText.text = "Speed!";
                break;
            case 2:
                powerupText.text = "Double Weapon!";
                break;
            case 3:
                powerupText.text = "Triple Weapon!";
                break;
            case 4:
                powerupText.text = "Shield!";
                break;
            default:
                powerupText.text = "No powerups yet!";
                break;
        }
    }

    public void AddScore(int earnedScore)
    {
        score = score + earnedScore;
        scoreText.text = "Score: " + score;
    }

    public void ChangeLivesText (int currentLives)
    {
        livesText.text = "Lives: " + currentLives;
    }

    public void GameOver()
    {
        //set our game over object menu to true
        gameOverMenu.SetActive(true);
        //game over to be true
        gameOver = true;
    }
}
