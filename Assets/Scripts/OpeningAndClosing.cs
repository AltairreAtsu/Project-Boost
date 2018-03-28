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

	private Vector3 startingPositionTop = Vector3.zero;
	private Vector3 startingPositionBottom = Vector3.zero;
	private Vector3 endingPointTop = Vector3.zero;
	private Vector3 endingPointBottom = Vector3.zero;
	Vector3 middlePoint = Vector3.zero;

	private float lastStep = 0f;

	private enum State { Convering, Diverging, Delay }
	private State state = State.Convering;

	// Use this for initialization
	void Start () {
		startingPositionTop = topWall.position;
		startingPositionBottom = bottomWall.position;

		//float y = (startingPositionTop.y + startingPositionBottom.y) / 2;
		//float x = (startingPositionTop.x + startingPositionTop.x) / 2;

		//endingPointTop = new Vector3(topWall.position.x + (topWall.localScale.x), topWall.position.y - (topWall.localScale.y), topWall.position.z);
		//endingPointBottom = new Vector3(bottomWall.position.x + (topWall.localScale.x), bottomWall.position.y + (bottomWall.localScale.y), bottomWall.position.z);

		//endingPointTop = new Vector3(x * 1.5f, y * 1.5f, startingPositionTop.z);
		//endingPointBottom = new Vector3(x * .5f, y * .5f, endingPointBottom.z);

		//endingPointTop = new Vector3((startingPositionTop.x + middlePoint.x) * (2 / 3), (startingPositionTop.y + middlePoint.y) * (2/3), startingPositionTop.z);
		//endingPointBottom = new Vector3((startingPositionBottom.x + middlePoint.x) * (2 / 3), (startingPositionBottom.y + middlePoint.y) * (2/3), startingPositionBottom.z);



		float y = (startingPositionTop.y + startingPositionBottom.y) / 2;
		float x = (startingPositionTop.x + startingPositionBottom.x) / 2;
		middlePoint = new Vector3(x, y, startingPositionTop.z);

		endingPointTop = new Vector3( (startingPositionTop.x) + 4 * (middlePoint.x - startingPositionTop.x), (startingPositionTop.y) + 4 * (middlePoint.y - startingPositionTop.y), startingPositionTop.z);
		endingPointBottom = new Vector3((startingPositionBottom.x) + 4 * (middlePoint.x - startingPositionBottom.x), (startingPositionBottom.y) + 4 * (middlePoint.y - startingPositionBottom.y), startingPositionBottom.z);

		lastStep = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(state == State.Convering)
		{
			float timeProgressed = Time.time - lastStep;
			float timePercent = timeProgressed / closingTime;
			topWall.position = Vector3.Lerp(startingPositionTop, endingPointTop, timePercent);
			bottomWall.position = Vector3.Lerp(startingPositionBottom, endingPointBottom, timePercent);
		}
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
