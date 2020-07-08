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

    [SerializeField]
    SpawnManager _spawnManager = null;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

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
        powBar.maxValue = time;
        powBar.value = time;
        _barTime = time + Time.time;
    }

    public void UpdateLives(int hasLives)
    { 
        LivesSpr.sprite = LivesImg[hasLives];
    }

    public void UpdateAmmo(int hasAmmo, int maxAmmo)
    {
        ammoText.text = "" + hasAmmo + "/" + maxAmmo;
    }

    public void UpdatePowDown()
    {
        ammoText.text = "Qa/02";
        UpdateThrusters("Disabled");
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
        _spawnManager.StopRoutines();
    }

}
