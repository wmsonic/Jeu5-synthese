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
    public EnnemiEtatPatrol patrol = new EnnemiEtatPatrol();

    public GameObject perso{get;set;}
    public GameObject home{get;set;}
    public List<Vector3> patrolPoints{get;set;}

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
        agent.autoBraking = true;
        _etatActuel = nouvelEtat;
        _etatActuel.InitEtat(this);
    }

    public IEnumerator GotoBait(Vector3 baitPosition){
        animator.SetBool("isRunning",true);
        agent.speed = 10f;
        agent.destination = baitPosition;
        while(agent.remainingDistance > 1.5f || agent.pathPending){
            yield return new WaitForSeconds(.25f);
        }
        animator.SetBool("isRunning",false);
        yield return new WaitForSeconds(2f);
        ChangerEtat(patrol);

    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("hache") || other.CompareTag("epee")){
            //run away 
            StopAllCoroutines();
            animator.SetBool("isRunning",false);
            ChangerEtat(promenade);
        }else if(other.CompareTag("water") && _etatActuel != promenade && _etatActuel != repos){
            if(_etatActuel == chasse){
                agent.speed = Mathf.Clamp(10 * 1.5f, 2.5f, 15f);
            }else if(_etatActuel == patrol){
                agent.speed = Mathf.Clamp(7.5f * 1.5f, 2.5f, 15f);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("water") && _etatActuel != promenade && _etatActuel != repos){
            if(_etatActuel == chasse){
                agent.speed = 10f;
            }else if(_etatActuel == patrol){
                agent.speed = 7.5f;
            }
        }
    }

}
