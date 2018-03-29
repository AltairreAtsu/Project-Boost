using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour, Triggerable
{

	[SerializeField] private Vector3 movementVector = new Vector3(10f, 0, 0);
	[SerializeField] private float period = 2f;
	[SerializeField] private bool active = true;
	[Space]
	[SerializeField] private float debugSphereRadius = 0.5f;

	private Vector3 startingPos;

	void Start ()
	{
		startingPos = transform.position;
		if (period <= Mathf.Epsilon)
		{
			Debug.LogError(gameObject.name + ": Ossilation Period must be greater than 0!");
			this.enabled = false;
		}
	}
	

	void Update ()
	{
		if(!active) { return; }

		float cycles = Time.time / period;

		const float tau = Mathf.PI * 2;
		float rawSineWave = Mathf.Sin(cycles * tau);

		float movmentFactor = rawSineWave / 2f + 0.5f;
		transform.position = startingPos + movementVector * movmentFactor;
	}

	private void OnDrawGizmos()
	{
		if (startingPos != transform.position && !Application.isPlaying)
			startingPos = transform.position;

		Gizmos.color = Color.yellow;
		Vector3 endVector = movementVector + startingPos;
		Gizmos.DrawLine(startingPos, endVector);
		Gizmos.DrawSphere(startingPos, debugSphereRadius);
		Gizmos.DrawSphere(endVector, debugSphereRadius);
	}

	public void Trigger()
	{
		active = true;
	}

	public void DeTrigger()
	{
		active = false;
	}

	public bool IsTriggered()
	{
		return active;
	}
}
