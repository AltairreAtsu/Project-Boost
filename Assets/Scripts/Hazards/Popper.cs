using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popper : MonoBehaviour, ITriggerable {

	private enum States { Idle, Popping, PopStick, Returning, StartDelay }
	private States state = States.Idle;

	private Vector3 startPosition = Vector3.zero;
	private Vector3 endPosition = Vector3.zero;

	private float lastStep = 0f;
	private float pauseTime = 0f;

	[SerializeField] private float distance = 5f;
	[Space]
	[SerializeField] private float idleTime = 3f;
	[SerializeField] private float popTime = 1f;
	[SerializeField] private float popStickTime = 1f;
	[SerializeField] private float StartDelay = 0f;
	[Space]
	[SerializeField] private bool active = true;
	[Space]
	[SerializeField] private float debugSphereRaidus = 0.5f;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		lastStep = Time.time;
		endPosition = startPosition + transform.up * (distance - (transform.lossyScale.y / 2));

		if(StartDelay > 0)
		{
			state = States.StartDelay;
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch (state)
		{
			case States.StartDelay:
				Idle(StartDelay);
				break;
			case States.Idle:
				Idle(idleTime);
				break;
			case States.Popping:
				Pop(false);
				break;
			case States.PopStick:
				Idle(popStickTime);
				break;
			case States.Returning:
				Pop(true);
				break;
		}
	}

	private void Idle(float durration)
	{
		if(Time.time - lastStep >= durration)
		{
			state = (state == States.PopStick) ? States.Returning : States.Popping;

			lastStep = Time.time;
			return;
		}
	}

	private void Pop(bool returning)
	{
		float timePassed = Time.time - lastStep;
		float timePercent = timePassed / popTime;

		if (timePercent >= 1f)
		{
			state = (state == States.Popping) ? States.PopStick : States.Idle;
			lastStep = Time.time;
			return;
		}

		if (!returning)
		{
			transform.position = Vector3.Lerp(startPosition, endPosition, timePercent);
		}
		else
		{
			transform.position = Vector3.Lerp(endPosition, startPosition, timePercent);
		}
		
	}

	public void Trigger()
	{
		active = true;
		lastStep = (pauseTime - lastStep) - Time.time;
	}

	public void DeTrigger()
	{
		active = false;
		pauseTime = Time.time;
	}

	public bool IsTriggered()
	{
		return active;
	}

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{
			if (startPosition != transform.position)
			{
				startPosition = transform.position;
			}
			if(endPosition != startPosition + transform.up * distance)
			{
				endPosition = startPosition + transform.up * distance;
			}
		}


		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(startPosition, endPosition);
		Gizmos.DrawSphere(startPosition, debugSphereRaidus);		
		Gizmos.DrawSphere(endPosition, debugSphereRaidus);
	}
}
