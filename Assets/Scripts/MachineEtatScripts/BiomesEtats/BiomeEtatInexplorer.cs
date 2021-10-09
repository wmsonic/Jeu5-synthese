using System.Collections;
using UnityEngine;

public class BiomeEtatInexplorer : BiomesEtatsBase
{
    public override void InitEtat(BiomesEtatsManager biome){
        // Debug.Log("hello je commence ma vie, je suis inexplorer!");
        // biome.ChangerEtat(biome._explorer);
    }

    public override void UpdateEtat(BiomesEtatsManager biome){

    }

    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider obj){
        Debug.Log("triggered!!");
        biome.StartCoroutine(Anime(biome));
    }

    private IEnumerator Anime(BiomesEtatsManager biome){
        yield return new WaitForSeconds(.5f);
        biome.GetComponent<Renderer>().material = biome.biomeMateriel;
        biome.ChangerEtat(biome._explorer);
    }

}
