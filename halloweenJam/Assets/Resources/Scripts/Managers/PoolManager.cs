using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

	public GameObject gameObject;
	public int numObj;

	private List<GameObject> listOfChild;
	public bool randomSpawn;
	public float spawnRadius = 0;
	// Use this for initialization
	void Awake () {
		GameObject instance;
		listOfChild = new List<GameObject> ();

		for (int i = 0; i < numObj; i++) {

			listOfChild.Add((GameObject)Instantiate(gameObject, transform));
			if(randomSpawn)
				listOfChild[listOfChild.Count - 1].transform.position += new Vector3(Random.Range(-spawnRadius, spawnRadius), 0,Random.Range(-spawnRadius, spawnRadius));
		}



	}

	void SetAllActive(bool active){
		for (int i = 0; i < listOfChild.Count; i++)
			listOfChild [i].SetActive (active);
	}

}
