using System.Collections;
using UnityEngine;

public class EnnemiEtatPromenade : EnnemiEtatsBase
{
    public override void InitEtat(EnnemiEtatsManager ennemi){
        ennemi.animator.SetBool("isWalking",true);
        ennemi.StartCoroutine(nav(ennemi));
    }

    private IEnumerator nav(EnnemiEtatsManager ennemi){
        // float patience = Random.Range(4f,15f);
        ennemi.agent.speed = 2f;
        ennemi.agent.destination = ennemi.home.transform.position;
        while(ennemi.agent.remainingDistance > 1.5f || ennemi.agent.pathPending){
            yield return new WaitForSeconds(.25f);
        }
        ennemi.ChangerEtat(ennemi.repos);
        ennemi.animator.SetBool("isWalking",false);
        yield return new WaitForSeconds(2f);
    }
}