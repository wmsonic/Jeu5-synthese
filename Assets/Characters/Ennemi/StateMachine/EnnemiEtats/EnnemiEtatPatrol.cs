using System.Collections;
using UnityEngine;

public class EnnemiEtatPatrol : EnnemiEtatsBase
{
    public override void InitEtat(EnnemiEtatsManager ennemi){
        ennemi.StopAllCoroutines();
        ennemi.animator.SetBool("isPatroling",true);
        ennemi.agent.destination = ennemi.patrolPoints[Random.Range(0, ennemi.patrolPoints.Count)];
        ennemi.StartCoroutine(Patrol(ennemi));
    }

    private IEnumerator Patrol(EnnemiEtatsManager ennemi){
        ennemi.agent.speed = 7f;
        while(true){
            float patience = Random.Range(2f,6f);
            while(ennemi.agent.remainingDistance > 1.2f || ennemi.agent.pathPending){
                
                yield return new WaitForSeconds(.25f);

                RaycastHit hit;
		        float directionX = ennemi.perso.transform.position.x - ennemi.transform.position.x ;
                float directionY = ennemi.perso.transform.position.y - ennemi.transform.position.y - 1.3f ;
                float directionZ = ennemi.perso.transform.position.z - ennemi.transform.position.z ;
                Vector3 raycastDirection = new Vector3(directionX,directionY,directionZ);
                Vector3 ennemiEye = new Vector3(ennemi.transform.position.x, ennemi.transform.position.y + 2f, ennemi.transform.position.z);
                if (Physics.Raycast(ennemiEye, raycastDirection, out hit, 25f, Physics.DefaultRaycastLayers , QueryTriggerInteraction.Ignore)){
                    // Debug.Log(hit.collider.gameObject.name);
                    if(hit.collider.CompareTag("Player")){
                        ennemi.ChangerEtat(ennemi.chasse);
                    }
                }
            }
            ennemi.animator.SetBool("isLooking",true);
            yield return new WaitForSeconds(patience);
            ennemi.animator.SetBool("isLooking",false);
            ennemi.agent.destination = ennemi.patrolPoints[Random.Range(0, ennemi.patrolPoints.Count)];
        }
    }
}