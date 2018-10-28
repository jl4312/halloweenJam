using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public enum WitchType
    {
        HealerMage,
        AttackerMage,
        EnhancerMage,
    }
    public class WitchScript : EnemyBase {

        [Header("Witch Settings")]
        public WitchType witchType = WitchType.HealerMage;
        public GameObject hat;
        public GameObject spellSpawnPoint;
        public float spellCastTime = 4.0f;
        public float spellBulletSpeed = 2.0f;

        GameObject spellBulletPool;
        GameObject bulletTarget;
        EnemyBase chosenEnemy;

        Color bulletColor = new Color(1.0f, 1.0f, 1.0f);

        Color healerHatColor = new Color(0.0f, 1.0f, 0.0f);
        Color attackerHatColor = new Color(1.0f, 0.0f, 0.0f);
        Color enhancerHatColor = new Color(0.0f, 0.0f, 1.0f);

        int spellBulletChildCount;

        // Use this for initialization
        void Start()
        {
            spellBulletPool = GameObject.FindGameObjectWithTag("SpellBulletPool");
            spellBulletChildCount = spellBulletPool.transform.childCount;

            SetWitchHat();
        }
       
        // Update is called once per frame
        void Update()
        {

            UpdateEnemy();
    
        }

        void SetWitchHat()
        {
            Color hatColor = new Color();
            switch (witchType)
            {
                case WitchType.HealerMage:
                    {
                        hatColor = healerHatColor;
                        break;
                    }
                case WitchType.AttackerMage:
                    {
                        hatColor = attackerHatColor;

                        break;
                    }
                case WitchType.EnhancerMage:
                    {
                        hatColor = enhancerHatColor;
                        break;
                    }
            }

            for (int i = 0; i < hat.transform.childCount - 1; i++)
            {
                hat.transform.GetChild(i).GetComponent<Renderer>().material.color = hatColor;
            }
        }

        public override void Attack()
        {
            if (canAttack)
            {
                canAttack = false;
                ChoosePerson();
                StartCoroutine(CastSpellCD(spellCastTime));
            }
        }

        void ChoosePerson()
        {
            switch (witchType)
            {
                case WitchType.HealerMage:
                    {
                        bulletColor = healerHatColor;
                        bulletTarget = FindPersonToHeal();

                        break;
                    }
                case WitchType.AttackerMage:
                    {
                        bulletColor = attackerHatColor;
                        bulletTarget = FindPersonToAttack();

                        break;
                    }
                case WitchType.EnhancerMage:
                    {
                        bulletColor = enhancerHatColor;
                        bulletTarget = FindPersonToEnhance();

                        break;
                    }
            }
        }

        void CastSpell()
        {
            if (newTarget)
            {
                return;
            }

            SpellBulletScript spellBullet = GetSpellBullet().GetComponent<SpellBulletScript>();
            Material bulletMat = spellBullet.GetComponent<Renderer>().material;


            bulletMat.color = bulletColor;
            spellBullet.ActivateBullet(this.gameObject, bulletTarget, spellSpawnPoint.transform.position, spellBulletSpeed);
        }
        GameObject FindPersonToHeal()
        {
            EnemyBase enemy = null;
            Collider[] objectsInArea = Physics.OverlapSphere(transform.position, 10);
            for (int i = 0; i < objectsInArea.Length; i++)
            {
                EnemyBase newEnemy = objectsInArea[i].GetComponent<EnemyBase>();
                if (newEnemy != null && newEnemy != this)
                {
                    enemy = newEnemy;
                    if (newEnemy.health < newEnemy.maxHealth)
                    {
                        chosenEnemy = newEnemy;
                        return newEnemy.gameObject;
                    }
                }
            }
            if (enemy != null)
            {
                chosenEnemy = enemy;
                return enemy.gameObject;
            }


            witchType = WitchType.AttackerMage;
            bulletColor = attackerHatColor;
            SetWitchHat();
            return FindPersonToAttack();
        }
        GameObject FindPersonToAttack()
        {
            return closestTarget.gameObject;
        }
        GameObject FindPersonToEnhance()
        {
            Collider[] objectsInArea = Physics.OverlapSphere(transform.position, 10);

            List<EnemyBase> nearByEnemies = new List<EnemyBase>();

            for (int i = 0; i < objectsInArea.Length; i++)
            {
                EnemyBase enemy = objectsInArea[i].GetComponent<EnemyBase>();
                if (enemy != null && enemy != this)
                {
                    nearByEnemies.Add(enemy);
                }
            }

            if (nearByEnemies.Count == 0)
            {
                witchType = WitchType.AttackerMage;
                bulletColor = attackerHatColor;
                SetWitchHat();
                return FindPersonToAttack();
            }

            int ranNum = Random.Range(0, nearByEnemies.Count);

            chosenEnemy = nearByEnemies[ranNum];
            GameObject target = nearByEnemies[ranNum].gameObject;

            return target;
        }

        GameObject GetSpellBullet()
        {
            GameObject bullet = null;


            for (int i= 0; i < spellBulletChildCount; i++)
            {
                bullet = spellBulletPool.transform.GetChild(i).gameObject;
                if (!bullet.activeInHierarchy)
                {
                    return bullet;
                }
            }

            Debug.Log("Couldn't get a spell bullet!");
            return bullet;
        }
        public override void AttackTarget()
        {
            if (!newTarget)
            {
                switch (witchType)
                {
                    case WitchType.HealerMage:
                        {
                            chosenEnemy.Heal(damage);
                            break;
                        }
                    case WitchType.AttackerMage:
                        {
                            closestTarget.TakeDamage(damage);
                            break;
                        }
                    case WitchType.EnhancerMage:
                        {
                            break;
                        }
                }
            }
        }
        public override IEnumerator AttackCD(float time)
        {
            yield return new WaitForSeconds(time);
            canAttack = true;
        }
        IEnumerator CastSpellCD(float time)
        {
            yield return new WaitForSeconds(time);
            CastSpell();
            StartCoroutine(AttackCD(atkSpeed));
        }
    }
}
