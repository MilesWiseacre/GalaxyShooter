using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    [SerializeField]
    private float _speed = 5;

    public float enHealth = 1;
    [SerializeField]
    private GameObject _explosion = null;
    [SerializeField]
    private UIManager _uiManager = null;
    GameManager _gameManager = null;

    float _cooldown = 0f;

    float _fireRate = 0f;

    [SerializeField]
    GameObject _laser = null;
    
    void Start() {
        _fireRate = Random.Range(.5f, 3.0f);
        _cooldown = Time.time + _fireRate;
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    void Update() {
        Movement();
        Shoot();
    }

    private void Movement()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
        if (transform.position.x <= -10 && !_gameManager.gameOver)
        {
            transform.position = new Vector3(10, Random.Range(-4.5f, 4.5f), 0);
        } else if (transform.position.x <= -10 && _gameManager.gameOver)
        {
            Destroy(gameObject);
        }
    }

    private void Shoot()
    {
        if (Time.time > _cooldown)
        {
            _fireRate = Random.Range(.5f, 3.0f);
            _cooldown = Time.time + _fireRate;
            Instantiate(_laser, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
        }
    }

    public void Damage()
    {
        enHealth--;
        if (enHealth == 0)
        {
            _uiManager.UpdateScore();
            Instantiate(_explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                Damage();
            }
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Damage();
        }
    }
    
}
