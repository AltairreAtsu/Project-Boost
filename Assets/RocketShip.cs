﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketShip : MonoBehaviour {
    private Rigidbody rigidBody;
    private AudioSource audioSource;

    enum State { Alive, Dying, Trancending}
    State state = State.Alive;

    [SerializeField] private float mainThrust = 1;
    [SerializeField] private float rcsThrust = 1;
	[Space]
	[SerializeField] private float loadDelay = 1f;
	[Space]
	[SerializeField] private AudioClip mainThrustSfx = null;
	[SerializeField] private AudioClip deathSfx = null;
	[SerializeField] private AudioClip jingleSfx = null;


	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
		if (state == State.Alive)
		{
			RespondToThrustInput();
			RespondToRotationInput();
		}

	}

    private void OnCollisionEnter(Collision collision)
    {
		if (state != State.Alive) { return; }

        switch (collision.gameObject.tag)
		{
			case "Friendly":
				print("Okay!");
				break;
			case "Finish":
				StartSucessSequence();
				break;
			default:
				StartDeathSequence();
				break;
		}
	}

	private void StartSucessSequence()
	{
		audioSource.Stop();
		audioSource.PlayOneShot(jingleSfx);
		state = State.Trancending;
		Invoke("LoadNextScene", loadDelay + jingleSfx.length);
	}

	private void StartDeathSequence()
	{
		audioSource.Stop();
		audioSource.PlayOneShot(deathSfx);
		state = State.Dying;
		Invoke("LoadFirstLevel", loadDelay + deathSfx.length);
	}

	private void RespondToRotationInput()
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

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
		{
			ApplyThrust();
		}
		else
        {
			audioSource.Stop();
        }
    }

	private void ApplyThrust()
	{
		rigidBody.AddRelativeForce(Vector3.up * mainThrust);
		if (!audioSource.isPlaying)
			audioSource.PlayOneShot(mainThrustSfx);
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
