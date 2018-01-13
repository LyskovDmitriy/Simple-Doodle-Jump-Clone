using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour 
{

	public float speed;
	

	private static float maxDistance;


	private Rigidbody2D rb;
	private bool movingRight;


	public void StartMovement()
	{
		movingRight = Random.Range(0, 2) > 0.5f;
		StartCoroutine("Move");
	}


	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		if (maxDistance == 0.0f)
		{
			PlatformSpawner platformSpawner = FindObjectOfType<PlatformSpawner>();
			float halfPlatformSize = platformSpawner.halfPlatformSize;
			float activeFieldHorizontalSize = platformSpawner.GetActiveFieldHorizontalSize();
			maxDistance = activeFieldHorizontalSize - halfPlatformSize;
		}
	}


	IEnumerator Move()
	{
		while (true)
		{
			Vector3 movement = ((movingRight == true) ? Vector2.right : Vector2.left) * speed * Time.deltaTime;
			movement += transform.position;
			movement.x = Mathf.Clamp(movement.x, -maxDistance, maxDistance);
			rb.MovePosition(movement);
			if (Mathf.Abs(movement.x) >= maxDistance)
			{
				movingRight = !movingRight;
			}
			yield return null;
		}
	}
}
