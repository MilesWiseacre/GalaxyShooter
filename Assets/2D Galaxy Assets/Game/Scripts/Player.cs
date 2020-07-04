using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private float _speed = 5.0f;
    public float speedMult = 1f;
    private float _thrust = 0f;
    //[SerializeField]
    //private float _spread = 0f;
    [SerializeField]
    private float _fireRate = 0.25f;

    private float _canFire = 0.0f;
    [SerializeField]
    private GameObject _Pla_Laser = null;
    [SerializeField]
    private GameObject _Pla_3Laser = null;

    public bool TripleShot = false;
    public float coolDown = 0.0f;
    public int plaHealth = 3;
    [SerializeField]
    private GameObject _explosion = null;
    public bool shield = false;
    [SerializeField]
    private GameObject _shieldObject = null;
    [SerializeField]
    private UIManager _uiManager = null;
    private GameManager _gameManager = null;
    private SpawnManager _spawnManager = null;
    private AudioSource _audioSource = null;
    [SerializeField]
    private GameObject[] _bleeding = null;

    private int _hit = 0;

    private int _maxShieldHealth = 4;
    private int _shieldHealth = 0;

    SpriteRenderer _shieldSprite = null;

    private int _maxAmmo = 15;
    private int _ammo = 0;

    private ParticleSystem _ps;

    private bool _seek = false;

    [SerializeField]
    private GameObject seekLaser = null;

    CameraShake _camShake;

    void Start () {
        _ammo = _maxAmmo;
        _canFire = Time.time + _fireRate;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _shieldSprite = _shieldObject.GetComponent<SpriteRenderer>();
        _ps = GetComponent<ParticleSystem>();
        _camShake = Camera.main.GetComponent<CameraShake>();
        if (_uiManager != null)
        {
            _uiManager.UpdateLives(plaHealth);
            _uiManager.UpdateAmmo(_ammo, _maxAmmo);
            _uiManager.UpdateThrusters("Disengaged");
        }
    }

	void Update () {
        Movement();
        CoolDown();
        Thrust();
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && Time.time > _canFire && _ammo >= 1)
        {
            Shoot();
        } else if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && Time.time > _canFire && _ammo < 1)
        {
            Misfire();
        }
    }

    public void Damage()
    {
        StartCoroutine(_camShake.Shake(.25f, .3f));
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
            _shieldHealth--;
            switch (_shieldHealth)
            {
                case 3:
                    _shieldSprite.color = Color.green;
                    break;
                case 2:
                    _shieldSprite.color = Color.yellow;
                    break;
                case 1:
                    _shieldSprite.color = Color.red;
                    break;
                default:
                    _shieldSprite.color = Color.white;
                    break;
            }
            if (_shieldHealth == 0)
            {
                shield = false;
                _shieldObject.SetActive(false);
            }
        }
        if (plaHealth < 1)
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            _gameManager.gameOver = true;
            _uiManager.ShowTitle();
            Destroy(gameObject);
        }
    }

    public void Seek()
    {
        _seek = true;
    }

    public void Shield()
    {
        _shieldSprite.color = Color.white;
        shield = true;
        _shieldHealth = _maxShieldHealth;
        _shieldObject.SetActive(true);
    }

    public void Heal()
    {
        if (plaHealth != 3)
        {
            plaHealth++;
            _uiManager.UpdateLives(plaHealth);
            _hit--;
            if (plaHealth == 2)
            {
                _bleeding[1].SetActive(false);
            }
            else if (plaHealth == 3)
            {
                _bleeding[0].SetActive(false);
            }
        }
    }

    public void Reload()
    {
        _ammo = _maxAmmo;
        _uiManager.UpdateAmmo(_ammo, _maxAmmo);
    }

    public void SetCoolDown(float time)
    {
        coolDown = Time.time + time;
        _uiManager.SetBar(time);
    }

    private void CoolDown()
    {
        if (Time.time > coolDown)
        {
            TripleShot = false;
            speedMult = 1;
            _seek = false;
        }
    }

    private void Shoot()
    {
        _canFire = Time.time + _fireRate;
        _audioSource.Play();
        _ammo--;
        _uiManager.UpdateAmmo(_ammo, _maxAmmo);
        if (_seek == true)
        {
            Instantiate(seekLaser, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        } else
        {
            if (TripleShot == true)
            {
                Instantiate(_Pla_3Laser, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_Pla_Laser, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
            }
        }
    }

    private void Misfire()
    {
        _canFire = Time.time + _fireRate;
        ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
        emitOverride.startLifetime = .1f;
        _ps.Emit(emitOverride, 20);
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * ((_speed * speedMult) + _thrust) * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8f, 0f), transform.position.y, 0);

        if (transform.position.y >= 4.8f)
        {
            //transform.position = new Vector3(transform.position.x, 4.8f, 0);
            transform.position = new Vector3(transform.position.x, -4.8f, 0);
        }
        else if (transform.position.y <= -4.8f)
        {
            //transform.position = new Vector3(transform.position.x, -4.8f, 0);
            transform.position = new Vector3(transform.position.x, 4.8f, 0);
        }
    }

    private void Thrust()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _uiManager.UpdateThrusters("Engaged");
            _thrust = 2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _uiManager.UpdateThrusters("Disengaged");
            _thrust = 0f;
        }
    }
}
