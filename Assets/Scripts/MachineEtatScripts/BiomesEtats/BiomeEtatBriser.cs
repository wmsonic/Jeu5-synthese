using System.Collections;
using UnityEngine;

public class BiomeEtatBriser : BiomesEtatsBase
{
    public override void InitEtat(BiomesEtatsManager biome){
        biome.GetComponent<Renderer>().material = biome.biomeMaterielBriser;
        if(biome.item != null){
            // biome.item.gameObject.SetActive(false);
            // DropItem = biome.item.GetComponent<DropItem>().dropItem();
            DropItem setDropItem = biome.item.GetComponent<DropItem>();
            if( setDropItem !=null){
                setDropItem.dropItem();
                GameObject.Destroy(biome.item.gameObject);
            }
        }
    }
    public override void UpdateEtat(BiomesEtatsManager biome){

    }
    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider obj){
        if(obj.CompareTag("niveau_eau")){
            biome.ChangerEtat(biome._briserMouiller);
        }
    }
}