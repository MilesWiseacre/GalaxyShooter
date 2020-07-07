using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    [SerializeField]
    private float _speed = 10.0f;

    private float xdir = 0f;
        
    private float ydir = 0f;

    private float damage;

    public bool playLaser = false;

    private bool _strafer = false;

    private bool _strafing = false;

    private bool _seeking = false;

    private GameObject[] _targets = null;

    private GameObject _target = null;

    private SpriteRenderer sprite = null;

    void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        if (_seeking)
        {
            Target();
            if (playLaser)
            {
                sprite.color = Color.blue;
            } else
            {
                sprite.color = Color.magenta;
            }
        }
        if (!playLaser)
        {
            sprite.color = Color.red;
        }
    }

    public void PlayerFire()
    {
        playLaser = true;
    }

    public void Seeking()
    {
        _seeking = true;
    }

    public void Strafing()
    {
        _strafer = true;
    }

    void Target()
    {
        if (playLaser)
        {
            _targets = GameObject.FindGameObjectsWithTag("Enemy");
            if (_targets == null)
            {
                return;
            }
        } else
        {
            _target = GameObject.FindGameObjectWithTag("Player");
        }
        foreach (GameObject targ in _targets)
        {
            if (targ.GetComponent<Enemy>() != null)
            {
                _target = targ;
                return;
            }
        }
    }

    void Update () {
        if (!_seeking)
        {
            if (playLaser)
            {
                xdir = 1f;
            }
            else
            {
                xdir = -1f;
            }
            if (_strafer)
            {
                if (_strafing)
                {
                    ydir = .25f;
                }
                else
                {
                    ydir = -.25f;
                }
            } else
            {
                ydir = 0f;
            }

            Vector3 direction = new Vector3(xdir, ydir, 0);
            transform.Translate(direction * _speed * Time.deltaTime);
        } else
        {
            if (_target == null)
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
                Target();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
            }
        }
        
        if (transform.position.x >= 10f || transform.position.x <= -10f)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playLaser && other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                Destroy(gameObject);
            }
        }
    }
}
