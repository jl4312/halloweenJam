using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
//resouce: https://vilbeyli.github.io/Projectile-Motion-Tutorial-for-Arrows-and-Missiles-in-Unity3D/
	public class ProjectileArc : ProjectileBase {
			
		
		void Update()
		{
			//mid air
			if (!ready && !grounded) {

				transform.rotation = Quaternion.LookRotation(rigid.velocity) * initialRotation;

			}

				//base.Update ();
		}

		public override void Launch(){

			Vector3 projectileXZPos = new Vector3 (transform.position.x, 0, transform.position.z);
			Vector3 targetXZPos = new Vector3 (targetTransform.position.x, 0, targetTransform.position.z);

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



	}


}
