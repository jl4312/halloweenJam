using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace MisfitMakers
{

    public class StructureBase : MonoBehaviour
    {


        [Header("Structure Base Setting")]
		public float startHealth;//total health
		public float buildTime;//total buildTime
			
		public float health;
		public float currentBuildTime;
        public float cost;
		[HideInInspector]

		[Header("Structure Status Setting")]
        public bool isDead;
        public bool building;
        [HideInInspector]
        public bool isActive;

		public List<GameObject> arrayList;
        [HideInInspector]

     

		public bool boundaryActive;
		//represents the border around structure that players cant place objects
		public List<Vector2> borderList = new List<Vector2> ();

		private GameObject statsUIPrefab;
		public GameObject statsUI;
		private float offset = 2.5f;
		public AnimationClip animClip;
		
		private Animation anim;
		// Update is called once per frame
		protected virtual void Awake()
		{
			if (!statsUI) {
				statsUIPrefab = Resources.Load<GameObject> ("Prefabs/UI/EnityStatsUI");
				statsUI = (GameObject)Instantiate(statsUIPrefab,this.transform, true);
				
				Vector3 tmp = this.transform.GetChild(0).position;
				tmp.y = transform.GetChild (0).GetChild(0).GetComponent<MeshRenderer> ().bounds.max.y + offset;
				statsUI.transform.position = tmp;
				
			}
			
			health = startHealth;
			currentBuildTime = buildTime;
			isDead = false;
			isActive = true;
			
			if (this.transform.GetChild (0).gameObject.GetComponent<Animation> () == null) {
				this.transform.GetChild (0).gameObject.AddComponent<Animation> ();
			}
			anim = this.transform.GetChild (0).gameObject.GetComponent<Animation> ();
			
			animClip.legacy = true;
			anim.AddClip(animClip, "Building");
			
			DisplayUI ();
			
			if (boundaryActive) {
				borderList.Add(new Vector2(-1, 1));
				borderList.Add(new Vector2(0, 1));
				borderList.Add(new Vector2(1, 1));
				borderList.Add(new Vector2(-1, 0));
				borderList.Add(new Vector2(1, 0));
				borderList.Add(new Vector2(-1, -1));
				borderList.Add(new Vector2(0, -1));
				borderList.Add(new Vector2(1, -1));
			}
		}
		
		protected virtual void Update()
        {  

            if (building)
                Build();
        }
       
		

        public void Build()
        {
			currentBuildTime -= Time.deltaTime;
			anim ["Building"].wrapMode = WrapMode.Once;
			anim.Play("Building");

			if (currentBuildTime <= 0)
			{
				building = false;
				anim["Building"].speed = 1.5f;
            }
			DisplayUI ();
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


		public void DisplayUI(){
			
			if (isDead)
				return;
			
			if (building) {
				//anim.clip = anim.GetClip ("Building");

				statsUI.transform.GetChild (1).gameObject.SetActive (true);
				statsUI.transform.GetChild (0).gameObject.SetActive (false);

				statsUI.transform.GetChild (1).GetComponent<Image> ().fillAmount = 1 - (float)currentBuildTime / buildTime;
		
			} else {

				statsUI.transform.GetChild (0).gameObject.SetActive (true);
				statsUI.transform.GetChild (1).gameObject.SetActive (false);

				statsUI.transform.GetChild (0).GetComponent<Image> ().fillAmount = health / startHealth;
				
				float percent = health / startHealth;
				
				if (percent < .2f)
					statsUI.transform.GetChild (0).GetComponent<Image> ().color = Color.red;
				else if (percent < .5f)
					statsUI.transform.GetChild (0).GetComponent<Image> ().color = Color.yellow;
				else
					statsUI.transform.GetChild (0).GetComponent<Image> ().color = Color.green;
			}


		}


    }
}
