using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {

	[SerializeField] private Transform target = null;
	[SerializeField] private bool panUp = false;
	private Vector3 differentialVetcor;


	private void Start()
	{
		differentialVetcor = transform.position - target.position;
	}

	// Update is called once per frame
	void Update () {
		if (panUp)
		{
			transform.position = new Vector3(transform.position.x, target.position.y + differentialVetcor.y, transform.position.z);
		}
		else
		{
			transform.position = new Vector3(differentialVetcor.x + target.position.x, transform.position.y, transform.position.z);
		}
		
	}

	private float DistanceInX(Vector3 start, Vector3 end)
	{
		return Mathf.Abs(start.x - end.x);
	}
}
