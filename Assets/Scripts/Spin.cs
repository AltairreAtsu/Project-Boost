using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {

	[SerializeField] private float spinSpeed = 10f;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0f, spinSpeed * Time.deltaTime, 0f));
	}
}
