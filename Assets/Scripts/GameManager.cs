using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private PlayerController playerController;

    public AudioSource music;

    public GameObject[] prefabs;

    public AudioSource coinSound;
    public AudioSource gameOverSound;

    public GameObject redFlashPrefab;
    public GameObject gameOverPanel;

    public Text scoreText;
    public Text coinsText;

    private float score;
    private int coins;

    [HideInInspector]

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //60 fps
        Application.targetFrameRate = 60;

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        score = 0;
        coins = 0;    
    }

    public void UpdateScore(float x)
    {
        score += x;
        scoreText.text = score.ToString("F1");
    }

    public void CollectCoin()
    {
        coinSound.Play();
        coins++;
        coinsText.text = coins.ToString();
    }

    public void Die()
    {
        Destroy(Instantiate(redFlashPrefab), 0.5f);

        gameOverSound.Play();
        gameOverPanel.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
