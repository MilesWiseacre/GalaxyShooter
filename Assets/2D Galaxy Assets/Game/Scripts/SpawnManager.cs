﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private GameObject Enemy = null;
    [SerializeField]
    private GameObject PowerUp = null;
    [SerializeField]
    private GameObject PowerDown = null;

    private GameManager _gameManager = null;

    [SerializeField]
    GameObject _enemyContainer = null;

    float _runtime = 0.00f;

	void Start () {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
	}

    void Update()
    {
        if (_gameManager.gameOver == false)
        {
            _runtime += Time.deltaTime;
        }
    }

    public void StopRoutines()
    {
        StopCoroutine(EnemySpawnRoutine());
        StopCoroutine(PowerUpRoutine());
        _runtime = 0.00f;
    }
	
	public void StartRoutines()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpRoutine());
    }

    private void SpawnEnemyRandomly(int id)
    {
        Vector3 toSpawn = new Vector3(12, Random.Range(-4.5f, 4.5f), 0);
        GameObject newEnemy = Instantiate(Enemy, toSpawn, Quaternion.identity);
        Enemy enemy = newEnemy.GetComponent<Enemy>();
        enemy.SetType(id);
        float decision = Random.Range(0, 5);
        if (decision == 0)
        {
            enemy.StartShield();
        }
        newEnemy.transform.parent = _enemyContainer.transform;
    }

    // Spawns more enemies the longer the game has been played.
    IEnumerator EnemySpawnRoutine()
    {
        while (_gameManager.gameOver == false)
        {
            if (_runtime < 10)
            {
                SpawnEnemyRandomly(0);
            }
            else if (_runtime >= 10 && _runtime < 20)
            {
                SpawnEnemyRandomly(0);
                SpawnEnemyRandomly(1);
            }
            else if (_runtime >= 20)
            {
                SpawnEnemyRandomly(0);
                SpawnEnemyRandomly(1);
                SpawnEnemyRandomly(3);
            }
            if (_runtime >= 15)
            {
                SpawnEnemyRandomly(2);
            }
            yield return new WaitForSeconds(4.0f);
        }
    }
    
    IEnumerator PowerUpRoutine()
    {
        while (_gameManager.gameOver == false)
        {
            float decision = Random.Range(0,3);
            if (decision == 1)
            {
                Vector3 toBe = new Vector3(12, Random.Range(-4.5f, 4.5f), 0);
                Instantiate(PowerDown, toBe, Quaternion.identity);
            }
            Vector3 toSpawn = new Vector3(12, Random.Range(-4.5f, 4.5f), 0);
            Instantiate(PowerUp, toSpawn, Quaternion.identity);
            yield return new WaitForSeconds(7.0f);
        }
    }
}
