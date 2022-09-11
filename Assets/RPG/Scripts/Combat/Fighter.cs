using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IACTION
    {

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;

        Health target;
        float timeSincLastAttak = 0;

        private void Update()
        {

            timeSincLastAttak += Time.deltaTime;
            if(target == null) return;

            if (target.IsDead()) return;
     

            if (!(getDistance() < weaponRange))
            {
                GetComponent<Mover>().Moveto(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();

                AttackBehaviour();

            }
        }

        private void AttackBehaviour()
        {

            transform.LookAt(target.transform);
            if (timeSincLastAttak > timeBetweenAttacks)
            {
                GetComponent<Animator>().ResetTrigger("stopAttack");
                // this will trigger the Hit() event
                GetComponent<Animator>().SetTrigger("attack");
                timeSincLastAttak = 0;
               
            }
           
        }


        void Hit()
        {
           
            target.TakeDamage(weaponDamage);

        }

        private float getDistance()
        {
            return Vector3.Distance(transform.position, target.transform.position);
        }


        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            print(targetToTest);
            return !targetToTest.IsDead();
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
    }
}

