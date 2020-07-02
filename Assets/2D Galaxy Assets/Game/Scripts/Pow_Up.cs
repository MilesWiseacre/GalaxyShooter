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
        _powID = Random.Range(0, 10);
        if (_powID >= 4)
        {
            _powID = _powID - 4;
        }
        _sprRender.sprite = _sprPow[_powID];
        _anim.SetInteger("Pow_ID", _powID);
        
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
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            switch (_powID)
            {
                case 0:
                    player.TripleShot = true;
                    player.SetCoolDown(5);
                    break;

                case 1:
                    player.speedMult = 1.5f;
                    player.SetCoolDown(5);
                    break;

                case 2:
                    player.Shield();
                    break;

                case 3:
                    player.Reload();
                    break;

                case 4:
                    player.Heal();
                    break;

                case 5:
                    player.Seek();
                    player.SetCoolDown(5);
                    break;

                default:
                    Debug.Log("Default Value");
                    break;
            }
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, .2f);
            player.coolDown = Time.time + 5.0f;
            Destroy(this.gameObject);
        }
    }
}
