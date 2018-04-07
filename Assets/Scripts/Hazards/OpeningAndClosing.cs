using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningAndClosing : MonoBehaviour, ITriggerable {

	[SerializeField] private Transform topWall = null;
	[SerializeField] private Transform bottomWall = null;
	[Space]
	[SerializeField] private float openingTime = 1f;
	[SerializeField] private float closingTime = 1f;
	[SerializeField] private float delayTime = 1f;
	[Space]
	[Tooltip ("Wether a door is active at the start or not. If not active it will either remain closed or open depending ont he closed at start variable.")]
	[SerializeField] private bool active = true;
	[Tooltip("When a door is set as Lock and Key, it skips the delay state and remains in eithier opening or closing states.")]
	[SerializeField] private bool lockAndKey = false;
	[Tooltip ("Whether a door is closed at the start of the game when set as inactive.")]
	[SerializeField] private bool closedAtStart = false;
	[Tooltip ("Whether a door is closed by trigger or simply pausing in it's place.")]
	[SerializeField] private bool closeOnDeTrigger = false;
	

	private Vector3 startingPositionTop = Vector3.zero;
	private Vector3 startingPositionBottom = Vector3.zero;
	private Vector3 endingPointTop = Vector3.zero;
	private Vector3 endingPointBottom = Vector3.zero;

	private float lastStep = 0f;

	private bool opening = false;

	private enum State { Converging, Diverging, Delay }
	private State state = State.Converging;

	void Start () {
		startingPositionTop = topWall.position;
		startingPositionBottom = bottomWall.position;

		endingPointTop = transform.TransformPoint(new Vector3(0f, (topWall.localScale.y/2), 0f));
		endingPointBottom = transform.TransformPoint(new Vector3(0f, (bottomWall.localScale.y/2)*-1, 0f));
		


		lastStep = Time.time;

		if (!active && closedAtStart)
		{
			state = State.Delay;
			topWall.position = endingPointTop;
			bottomWall.position = endingPointBottom;
		}
		else
		{
			state = State.Delay;
		}
	}
	
	void Update () {
		if (!active) { return; }

		switch (state)
		{
			case State.Converging:
				Converge();
				break;
			case State.Diverging:
				Diverge();
				break;
			case State.Delay:
				Delay();
				break;
		}


		if (state == State.Diverging)
		{
			Diverge();
		}

	}

	private void Delay()
	{
		if (lockAndKey)
		{
			return;
		}

		if (Time.time - lastStep >= delayTime)
		{
			lastStep = Time.time;
			if (opening)
			{
				state = State.Diverging;
			}
			else
			{
				state = State.Converging;
			}
		}
	}

	private void Diverge()
	{
		float timeProgressed = Time.time - lastStep;
		float timePercent = timeProgressed / openingTime;
		topWall.position = Vector3.Lerp(endingPointTop, startingPositionTop, timePercent);
		bottomWall.position = Vector3.Lerp(endingPointBottom, startingPositionBottom, timePercent);

		bool topWallInPosition = topWall.position == startingPositionTop;
		bool bottomWallInPosition = bottomWall.position == startingPositionBottom;
		if (topWallInPosition && bottomWallInPosition)
		{
			lastStep = Time.time;
			state = State.Delay;
			opening = false;
		}
	}

	private void Converge()
	{
		float timeProgressed = Time.time - lastStep;
		float timePercent = timeProgressed / closingTime;

		topWall.position = Vector3.Lerp(startingPositionTop, endingPointTop, timePercent);
		bottomWall.position = Vector3.Lerp(startingPositionBottom, endingPointBottom, timePercent);

		bool topWallInPosition = topWall.position == endingPointTop;
		bool bottomWallInPosition = bottomWall.position == endingPointBottom;
		if (topWallInPosition && bottomWallInPosition)
		{
			lastStep = Time.time;
			state = State.Delay;
			opening = true;
		}
	}

	public void Trigger()
	{
		active = true;
		lastStep = Time.time;
		state = State.Diverging;
	}

	public void DeTrigger()
	{
		if (!closeOnDeTrigger)
		{
			active = false;
			lastStep = Time.time;
		}
		else
		{
			lastStep = Time.time;
			state = State.Converging;
		}

	}

	public bool IsTriggered()
	{
		return active;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere( transform.TransformPoint(new Vector3(0,0,0)) , 0.5f);
	}
}
