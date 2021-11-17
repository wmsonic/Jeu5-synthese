using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomesEtatsManager : MonoBehaviour
{
    
    private BiomesEtatsBase _etatActuel;
    public BiomeEtatExplorer _explorer = new BiomeEtatExplorer();
    public BiomeEtatInexplorer _inexplorer = new BiomeEtatInexplorer();
    public BiomeEtatBriser _briser = new BiomeEtatBriser();
    public BiomeEtatExplorerMouiller _explorerMouiller = new BiomeEtatExplorerMouiller();
    public BiomeEtatInexplorerMouiller _inexplorerMouiller = new BiomeEtatInexplorerMouiller();
    public BiomeEtatBriserMouiller _briserMouiller = new BiomeEtatBriserMouiller();

    public Material biomeMateriel {get; set;}
    public Material biomeMaterielBriser {get; set;}
    public Material biomeMaterielInexplorerMouiller;
    public Material biomeMaterielExplorerMouiller {get; set;}
    public Material biomeMaterielBriserMouiller {get; set;}
    [SerializeField] GameObject biomeItemShaderPrefab;
    [HideInInspector] public Transform item;
    [HideInInspector] public GameObject itemShader;

    void Start()
    {
        // GetComponent<Renderer>().material = biomeMateriel;
        ChangerEtat(_inexplorer);

        item = transform.Find("item");
        if(item != null){
            itemShader = Instantiate(biomeItemShaderPrefab, this.transform.position, Quaternion.identity, transform);
            itemShader.name = "itemShader";
            itemShader.SetActive(false);
        }
    }

    public void ChangerEtat(BiomesEtatsBase nouvelEtat)
    {
        _etatActuel = nouvelEtat;
        _etatActuel.InitEtat(this);
    }


    void Update()
    {
        _etatActuel.UpdateEtat(this);
    }

    private void OnTriggerEnter(Collider other) {
        _etatActuel.TriggerEnterEtat(this, other);
    }

}
