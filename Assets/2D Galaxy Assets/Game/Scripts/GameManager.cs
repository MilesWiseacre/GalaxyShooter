using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool gameOver = true;
    public GameObject player;
    public GameObject asteroid;

    private UIManager _uiManager;
    private GameObject _spawner;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    private void Update()
    {
        if (gameOver == true)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            {
                gameOver = false;
                Instantiate(player, new Vector3(-4, 0, 0), Quaternion.identity);
                Instantiate(asteroid, new Vector3(0, 0, 0), Quaternion.identity);
                _uiManager.HideTitle();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
