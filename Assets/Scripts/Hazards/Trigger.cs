using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

	[SerializeField] private GameObject target = null;
	[SerializeField] private bool deTrigger = false;

	private ITriggerable triggerTarget = null;
	private bool onPickup = false;

	private void Start()
	{
		if(GetComponent<Pickup>()) { onPickup = true; }

		if (!target)
		{
			throw new System.Exception(gameObject.name + ": Trigger target cannot be null!");
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

		throw new System.Exception(gameObject.name + ": Trigger target has no Triggerable script!");
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag != "Player" || onPickup) { return;  }

		DoTrigger();
	}

	public void DoTrigger()
	{
		if (deTrigger && triggerTarget.IsTriggered())
			triggerTarget.DeTrigger();

		if (!deTrigger && !triggerTarget.IsTriggered())
			triggerTarget.Trigger();
	}
}
