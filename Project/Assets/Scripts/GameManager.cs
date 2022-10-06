using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    public TextMeshProUGUI gameOverText;

    public TextMeshProUGUI livesText;

    public Button restartButton;

    public bool isGameActive;

    public GameObject titleScreen;

    private SpawnManager spawnManagerScript;

    public TextMeshProUGUI Level1;

    public TextMeshProUGUI Level2;

    public TextMeshProUGUI Level3;

    public TextMeshProUGUI Level4;

    public TextMeshProUGUI Level5;

    public TextMeshProUGUI Level6;

    // Start is called before the first frame update
    void Start()
    {
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

    }

    public void StartGame()
    {
        isGameActive = true;
        spawnManagerScript.StartGame();
        titleScreen.gameObject.SetActive(false);
        livesText.gameObject.SetActive(true);

        Level1.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
        livesText.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
