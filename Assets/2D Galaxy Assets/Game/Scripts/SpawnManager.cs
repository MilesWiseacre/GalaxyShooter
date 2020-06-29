using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] Enemy;
    [SerializeField]
    private GameObject PowerUp;

    private GameManager _gameManager;

	void Start () {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
	}
	
	public void StartRoutines()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        while (_gameManager.gameOver == false)
        {
            Instantiate(Enemy[0], new Vector3(12, Random.Range(-4.5f, 4.5f), 0), Quaternion.identity);
            yield return new WaitForSeconds(4.0f);
        }
    }
    
    IEnumerator PowerUpRoutine()
    {
        while (_gameManager.gameOver == false)
        {
            Instantiate(PowerUp, new Vector3(12, Random.Range(-4.5f, 4.5f), 0), Quaternion.identity);
            yield return new WaitForSeconds(4.0f);
        }
    }
}
