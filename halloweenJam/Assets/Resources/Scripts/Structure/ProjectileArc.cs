using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
//resouce: https://vilbeyli.github.io/Projectile-Motion-Tutorial-for-Arrows-and-Missiles-in-Unity3D/
	public class ProjectileArc : ProjectileBase {
			
		private bool explosion = false;
		public float explosionTime = .5f;
		public float explosionRadius = 1.10f;
		void Update()
		{
			//mid air
			if (!ready && !grounded) {

				if(rigid.velocity != Vector3.zero)
					transform.rotation = Quaternion.LookRotation(rigid.velocity) * initialRotation;

			}

			if (explosion) {
				transform.GetChild(0).localScale = transform.GetChild(0).localScale + new Vector3(1,1,1) * (explosionRadius / explosionTime);

				List<GameObject> tmp = transform.GetChild(0).GetComponent<RangeDetection>().GetCollideObject();
				for(int i =0; i < tmp.Count;i++)
					tmp[i].GetComponent<EnemyBase>().TakeDamage(1);

				transform.GetChild(0).GetComponent<RangeDetection>().ResetGameObjectList();
			}

				//base.Update ();
		}

		public override void Launch(){

			rigid.useGravity = true; 
			Vector3 projectileXZPos = new Vector3 (transform.position.x, 0, transform.position.z);
			Vector3 tmp = targetTransform.position + targetTransform.forward * 3f;
			Vector3 targetXZPos = new Vector3 (tmp.x, 0, tmp.z);

			transform.LookAt (targetXZPos);

			float R = Vector3.Distance (projectileXZPos, targetXZPos);
			float G = Physics.gravity.y;
			float tanAlpha = Mathf.Tan (angle * Mathf.Deg2Rad);
			float H = (targetTransform.position.y - transform.position.y);

			float Vz = Mathf.Sqrt (G * R * R / (2.0f * (H - R * tanAlpha)));
			float Vy = tanAlpha * Vz;

			Vector3 localVelocity = new Vector3 (0, Vy, Vz);
			Vector3 globalVelocity = transform.TransformDirection (localVelocity);

			rigid.velocity = globalVelocity;
			ready = false;
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Enemy" || other.tag == "Ground") {
				explosion = true;

				rigid.useGravity = false;
				rigid.velocity = Vector3.zero;
				this.transform.GetChild(0).gameObject.SetActive(true);
				StartCoroutine(DisplayToggle(explosionTime, this.transform.GetChild(0).gameObject, false));
				StartCoroutine(DisplayToggle(explosionTime, this.gameObject, false));


			}
		}

		public virtual IEnumerator DisplayToggle(float time, GameObject obj, bool active)
		{
			yield return new WaitForSeconds(time);
			obj.SetActive (active);
			this.transform.GetChild (0).gameObject.transform.localScale = new Vector3 (.5f, .5f, .5f);
			explosion = false;
		}
		
	}
	
	
}
