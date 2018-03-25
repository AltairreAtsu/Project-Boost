using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

	[SerializeField] private Vector3 movementVector = new Vector3(10f, 0, 0);
	[SerializeField] private float period = 2f;
	[Space]
	[SerializeField] private float debugSphereRadius = 0.5f;

	private Vector3 startingPos;

	// Use this for initialization
	void Start ()
	{
		startingPos = transform.position;
		if (period <= Mathf.Epsilon)
		{
			Debug.LogError(gameObject.name + ": Ossilation Period must be greater than 0!");
			this.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
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
}
