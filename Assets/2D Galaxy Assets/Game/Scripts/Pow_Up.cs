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
        int idAdjust = _powID;
        if (idAdjust >= 5)
        {
            idAdjust = idAdjust - 5;
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
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
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
                    player.Reload();
                    break;

                case 4:
                case 9:
                    player.Heal();
                    break;

                case 10:
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
