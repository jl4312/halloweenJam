using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public class EnemyBase : MonoBehaviour
    {

        [Header("Enemy Base Setting")]
        public float health = 1.0f;
        public float movementSpeed = 1.0f;
        public float damage = 1.0f;
        public float atkSpeed = 2.0f;

        [HideInInspector]
        public bool isDead = false;

        protected bool hasReachedTarget = false;
        protected bool canAttack = true;

        protected GameObject structuresPool = null;
        protected StructureBase closestTarget = null;

        // Use this for initialization
        void Start()
        {

        }
        void Update()
        {

        }
        public void UpdateEnemy()
        {
           

            if (closestTarget == null)
            {
                //Debug.Log("Yererrre");
                FindNearestTower();
            }
            else
            {
                if (!hasReachedTarget)
                {
                    SeekStructure();
                }
                else
                {
                    Attack();
                }

            }
        }
        public void FindNearestTower()
        {
            if (structuresPool == null)
            {
                structuresPool = GameObject.FindGameObjectWithTag("StructurePool");
            }
            int numOfStructures = structuresPool.transform.childCount;

            float closestDist = 999999;

            for (int i = 0; i < numOfStructures; i++)
            {
                GameObject structure = structuresPool.transform.GetChild(i).gameObject;

                Vector3 dist2Structure = structure.transform.position - transform.position;
                float magSquared = dist2Structure.sqrMagnitude;

                if (magSquared < closestDist || i == 0)
                {
                    closestDist = magSquared;
                    //closestTarget = structure;
                    closestTarget = structure.GetComponent<StructureBase>();
                }
            }

        }

        public void SeekStructure()
        {
            Vector3 dist2Structure = closestTarget.transform.position - transform.position;
            dist2Structure.Normalize();

            transform.position += (dist2Structure * movementSpeed) * Time.deltaTime;
        }
        public virtual void Attack()
        {
            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(AttackCD(atkSpeed));
            }
        }

        public void TakeDamage(float dam)
        {
            if (isDead) return;

            health -= dam;

            if (health <= 0.0f)
            {
                health = 0.0f;
                isDead = true;
            }
        }
        public IEnumerator AttackCD(float time)
        {
            yield return new WaitForSeconds(time);
            canAttack = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            StructureBase structure = other.GetComponent<StructureBase>();
            if (structure != null && structure == closestTarget)
            {
                hasReachedTarget = true;
            }
        }
    }
}
