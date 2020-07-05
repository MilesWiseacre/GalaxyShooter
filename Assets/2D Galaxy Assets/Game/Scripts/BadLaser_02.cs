using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadLaser_02 : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;

    private float damage;

    public bool reverse;

    void Update()
    {
        if (!reverse)
        {
            transform.Translate(new Vector3(-1f, .25f, 0) * _speed * Time.deltaTime);
        } else if (reverse)
        {
            transform.Translate(new Vector3(-1f, -.25f, 0) * _speed * Time.deltaTime);
        }
        
        if (transform.position.x <= -10f)
        {
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
                Destroy(gameObject);
            }
        }
    }
}
