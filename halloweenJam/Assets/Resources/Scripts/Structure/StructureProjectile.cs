using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureProjectile : MonoBehaviour {

	public Vector3 pos;
	public Vector3 target;
	public Vector3 orientation;

	public bool midAir = false;

	private int numProjectilePoints = 30;
	private List<GameObject> trajectoryPoints;


	[Header("Trajectory Debug")]

	public StructureProjectile(Vector3 pos, Vector3 target, Vector3 orientation){


	
	}
	// Use this for initialization
	void Start () {
		trajectoryPoints = new List<GameObject> ();

		for (int i = 0; i < numProjectilePoints; i++) {
			GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);

			trajectoryPoints.Add(dots);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Shoot(){

	}
}
