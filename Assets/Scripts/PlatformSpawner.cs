using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour 
{

	public static float lowestPoint;


	public ObjectPool basePlatformPool;
	public ObjectPool movingPlatformPool;
	public ObjectPool breakablePlatformPool;
	public float maxVerticalOffset;
	public float platformVerticalOffset;
	public float platformRandomAddend;
	public float halfPlatformSize;
	public float timeToMaxDifficulty;
	[Range(0.0f,1.0f)] public float maxMovingPlatformChance;
	[Range(0.0f,1.0f)] public float maxBreakablePlatformChance;


	private new Transform camera;
	private LinkedList<GameObject> platforms;
	private float activeFieldVerticalSize;
	private float activeFieldHorizontalSize;
	private float highestPoint;
	private float roundStartTime;


	public float GetActiveFieldHorizontalSize()
	{
		return activeFieldHorizontalSize;
	}


	void Awake () 
	{
		platforms = new LinkedList<GameObject>();
		camera = Camera.main.transform;
		activeFieldVerticalSize = Camera.main.orthographicSize * 2;
		activeFieldHorizontalSize = Camera.main.orthographicSize * Camera.main.aspect;
		PlayerController.OnPlatformCollide += ErasePlatforms;
		PlayerController.OnPlatformCollide += SpawnPlatforms;
	}


	void Start()
	{
		highestPoint = camera.position.y - activeFieldVerticalSize;
		lowestPoint = highestPoint;
		roundStartTime = Time.time;
		SpawnPlatforms();
	}


	void SpawnPlatforms()
	{
		float activeFieldHighestPoint = camera.position.y + activeFieldVerticalSize;

		while (activeFieldHighestPoint > highestPoint)
		{
			float difficultyModifier = GetDifficulty();
			//Debug.Log(difficultyModifier);
			float platformYPos = highestPoint;
			platformYPos += Mathf.Lerp(Random.Range(platformVerticalOffset - platformRandomAddend, platformVerticalOffset + platformRandomAddend), maxVerticalOffset, difficultyModifier);
			Vector3 randomPosition = new Vector3(Random.Range(-(activeFieldHorizontalSize - halfPlatformSize), 
					                         (activeFieldHorizontalSize - halfPlatformSize)), platformYPos, 0.0f);
			GameObject platform = null;
			float movingPlatformChance = maxMovingPlatformChance * difficultyModifier;
			float breakablePlatformChance = maxBreakablePlatformChance * difficultyModifier;
			if (Random.Range(0.0f, 1.0f) < movingPlatformChance)
			{
				platform = movingPlatformPool.GetObject();
			}
			else if (Random.Range(0.0f, 1.0f) < breakablePlatformChance)
			{
				platform = breakablePlatformPool.GetObject();
				platform.GetComponent<BreakablePlatform>().Reset();
			}
			else
			{
				platform = basePlatformPool.GetObject();
			}
			platform.transform.position = randomPosition;
			platform.SetActive(true);
			platform.transform.SetParent(transform);
			platforms.AddLast(platform);
			highestPoint = randomPosition.y;
			MovingPlatform movingPlatform = platform.GetComponent<MovingPlatform>();
			if (movingPlatform != null)
			{
				movingPlatform.StartMovement();
			}
		}
	}


	void ErasePlatforms()
	{
		GameObject lowestPlatform = platforms.First.Value;
		lowestPoint = camera.transform.position.y - activeFieldVerticalSize / 2;

		while (lowestPlatform.transform.position.y < lowestPoint)
		{
			lowestPlatform.SetActive(false);
			platforms.RemoveFirst();
			lowestPlatform.GetComponent<CreatingPool>().originalPool.ReturnObject(lowestPlatform);
			lowestPlatform = platforms.First.Value;
		}
	}


	void OnDestroy()
	{
		PlayerController.OnPlatformCollide -= SpawnPlatforms;
		PlayerController.OnPlatformCollide -= ErasePlatforms;
	}


	float GetDifficulty()
	{
		float difficultyModifier = (Time.time - roundStartTime) / timeToMaxDifficulty;
		if (difficultyModifier > 1.0f)
		{
			difficultyModifier = 1.0f;
		}
		return difficultyModifier;
	}
}
