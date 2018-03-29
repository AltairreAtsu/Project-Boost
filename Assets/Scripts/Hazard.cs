using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour, Triggerable {

	[SerializeField] private float fireDurration = 1f;
	[SerializeField] private float tellDurration = 1f;
	[SerializeField] private float primaryDelay = 1f;
	[SerializeField] private float startDelay = 0f;
	[Space]
	[SerializeField] private bool preFire = false;
	[SerializeField] private bool active = true;
	[Space]
	[SerializeField] private ParticleSystem firingSystem = null;
	[SerializeField] private ParticleSystem primingSystem = null;

	private enum State { Starting, Firing, Priming, Delay }
	State state = State.Starting;

	private float lastStep = 0f;
	
	private CapsuleCollider hazardCollider = null;

	protected void Start()
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

		if (preFire)
		{
			Prime();
			return;
		}
		if(startDelay == 0f)
		{
			state = State.Delay;
		}
	}

	private void Update () {
		if (!active) { return; }

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
			case State.Starting:
				if (Time.time - lastStep >= startDelay) { Prime(); }
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

	public bool IsTriggered()
	{
		return active;
	}

	public void Trigger()
	{
		active = true;
		lastStep = Time.time;
		state = State.Delay;
	}

	public void DeTrigger()
	{
		active = false;
		firingSystem.Stop();
		primingSystem.Stop();
		hazardCollider.enabled = false;
		state = State.Delay;
	}
}