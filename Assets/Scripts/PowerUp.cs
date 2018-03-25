using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
	[SerializeField] private RocketShip.PowerUpTypes powerUpType;
	[SerializeField] private AudioClip pickupSFX = null;

	private ParticleSystem pickupParticles = null;
	private AudioSource audioSource = null;
	private MeshRenderer meshRender = null;
	private Light powerLight = null;
	private bool detonating = false;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		pickupParticles = GetComponent<ParticleSystem>();
		meshRender = GetComponent<MeshRenderer>();
		powerLight = GetComponent<Light>();
	}

	private void OnTriggerEnter(Collider other)
	{
		RocketShip rocketShip = other.GetComponentInParent<RocketShip>();
		if(rocketShip != null && !detonating)
		{
			rocketShip.AwardPowerUp(powerUpType);
			pickupParticles.Play();
			audioSource.PlayOneShot(pickupSFX);
			meshRender.enabled = false;
			detonating = true;
			powerLight.enabled = false;
			Invoke("SelfDestruct", pickupSFX.length);
		}
	}

	private void SelfDestruct()
	{
		Destroy(this.gameObject);
	}
}
