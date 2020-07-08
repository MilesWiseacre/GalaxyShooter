using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pow_Up : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powID = 0;

    private SpriteRenderer _sprRender = null;
    [SerializeField]
    private Sprite[] _sprPow = null;
    [SerializeField]
    private AudioClip _clip = null;

    private Animator _anim = null;

    private GameObject _player = null;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _sprRender = GetComponent<SpriteRenderer>();
        _player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        _powID = Random.Range(0, 10);
        int idAdjust = _powID;
        if (idAdjust >= 6 && idAdjust != 10)
        {
            idAdjust = idAdjust - 6;
        }
        else if (idAdjust == 10)
        {
            idAdjust = idAdjust - 7;
        }
        _sprRender.sprite = _sprPow[idAdjust];
        _anim.SetInteger("Pow_ID", idAdjust);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, (_speed + 2) * Time.deltaTime);
        }
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
        if (transform.position.x <= -10)
        {
            Destroy(this.gameObject);
        }
    }

    private void Draw()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if (other.GetComponent<Laser>().playLaser == false)
            {
                Destroy(other);
                Destroy(gameObject);
            }
        }
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player._powDown)
            {
                player.RemoveDebuff();
            }
            switch (_powID)
            {
                case 0:
                case 6:
                    player.TripleShot = true;
                    player.SetCoolDown(5);
                    break;

                case 1:
                case 7:
                    player.speedMult = 1.5f;
                    player.SetCoolDown(5);
                    break;

                case 2:
                case 8:
                    player.Shield();
                    break;

                case 3:
                case 9:
                case 10:
                    player.Reload();
                    break;

                case 4:
                    player.Seek();
                    player.SetCoolDown(5);
                    break;

                case 5:
                    player.Heal();
                    break;

                default:
                    Debug.Log("Default Value");
                    break;
            }
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, .2f);
            Destroy(this.gameObject);
        }
    }
}
