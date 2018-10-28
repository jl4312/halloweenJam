using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureTurrent : StructureBase {


	private List<GameObject> enemyList;

	private GameObject closestEnemy;

	public StructureTurrent(float health, float buildTime, float cost) : base(health, buildTime, cost)
	{
		enemyList = new List<GameObject> ();

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (alive) {
			
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (!alive) {
		}

		if (other.tag == "Enemy") {
			enemyList.Add(other.gameObject);

		}
	}



}
