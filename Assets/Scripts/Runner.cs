using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Runner : MonoBehaviour {

	[SerializeField]
		private Rigidbody2D _rb = null;
	[SerializeField]
		private float _speed = 5;

	// Use this for initialization
	void Awake () {
		if (!_rb)
		{
			Debug.LogError("Runner's Rigidbody not found");
			Debug.Break();
		}
	}
	
	// Update is called once per frame
	void Update () {
		_rb.velocity = Vector2.right * _speed;
	}
}
