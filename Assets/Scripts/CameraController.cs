using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{

	public Transform target;
	public float speed;


	private Vector3 offset;


	void Start () 
	{
		offset = transform.position - target.position;
	}
	

	void FixedUpdate () 
	{
		if (target != null)
		{
			Vector3 newPosition = Vector3.Lerp(transform.position, target.position + offset, speed * Time.fixedDeltaTime);
			newPosition.x = offset.x;
			transform.position = newPosition;
		}
	}
}
