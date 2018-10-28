using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
//resouce: https://vilbeyli.github.io/Projectile-Motion-Tutorial-for-Arrows-and-Missiles-in-Unity3D/
public class Projectile : MonoBehaviour {

	[SerializeField]private Transform targetTransform;

	[Header("Bullet Stats")]
	public float bulletSpeed;
	public float damage;
	public bool acceleration = true;
	[HideInInspector]

	private bool ready = false;
	private bool grounded;

	private Rigidbody rigid;

	public GameObject startPos;

	private Vector3 initialPosition;
	private Quaternion initialRotation;

	public float despawnTime = 10f;
	//Debug 
	public bool debug;
	[Range(1.0f, 6.0f)] public float distance;
	[Range(00.0f, 90.0f)] public float angle;

	// Use this for initialization
	void Awake () {
		rigid = GetComponent<Rigidbody> ();
		ready = true;
		initialPosition = startPos.transform.position;
		initialRotation = startPos.transform.rotation;
		grounded = false;
	}

	void Update()
	{
		//mid air
		if (!ready && !grounded) {
			
			if(acceleration)
				rigid.AddForce(transform.forward * bulletSpeed);
			else
				transform.position += transform.forward * bulletSpeed * Time.deltaTime;
		}

		if (debug) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (ready)
					Launch ();
				else {
					Reset ();
					SetNewTarget ();
				}
			}
			if (Input.GetKeyDown (KeyCode.R))
				Reset ();
		}
	}
	public void Launch(){

		StartCoroutine (Despawner (despawnTime));
		Vector3 dist = targetTransform.transform.position - this.transform.position;
		dist.Normalize();
		transform.forward = dist;

		
		if (acceleration)
			rigid.AddForce (transform.forward * bulletSpeed, ForceMode.Impulse);
		ready = false;

		/*
		Vector3 projectileXZPos = new Vector3 (transform.position.x, 0, transform.position.z);
		Vector3 targetXZPos = new Vector3 (targetTransform.position.x, 0, targetTransform.position.z);

		transform.LookAt (targetTransform.position);


		float R = Vector3.Distance (projectileXZPos, targetXZPos);
		float G = Physics.gravity.y;
		float tanAlpha = Mathf.Tan (angle * Mathf.Deg2Rad);
		float H = targetTransform.position.y - transform.position.y;

		float Vz = Mathf.Sqrt (G * R * R / (2.0f * (H - R * tanAlpha)));
		float Vy = tanAlpha * Vz;

		Vector3 localVelocity = new Vector3 (0f, Vy, Vz);
		Vector3 globalVelocity = transform.TransformDirection (localVelocity);

		rigid.velocity = globalVelocity;*/


	}

	public void SetNewTarget(GameObject target){
		this.targetTransform = target.transform;

		ready = true;

	}


	public void SetNewTarget(){
		if (!debug)
			return;

		Transform targetTF = targetTransform.GetComponent<Transform> ();
		
		Vector3 rotationAxis = Vector3.up;
		float randomAngle = Random.Range (0, 360f);
		Vector3 randomVectorOnGroundPlane = Quaternion.AngleAxis (randomAngle,rotationAxis) * Vector3.right;
		
		Vector3 randomPoint = randomVectorOnGroundPlane * distance + new Vector3 (0, targetTF.position.y, 0);
		
		targetTransform.SetPositionAndRotation (randomPoint, targetTF.rotation);
		ready = true;
		
		
	}

	public void Reset(Vector3 pos, Quaternion rot){
		this.gameObject.SetActive (true);
		rigid.velocity = Vector3.zero;
		this.transform.position = pos;
		//this.transform.SetPositionAndRotation (pos, rot);
		ready = false;

	}

	public void Reset(){
		this.gameObject.SetActive (true);
		rigid.velocity = Vector3.zero;
		rigid.angularVelocity = Vector3.zero;

		this.transform.SetPositionAndRotation (initialPosition, initialRotation);
		ready = false;
	
	}



	void OnTriggerEnter(Collider other){
		if (other.tag == "Ground" || other.tag == "Enemy") {
		
			grounded = true;
			this.gameObject.SetActive (false);

			if(other.tag == "Enemy")
			other.GetComponent<EnemyBase>().TakeDamage(1);
		}
	}

	void OnCollisionExit(){
		grounded = false;
	}

	IEnumerator Despawner(float time)
	{
		yield return new WaitForSeconds(time);
		rigid.velocity = Vector3.zero;
		rigid.angularVelocity = Vector3.zero;
		this.gameObject.SetActive(false);
	}


	
}
}
