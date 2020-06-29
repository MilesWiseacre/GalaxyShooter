using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pow_Up : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powID;

    private SpriteRenderer _sprRender;
    [SerializeField]
    private Sprite[] _sprPow;
    [SerializeField]
    private AudioClip _clip;

    private Animator _anim;

    // Use this for initialization
    void Start()
    {
        _anim = GetComponent<Animator>();
        _sprRender = GetComponent<SpriteRenderer>();
        _powID = Random.Range(0, 3);
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
            if (_powID == 0)
            {
                player.TripleShot = true;
            }
            else if (_powID == 1)
            {
                player.speedMult = 1.5f;
            }
            else if (_powID == 2)
            {
                player.Shield();
            }
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, .2f);
            player.coolDown = Time.time + 5.0f;
            Destroy(this.gameObject);
        }
    }
}
