using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Movement speed
    [SerializeField]
    private float _speed = 5;

    // Number of hits can take
    public float enHealth = 1;

    // Explosion to leave behind at death
    [SerializeField]
    private GameObject _explosion = null;

    // Reference to UI manager to increase score
    [SerializeField]
    private UIManager _uiManager = null;

    // Reference to Game Manager to check if game is over
    private GameManager _gameManager = null;

    // Float to control when shots are fired
    private float _cooldown = 0f;

    // Float to determine how much time is between shots
    private float _fireRate = 0f;

    // Prefab of projectile
    [SerializeField]
    private GameObject _laser = null;

    // Whether the enemy is strafing
    private bool _strafing = false;
    // Whether the enemy is strafing up or down
    private bool _revStrafe = false;
    // Whether the enemy is moving forward or back
    private bool _revThrust = false;

    // Child shield object
    [SerializeField]
    private GameObject _shield = null;

    // Whether the enemy is shielded
    private bool shielded = false;

    // Determines enemy behavior
    public int enemyType = 0;

    private GameObject _player = null;

    void Awake()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _player = GameObject.FindWithTag("Player");
    }

    public void SetType(int type)
    {
        enemyType = type;
    }

    void Start()
    {
        Reload();
        if (enemyType == 0)
        {
            AllDecide();
            StartCoroutine(DecideMovement());
        } else if (enemyType == 1)
        {
            ReverseStrafe();
            StartCoroutine(ToggleStrafe());
        }
    }

    private void Reload()
    {
        _fireRate = Random.Range(.5f, 3.0f);
        _cooldown = Time.time + _fireRate;
    }

    // Decides whether to start strafing, or reverse direction
    private void AllDecide()
    {
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
                _revStrafe = !_revStrafe;
                break;
            default:
                break;
        }
    }

    // Decides what direction to start strafing in at spawn
    private void ReverseStrafe()
    {
        float decision = Random.Range(0, 1);
        switch (decision)
        {
            case 0:
                break;
            case 1:
                _revStrafe = !_revStrafe;
                break;
            default:
                break;
        }
    }

    IEnumerator DecideMovement()
    {
        yield return new WaitForSeconds(3f);
        float decision = Random.Range(0, 1);
        switch (decision)
        {
            case 0:
                _strafing = !_strafing;
                break;
            case 1:
                _revStrafe = !_revStrafe;
                break;
            default:
                break;
        }
    }

    IEnumerator ToggleStrafe()
    {
        yield return new WaitForSeconds(3f);
        float decision = Random.Range(0, 1);
        switch (decision)
        {
            case 0:
                break;
            case 1:
                _revStrafe = !_revStrafe;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        Movement();
        Shoot();
    }

    private void Movement()
    {
        if (_player != null)
        {
            float dist = Vector3.Distance(_player.transform.position, transform.position);
            if (dist < 3)
            {
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
            }
        }
            // Enemy type 1 switches reverse bool when out of bounds.
            if (enemyType == 1 && transform.position.x > 9)
            {
                transform.position = new Vector3(9, transform.position.y, 0);
                _revThrust = !_revThrust;
            }
            else if (enemyType == 1 && transform.position.x < 0)
            {
                transform.position = new Vector3(0, transform.position.y, 0);
                _revThrust = !_revThrust;
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
            else if (transform.position.x <= -10 && _gameManager.gameOver)
            {
                Destroy(gameObject);
            }

            // Wraps around when out of bounds.
            if (transform.position.y > 4.5f)
            {
                transform.position = new Vector3(transform.position.x, -4.5f, 0);
            }
            else if (transform.position.y < -4.5f)
            {
                transform.position = new Vector3(transform.position.x, 4.5f, 0);
            }
            if (transform.position.x <= -10)
            {
                transform.position = new Vector3(10, Random.Range(-4.5f, 4.5f), 0);
            }

            // Forward movement determined by a bool.
            float thrustdir = 0;
            if (!_revThrust)
            {
                thrustdir = -1;
            }
            else if (_revThrust)
            {
                thrustdir = 1;
            }

            // Strafing movement determined by a bool.
            float strafedir = 0;
            if (!_revStrafe)
            {
                strafedir = .5f;
            }
            else if (_revStrafe)
            {
                strafedir = -.5f;
            }

            // Move the enemy along.
            Vector3 moveDir = new Vector3(thrustdir, strafedir, 0);
            transform.Translate(moveDir * _speed * Time.deltaTime);
    }

    public void StartShield()
    {
        shielded = true;
        _shield.SetActive(true);
    }

    private void Shoot()
    {
        if (Time.time > _cooldown)
        {
            Reload();
            GameObject proj = Instantiate(_laser, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
            if (enemyType == 1)
            {
                proj.GetComponent<Laser>().Strafing();
            } else if (enemyType == 2)
            {
                proj.GetComponent<Laser>().Seeking();
            }
        }
    }

    public void Damage()
    {
        if (shielded)
        {
            shielded = false;
            _shield.SetActive(false);
        }
        else
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
            if (other.GetComponent<Laser>().playLaser)
            {
                Destroy(other.gameObject);
                Damage();
            }
        }
    }
}
