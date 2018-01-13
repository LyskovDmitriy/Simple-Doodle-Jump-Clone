using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{

	public static event System.Action OnPlatformCollide; 
	public static event System.Action OnPlayerDeath; 

	public Text scoreText;
	public float gravityScale;
	public float horizontalSpeed;
	public float horizontalDrag;
	public float platformCollisionSpeed;
	public float halfPayerWidthInWorldUnits;


	private Rigidbody2D rb;
	private new Collider2D collider;
	private int currentScore;
	private float hMovement;
	private float halfScreenSize;
	private float startingPosition;

	
	void Awake () 
	{
		rb = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();
		OnPlayerDeath += GameOver;
	}


	void Start()
	{
		halfScreenSize = Camera.main.orthographicSize * Camera.main.aspect;
		startingPosition = transform.position.y;
		currentScore = 0;
		scoreText.text = currentScore.ToString();
	}
	
	
	void Update () 
	{
		#if UNITY_EDITOR_WIN
			hMovement = Input.GetAxisRaw("Horizontal");
		#elif UNITY_ANDROID
		Vector3 input = Input.acceleration;
		hMovement = input.x * 2;
		#endif
	}


	void FixedUpdate()
	{
		Vector2 movement = rb.velocity + new Vector2(hMovement * horizontalSpeed, -gravityScale * Time.fixedDeltaTime);
		if (movement.x > 0)
		{
			movement.x = Mathf.Clamp(movement.x - horizontalDrag * Time.fixedDeltaTime, 0, horizontalSpeed);
		}
		else
		{
			movement.x = Mathf.Clamp(movement.x + horizontalDrag * Time.fixedDeltaTime, -horizontalSpeed, 0);
		}
		rb.velocity = movement;
		Vector3 currentPosition = transform.position;
		if (currentPosition.x > halfScreenSize + halfPayerWidthInWorldUnits)
		{
			currentPosition.x = -(halfScreenSize + halfPayerWidthInWorldUnits);
			transform.position = currentPosition;
		}
		else if (currentPosition.x < -(halfScreenSize + halfPayerWidthInWorldUnits))
		{
			currentPosition.x = halfScreenSize + halfPayerWidthInWorldUnits;
			transform.position = currentPosition;
		}
		//scoreText.text = Input.acceleration.ToString();
		if (movement.y > 0)
		{
			int newScore = Mathf.RoundToInt(currentPosition.y - startingPosition);
			if (newScore > currentScore)
			{
				currentScore = newScore;
				scoreText.text = currentScore.ToString();
			}
			collider.enabled = false;
		}
		else
		{
			collider.enabled = true;
		}
		if (transform.position.y < PlatformSpawner.lowestPoint)
		{
			if (OnPlayerDeath != null)
			{
				OnPlayerDeath();
			}
		}
	}


	void OnCollisionEnter2D()
	{
		rb.velocity = Vector2.up * platformCollisionSpeed;

		if (OnPlatformCollide != null)
		{
			OnPlatformCollide();
		}
	}


	void GameOver()
	{
		if ((PlayerPrefs.HasKey("Max score") && currentScore > PlayerPrefs.GetInt("Max score")) || !PlayerPrefs.HasKey("Max score"))
		{

			PlayerPrefs.SetInt("Max score", currentScore);
		}
		PlayerPrefs.SetInt("Current score", currentScore);
		OnPlayerDeath -= GameOver;
		Destroy(gameObject);
	}


	void OnDestroy()
	{
		OnPlayerDeath -= GameOver;
	}
}
