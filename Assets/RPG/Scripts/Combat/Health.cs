using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;


        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            
            if(healthPoints == 0 && !isDead)
            {
                Die();
                isDead = true;
            }
        }

        private void Die()
        {
            GetComponent<Animator>().SetTrigger("die");
        }
    }
}

