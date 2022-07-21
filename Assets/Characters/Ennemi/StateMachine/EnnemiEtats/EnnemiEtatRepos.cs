using System.Collections;
using UnityEngine;

public class EnnemiEtatRepos : EnnemiEtatsBase
{
    public override void InitEtat(EnnemiEtatsManager ennemi){
        ennemi.StartCoroutine(nav(ennemi));
    }

    private IEnumerator nav(EnnemiEtatsManager ennemi){
        float patience = Random.Range(1f,6f);
        yield return new WaitForSeconds(patience);
        ennemi.ChangerEtat(ennemi.patrol);
    }
}