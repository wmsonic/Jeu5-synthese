using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    enum _dropType{
        none,
        bamboo,
        branche,
        feuille,
        pieces,
        vases
    };

    [SerializeField] private _dropType _typeDropDown =  _dropType.none;
    private Collider _collider;

    [HideInInspector] public string itemType;
    private Transform _player;
    private Vector3 _velocity = Vector3.zero;
    private bool _isFollowing = false;
    private bool _canAnimateUp = true;

    private void Awake() {
        itemType = _typeDropDown.ToString();
    }

    void Start()
    {
        _collider = GetComponent<Collider>();
        StartCoroutine("AnimateDrop");
        if(_typeDropDown == _dropType.none){
            Debug.Log("Please set drop type of prefab : " + this.name);
        }
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        // if(_typeDropDown == _dropType.pieces || _typeDropDown == _dropType.vases){
        //     GameObject.FindGameObjectWithTag("inventaire").GetComponent<Inventaire>().AddToTotal(itemType);
        // }
    }

    private void Update() {
        if(_isFollowing){
            transform.position = Vector3.SmoothDamp(transform.position, _player.position, ref _velocity, Time.deltaTime * 2f);
        }
    }

    IEnumerator AnimateDrop(){
        Transform itemTransform = this.transform;
        Vector3 originalScale = itemTransform.localScale; // Récupère le scale original du cube
        Vector3 targetScale = itemTransform.localScale * Random.Range(.2f,1.5f); // Définie un scale aléatoire entre deux valeur, qui sera la grosseur vers laquelle on veut transformer l'item
        Vector3 originalPos = itemTransform.position; // Récupère la position original du cube
        Vector3 targetPos = new Vector3(itemTransform.position.x, itemTransform.position.y + Random.Range(3f,3.7f), itemTransform.position.z); // Définie une position qui sera la position vers laquelle on veut déplacer l'item (Dans se cas : hauteur aléatoire entre deux valeur)
        yield return AnimateTransform(itemTransform,originalScale, targetScale, originalPos, targetPos, 1f);
        yield return AnimateTransform(itemTransform,targetScale, originalScale, targetPos, originalPos, .5f);
        yield return null;
        _collider.enabled = true;
    }

    private IEnumerator AnimateTransform(Transform itemTransform,Vector3 originalScale, Vector3 targetScale, Vector3 originalPos, Vector3 targetPos, float time){
        float animProgress = 0.0f; // déclaration d'un float pour tenir note de la progression de l'animation
        float rate = (1.0f / time) * 2f; // déclaration d'un rate auquel l'animation doit progrésser
        while(animProgress < 1.0f){ // tant que la porgression de l'animation n'est pas rendu à 1 (donc 100%)
            animProgress += Time.deltaTime * rate; // incrémentation de la progression selon le temps depuis la dernière frame multiplier par le rate
            itemTransform.localScale = Vector3.Lerp(originalScale, targetScale, animProgress); // Set le scale de l'object à la grosseur approprier entre l'original et la cible selon la progression de l'animation
            itemTransform.position = Vector3.Lerp(originalPos, targetPos, animProgress); // Set la position de l'object à la position approprier entre l'original et la cible selon la progression de l'animation
            yield return null;
        }
    }

    public void StartFollowing(){
        _isFollowing = true;
        _collider.isTrigger = true;
        if(_canAnimateUp){
            Vector3 targetHeightTransform = new Vector3(this.transform.position.x, this.transform.position.y + 6f, this.transform.position.z);
            StartCoroutine(AnimateTransform(this.transform, this.transform.localScale, this.transform.localScale, this.transform.position, targetHeightTransform, .8f ));
            _canAnimateUp = false;
        }
        // Debug.Log("is following player");
    }
}
