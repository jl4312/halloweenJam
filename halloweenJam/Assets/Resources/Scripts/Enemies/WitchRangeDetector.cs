using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public class WitchRangeDetector : MonoBehaviour
    {
        EnemyBase parent;
        // Use this for initialization
        void Awake()
        {
            parent = transform.parent.GetComponent<EnemyBase>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            StructureBase structure = other.GetComponent<StructureBase>();
            if (structure != null && structure == parent.closestTarget)
            {
                parent.hasReachedTarget = true;
                parent.newTarget = false;
            }
        }
    }
}
