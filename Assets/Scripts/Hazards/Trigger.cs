using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
	[SerializeField] private TriggerTarget[] targets;

	private ITriggerable[] triggerTargets;
	private bool doCollisionCheck = true;

	private void Start()
	{
		if (GetComponent<Pickup>()) { doCollisionCheck = false; }
		if (GetComponent<PressurePlate>()) { doCollisionCheck = false; }

		ErrorHandling();

		InitializeTriggerTargets();
	}

	private void ErrorHandling()
	{
		if (targets.Length <= 0)
		{
			throw new System.Exception(gameObject.name + "Targets Array length must have at least one element!");
		}

		foreach (TriggerTarget target in targets)
		{
			if (!target.target)
			{
				throw new System.Exception(gameObject.name + "Trigger Target game objects cannot be null!");
			}
		}
	}

	private void InitializeTriggerTargets()
	{
		triggerTargets = new ITriggerable[targets.Length];
		for (int i = 0; i < targets.Length; i++)
		{
			triggerTargets[i] = CastTargetToTriggerable(targets[i].target);
		}
	}

	private ITriggerable CastTargetToTriggerable(GameObject target)
	{
		if (!target)
		{
			throw new System.Exception(gameObject.name + ": Trigger target cannot be null!");
		}

		Component[] components = target.GetComponents(typeof(Component));
		foreach (Component component in components)
		{
			if (component is ITriggerable)
			{
				return (ITriggerable)component;
			}
		}

		throw new System.Exception(gameObject.name + ": Trigger target has no Triggerable script!");
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag != "Player" || !doCollisionCheck) { return;  }

		DoTrigger();
	}

	public void DoTrigger()
	{
		for (int i = 0; i < triggerTargets.Length; i++)
		{
			if (targets[i].deTrigger && triggerTargets[i].IsTriggered())
				triggerTargets[i].DeTrigger();

			if (!targets[i].deTrigger && !triggerTargets[i].IsTriggered())
				triggerTargets[i].Trigger();
		}
	}
}
