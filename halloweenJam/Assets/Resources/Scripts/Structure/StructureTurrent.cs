using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public class StructureTurrent : StructureBase
    {

        private bool canAttack = false;
        private List<GameObject> enemyList;

        private GameObject closestEnemy;

        public StructureTurrent(float health, float buildTime, float cost) : base(health, buildTime, cost)
        {
            enemyList = new List<GameObject>();
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!isDead)
            {

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

        public IEnumerator AttackCD(float time)
        {
            yield return new WaitForSeconds(time);
            canAttack = true;
        }



    }
}
