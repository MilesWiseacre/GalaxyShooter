using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private float _speed = 5.0f;
    public float speedMult = 1f;
    //[SerializeField]
    //private float _spread = 0f;
    [SerializeField]
    private float _fireRate = 0.25f;

    private float _canFire = 0.0f;
    [SerializeField]
    private GameObject _Pla_Laser;
    [SerializeField]
    private GameObject _Pla_3Laser;

    public bool TripleShot = false;
    public float coolDown = 0.0f;
    public int plaHealth = 3;
    [SerializeField]
    private GameObject _explosion;
    public bool shield = false;
    [SerializeField]
    private GameObject _shieldObject;
    [SerializeField]
    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject[] _bleeding;

    private int _hit = 0;

    // Use this for initialization
    void Start () {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        if (_uiManager != null)
        {
            _uiManager.UpdateLives(plaHealth);
        }
        if (_spawnManager != null)
        {
            _spawnManager.StartRoutines();
        }
    }

	// Update is called once per frame
	void Update () {
        Movement();
        CoolDown();
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    public void Damage()
    {
        if (shield == false)
        {
            plaHealth--;
            _hit++;
            _uiManager.UpdateLives(plaHealth);
            if (_hit == 1)
            {
                _bleeding[0].SetActive(true);
            }
            else if (_hit == 2)
            {
                _bleeding[1].SetActive(true);
            }
        }
        else
        {
            shield = false;
            _shieldObject.SetActive(false);
        }
        if (plaHealth < 1)
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            _gameManager.gameOver = true;
            _uiManager.ShowTitle();
            Destroy(gameObject);
        }
    }

    public void Shield()
    {
        shield = true;
        _shieldObject.SetActive(true);
    }

    public void Heal()
    {
        plaHealth++;
    }

    private void CoolDown()
    {
        if (Time.time > coolDown)
        {
            TripleShot = false;
            speedMult = 1;
        }
    }

    private void Shoot()
    {
        if (Time.time > _canFire)
        {
            _audioSource.Play();
            if (TripleShot == true)
            {
                Instantiate(_Pla_3Laser, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_Pla_Laser, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
            }
            _canFire = Time.time + _fireRate;
        }
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * _speed * speedMult * horizontalInput * Time.deltaTime);
        transform.Translate(Vector3.up * _speed * speedMult * verticalInput * Time.deltaTime);

        if (transform.position.x >= 0.0f)
        {
            transform.position = new Vector3(0, transform.position.y, 0);
        }
        else if (transform.position.x <= -8.5f)
        {
            transform.position = new Vector3(-8.5f, transform.position.y, 0);
        }

        if (transform.position.y >= 4.8f)
        {
            transform.position = new Vector3(transform.position.x, 4.8f, 0);
        }
        else if (transform.position.y <= -4.8f)
        {
            transform.position = new Vector3(transform.position.x, -4.8f, 0);
        }
    }
}
