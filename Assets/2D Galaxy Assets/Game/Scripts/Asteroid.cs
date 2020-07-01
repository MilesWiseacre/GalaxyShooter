using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    float _rotateSpeed = 3.0f;

    [SerializeField]
    GameObject _explosion = null;

    [SerializeField]
    SpawnManager _spawnManager = null;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                Instantiate(_explosion, transform.position, transform.rotation);
                _spawnManager.StartRoutines();
                Destroy(gameObject);
            }
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_explosion, transform.position, transform.rotation);
            _spawnManager.StartRoutines();
            Destroy(gameObject);
        }
    }
}
