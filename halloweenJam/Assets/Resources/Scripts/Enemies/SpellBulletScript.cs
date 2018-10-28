using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public class SpellBulletScript : MonoBehaviour
    {
        EnemyBase spellOwner;
        GameObject target;
        Vector3 startingPos;
        float speed;

        float despawnTime = 10.0f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            SeekTarget();
        }

        public void ActivateBullet(GameObject owner, GameObject tar, Vector3 starting, float spd)
        {
            spellOwner = owner.GetComponent<EnemyBase>();
            target = tar;
            startingPos = starting;
            transform.position = startingPos;
            speed = spd;

            this.gameObject.SetActive(true);
            StartCoroutine(DespawnTimer(despawnTime));
        }

        void SeekTarget()
        {
            Vector3 dist2Structure = target.transform.position - transform.position;
            dist2Structure.Normalize();

            transform.forward = dist2Structure;
            transform.position += (dist2Structure * speed) * Time.deltaTime;
        }
        void DeactivateBullet()
        {
            this.gameObject.SetActive(false);
        }

        IEnumerator DespawnTimer(float time)
        {
            yield return new WaitForSeconds(time);
            DeactivateBullet();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == target)
            {
                spellOwner.AttackTarget();
                DeactivateBullet();
            }
        }
    }
}