using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

	[SerializeField] private GameObject target = null;

	private enum Triggerables { MovingWall, Passage, None }
	[SerializeField] Triggerables triggerType = Triggerables.None;

	private Triggerable triggerTarget = null;

	private void Start()
	{
		if (target == null)
		{
			Debug.LogWarning(gameObject.name + "Target object cannot be null!");
			this.enabled = false;
		}
		if(triggerType == Triggerables.None)
		{
			Debug.LogWarning(gameObject.name + " trigger tpye must not be none! Turning off script!");
			this.enabled = false;
		}
		if (triggerType == Triggerables.MovingWall)
		{
			MovingWall wall = target.GetComponent<MovingWall>();
			if (wall != null)
			{
				triggerTarget = (Triggerable)wall;
				return;
			}							
		}
		if (triggerType == Triggerables.Passage)
		{
			OpeningAndClosing passage = target.GetComponent<OpeningAndClosing>();
			if (passage != null)
			{
				triggerTarget = (Triggerable)passage;
				return;
			}
		}

		Debug.LogWarning(gameObject + " Improper Target type set! Turning off script!");
		this.enabled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(!triggerTarget.IsTriggered() && other.tag == "Player")
			triggerTarget.Trigger();
	}
}
