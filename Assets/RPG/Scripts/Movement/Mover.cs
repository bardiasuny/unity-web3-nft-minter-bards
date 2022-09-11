using Pixelplacement.TweenSystem;
using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IACTION
    {

        [SerializeField] Transform target;
        // Start is called before the first frame update

        NavMeshAgent navMeshAgent;

     
        Ray lastRay;


        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();
            // Debug.DrawRay(lastRay.origin, lastRay.direction * 100);     
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            // move to local velocity
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            GetComponent<Fighter>().Cancel();
            Moveto(destination);
        }

        public void Moveto(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }
    }

}