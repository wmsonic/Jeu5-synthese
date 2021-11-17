using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemiEtatsManager : MonoBehaviour
{
    
    private EnnemiEtatsBase _etatActuel;
    public EnnemiEtatChasse chasse = new EnnemiEtatChasse();
    public EnnemiEtatPromenade promenade = new EnnemiEtatPromenade();
    public EnnemiEtatRepos repos = new EnnemiEtatRepos();

    public GameObject perso{get;set;}
    public GameObject home{get;set;}

    public NavMeshAgent agent {get;set;}
    public Animator animator {get;set;}

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // GetComponent<Renderer>().material = biomeMateriel;
        ChangerEtat(repos);
    }

    public void ChangerEtat(EnnemiEtatsBase nouvelEtat)
    {
        _etatActuel = nouvelEtat;
        _etatActuel.InitEtat(this);
    }

}
