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

    // Use this for initialization
    void Start()
    {
        _anim = GetComponent<Animator>();
        _sprRender = GetComponent<SpriteRenderer>();
        _powID = Random.Range(0, 11);
        int idAdjust = _powID;
        if (idAdjust >= 5 && idAdjust != 11)
        {
            idAdjust = idAdjust - 5;
        }
        else if (idAdjust == 11)
        {
            idAdjust = idAdjust - 8;
        }
        _sprRender.sprite = _sprPow[idAdjust];
        _anim.SetInteger("Pow_ID", idAdjust);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
        if (transform.position.x <= -10)
        {
            Destroy(this.gameObject);
        }
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
                case 5:
                    player.TripleShot = true;
                    player.SetCoolDown(5);
                    break;

                case 1:
                case 6:
                    player.speedMult = 1.5f;
                    player.SetCoolDown(5);
                    break;

                case 2:
                case 7:
                    player.Shield();
                    break;

                case 3:
                case 8:
                case 11:
                    player.Reload();
                    break;

                case 4:
                case 9:
                    player.Seek();
                    player.SetCoolDown(5);
                    break;

                case 10:
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
