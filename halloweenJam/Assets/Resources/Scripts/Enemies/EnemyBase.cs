using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

    [Header("Enemy Base Setting")]
    public float health = 1.0f;
    public float movementSpeed = 1.0f;
    public float damage = 1.0f;
    public float atkSpeed = 2.0f;

    [HideInInspector]
    public bool isDead = false;
   
    protected bool hasReachedTarget = false;
    protected bool canAttack = true;

    protected GameObject closestTarget;

	// Use this for initialization
	void Start ()
    {
		
	}
    public void UpdateEnemy()
    {
        if (closestTarget == null)
        {
            FindNearestTower();
        }
        else
        {
            if (!hasReachedTarget)
            {
                SeekTower();
            }
            else
            {
                Attack();
            }

        }
    }
    public void FindNearestTower()
    {

    }

    public void SeekTower()
    {

    }
    public virtual void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            StartCoroutine(AttackCD(atkSpeed));
        }
    }

    public void TakeDamage(float dam)
    {
        if (isDead) return;

        health -= dam;

        if (health <= 0.0f)
        {
            health = 0.0f;
            isDead = true;
        }
    }
    public IEnumerator AttackCD(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

}
