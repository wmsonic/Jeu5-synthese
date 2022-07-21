using UnityEngine;

public class BiomeEtatExplorerMouiller : BiomesEtatsBase
{
    public override void InitEtat(BiomesEtatsManager biome){
        //display material explorerMouiller
        biome.GetComponent<Renderer>().material = biome.biomeMaterielExplorerMouiller;

        if(biome.item != null && biome.item.GetComponent<Item>()!=null && biome.item.CompareTag("collectible")){
                biome.item.gameObject.SetActive(true);
                biome.itemShader.SetActive(false);
        }
        // Debug.Log(item);
    }

    public override void UpdateEtat(BiomesEtatsManager biome){

    }

    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider obj){
        // if(obj.CompareTag("ChampForce_player")){//change state pour briserMouiller et brise aussi item dessus
        //     biome.ChangerEtat(biome._briserMouiller);
        // }//Maybe si les ennemis walk dessus ils les brisent et donc rip l'item dessus ?
    }
}
