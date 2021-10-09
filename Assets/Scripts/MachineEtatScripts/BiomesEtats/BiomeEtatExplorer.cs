using UnityEngine;

public class BiomeEtatExplorer : BiomesEtatsBase
{
    public override void InitEtat(BiomesEtatsManager biome){
        Debug.Log("hello je continue ma vie, je suis explorer!");
    }

    public override void UpdateEtat(BiomesEtatsManager biome){

    }

    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider obj){
        
    }
}
