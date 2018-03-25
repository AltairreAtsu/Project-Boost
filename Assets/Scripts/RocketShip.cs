using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketShip : MonoBehaviour {
    private Rigidbody rigidBody;
    private AudioSource audioSource;

	private bool invulnerable = false;

    enum State { Alive, Dying, Trancending}
    State state = State.Alive;

    [SerializeField] private float mainThrust = 1;
    [SerializeField] private float rcsThrust = 1;
	[Space]
	[SerializeField] private float loadDelay = 1f;
	[Space]
	[SerializeField] private ParticleSystem mainThrustParticles = null;
	[SerializeField] private ParticleSystem deathParticles = null;
	[SerializeField] private ParticleSystem sucessParticles = null;
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
			if (Application.isEditor)
				RespondToDebugInput();
		}

	}

    private void OnCollisionEnter(Collision collision)
    {
		if (state != State.Alive || invulnerable) { return; }

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

	private void OnTriggerEnter(Collider other)
	{
		if (state != State.Alive) { return; }
		
		if(other.tag != "Friendly")
			StartDeathSequence();
	}

	private void StartSucessSequence()
	{
		mainThrustParticles.Stop();
		audioSource.Stop();
		audioSource.PlayOneShot(jingleSfx);
		sucessParticles.Play();
		state = State.Trancending;
		Invoke("LoadNextScene", loadDelay + jingleSfx.length);
	}

	private void StartDeathSequence()
	{
		mainThrustParticles.Stop();
		audioSource.Stop();
		audioSource.PlayOneShot(deathSfx);
		deathParticles.Play();
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
			mainThrustParticles.Stop();
        }
    }

	private void ApplyThrust()
	{
		rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
		if (!audioSource.isPlaying)
			audioSource.PlayOneShot(mainThrustSfx);
		mainThrustParticles.Play();
	}

	private void RespondToDebugInput()
	{
		if( Input.GetKeyDown(KeyCode.C))
		{
			invulnerable = !invulnerable;
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			LoadNextScene();
		}
	}

	private void LoadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		state = State.Alive;
	}

	private void LoadFirstLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		state = State.Alive;
	}

	//TEMP
	public void Quit()
	{
		Application.Quit();
	}

}
