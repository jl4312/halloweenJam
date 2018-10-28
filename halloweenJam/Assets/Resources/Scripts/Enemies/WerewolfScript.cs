using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{

    public class WerewolfScript : EnemyBase
    {
        [Header("Werewolf Settings")]
        public float furyAttackSpeed = 0.2f;
        public int furyAttackHits = 3;

        int furyAttackHitCounts = 0;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateEnemy();
        }
        public override void Attack()
        {
            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(FuryAttackCD(furyAttackSpeed));
            }
        }

        IEnumerator FuryAttackCD(float time)
        {
            yield return new WaitForSeconds(time);

            AttackTarget();
            furyAttackHitCounts++;

            if (furyAttackHitCounts >= furyAttackHits)
            {
                StartCoroutine(AttackCD(atkSpeed));
            }
            else
            {
                StartCoroutine(FuryAttackCD(furyAttackSpeed));
            }
        }

        public override IEnumerator AttackCD(float time)
        {
            yield return new WaitForSeconds(time);
            furyAttackHitCounts = 0;
            canAttack = true;
        }
    }
}
