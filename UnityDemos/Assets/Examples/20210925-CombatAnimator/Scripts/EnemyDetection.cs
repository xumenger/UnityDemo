using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20210925
{
    public class EnemyDetection : MonoBehaviour
    {
        [SerializeField] private EnemyManager enemyManager;
        private MovementInput movementInput;
        private CombatScript combatScript;

        public LayerMask layerMask;

        [SerializeField] Vector3 inputDirection;
        [SerializeField] private EnemyScript currentTarget;

        public Camera cam;

        private void Start()
        {
            cam = Camera.main;
            movementInput = GetComponentInParent<MovementInput>();
            combatScript = GetComponentInParent<CombatScript>();
        }

        private void Update()
        {
            var forward = cam.transform.forward;
            var right = cam.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            inputDirection = forward * movementInput.moveAxis.y + right * movementInput.moveAxis.x;
            inputDirection = inputDirection.normalized;

            // 用一个Sphere（球形）的触发器探测是否特定范围内有敌人
            // 这个放在Update() 中，性能怎么样呢？
            RaycastHit info;
            if (Physics.SphereCast(transform.position, 3f, inputDirection, out info, 10, layerMask))
            {
                if (info.collider.transform.GetComponent<EnemyScript>().IsAttackable())
                {
                    currentTarget = info.collider.transform.GetComponent<EnemyScript>();
                }
            }
        }

        public EnemyScript CurrentTarget()
        {
            return currentTarget;
        }

        public void SetCurrentTarget(EnemyScript target)
        {
            currentTarget = target;
        }

        public float InputMagnitude()
        {
            return inputDirection.magnitude;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, inputDirection);
            Gizmos.DrawWireSphere(transform.position, 1);
            if (CurrentTarget() != null)
            {
                Gizmos.DrawSphere(CurrentTarget().transform.position, 0.5f);
            }   
        }
    }
}