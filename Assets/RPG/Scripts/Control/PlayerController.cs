using RPG.Combat;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            if (InteractWithCombat()) return;
            if(InteractionWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            // throw new NotImplementedException();
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                bool canAttak = GetComponent<Fighter>().CanAttack(target);
                 if (!canAttak) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target);

                }
                return true;
            }
            return false;

        }

        private bool InteractionWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}
