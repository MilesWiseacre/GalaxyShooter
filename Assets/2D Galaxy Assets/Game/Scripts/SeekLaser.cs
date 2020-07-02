using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;

    private float damage;

    private GameObject[] _targets = null;

    private GameObject _target = null;

    void Start()
    {
        Target();
    }

    void Target()
    {
        _targets = GameObject.FindGameObjectsWithTag("Enemy");
        if (_targets == null)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        foreach (GameObject targ in _targets)
        {
            if (targ.GetComponent<EnemyAI>() != null)
            {
                _target = targ;
                return;
            }
        }

    }

    void Update()
    {
        if (_target == null)
        {
            Target();
        } else
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
        }
        if (transform.position.x >= 10f)
        {
            Destroy(gameObject);
        }
    }
}
