using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : MonoBehaviour {

    private Animator _anim;

	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _anim.SetBool("Turn_Up", true);
            _anim.SetBool("Turn_Down", false);
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            _anim.SetBool("Turn_Up", false);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _anim.SetBool("Turn_Down", true);
            _anim.SetBool("Turn_Up", false);
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            _anim.SetBool("Turn_Down", false);
        }
    }
}
