using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Sprite[] LivesImg;
    public Image LivesSpr;
    public GameObject titleScreen;

    public Text scoreText;
    public int score;

    public void UpdateLives(int hasLives)
    { 
        Debug.Log("Player lives: " + hasLives);
        LivesSpr.sprite = LivesImg[hasLives];
    }

    public void UpdateScore()
    {
        score += 10;
        scoreText.text = "" + score;
    }

    public void HideTitle()
    {
        titleScreen.SetActive(false);
        score = 0;
        scoreText.text = "" + score;
    }

    public void ShowTitle()
    {
        titleScreen.SetActive(true);
    }

}
