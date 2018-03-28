using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningAndClosing : MonoBehaviour {

	[SerializeField] private Transform topWall = null;
	[SerializeField] private Transform bottomWall = null;
	[Space]
	[SerializeField] private float openingTime = 1f;
	[SerializeField] private float closingTime = 1f;
	[SerializeField] private float delayTime = 1f;
	[Space]
	[SerializeField] private bool active = true;
	[SerializeField] private bool lockAndKey = false;

	private Vector3 startingPositionTop = Vector3.zero;
	private Vector3 startingPositionBottom = Vector3.zero;
	private Vector3 endingPointTop = Vector3.zero;
	private Vector3 endingPointBottom = Vector3.zero;
	Vector3 middlePoint = Vector3.zero;

	private float lastStep = 0f;

	private bool opening = false;
	

	private enum State { Converging, Diverging, Delay }
	private State state = State.Converging;

	void Start () {
		startingPositionTop = topWall.position;
		startingPositionBottom = bottomWall.position;

		float y = (startingPositionTop.y + startingPositionBottom.y) / 2;
		float x = (startingPositionTop.x + startingPositionBottom.x) / 2;
		middlePoint = new Vector3(x, y, startingPositionTop.z);

		endingPointTop = ((startingPositionTop - middlePoint)*.75f )*-1;
		endingPointBottom = ((startingPositionBottom - middlePoint) * .75f) * -1;

		lastStep = Time.time;

		if (!active)
		{
			state = State.Delay;
			topWall.position = endingPointTop + startingPositionTop;
			bottomWall.position = endingPointBottom + startingPositionBottom;
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
		topWall.position = Vector3.Lerp(endingPointTop + startingPositionTop, startingPositionTop, timePercent);
		bottomWall.position = Vector3.Lerp(endingPointBottom + startingPositionBottom, startingPositionBottom, timePercent);

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
		topWall.position = Vector3.Lerp(startingPositionTop, endingPointTop + startingPositionTop, timePercent);
		bottomWall.position = Vector3.Lerp(startingPositionBottom, endingPointBottom + startingPositionBottom, timePercent);

		bool topWallInPosition = topWall.position == endingPointTop + startingPositionTop;
		bool bottomWallInPosition = bottomWall.position == endingPointBottom + startingPositionBottom;
		if (topWallInPosition && bottomWallInPosition)
		{
			lastStep = Time.time;
			state = State.Delay;
			opening = true;
		}
	}

	public void Activate()
	{
		active = true;
		lastStep = Time.time;
		state = State.Diverging;
	}

	private void OnDrawGizmos()
	{
		
		if (topWall == null && bottomWall == null) { return; }
		if (!Application.isPlaying)
		{
			startingPositionTop = topWall.position;
			startingPositionBottom = bottomWall.position;
			float y = (startingPositionTop.y + startingPositionBottom.y) / 2;
			float x = (startingPositionTop.x + startingPositionBottom.x) / 2;
			middlePoint = new Vector3(x, y, startingPositionTop.z);
			endingPointTop = new Vector3(topWall.position.x - (topWall.localScale.x / 2), topWall.position.y - (topWall.localScale.y / 2), topWall.position.z);
		}
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(middlePoint, 0.5f);
	}
}
