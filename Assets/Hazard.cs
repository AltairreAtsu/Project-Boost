using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

	[SerializeField] private float fireDurration = 1f;
	[SerializeField] private float tellDurration = 1f;
	[SerializeField] private float primaryDelay = 1f;
	[SerializeField] private ParticleSystem firingSystem = null;
	[SerializeField] private ParticleSystem primingSystem = null;

	private enum State { Firing, Priming, Delay}
	State state = State.Delay;

	private float lastStep = 0f;

	private CapsuleCollider hazardCollider = null;

	private void Start()
	{
		if (firingSystem == null || primingSystem == null)
		{
			Debug.LogWarning(gameObject.name + " Particle Systems refrences must not be null! Disabling Hazard Script!");
			this.enabled = false;
		}

		hazardCollider = GetComponent<CapsuleCollider>();
		if(hazardCollider == null)
		{
			Debug.LogWarning(gameObject.name + " No Capsule Collider on Hazard Object!");
			this.enabled = false;
		}
		hazardCollider.enabled = false;
		lastStep = 0f;
	}

	private void Update () {
		switch (state) {
			case State.Delay:
				if (Time.time - lastStep >= primaryDelay) { Prime(); }
				break;
			case State.Priming:
				if (Time.time - lastStep >= tellDurration) { Fire(); }
				break;
			case State.Firing:
				if (Time.time - lastStep >= fireDurration) { Delay(); }
				break;
			default:
				Debug.LogWarning(gameObject.name + "is in an Invalid State!");
				this.enabled = false;
				break;
		}
		
	}

	private void Prime()
	{
		state = State.Priming;
		primingSystem.Play();
		lastStep = Time.time;
	}
	private void Fire()
	{
		state = State.Firing;
		primingSystem.Stop();
		firingSystem.Play();
		hazardCollider.enabled = true;
		lastStep = Time.time;
	}
	private void Delay()
	{
		state = State.Delay;
		firingSystem.Stop();
		hazardCollider.enabled = false;
		lastStep = Time.time;
	}
}
