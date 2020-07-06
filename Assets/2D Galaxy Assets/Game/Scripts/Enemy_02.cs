using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_02 : MonoBehaviour
{
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

    bool _reverse = false;
    bool _forReverse = false;

    [SerializeField]
    private GameObject _shield = null;
    bool shielded = false;

    void Start()
    {
        Reload();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        float decision = Random.Range(0, 1);
        switch (decision)
        {
            case 0:
                break;
            case 1:
                _reverse = !_reverse;
                break;
            default:
                break;
        }
        StartCoroutine(Toggle());
    }

    void Update()
    {
        Movement();
        Shoot();
    }

    public void StartShield()
    {
        shielded = true;
        _shield.SetActive(true);
    }

    IEnumerator Toggle()
    {
        yield return new WaitForSeconds(3f);
        float decision = Random.Range(0, 1);
        switch (decision)
        {
            case 0:
                break;
            case 1:
                _reverse = !_reverse;
                break;
            default:
                break;
        }
    }

    private void Movement()
    {
        
        // Switches reverse bool when out of bounds.
        if (transform.position.x > 9)
        {
            transform.position = new Vector3(9, transform.position.y, 0);
            _forReverse = !_forReverse;
        }
        else if (transform.position.x < 0)
        {
            transform.position = new Vector3(0, transform.position.y, 0);
            _forReverse = !_forReverse;
        }
        // If game is over, leave when wrapping around.
        if (transform.position.y <= -4.5f && _gameManager.gameOver)
        {
            Destroy(gameObject);
        }
        else if (transform.position.y >= 4.5f && _gameManager.gameOver)
        {
            Destroy(gameObject);
        }
        // Wraps around the y position when out of bounds.
        if (transform.position.y > 4.5f)
        {
            transform.position = new Vector3(transform.position.x, -4.5f, 0);
        }
        else if (transform.position.y < -4.5f)
        {
            transform.position = new Vector3(transform.position.x, 4.5f, 0);
        }
        // Forward movement determined by a bool.
        float thrustdir = 0;
        if (!_forReverse)
        {
            thrustdir = -1;
        } else if (_forReverse)
        {
            thrustdir = 1;
        }
        // Strafing movement determined by a bool.
        float strafedir = 0;
        if (!_reverse)
        {
            strafedir = .5f;
        }
        else if (_reverse)
        {
            strafedir = -.5f;
        }
        // Move the enemy along.
        Vector3 moveDir = new Vector3(thrustdir, strafedir, 0);
        transform.Translate(moveDir * _speed * Time.deltaTime);
    }

    private void Shoot()
    {
        if (Time.time > _cooldown)
        {
            Reload();
            GameObject laser = Instantiate(_laser, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
            if (_reverse)
            {
                laser.GetComponent<BadLaser_02>().reverse = true;
            }
        }
    }

    private void Reload()
    {
        _fireRate = Random.Range(.5f, 3.0f);
        _cooldown = Time.time + _fireRate;
    }

    public void Damage()
    {
        if (shielded)
        {
            shielded = false;
            _shield.SetActive(false);
        } else
        {
            enHealth--;
        }
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
