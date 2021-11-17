using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GenerateurIle : MonoBehaviour
{
    [SerializeField] private bool _desactiverRandom = false;
    [SerializeField] private bool _genererMapSeulement = false;
    [SerializeField] private bool _activerNoise = true;    
    [SerializeField] private GameObject _cube;
    [SerializeField] private Renderer _textureRender;    
    [SerializeField] private int _largeurIle = 10;    
    [SerializeField] private int _profondeurIle = 10;    
    [SerializeField] private float _attenuateur;    
    [SerializeField] private float _rayon;    
    [SerializeField] private int _coefAltitude = 10;
    [SerializeField] private bool _inexplorer;
    [SerializeField] private GameObject _ennemi;
    [SerializeField] [Range(0,20)]private int _amountOfEnnemis = 20;
    [SerializeField] private GameObject perso;
    

    private List<List<Material>> _biomesMats = new List<List<Material>>();
    private List<List<GameObject>> _biomesItems = new List<List<GameObject>>();

    // private void Awake() {

    // }

    void Start(){
        // GenererListeMateriauxBiomes();
        GenererListeRessourcesBiomes<Material>("materials", "B", _biomesMats);
        GenererListeRessourcesBiomes<GameObject>("objects", "I", _biomesItems);
        creerMap();
        perso.SetActive(false);
        GetComponent<NavMeshSurface>().BuildNavMesh();
        perso.SetActive(true);
    }


    ///<summary>
    ///Fonction générique qui remplis une liste (<paramref name="listeRessourcesParBiome"/>) à deux dimensions des biomes et de ces ressources (<typeparamref name="Type"/> donné lors de l'appel) 
    ///en parcrourant le dossier donné (<paramref name="path"/>) dans le dossier Ressources et en les ajoutants si le nom de la ressource loader correspond au format de nom : <paramref name="prefix"/>*nombre*_*nombre*
    ///</summary>
    ///<typeparam name="Type">Le type d'objet des ressources qui seront loader </typeparam>
    ///<param name="path">Chemin du dossier à parcourir dans Ressources</param>
    ///<param name="prefix">Lettre qui est située au début du nom de la ressource.
    ///Habituellement pour indiqué le type de ressource dont il s'agis. 
    ///Ex: "B" pour biome, "I" pour items
    ///</param>
    ///<param name="listeRessourcesParBiome">La liste à deux dimension à remplir</param>
    ///
    ///    
    void GenererListeRessourcesBiomes<Type>(string path, string prefix, List<List<Type>> listeRessourcesParBiome){
        int biomeId=1; // déclaration du int réprésentant le id du biome
        int ressourceId=1; // déclaration du int réprésentant le id du variant de biome
        bool resteMateriaux = true; // Déclaration d'un boolean qui servira à savoir s'il reste des matériaux dans le dossier Ressources
        List<Type> biomeTemp = new List<Type>(); // Déclaration d'une liste de materiaux
        do{
            Object ressource = Resources.Load(path+"/"+prefix+biomeId+"_"+ressourceId); //load la ressource
            Type goodTypeRessource = (Type)System.Convert.ChangeType(ressource, typeof(Type));
            if(ressource){ //si le variant de biome qu'on load n'est pas null
                biomeTemp.Add(goodTypeRessource); //ajoute le variant a la liste de biome temporaire
                ressourceId++; //incrémente le id de variant
                // Debug.Log(ressource.name);
            }else{ // variant loader n'est pas définie
            
                /*si notre id de variant est a 1 mais que le material loader est null on peu assumer qu'il n'y a plus de materials 
                dans les ressources*/   
                if(ressourceId==1){ 
                    resteMateriaux = false; // set resteMateriaux à false se qui désactive notre boucle "do while"
                    // Debug.Log(_biomesMats.Count);
                    foreach(List<Type> biome in listeRessourcesParBiome){
                        foreach(Type r in biome){
                            Debug.Log(r);
                        }
                    }
                }else{
                    listeRessourcesParBiome.Add(biomeTemp); // ajoute liste de variants de biomes à notre liste de listes de biomes
                    biomeTemp = new List<Type>(); //reset liste temporaire de biomes
                    ressourceId = 1; //reset id de variant de biome
                    biomeId++; //passe au prochain biome
                }
            }
            
        }while(resteMateriaux); //execute la boucle tant qu'il reste des matériaux
    }


    /*  Sert à convertir une valeur d'entrer en une valeur équivalente entre 0 et 1 en la passant à travers une fonction Sigmoïde.
        La valeur d'entrer est notre position en X dans notre fonction sigmoïde.
        La fonction retourne la valeur de Y sur la courbe de la fonction lorsqu'à la position X.
        Une fonction sigmoïde est une fonction dont la valeur en Y se rapproche de 0 en -X ou de 1 en +X sans jamais y toucher
    */
    float Sigmoid(float value){
        float k = 20f; // Intensiter de la courbe. Plus cette valeur est élever plus notre valeur d'entrer atteindra une valeur de 1 rapidement
        float c = .7f; // Valeur de base de la courbe (si on regarde sur un graphique c'est la valeur de Y quand X est 0)lorsque la valeur d'entrer est 0.
        return 1/(1+Mathf.Exp(-k*(value-c))); //Formule de la fonction sigmoïde. Retourne la valeur équivalente entre 0 et 1 à notre valeur d'entrer appliqué sur la courbe de la fonction sigmoïde
    }
    

    /* Fonction qui décide se qui est générer selon les paramètres séléctionnés dans l'éditeur.
        Donc est-ce qu'on déssine seulement un map noir et blanc (avec ou sans noise appliqué dessus) ou bien est-ce qu'on génère 
        l'île au complet avec des blocs en utilisant la map en noir et blanc.
    */
    void creerMap(){
        // float[,] map = GenererBordureEau(_largeurIle,_profondeurIle); // Génère un dégrader en forme de carré
        float[,] map = GenererInnondationCirculaire(_largeurIle,_profondeurIle, _rayon);
        float[,] ile = GenererTerrain(_largeurIle,_profondeurIle,_attenuateur, map);
        if(_genererMapSeulement){
            if(_activerNoise){
                DessinerMap(ile);
            }else{
                DessinerMap(map);
            }
        }else{
            if(_activerNoise){
                GenererIle(ile);
            }else{
                GenererIle(map);
            }
        }
        // StartCoroutine("BuildNavMesh");
    }

    // IEnumerator BuildNavMesh(){
    //     yield return new WaitForSeconds(1f);
    // }


    // Fonction qui génère un tableau en deux dimension représentant un map dégrader circulaire
    private float[,] GenererInnondationCirculaire(int maxX, int maxZ, float rayonEau){
        float[,] ocean = new float[maxX,maxZ]; // Declaration d'un tableau de float a 2 dimensions
        float cX = maxX/2; //centre du cercle en x
        float cZ = maxZ/2; //centre du cercle en z
        float val; // Declaration de la variable qui sera la distance entre notre position dans la boucle et le centre du cercle
        for(int x = 0; x < maxX; x++){
            for(int z = 0; z < maxZ; z++){
                // équation de distance entre deux point (pythagore). On l'utilise pour calculez la distance entre notre position courante et le centre du cercle
                val = Mathf.Sqrt(Mathf.Pow(x - cX,2) + Mathf.Pow(z - cZ,2)); 

                /* division de "val" par le rayon permet de rapporter la valeur de "val" entre 0 et 1
                puisque pour obtenir une valeur entre 0 et 1 il ne suffit qu'à diviser la valeur par sa valeur maximal possible. */
                float y = val/rayonEau;
                // on passe notre valeur de hauteur dans notre fonction sigmoide pour controller le fall-off du dégradé de notre île
                ocean[x,z]=Sigmoid(y);
            }
        }
        return ocean;
    }


    // Fonction qui génère un tableau en deux dimension représentant un map dégrader rectangulaire
    private float[,] GenererBordureEau(int maxX,int maxZ){
        float[,] ocean = new float[maxX,maxZ]; // Declaration d'un tableau de float a 2 dimensions\
        float centreX = maxX/2;
        float centreZ = maxZ/2;
        for(int x = 0; x<maxX; x++){
            for(int z = 0; z<maxZ; z++){
                float yx = Mathf.Abs(x-centreX)/centreX;
                float yz = Mathf.Abs(z-centreZ)/centreZ;
                float y = Mathf.Max(yx,yz);
                ocean[x,z]=Sigmoid(y);
            }
        }
        return ocean;
    }


    /* Fonction qui reçoit un map et applique un perlin noise dessus, cela crée essentiellement des montagnes et des valées aléatoirement dans 
    dans les limites de notre map reçu. Reçoit aussi un attenuateur qui sert à jouer avec l'intensité du noise*/ 
    private float[,] GenererTerrain(int maxX,int maxZ, float attenuateur, float[,] mapBordure){
        int bruitAleatoire = 0; // déclaration du seed de notre noise
        if(_desactiverRandom == false){ // si le dans l'éditeur je n'ai pas désactiver le random pour mon noise
            bruitAleatoire = Random.Range(0,100000); // set le seed du noise à une valeur random entre 0 et 100 000
        }
        float[,] terrain = new float[maxX,maxZ]; // Declaration d'un tableau de float a 2 dimensions
        for(int x = 0; x<maxX; x++){
            for(int z = 0; z<maxZ; z++){ // Double boucle pour parcourir le tableau en deux dimensions

                /* Le perlin noise s'assure que la valeur de Y ne soit pas totalement aléatoire, mais qu'elle soit similaire 
                au autres positions qui l'entours */
                float y = Mathf.PerlinNoise(x/attenuateur + bruitAleatoire,z/attenuateur + bruitAleatoire); 

                float yBordure = mapBordure[x,z]; // récupère notre position dans le tableau en deux dimension

                //On clamp la valeur que notre noise nous donne dans les limites de notre île pour garder la forme générale de l'île
                terrain[x,z] = Mathf.Clamp01(y - yBordure); 
            }
        }
        //retourne un map de la même forme qu'avant, mais avec des vallées et des montagnes*/
        return terrain;
    }


    /* Fonction qui convertie un tableau en deux dimensions en un tableau à une dimension. On utilise ensuite ces valeur pour
    générer un pixel d'une couleur entre noir et blanc sur une texture, qui est ensuite appliqué sur matériel sur une plane se qui nous 
    permet de visuallisé notre map */
    void DessinerMap(float[,] map){
        int larg = map.GetLength(0);
        int prof = map.GetLength(1);
        Texture2D ileTexture = new Texture2D(larg,prof);
        Color[] couleursTexture = new Color[larg*prof];
        for(int x = 0; x<larg; x++){
            for(int z = 0; z<prof; z++){
                float y = map[x,z];
                Color couleur = new Color(y,y,y,1);
                couleursTexture[x * prof + z] = couleur; //Formule qui permet de prendre une double boucle pour la rapporter dans un array a 1 dimension

            }
        }        
        ileTexture.SetPixels(couleursTexture);
        ileTexture.Apply();
        _textureRender.sharedMaterial.mainTexture = ileTexture;
        _textureRender.transform.localScale = new Vector3(larg, 1f, prof);
    }


    /* Fonction qui prend un map et l'utilise pour générer un cube pour chaque position qu'il contiens */
    void GenererIle(float[,] map){
        int larg = map.GetLength(0);
        int prof = map.GetLength(1);
        int QuantiterBiome = _biomesMats.Count - 1;
        
        for(int x = 0; x<larg; x++){
            for(int z = 0; z<prof; z++){
                float y = map[x,z];
                if(y>0f){
                    GameObject unCube = Instantiate(_cube, new Vector3(x-(larg/2),y*_coefAltitude,z-(prof/2)),Quaternion.identity);

                    //spawn ennemis aléatoirement
                    if(Random.Range(0,100) > 95 && _amountOfEnnemis>0){
                        GameObject unAgent = Instantiate((GameObject)Resources.Load("ennemi"),unCube.transform.position, Quaternion.identity);
                        unAgent.GetComponent<EnnemiEtatsManager>().perso = perso;
                        unAgent.GetComponent<EnnemiEtatsManager>().home = unCube;
                        _amountOfEnnemis--;
                    }

                    int quelBiome = Mathf.FloorToInt(map[x,z]*QuantiterBiome);
                    int quelVariant = Random.Range(0, 1);
                    int quelVariantBriser = Random.Range(2, 3);
                    // int quelVariantInexplorerMouiller = Random.Range(2, 3);
                    int quelVariantExplorerMouiller = 4;
                    int quelVariantBriserMouiller = 5;

                    unCube.transform.parent = transform;
                    unCube.GetComponent<BiomesEtatsManager>().biomeMateriel = _biomesMats[quelBiome][quelVariant];
                    unCube.GetComponent<BiomesEtatsManager>().biomeMaterielBriser = _biomesMats[quelBiome][quelVariantBriser];
                    // unCube.GetComponent<BiomesEtatsManager>().biomeMaterielInexplorerMouiller = _biomesMats[quelBiome][quelVariantBriser];
                    unCube.GetComponent<BiomesEtatsManager>().biomeMaterielExplorerMouiller = _biomesMats[quelBiome][quelVariantExplorerMouiller];
                    unCube.GetComponent<BiomesEtatsManager>().biomeMaterielBriserMouiller = _biomesMats[quelBiome][quelVariantBriserMouiller];

                    // ajoute un item possible en enfant aléatoirement
                    if(Random.Range(0,100) > 95 && _biomesItems.Count-1>=quelBiome){
                        int quelItem = Random.Range(0, (_biomesItems[quelBiome].Count -1));
                        //modifier +.4f pour la hauteur set dans le prefab
                        GameObject item = Instantiate(_biomesItems[quelBiome][quelItem], new Vector3(unCube.transform.position.x, unCube.transform.position.y + _biomesItems[quelBiome][quelItem].transform.position.y,unCube.transform.position.z), _biomesItems[quelBiome][quelItem].transform.rotation, unCube.transform);
                        // Vector3 itemLocation = new Vector3(unCube.transform.position.x, unCube.transform.position.y + .4f,unCube.transform.position.z);
                        item.name = "item";
                        item.SetActive(false);
                    }
                }
            }
        }
    }

}
