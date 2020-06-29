using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powID;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
            player.coolDown = Time.time + 5.0f;
            Destroy(this.gameObject);
        }
    }
}
