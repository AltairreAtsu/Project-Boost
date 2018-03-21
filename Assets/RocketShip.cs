using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketShip : MonoBehaviour {
    private Rigidbody rigidBody;
    private AudioSource audioSource;

    enum State { Alive, Dying, Trancending}
    State state = State.Alive;

    [SerializeField]
    private float mainThrust = 1;
    [SerializeField]
    private float rcsThrust = 1;
	[Space]
	[SerializeField]
	private float loadDelay = 1f;
	[Space]
	[SerializeField]
	private AudioClip mainThrustSfx = null;
	[SerializeField]
	private AudioClip deathSfx = null;
	[SerializeField]
	private AudioClip jingleSfx = null;


	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
		if (state != State.Dying)
		{
			Thrust();
			Rotate();
		}

	}

    private void OnCollisionEnter(Collision collision)
    {
		if (state != State.Alive) { return; }

        if(collision.gameObject.tag == "Friendly")
        {
            // Do Nothing
            print("Okay!");
        } else if (collision.gameObject.tag == "Finish")
        {
			state = State.Trancending;
			Invoke("LoadNextScene", loadDelay);
        }
        else
		{
			state = State.Dying;
			audioSource.Stop();
			Invoke("LoadFirstLevel", loadDelay);
		}
	}


	private void Rotate()
    {
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

	private void LoadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		state = State.Alive;
	}

	private void LoadFirstLevel()
	{
		SceneManager.LoadScene(0);
		state = State.Alive;
	}

}
