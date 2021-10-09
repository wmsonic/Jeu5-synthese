using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomesEtatsManager : MonoBehaviour
{
    
    private BiomesEtatsBase _etatActuel;
    public BiomeEtatExplorer _explorer = new BiomeEtatExplorer();
    public BiomeEtatInexplorer _inexplorer = new BiomeEtatInexplorer();

    public Material biomeMateriel {get; set;}

    void Start()
    {
        // GetComponent<Renderer>().material = biomeMateriel;
        ChangerEtat(_inexplorer);
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
