using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public class StructureBase : MonoBehaviour
    {

        [Header("Structure Base Setting")]
        public float health;
        public float buildTime;
        public float cost;
		[HideInInspector]

		[Header("Structure Status Setting")]
        public bool isDead;
        public bool building = true;
        [HideInInspector]
        public bool isActive;

		public List<GameObject> arrayList;
        [HideInInspector]

        float startHealth;

		public StructureBase(float health, float buildTime, float cost)
        {          
            this.health = health;
            this.buildTime = buildTime;
            this.cost = cost;

        }

        // Update is called once per frame
        void Update()
        {
            if (!isDead)
            {

            }

            if (building)
                Build();

        }
        void Awake()
        {
            startHealth = health;
            isDead = false;
            building = true;
            isActive = true;
        }

        public void TakeDamage(float damage)
        {
            if (isDead)
                return;

            health -= damage;
            if (health <= 0)
            {
                isDead = true;
                this.gameObject.SetActive(false);
            }
        }
        public void ResetStructure()
        {
            isDead = false;
            health = startHealth;
        }
        public void Build()
        {
            buildTime -= Time.deltaTime;

            if (buildTime <= 0)
            {
                building = false;
            }
        }


    }
}
