using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour 
{

	public GameObject objectForPool;
	public int maxSize;


	private LinkedList<GameObject> pool;


	public void ReturnObject(GameObject returnedObject)
	{
		CreatingPool poolIdentifier = returnedObject.GetComponent<CreatingPool>();
		if (poolIdentifier == null || poolIdentifier.originalPool != this)
		{
			Destroy(returnedObject);
		}

		if (pool.Count < maxSize)
		{
			pool.AddLast(returnedObject);
		}
		else
		{
			Destroy(returnedObject);
		}
	}


	public GameObject GetObject()
	{
		GameObject objectToReturn = null;
		if (pool.Count > 0)
		{
			objectToReturn = pool.First.Value;
			pool.RemoveFirst();
			return objectToReturn;
		}
		else
		{
			Debug.Log("Instantiate");
			objectToReturn = Instantiate(objectForPool, Vector3.zero, Quaternion.identity);
			CreatingPool poolIdentifier = objectToReturn.AddComponent<CreatingPool>();
			poolIdentifier.originalPool = this;
			objectToReturn.SetActive(false);
		}
		return objectToReturn;
	}


	void Awake()
	{
		pool = new LinkedList<GameObject>();
	}
}
