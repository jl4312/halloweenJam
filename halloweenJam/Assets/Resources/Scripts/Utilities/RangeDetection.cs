using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
	public class RangeDetection : MonoBehaviour {


		public StructureBase parent;
		public List<GameObject> gameObjectList;
		public string tagName;

		void Awake(){
			parent = transform.parent.GetComponent<StructureBase>();
			gameObjectList = new List<GameObject> ();
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Enemy") {
                if (parent && !parent.arrayList.Contains(other.gameObject))
                {
                    parent.arrayList.Add(other.gameObject);
                }				
			}

			if (other.tag == tagName) {
				gameObjectList.Add(other.gameObject);
			}
		}

		public List<GameObject> GetCollideObject(){
			return gameObjectList;
		}

		public void ResetGameObjectList(){
			gameObjectList = new List<GameObject> ();
		}
	}
}