using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
    // UI Elements
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI gameOverText;
    public GameObject titleScreen;
    public Button restartButton; 

    // Game Elements
    public List<GameObject> targetPrefabs;
    public float timeValue; // Game timer
    private int score;
    private float spawnRate = 1.5f;
    public bool isGameActive;

    // Positioning Variables
    private float spaceBetweenSquares = 2.5f; 
    private float minValueX = -3.75f; // X value of the center of the left-most square
    private float minValueY = -3.75f; // Y value of the center of the bottom-most square

    // Start the game, remove title screen, reset score, and adjust spawn rate based on difficulty
    public void StartGame(int difficulty)
    {
        spawnRate /= difficulty;
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        score = 0;
        timeValue = 60;
        UpdateScore(0);
        titleScreen.SetActive(false);
    }

    // Coroutine to spawn targets while the game is active
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);

            if (isGameActive)
            {
                Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);
            }
        }
    }

    // Generate a random spawn position within the grid
    Vector3 RandomSpawnPosition()
    {
        float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
        float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

        return new Vector3(spawnPosX, spawnPosY, 0);
    }

    // Generate a random index from 0 to 3 to determine which grid square to spawn in
    int RandomSquareIndex()
    {
        return Random.Range(0, 4);
    }

    // Update the player's score based on target clicked
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    // Update the game every frame
    void Update()
    {
        if (isGameActive)
        {
            UpdateTimeLeft();
        }
        
        if (timeValue <= 0 && isGameActive)
        {
            GameOver();
        }
    }

    // Update the timer and display it in the UI
    void UpdateTimeLeft()
    {
        timeValue -= Time.deltaTime;
        timeText.text = "Time: " + Mathf.Round(timeValue);
    }

    // Stop the game, show game over message and restart button
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    // Restart the game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
