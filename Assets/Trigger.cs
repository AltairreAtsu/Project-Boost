using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

	[SerializeField] private MovingWall target = null;

	private void Start()
	{
		if (target == null)
		{
			Debug.LogWarning(gameObject.name + "Target object cannot be null!");
			this.enabled = false;
		}
			
	}

	private void OnTriggerEnter(Collider other)
	{
		if(!target.IsTriggered())
			target.Trigger();
	}
}
