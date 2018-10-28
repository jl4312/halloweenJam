using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
//resouce: https://vilbeyli.github.io/Projectile-Motion-Tutorial-for-Arrows-and-Missiles-in-Unity3D/
public class ProjectileBase : MonoBehaviour {

	[SerializeField]protected Transform targetTransform;

	[Header("Bullet Stats")]
	
	public float damage;
	[HideInInspector]

	protected bool ready = false;
	protected bool grounded;

	protected Rigidbody rigid;

	protected GameObject startPos;

	protected Vector3 initialPosition;
	protected Quaternion initialRotation;

	public float despawnTime = 5f;
	//Debug 
	public bool debug;

	[Range(1.0f, 6.0f)] public float distance;
	[Range(00.0f, 90.0f)] public float angle;
		
	// Use this for initialization
	void Awake () {
		rigid = GetComponent<Rigidbody> ();
		ready = true;
		initialPosition = this.transform.position;
		initialRotation = this.transform.rotation;
		grounded = false;
	}

	protected void Update()
	{
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

	
	
	public virtual void Launch(){

		

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

	public IEnumerator Despawner(float time)
	{
		yield return new WaitForSeconds(time);
		rigid.velocity = Vector3.zero;
		rigid.angularVelocity = Vector3.zero;
		this.gameObject.SetActive(false);
	}


	
}
}
