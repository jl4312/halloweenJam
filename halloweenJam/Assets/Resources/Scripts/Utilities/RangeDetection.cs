using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
	public class RangeDetection : MonoBehaviour {


		public StructureBase parent;
		void Awake(){
			parent = transform.parent.GetComponent<StructureBase>();
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Enemy") {
                if (!parent.arrayList.Contains(other.gameObject))
                {
                    parent.arrayList.Add(other.gameObject);
                }				
			}
		}
	}
}