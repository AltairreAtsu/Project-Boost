using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirHazard : MonoBehaviour {
	[SerializeField] private float recoilAmount = 300f;

	public float GetRecoil()
	{
		return recoilAmount;
	}
}
