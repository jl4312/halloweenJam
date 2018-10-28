using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBase : MonoBehaviour {

	[Header("Structure Base Setting")]
	public float health;
	public float buildTime;
	public float cost;
	public bool alive;
	public bool building = true;

	public StructureBase(float health, float buildTime, float cost){
		this.health = health;
		this.buildTime = buildTime;
		this.cost = cost;
		alive = true;
		building = true;

	}

	// Update is called once per frame
	void Update () {
		if (alive) {
			
		}

		if (building)
			Build ();
		
	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0) {
			alive = false;
			this.gameObject.SetActive(false);
		}
	}

	public void Build()
	{
		buildTime -= Time.deltaTime;

		if(buildTime <= 0){
			building = false;
		}
	}






}
