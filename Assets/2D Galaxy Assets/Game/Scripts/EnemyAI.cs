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

    bool _strafing = false;

    bool _reverse = false;
    
    void Start() {
        Reload();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        float decision = Random.Range(0, 2);
        switch (decision)
        {
            case 0:
                break;
            case 1:
                _strafing = true;
                break;
            case 2:
                _strafing = true;
                _reverse = !_reverse;
                break;
            default:
                break;
        }
        StartCoroutine(Toggle());
    }

    void Update() {
        Movement();
        Shoot();
    }

    IEnumerator Toggle()
    {
        yield return new WaitForSeconds(3f);
        float decision = Random.Range(0, 5);
        switch (decision)
        {
            case 0:
            case 1:
                _strafing = false;
                break;
            case 2:
            case 3:
                _strafing = true;
                break;
            case 5:
                _reverse = !_reverse;
                break;
            default:
                break;
        }
    }

    private void Movement()
    {
        // Switches reverse bool when out of bounds.
        if (transform.position.y > 4.5f)
        {
            transform.position = new Vector3(transform.position.x, 4.5f, 0);
            _reverse = !_reverse;
        }
        else if (transform.position.y < -4.5f)
        {
            transform.position = new Vector3(transform.position.x, -4.5f, 0);
            _reverse = !_reverse;
        }
        // Strafing movement determined by a pair of bools.
        float strafedir = 0;
        if (_strafing && !_reverse)
        {
            strafedir = .5f;
        } else if (_strafing && _reverse)
        {
            strafedir = -.5f;
        }
        // Move the enemy along.
        Vector3 moveDir = new Vector3(-1, strafedir, 0);
        transform.Translate(moveDir * _speed * Time.deltaTime);
        // Wraps around, unless the game is over.
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
            Reload();
            Instantiate(_laser, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
        }
    }

    private void Reload()
    {
        _fireRate = Random.Range(.5f, 3.0f);
        _cooldown = Time.time + _fireRate;
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
