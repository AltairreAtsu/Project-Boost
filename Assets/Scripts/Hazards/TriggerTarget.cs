using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TriggerTarget {
	[SerializeField] public GameObject target;
	[SerializeField] public bool deTrigger;
}
