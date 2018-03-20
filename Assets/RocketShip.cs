using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShip : MonoBehaviour {
    private Rigidbody rigidBody;
    private AudioSource audioSource;

    [SerializeField]
    private float thrust = 1;

	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
        ProcessInput();
	}

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrust);
            if(!audioSource.isPlaying)
                audioSource.Play();  
        } else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back);
        }

    }
}
