using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{

	public GameObject breakingEffect;


	private Rigidbody2D rb;
	private new Collider2D collider;


	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();
	}


	public void Reset()
	{
		rb.bodyType = RigidbodyType2D.Kinematic;
		collider.enabled = true;
	}
	

	void OnCollisionEnter2D () 
	{
		Instantiate(breakingEffect, transform.position, Quaternion.identity);
		rb.bodyType = RigidbodyType2D.Dynamic;
		collider.enabled = false;
	}
}
