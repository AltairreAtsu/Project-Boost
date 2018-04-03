using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Trigger))]
public class Pickup : MonoBehaviour {
	public enum Types { Heatshield, Key, None }
	
	#region Serialized Fields
	[Tooltip ("Select a pickup type form the list to define what this Power Up Awards. Keys are never awarded to the player as a holdable powerup.")]
	[SerializeField] private Types pickUpType;
	[Tooltip ("Set this to true to run the trigger script when the object is pickedup. Keys Always run the Trigger script.")]
	[SerializeField] private bool triggerOnPickup = false;
	[Tooltip ("Select an audioclip to play when you pickup the pickup!")]
	[SerializeField] private AudioClip pickupSFX = null;
	#endregion
	#region Component Variables
	private Trigger trigger = null;
	private ParticleSystem pickupParticles = null;
	private AudioSource audioSource = null;
	private MeshRenderer meshRender = null;
	private Light powerLight = null;
	#endregion

	private bool detonating = false;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		pickupParticles = GetComponent<ParticleSystem>();
		meshRender = GetComponent<MeshRenderer>();
		powerLight = GetComponent<Light>();
		trigger = GetComponent<Trigger>();
	}

	private void OnTriggerEnter(Collider other)
	{
		RocketShip rocketShip = other.GetComponentInParent<RocketShip>();
		if(rocketShip != null && !detonating)
		{
			AwardPickup(rocketShip);
			StartSelfDestructSequence();
		}
	}

	private void AwardPickup(RocketShip rocketShip)
	{
		if (pickUpType != Types.Key)
		{
			rocketShip.AwardPowerUp(pickUpType);
			if (triggerOnPickup) { trigger.DoTrigger(); }
		}
		else
		{
			trigger.DoTrigger();
		}
	}

	private void StartSelfDestructSequence()
	{
		pickupParticles.Play();
		audioSource.PlayOneShot(pickupSFX);
		meshRender.enabled = false;
		detonating = true;
		powerLight.enabled = false;
		Invoke("SelfDestruct", pickupSFX.length);
	}

	private void SelfDestruct()
	{
		Destroy(this.gameObject);
	}
}
