using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

	[SerializeField] private GameObject target = null;
	[SerializeField] private bool deTrigger = false;

	private ITriggerable triggerTarget = null;

	private void Start()
	{
		if (target == null)
		{
			Debug.LogWarning(gameObject.name + "Target object cannot be null!");
			this.enabled = false;
		}

		Component[] components = target.GetComponents(typeof(Component));
		foreach (Component component in components)
		{
			if(component is ITriggerable)
			{
				triggerTarget = (ITriggerable)component;
				return;
			}
		}

		Debug.LogWarning(gameObject + " Improper Target type set! Turning off script!");
		this.enabled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag != "Player") { return;  }

		if (deTrigger && triggerTarget.IsTriggered())
			triggerTarget.DeTrigger();

		if (!deTrigger && !triggerTarget.IsTriggered())
			triggerTarget.Trigger();

	}
}
