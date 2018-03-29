using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour, Triggerable {

	[SerializeField] private Vector3 movementVector;
	[SerializeField] private float speed = 1f;
	[Space]
	[SerializeField] private float debugSphereRadius = 0.5f;

	private Vector3 startingPos;
	[SerializeField] private bool active = false;

	void Start () {
		startingPos = transform.position;
	}
	
	void Update () {
		if (active && Vector3.Distance(transform.position, startingPos + movementVector) > .6)
		{
			transform.position += (movementVector.normalized * speed) * Time.deltaTime;
		}
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
