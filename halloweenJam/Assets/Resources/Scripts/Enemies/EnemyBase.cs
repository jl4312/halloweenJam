using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public class EnemyBase : MonoBehaviour
    {

        [Header("Enemy Base Setting")]
        public float health = 1.0f;
        public float maxHealth = 1.0f;
        public float movementSpeed = 1.0f;
        public float damage = 1.0f;
        public float atkSpeed = 2.0f;

        [HideInInspector]
        public bool isDead = false;
        [HideInInspector]
        public bool hasReachedTarget = false;
        [HideInInspector]
        public bool newTarget = true;
        protected bool canAttack = true;

        protected GameObject[] structuresPool = null;
        [HideInInspector]
        public StructureBase closestTarget = null;

        float groundLevel = -1;

		private GameObject statsUIPrefab;
		public GameObject statsUI;
		private float offset = 2.5f;

        // Use this for initialization
        void Awake()
        {
			if (!statsUI) {
				statsUIPrefab = Resources.Load<GameObject> ("Prefabs/UI/EnityStatsUI");
				statsUI = (GameObject)Instantiate(statsUIPrefab,this.transform, true);
				

			}
			Vector3 tmp = this.transform.position;
			
			MeshRenderer[] list = this.transform.GetComponentsInChildren<MeshRenderer>();
			
			float highestPoint = 0;
			for(int i =0; i < list.Length;i++)
				if(highestPoint < list[i].bounds.max.y)
					highestPoint = list[i].bounds.max.y;
			
			tmp.y = highestPoint + offset;
			this.statsUI.transform.position = tmp;
		
        }
        void Update()
        {

        }
        public void UpdateEnemy()
        {
           

            if (closestTarget == null || !closestTarget.gameObject.activeInHierarchy)
            {
                //Debug.Log("Yererrre");
                hasReachedTarget = false;
                newTarget = true;
                FindNearestTower();
            }
            else
            {
                if (!hasReachedTarget)
                {
                    FindNearestTower();
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
            structuresPool = GameObject.FindGameObjectsWithTag("Structure");
            int numOfStructures = structuresPool.Length;
            //Debug.Log(numOfStructures);
            float closestDist = 999999;

            for (int i = 0; i < numOfStructures; i++)
            {
                GameObject structure = structuresPool[i];
                if (!structure.activeInHierarchy || !structure.GetComponent<StructureBase>().isActive)
                {
                    continue;
                }
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
            if (groundLevel == -1)
            {
                groundLevel = transform.position.y;
            }
            Vector3 dist2Structure = new Vector3(closestTarget.transform.position.x, groundLevel, closestTarget.transform.position.z) - transform.position;
            dist2Structure.Normalize();

            transform.forward = dist2Structure;
            transform.position += (dist2Structure * movementSpeed) * Time.deltaTime;
        }
        public void TakeDamage(float dam)
        {
            if (isDead) return;

            health -= dam;

            if (health <= 0.0f)
            {
                health = 0.0f;
                isDead = true;
                this.gameObject.SetActive(false);
            }
        }
        public void Heal(float heal)
        {
            health += heal;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
        public virtual void AttackTarget()
        {
            if (!newTarget)
            {
                closestTarget.TakeDamage(damage);
            }
        }

        public virtual void Attack()
        {
            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(AttackCD(atkSpeed));
            }
        }
        public virtual IEnumerator AttackCD(float time)
        {
            yield return new WaitForSeconds(time);
            AttackTarget();
            canAttack = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            StructureBase structure = other.GetComponent<StructureBase>();
            if (structure != null && structure == closestTarget)
            {
                hasReachedTarget = true;
                newTarget = false;
            }
        }
    }
}
