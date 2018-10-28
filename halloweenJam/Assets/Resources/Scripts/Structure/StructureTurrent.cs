using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public class StructureTurrent : StructureBase
    {

        private bool canAttack = false;
		private float coolDown = 1f;


        private List<GameObject> enemyList;
		private GameObject closestEnemy;

        public StructureTurrent(float health, float buildTime, float cost) : base(health, buildTime, cost)
        {
            
        }
        // Use this for initialization
        void Start()
        {
			enemyList = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isDead)
            {
				UpdateEnemyList ();

            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (isDead)
            {
            }

            if (other.tag == "Enemy")
            {
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

				canAttack = false;
				AttackCD (coolDown);
			}
		}

    }
}
