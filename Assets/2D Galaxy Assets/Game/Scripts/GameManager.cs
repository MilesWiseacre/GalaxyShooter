using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool gameOver = true;
    public GameObject player;

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
                _uiManager.HideTitle();
            }
        }
    }

}
