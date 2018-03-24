using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {

	[SerializeField] private Transform target = null;
	//[SerializeField] private float speed = 0.3f;
	[SerializeField] private float breakThreshold = 0.5f;
	[SerializeField] private float marginOfError = 0.1f;
	[SerializeField] private float followDistance = 3f;

	private bool panning = false;
	private Vector3 differentialVetcor;

	private void Start()
	{
		//differentialVetcor = new Vector3( DistanceInX(transform.position, target.position), 0f, 0f);
		differentialVetcor = transform.position - target.position;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(differentialVetcor.x + target.position.x, transform.position.y, transform.position.z);
	}

	private float DistanceInX(Vector3 start, Vector3 end)
	{
		return Mathf.Abs(start.x - end.x);
	}
}
