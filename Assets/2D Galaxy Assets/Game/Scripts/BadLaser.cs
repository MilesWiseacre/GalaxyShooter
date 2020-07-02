using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;

    private float damage;

    void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
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
