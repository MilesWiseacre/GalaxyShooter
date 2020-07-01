using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] Enemy = null;
    [SerializeField]
    private GameObject PowerUp = null;

    private GameManager _gameManager = null;

    [SerializeField]
    GameObject _enemyContainer = null;

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
            Vector3 toSpawn = new Vector3(12, Random.Range(-4.5f, 4.5f), 0);
            GameObject newEnemy = Instantiate(Enemy[0], toSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(4.0f);
        }
    }
    
    IEnumerator PowerUpRoutine()
    {
        while (_gameManager.gameOver == false)
        {
            Vector3 toSpawn = new Vector3(12, Random.Range(-4.5f, 4.5f), 0);
            Instantiate(PowerUp, toSpawn, Quaternion.identity);
            yield return new WaitForSeconds(4.0f);
        }
    }
}
