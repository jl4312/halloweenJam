using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace MisfitMakers
{
    public class StructureBase : MonoBehaviour
    {

        [Header("Structure Base Setting")]
        public float health;
        public float currentBuildTime;
        public float cost;
		[HideInInspector]

		[Header("Structure Status Setting")]
        public bool isDead;
        public bool building = true;
        [HideInInspector]
        public bool isActive;

		public List<GameObject> arrayList;
        [HideInInspector]

        float startHealth;//total health
		float buildTime;//total buildTime
       
		public AnimationClip animClip;

		private Animation anim;
		// Update is called once per frame
        protected void Update()
        {
            

            if (this.gameObject.activeInHierarchy && building)
                Build();

        }
        void Awake()
        {
            startHealth = health;
			buildTime = currentBuildTime;
            isDead = false;
            building = true;
            isActive = true;


			DisplayUI ();

			if (this.transform.GetChild (0).gameObject.GetComponent<Animation> () == null) {
				//this.transform.GetChild (0).gameObject.AddComponent<Animator> ();
				this.transform.GetChild (0).gameObject.AddComponent<Animation> ();
			}
			anim = this.transform.GetChild (0).gameObject.GetComponent<Animation> ();

			//this.transform.GetChild (0).gameObject.AddComponent<AnimatorOverrideController> ();
		
			animClip.legacy = true;
			anim.AddClip(animClip, "Building");


        }

		public void TakeDamage(float damage)
		{
            if (isDead)
                return;

            health -= damage;

			DisplayUI ();

            if (health <= 0)
            {
                isDead = true;
                this.gameObject.SetActive(false);
            }
        }
        public void ResetStructure()
        {
            isDead = false;
			building = true;
			isActive = true;

			health = startHealth;
			currentBuildTime = buildTime;

			DisplayUI ();

        }

        public void Build()
        {
			anim ["Building"].wrapMode = WrapMode.Once;
			anim.Play("Building");

			currentBuildTime -= Time.deltaTime;

			DisplayUI ();

			if (currentBuildTime <= 0)
			{
				anim["Building"].speed = 2f;
				//anim.Play
			//	anim.Stop();
                building = false;
				this.transform.GetChild (3).GetChild (1).gameObject.SetActive (false);

            }
        }

		public void DisplayUI(){

			if (isDead)
				return;

			if (building) {
				//anim.clip = anim.GetClip ("Building");

				this.transform.GetChild (3).GetChild (1).gameObject.SetActive (true);
				this.transform.GetChild (3).GetChild (0).gameObject.SetActive (false);

				this.transform.GetChild (3).GetChild (1).GetComponent<Image> ().fillAmount = 1 - (float)currentBuildTime / buildTime;
		
			} else {

				this.transform.GetChild (3).GetChild (0).gameObject.SetActive (true);
				this.transform.GetChild (3).GetChild (1).gameObject.SetActive (false);

				this.transform.GetChild (3).GetChild (0).GetComponent<Image> ().fillAmount = health / startHealth;
				
				float percent = health / startHealth;
				
				if (percent < .2f)
					this.transform.GetChild (3).GetChild (0).GetComponent<Image> ().color = Color.red;
				else if (percent < .5f)
					this.transform.GetChild (3).GetChild (0).GetComponent<Image> ().color = Color.yellow;
				else
					this.transform.GetChild (3).GetChild (0).GetComponent<Image> ().color = Color.green;
			}


		}


    }
}
