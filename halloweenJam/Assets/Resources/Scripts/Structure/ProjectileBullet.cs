using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
//resouce: https://vilbeyli.github.io/Projectile-Motion-Tutorial-for-Arrows-and-Missiles-in-Unity3D/
	public class ProjectileBullet : ProjectileBase {
			
		[Header("Bullet Stats")]
		public float bulletSpeed;
		public bool acceleration = false;
		[HideInInspector]

		
		void Update()
		{
			//mid air
			if (!ready && !grounded) {

				if(acceleration)
					rigid.AddForce(transform.forward * bulletSpeed);
				else
					transform.position += transform.forward * bulletSpeed * Time.deltaTime;


			}

				//base.Update ();
		}

		public override void Launch(){


			StartCoroutine (Despawner (despawnTime));
			Vector3 dist = targetTransform.transform.position - this.transform.position;
			dist.Normalize();
			transform.forward = dist;

			
			if (acceleration)
				rigid.AddForce (transform.forward * bulletSpeed, ForceMode.Impulse);


			ready = false;


		}

	}


}
