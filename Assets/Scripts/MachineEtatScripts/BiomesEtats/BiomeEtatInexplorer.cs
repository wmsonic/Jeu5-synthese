using System.Collections;
using UnityEngine;

public class BiomeEtatInexplorer : BiomesEtatsBase
{
    ParticleSystem _particleSystem;
    ParticleSystem _particleSystemSubEmitter;
    bool _canAnimate = true;
    public override void InitEtat(BiomesEtatsManager biome){
        if(biome.item != null){
            if(biome.item.CompareTag("Structure")){
                biome.item.gameObject.SetActive(true);
            }
        }
        // Debug.Log("hello je commence ma vie, je suis inexplorer!");
        // biome.ChangerEtat(biome._explorer);
    }

    public override void UpdateEtat(BiomesEtatsManager biome){

    }

    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider obj){
        // Debug.Log("triggered!!");
        if(obj.CompareTag("ChampForce_player")){
            if(_canAnimate){
                _canAnimate = false;
                biome.StartCoroutine(Anime(biome));
            }
        }else if(obj.CompareTag("niveau_eau")){
            biome.ChangerEtat(biome._inexplorerMouiller);
        }
    }

    //Coroutine d'animation de transformation d'un cube de l'état inexplorer à l'état explorer
    private IEnumerator Anime(BiomesEtatsManager biome){
        yield return new WaitForSeconds(.1f); // Attend un peu
        GameObject loadedParticleSys = (GameObject)Resources.Load("cubeEmitter");
        Transform biomeTransform = biome.GetComponent<Transform>(); // Récupère le Transform du cube toucher
        _particleSystem = GameObject.Instantiate(loadedParticleSys,biomeTransform.position,Quaternion.identity).GetComponent<ParticleSystem>();
        _particleSystemSubEmitter = _particleSystem.GetComponentInChildren<ParticleSystem>();
        Vector3 originalScale = biomeTransform.localScale; // Récupère le scale original du cube
        Vector3 targetScale = biomeTransform.localScale * Random.Range(.2f,1.5f); // Définie un scale aléatoire entre deux valeur cube, qui sera la grosseur vers laquelle on veut transformer le cube
        Vector3 originalPos = biomeTransform.position; // Récupère la position original du cube
        Vector3 targetPos = new Vector3(biomeTransform.position.x, biomeTransform.position.y + Random.Range(.2f,1.5f), biomeTransform.position.z); // Définie une position qui sera la position vers laquelle on veut déplacer le cube (Dans se cas : hauteur aléatoire entre deux valeur cube)
        yield return AnimateBetweenScalesAndPosition(biomeTransform,originalScale, targetScale, originalPos, targetPos, 0.5f); // début : Appel d'une coroutine pour animer mon cube vers mes cibles de grosseur et position depuis sa grosseur et position d'origine
        biome.GetComponent<Renderer>().material = biome.biomeMateriel; // change le material du cube à son material explorer
        yield return new WaitForSeconds(.1f); // Attend un peu
        _particleSystem.Play(); // Fait jouer le système de particule attacher au cube
        _particleSystem.GetComponent<ParticleSystemRenderer>().material = biome.biomeMateriel; //Set le matériel de mes particules au même matériel que celui du cube
        yield return AnimateBetweenScalesAndPosition(biomeTransform,targetScale, originalScale, targetPos, originalPos, 0.2f); // fin : Appel d'une coroutine pour animer mon cube vers sa grosseur et position d'origine depuis mes cibles de grosseur et position
        biome.ChangerEtat(biome._explorer); // change l'état de mon cube pour qu'il soit à l'état explorer
    }

    //Coroutine d'animation d'un transform reçu vers un scale et une position cible, sur un temps donnée
    private IEnumerator AnimateBetweenScalesAndPosition(Transform biomeTransform,Vector3 originalScale, Vector3 targetScale, Vector3 originalPos, Vector3 targetPos, float time){
        float animProgress = 0.0f; // déclaration d'un float pour tenir note de la progression de l'animation
        float rate = (1.0f / time) * 2f; // déclaration d'un rate auquel l'animation doit progrésser
        while(animProgress < 1.0f){ // tant que la porgression de l'animation n'est pas rendu à 1 (donc 100%)
            animProgress += Time.deltaTime * rate; // incrémentation de la progression selon le temps depuis la dernière frame multiplier par le rate
            biomeTransform.localScale = Vector3.Lerp(originalScale, targetScale, animProgress); // Set le scale de l'object à la grosseur approprier entre l'original et la cible selon la progression de l'animation
            biomeTransform.position = Vector3.Lerp(originalPos, targetPos, animProgress); // Set la position de l'object à la position approprier entre l'original et la cible selon la progression de l'animation
            yield return null;
        }
    }

}
