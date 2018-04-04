using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Trigger))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent (typeof(Collider))]
public class PressurePlate : MonoBehaviour {

	[SerializeField] private float durration = 1f;

	private bool triggered = false;
	private AudioSource audioSource = null;
	private Trigger triggerScript = null;
	private Vector3 startPosition = Vector3.zero;

	private void Start()
	{
		startPosition = transform.position;
		triggerScript = GetComponent<Trigger>();
		audioSource = GetComponent<AudioSource>();
	}

	private IEnumerator Recede()
	{
		Vector3 endPosition = new Vector3(0f, -(transform.lossyScale.y/2), 0f);
		endPosition = transform.TransformPoint(endPosition);

		float startTime = Time.time;
		while (Time.time - startTime <= durration)
		{
			float timePercent = (Time.time - startTime) / durration;
			transform.position = Vector3.Lerp(startPosition, endPosition, timePercent);
			yield return null;
		}

		print("Running!");
		triggerScript.DoTrigger();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(triggered) { return;  }
		RocketShip rocketship = collision.gameObject.GetComponent<RocketShip>();

		if (rocketship)
		{
			if (audioSource.clip)
			{
				audioSource.Play();
			}

			StartCoroutine("Recede");
			triggered = true;
		}
	}

}
