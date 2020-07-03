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

    public Text ammoText;

    public Text thrustText;

    public Slider powBar;

    float _barValue;

    float _barTime;

    void Update()
    {
        if (powBar.value > 0)
        {
            _barValue = _barTime - Time.time;
            powBar.value = _barValue;
        }
    }

    public void SetBar(float time)
    {
        powBar.value = time;
        _barTime = time + Time.time;
    }

    public void UpdateLives(int hasLives)
    { 
        Debug.Log("Player lives: " + hasLives);
        LivesSpr.sprite = LivesImg[hasLives];
    }

    public void UpdateAmmo(int hasAmmo)
    {
        ammoText.text = "" + hasAmmo;
    }

    public void UpdateThrusters(string display)
    {
        thrustText.text = "" + display;
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
