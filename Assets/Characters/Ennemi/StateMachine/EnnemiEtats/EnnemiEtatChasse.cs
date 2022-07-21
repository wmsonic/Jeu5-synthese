using System.Collections;
using UnityEngine;

public class EnnemiEtatChasse : EnnemiEtatsBase
{
    public override void InitEtat(EnnemiEtatsManager ennemi){
        ennemi.StopAllCoroutines();
        ennemi.animator.SetBool("isPatroling",false);
        ennemi.animator.SetBool("isLooking",false);
        ennemi.animator.SetBool("isRunning",true);
        ennemi.StartCoroutine(nav(ennemi));
    }

    private IEnumerator nav(EnnemiEtatsManager ennemi){
        // float patience = Random.Range(4f,15f);
        ennemi.agent.speed = 10f;
        ennemi.agent.destination = ennemi.perso.transform.position;
        ennemi.agent.autoBraking = false;
        while(ennemi.agent.remainingDistance > 1.5f || ennemi.agent.pathPending){
            yield return new WaitForSeconds(.25f);
            ennemi.agent.destination = ennemi.perso.transform.position;
            RaycastHit hit;
		    float directionX = ennemi.agent.destination.x - ennemi.transform.position.x ;
            float directionY = ennemi.agent.destination.y - ennemi.transform.position.y - 1.3f ;
            float directionZ = ennemi.agent.destination.z - ennemi.transform.position.z ;
            Vector3 raycastDirection = new Vector3(directionX,directionY,directionZ);
            Vector3 ennemiEye = new Vector3(ennemi.transform.position.x, ennemi.transform.position.y + 2f, ennemi.transform.position.z);
            Debug.DrawRay(ennemiEye, raycastDirection, Color.red);
            if (Physics.Raycast(ennemiEye, raycastDirection, out hit, 1000f)){
                if(ennemi.agent.remainingDistance > 50f){
                    if(!hit.collider.CompareTag("Player")){
                        ennemi.animator.SetBool("isRunning",false);
                        ennemi.ChangerEtat(ennemi.patrol);
                    }
                }
            }
        }
        ennemi.animator.SetBool("isAttacking",true);
        ennemi.animator.SetBool("isRunning",false);
        yield return new WaitForSeconds(2f);
        ennemi.animator.SetBool("isAttacking",false);
        ennemi.ChangerEtat(ennemi.promenade);

    }
}