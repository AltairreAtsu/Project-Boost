using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScroll : MonoBehaviour {

	private Renderer Renderer = null;
	[SerializeField] private Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
	private Vector2 uvOffset = Vector2.zero;

	// Use this for initialization
	void Start () {
		Renderer = GetComponent<Renderer>();
	}

	void Update () {
		uvOffset += (uvAnimationRate * Time.deltaTime);
		Renderer.material.SetTextureOffset("_MainTex", uvOffset);
	}
}
