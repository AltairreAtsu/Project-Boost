using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointObject : MonoBehaviour {

	[SerializeField] private Vector3[] wayPoints;
	[SerializeField] private float totalDurration = 5f;
	[Space]
	[SerializeField] private bool doBackTrack = false;
	[Space]
	[SerializeField] private float debugSphereRadius = 0.5f;

	private Vector3 startPosition = Vector3.zero;

	private float totalDistance = 0f;
	private float[] distances = null;
	private int section = 0;

	private float lastStep = 0f;

	private bool backTracking = false;
	//private bool lastLeg = false;

	void Start () {
		if(wayPoints.Length <= 0)
		{
			Debug.LogWarning(gameObject.name + " Not Enough Waypoints! Waypoint array must be larger than 0! Disabling script!");
			this.enabled = false;
		}

		lastStep = Time.time;
		startPosition = transform.position;

		CalculateDistances();
	}
	
	void Update ()
	{
		if (!backTracking)
		{
			float timePercent = CalculateTimePercent(distances[section]);
			MoveForward(timePercent);
			CheckPathProgressionForward();
		}
		else
		{
			float timePercent = CalculateTimePercent(distances[section+1]);
			MoveBackward(timePercent);
			CheckPathProgressionBackward();
		}

	}

	private void MoveForward(float timePercent)
	{
		if (section == 0)
		{
			transform.position = Vector3.Lerp(startPosition, wayPoints[section] + startPosition, timePercent);
		}
		else
		{
			transform.position = Vector3.Lerp(wayPoints[section - 1] + startPosition, wayPoints[section] + startPosition, timePercent);
		}
	}

	private void MoveBackward(float timePercent)
	{
		if (section < 0)
		{
			transform.position = Vector3.Lerp(wayPoints[section+1] + startPosition, startPosition, timePercent);
		}
		else
		{
			transform.position = Vector3.Lerp(wayPoints[section + 1] + startPosition, wayPoints[section] + startPosition, timePercent);
		}
	}

	private void CheckPathProgressionForward()
	{
		bool endOfPath = section + 1 == wayPoints.Length;

		if (!endOfPath && IsAtWaypoint(wayPoints[section]))
		{
			section++;
			lastStep = Time.time;
		}
		else if (endOfPath && IsAtWaypoint(wayPoints[wayPoints.Length - 1]))
		{
			if (doBackTrack && !backTracking)
			{
				backTracking = true;
				section--;
				lastStep = Time.time;
			}
			print("End Of Path Reached!");
		}
	}

	private void CheckPathProgressionBackward()
	{
		bool startOfPath = section == -1;

		if (!startOfPath && IsAtWaypoint(wayPoints[section]))
		{
			section--;
			lastStep = Time.time;
		}
		else if (startOfPath && transform.position == startPosition)
		{
			backTracking = false;
			section++;
			lastStep = Time.time;
			print("Start Of Path Reached!");
		}
	}

	private float CalculateTimePercent(float distance)
	{
		float sectionDurration = totalDurration * (distance / totalDistance);
		float passedTime = (Time.time - lastStep);
		return Mathf.Clamp(passedTime / sectionDurration, 0f, 1f);
	}

	private bool IsAtWaypoint(Vector3 waypoint)
	{
		return (transform.position == waypoint + startPosition);
	}

	private void CalculateDistances()
	{
		distances = new float[wayPoints.Length];
		for (int i = 0; i < wayPoints.Length; i++)
		{
			if (i == 0)
			{
				distances[i] = Vector3.Distance(transform.position, wayPoints[i] + transform.position);
				totalDistance += distances[i];
				continue;
			}

			distances[i] = Vector3.Distance(wayPoints[i - 1] + transform.position, wayPoints[i] + transform.position);
			totalDistance += distances[i];
		}
	}

	private void OnDrawGizmos()
	{
		if (wayPoints.Length <= 0)
		{
			return;
		}

		if (startPosition != transform.position && !Application.isPlaying)
			startPosition = transform.position;

		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(startPosition, debugSphereRadius);
		
		for(int i = 0; i < wayPoints.Length ; i++)
		{
			if (i == 0)
			{
				Gizmos.DrawLine(startPosition, wayPoints[i] + startPosition);
				Gizmos.DrawSphere(wayPoints[i] + startPosition, debugSphereRadius);
				continue;
			}
			Gizmos.DrawLine(wayPoints[i - 1] + startPosition, wayPoints[i] + startPosition);
			Gizmos.DrawSphere(wayPoints[i] + startPosition, debugSphereRadius);

		}
	}
}
