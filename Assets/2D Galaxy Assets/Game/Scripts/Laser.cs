using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    [SerializeField]
    private float _speed = 10.0f;

    private float damage;

	void Update () {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
        if (transform.position.x >= 10f)
        {
            Destroy(gameObject);
        }
	}
}
