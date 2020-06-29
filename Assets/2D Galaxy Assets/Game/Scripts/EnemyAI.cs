using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    [SerializeField]
    private float _speed = 5;

    public float enHealth = 1;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private UIManager _uiManager;
    // Use this for initialization
    void Start() {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update() {
        Movement();
    }

    private void Movement()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
        if (transform.position.x <= -10)
        {
            transform.position = new Vector3(10, Random.Range(-4.5f, 4.5f), 0);
        }
    }

    public void Damage()
    {
        enHealth--;
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
            }
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Damage();
        }
    }
    
}
