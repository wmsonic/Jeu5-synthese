using UnityEngine;

public class BiomeEtatExplorer : BiomesEtatsBase
{
    public override void InitEtat(BiomesEtatsManager biome){
        // Debug.Log("hello je continue ma vie, je suis explorer!");
        // biome.GetComponent<Renderer>().material = biome.biomeMateriel;
        if(biome.item != null){
            biome.item.gameObject.SetActive(true);
        }
        // Debug.Log(item);
    }

    public override void UpdateEtat(BiomesEtatsManager biome){
        
    }

    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider obj){
        // if(obj.CompareTag("ChampForce_player")){//brise aussi item dessus
        //     biome.ChangerEtat(biome._briser);
        // }else 
        if(obj.CompareTag("niveau_eau")){
            biome.ChangerEtat(biome._explorerMouiller);
        }
    }
}
