using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Project.Chess3D;
using Assets.Project.ChessEngine;
using Assets.Project.ChessEngine.Pieces;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Project.Chess3D.Pieces
{
    public class PieceWrapper : MonoBehaviour
    {
        public Square Square;

        Animator anim;

        NavMeshAgent agent;

        Vector3 targetPos;

        

        bool yesJump = false;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();

            /*agent = GetComponent<NavMeshAgent>();
            if (agent == null) {
                gameObject.AddComponent<NavMeshAgent>();
                agent = GetComponent<NavMeshAgent>();
            }*/
            targetPos = transform.position;
            //agent.SetDestination(transform.TransformPoint(targetPos));
        }

        private void Update()
        {
            if (
                Vector3.Distance(transform.position, targetPos) > 0.2f &&
                yesJump
            )
            {
                AudioManager.Instance.PlayWalk();

                if (!anim.GetBool("walking")) anim.SetBool("walking", true);
                transform.position =
                    Vector3
                        .Lerp(transform.position,
                        targetPos,
                        0.15f); /** Time.deltaTime*/
                transform.LookAt (targetPos);
            }
            else
            {
                yesJump = false;
                AudioManager.Instance.ResetPlayWalk();
                if (anim.GetBool("walking")) anim.SetBool("walking", false);
            }
        }

        internal void Die()
        {
            StartCoroutine(DieIn(1f));
        }

        IEnumerator DieIn(float t)
        {
            yield return new WaitForSeconds(t);
            AudioManager.Instance.PlayHurt();
            anim.SetBool("dead", true);
            Destroy(gameObject, 2);
        }

        internal void moveToPos(Vector3 worldPoint)
        {
            yesJump = true;
            targetPos =
                new Vector3(worldPoint.x, transform.position.y, worldPoint.z);
            //agent.SetDestination(targetPos);
            //transform.position = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);
        }

        internal void Attack()
        {
            Debug.Log("initiate attack");
            // if (gameObject.name.Contains("White"))
            // {
            //     string[] stringSeparators = new string[] { "White", "New" };
            //     string[] subs =
            //         gameObject
            //             .name
            //             .Split(stringSeparators, StringSplitOptions.None);
            //     print(subs[1]);
            //     UiController.ShowWhiteText("asf");
            //     UiController.ShowBlackText("fdh");
            // }
            // else
            // {
            //     string[] stringSeparators = new string[] { "White", "New" };
            //     string[] subs =
            //         gameObject
            //             .name
            //             .Split(stringSeparators, StringSplitOptions.None);
            //     print(subs[1]);
            //     UiController.ShowWhiteText("shfsh");
            //     UiController.ShowBlackText("fdh");
            // }
            StartCoroutine(AttackIn(.35f));
        }

        IEnumerator AttackIn(float t)
        {
            Debug.Log("attacker: " + gameObject.name);
            
            yield return new WaitForSeconds(t);
            AudioManager.Instance.PlayAttack();
            anim.SetTrigger("attack");
        }
    }
}
