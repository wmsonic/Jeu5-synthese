using System.Collections;
using UnityEngine;

public class EnnemiEtatChasse : EnnemiEtatsBase
{
    public override void InitEtat(EnnemiEtatsManager ennemi){
        ennemi.animator.SetBool("isRunning",true);
        ennemi.StartCoroutine(nav(ennemi));
    }

    private IEnumerator nav(EnnemiEtatsManager ennemi){
        // float patience = Random.Range(4f,15f);
        ennemi.agent.speed = 10f;
        ennemi.agent.destination = ennemi.perso.transform.position;
        while(ennemi.agent.remainingDistance > 1.5f || ennemi.agent.pathPending){
            ennemi.agent.destination = ennemi.perso.transform.position;
            yield return new WaitForSeconds(.25f);
        }
        ennemi.animator.SetBool("isRunning",false);
        ennemi.animator.SetBool("isAttacking",true);
        yield return new WaitForSeconds(2f);
        ennemi.animator.SetBool("isAttacking",false);
        ennemi.ChangerEtat(ennemi.promenade);


    }
}