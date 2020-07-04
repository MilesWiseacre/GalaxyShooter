using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pow_Down : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private AudioClip _clip = null;

    // Start is called before the first frame update
    void Start()
    {
        
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
            player.PowDown();
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, .2f);
            player.coolDown = Time.time + 5.0f;
            Destroy(this.gameObject);
        }
    }
}
