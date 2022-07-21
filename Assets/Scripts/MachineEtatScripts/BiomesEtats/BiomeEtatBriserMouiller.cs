using System.Collections;
using UnityEngine;

public class BiomeEtatBriserMouiller : BiomesEtatsBase
{
    public override void InitEtat(BiomesEtatsManager biome){
        biome.GetComponent<Renderer>().material = biome.biomeMaterielBriserMouiller;
        // if(biome.item != null){
        //     biome.item.gameObject.SetActive(false);
        // }
    }
    public override void UpdateEtat(BiomesEtatsManager biome){

    }
    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider obj){

    }
}