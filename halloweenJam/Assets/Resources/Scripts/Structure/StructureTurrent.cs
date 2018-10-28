using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public class StructureTurrent : StructureBase
    {

        private bool canAttack;
		public float coolDown = 1f;
		
        private List<GameObject> enemyList;
		private GameObject closestEnemy;

		public GameObject projectilePool;
		private ProjectileBase[] projectileList;

        public StructureTurrent(float health, float buildTime, float cost) : base(health, buildTime, cost)
        {  
        }

        // Use this for initialization
        void Start()
        {
			enemyList = new List<GameObject>();
			projectileList = projectilePool.GetComponentsInChildren<ProjectileBase>(true);
			canAttack = true;
		}

        // Update is called once per frame
        void Update()
        {
            if (!isDead)
            {
				UpdateEnemyList ();
				Attack();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (isDead)
            {
				return;
            }
	        if (other.tag == "Enemy"){
                enemyList.Add(other.gameObject);
            }
        }

		void OnTriggerExit(Collider other)
		{
			if (other.tag == "Enemy") {
				enemyList.Remove(other.gameObject);
			}
		}
		void UpdateEnemyList()
		{
			float closetEnemyDist;
			Vector3 dist;

			closetEnemyDist = 100000f; 

			for (int i = 0; i < enemyList.Count; i++) {

				dist = enemyList[i].transform.position - this.transform.position;

				if(dist.magnitude < closetEnemyDist)
				{
					closetEnemyDist = dist.magnitude;
					closestEnemy = enemyList[i];
				}

				if(enemyList[i].GetComponent<EnemyBase>().isDead)
					enemyList.RemoveAt (i);


			}

				
		}
        public IEnumerator AttackCD(float time)
        {
            yield return new WaitForSeconds(time);
            canAttack = true;

        }

		public void Attack()
		{

			if (closestEnemy && canAttack) {
				ProjectileBase currentProjectile = Reload();

				if(currentProjectile){
					currentProjectile.Reset(this.transform.GetChild(0).GetChild(0).position,
				                        this.transform.GetChild(0).GetChild(0).rotation);

					currentProjectile.SetNewTarget (closestEnemy);
					currentProjectile.Launch();
				}

				canAttack = false;
				StartCoroutine( AttackCD(coolDown));
			}
		}

		ProjectileBase Reload()
		{
			Debug.Log (projectileList.Length);
			for (int i = 0; i < projectileList.Length; i++) {

				if(!projectileList[i].gameObject.activeInHierarchy)
					return projectileList[i];
			}
			return null;
		}
    }
}
