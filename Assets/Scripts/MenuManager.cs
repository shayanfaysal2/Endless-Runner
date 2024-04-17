using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Image soundButton;
    public Sprite[] soundSprites;

    // Start is called before the first frame update
    void Start()
    {
        int sound = PlayerPrefs.GetInt("sound", 1);
        soundButton.sprite = soundSprites[sound];
        AudioListener.volume = sound;
    }

    public void StartGame(int x)
    {
        //save character in playerprefs
        PlayerPrefs.SetInt("character", x);

        //start the scene
        SceneManager.LoadScene(1);
    }

    public void ToggleSound()
    {
        if (AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
            soundButton.sprite = soundSprites[0];
            PlayerPrefs.SetInt("sound", 0);
        }
        else
        {
            AudioListener.volume = 1;
            soundButton.sprite = soundSprites[1];
            PlayerPrefs.SetInt("sound", 1);
        }
    }
}
