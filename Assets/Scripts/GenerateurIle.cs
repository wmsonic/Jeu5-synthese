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
    [SerializeField] private Inventaire inventaire;
    [SerializeField] private GameObject waterPlane;
    [SerializeField] private GameObject oiseaux;
    

    private List<List<Material>> _biomesMats = new List<List<Material>>();
    private List<List<GameObject>> _biomesItems = new List<List<GameObject>>();
    private PowerUpSpawner _powerUpSpawner;
    private bool _bouerSpawnSet = false;
    private bool _masqueSpawnSet = false;
    private List<GameObject> _listeEnnemis = new List<GameObject>();
    private List<Transform> _listeCatcus = new List<Transform>();
    // private List<GameObject> _listeVases = new List<GameObject>();
    // private List<GameObject> _listePieces = new List<GameObject>();
    private int _amountVases = 0;
    private int _amountPieces = 0;

    void Start(){
        Time.timeScale = 1f;
        // GenererListeMateriauxBiomes();
        _powerUpSpawner = GetComponent<PowerUpSpawner>();
        GenererListeRessourcesBiomes<Material>("materials", "B", _biomesMats);
        GenererListeRessourcesBiomes<GameObject>("objects", "I", _biomesItems);
        creerMap();
        perso.SetActive(false);
        waterPlane.SetActive(false);
        oiseaux.SetActive(false);
        GetComponent<NavMeshSurface>().BuildNavMesh();
        perso.SetActive(true);
        waterPlane.SetActive(true);
        oiseaux.SetActive(true);
        waterPlane.GetComponent<WaterRiser>().riseWater(0f);
    }


    ///<summary>
    ///Fonction g??n??rique qui remplis une liste (<paramref name="listeRessourcesParBiome"/>) ?? deux dimensions des biomes et de ces ressources (<typeparamref name="Type"/> donn?? lors de l'appel) 
    ///en parcrourant le dossier donn?? (<paramref name="path"/>) dans le dossier Ressources et en les ajoutants si le nom de la ressource loader correspond au format de nom : <paramref name="prefix"/>*nombre*_*nombre*
    ///</summary>
    ///<typeparam name="Type">Le type d'objet des ressources qui seront loader </typeparam>
    ///<param name="path">Chemin du dossier ?? parcourir dans Ressources</param>
    ///<param name="prefix">Lettre qui est situ??e au d??but du nom de la ressource.
    ///Habituellement pour indiqu?? le type de ressource dont il s'agis. 
    ///Ex: "B" pour biome, "I" pour items
    ///</param>
    ///<param name="listeRessourcesParBiome">La liste ?? deux dimension ?? remplir</param>
    ///
    ///    
    void GenererListeRessourcesBiomes<Type>(string path, string prefix, List<List<Type>> listeRessourcesParBiome){
        int biomeId=1; // d??claration du int r??pr??sentant le id du biome
        int ressourceId=1; // d??claration du int r??pr??sentant le id du variant de biome
        bool resteMateriaux = true; // D??claration d'un boolean qui servira ?? savoir s'il reste des mat??riaux dans le dossier Ressources
        List<Type> biomeTemp = new List<Type>(); // D??claration d'une liste de materiaux
        do{
            Object ressource = Resources.Load(path+"/"+prefix+biomeId+"_"+ressourceId); //load la ressource
            Type goodTypeRessource = (Type)System.Convert.ChangeType(ressource, typeof(Type));
            if(ressource){ //si le variant de biome qu'on load n'est pas null
                biomeTemp.Add(goodTypeRessource); //ajoute le variant a la liste de biome temporaire
                ressourceId++; //incr??mente le id de variant
                // Debug.Log(ressource.name);
            }else{ // variant loader n'est pas d??finie
            
                /*si notre id de variant est a 1 mais que le material loader est null on peu assumer qu'il n'y a plus de materials 
                dans les ressources*/   
                if(ressourceId==1){ 
                    resteMateriaux = false; // set resteMateriaux ?? false se qui d??sactive notre boucle "do while"
                    // Debug.Log(_biomesMats.Count);
                    foreach(List<Type> biome in listeRessourcesParBiome){
                        foreach(Type r in biome){
                            Debug.Log(r);
                        }
                    }
                }else{
                    listeRessourcesParBiome.Add(biomeTemp); // ajoute liste de variants de biomes ?? notre liste de listes de biomes
                    biomeTemp = new List<Type>(); //reset liste temporaire de biomes
                    ressourceId = 1; //reset id de variant de biome
                    biomeId++; //passe au prochain biome
                }
            }
            
        }while(resteMateriaux); //execute la boucle tant qu'il reste des mat??riaux
    }


    /*  Sert ?? convertir une valeur d'entrer en une valeur ??quivalente entre 0 et 1 en la passant ?? travers une fonction Sigmo??de.
        La valeur d'entrer est notre position en X dans notre fonction sigmo??de.
        La fonction retourne la valeur de Y sur la courbe de la fonction lorsqu'?? la position X.
        Une fonction sigmo??de est une fonction dont la valeur en Y se rapproche de 0 en -X ou de 1 en +X sans jamais y toucher
    */
    float Sigmoid(float value){
        float k = 20f; // Intensiter de la courbe. Plus cette valeur est ??lever plus notre valeur d'entrer atteindra une valeur de 1 rapidement
        float c = .7f; // Valeur de base de la courbe (si on regarde sur un graphique c'est la valeur de Y quand X est 0)lorsque la valeur d'entrer est 0.
        return 1/(1+Mathf.Exp(-k*(value-c))); //Formule de la fonction sigmo??de. Retourne la valeur ??quivalente entre 0 et 1 ?? notre valeur d'entrer appliqu?? sur la courbe de la fonction sigmo??de
    }
    

    /* Fonction qui d??cide se qui est g??n??rer selon les param??tres s??l??ctionn??s dans l'??diteur.
        Donc est-ce qu'on d??ssine seulement un map noir et blanc (avec ou sans noise appliqu?? dessus) ou bien est-ce qu'on g??n??re 
        l'??le au complet avec des blocs en utilisant la map en noir et blanc.
    */
    void creerMap(){
        // float[,] map = GenererBordureEau(_largeurIle,_profondeurIle); // G??n??re un d??grader en forme de carr??
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


    // Fonction qui g??n??re un tableau en deux dimension repr??sentant un map d??grader circulaire
    private float[,] GenererInnondationCirculaire(int maxX, int maxZ, float rayonEau){
        float[,] ocean = new float[maxX,maxZ]; // Declaration d'un tableau de float a 2 dimensions
        float cX = maxX/2; //centre du cercle en x
        float cZ = maxZ/2; //centre du cercle en z
        float val; // Declaration de la variable qui sera la distance entre notre position dans la boucle et le centre du cercle
        for(int x = 0; x < maxX; x++){
            for(int z = 0; z < maxZ; z++){
                // ??quation de distance entre deux point (pythagore). On l'utilise pour calculez la distance entre notre position courante et le centre du cercle
                val = Mathf.Sqrt(Mathf.Pow(x - cX,2) + Mathf.Pow(z - cZ,2)); 

                /* division de "val" par le rayon permet de rapporter la valeur de "val" entre 0 et 1
                puisque pour obtenir une valeur entre 0 et 1 il ne suffit qu'?? diviser la valeur par sa valeur maximal possible. */
                float y = val/rayonEau;
                // on passe notre valeur de hauteur dans notre fonction sigmoide pour controller le fall-off du d??grad?? de notre ??le
                ocean[x,z]=Sigmoid(y);
            }
        }
        return ocean;
    }


    // Fonction qui g??n??re un tableau en deux dimension repr??sentant un map d??grader rectangulaire
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


    /* Fonction qui re??oit un map et applique un perlin noise dessus, cela cr??e essentiellement des montagnes et des val??es al??atoirement dans 
    dans les limites de notre map re??u. Re??oit aussi un attenuateur qui sert ?? jouer avec l'intensit?? du noise*/ 
    private float[,] GenererTerrain(int maxX,int maxZ, float attenuateur, float[,] mapBordure){
        int bruitAleatoire = 0; // d??claration du seed de notre noise
        if(_desactiverRandom == false){ // si le dans l'??diteur je n'ai pas d??sactiver le random pour mon noise
            bruitAleatoire = Random.Range(0,100000); // set le seed du noise ?? une valeur random entre 0 et 100 000
        }
        float[,] terrain = new float[maxX,maxZ]; // Declaration d'un tableau de float a 2 dimensions
        for(int x = 0; x<maxX; x++){
            for(int z = 0; z<maxZ; z++){ // Double boucle pour parcourir le tableau en deux dimensions

                /* Le perlin noise s'assure que la valeur de Y ne soit pas totalement al??atoire, mais qu'elle soit similaire 
                au autres positions qui l'entours */
                float y = Mathf.PerlinNoise(x/attenuateur + bruitAleatoire,z/attenuateur + bruitAleatoire); 

                float yBordure = mapBordure[x,z]; // r??cup??re notre position dans le tableau en deux dimension

                //On clamp la valeur que notre noise nous donne dans les limites de notre ??le pour garder la forme g??n??rale de l'??le
                terrain[x,z] = Mathf.Clamp01(y - yBordure); 
            }
        }
        //retourne un map de la m??me forme qu'avant, mais avec des vall??es et des montagnes*/
        return terrain;
    }


    /* Fonction qui convertie un tableau en deux dimensions en un tableau ?? une dimension. On utilise ensuite ces valeur pour
    g??n??rer un pixel d'une couleur entre noir et blanc sur une texture, qui est ensuite appliqu?? sur mat??riel sur une plane se qui nous 
    permet de visuallis?? notre map */
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


    /* Fonction qui prend un map et l'utilise pour g??n??rer un cube pour chaque position qu'il contiens */
    void GenererIle(float[,] map){
        int larg = map.GetLength(0);
        int prof = map.GetLength(1);
        int QuantiterBiome = _biomesMats.Count - 1;
        for(int x = 0; x<larg; x++){
            for(int z = 0; z<prof; z++){
                float y = map[x,z];
                if(y>0f){
                    GameObject unCube = Instantiate(_cube, new Vector3(x-(larg/2),y*_coefAltitude,z-(prof/2)),Quaternion.identity);

                    //spawn ennemis al??atoirement
                    if(Random.Range(0,100) > 95 && _amountOfEnnemis>0 && unCube.transform.position.y < 5f){
                        GameObject unAgent = Instantiate((GameObject)Resources.Load("ennemi"),unCube.transform.position, Quaternion.identity);
                        unAgent.GetComponent<EnnemiEtatsManager>().perso = perso;
                        unAgent.GetComponent<EnnemiEtatsManager>().home = unCube;
                        // GameObject patrolPoints = Instantiate((GameObject)Resources.Load("EnnemiPatrolPoints"),unCube.transform.position, Quaternion.identity);
                        // unAgent.GetComponent<EnnemiEtatsManager>().patrolPoints = patrolPoints.GetComponent<PatrolPoints>().GetPatrolPoints();
                        _listeEnnemis.Add(unAgent);
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

                    if(Random.Range(0,100) > 50 && unCube.transform.position.y < 4f && !_bouerSpawnSet && NearCenter(larg,prof,x,z)){
                        _powerUpSpawner.SetSpawnPosition("bouer",unCube.transform.position);
                        _bouerSpawnSet = true;
                        Debug.Log("bouer spawn set");
                    }

                    if(Random.Range(0,100) > 50 && unCube.transform.position.y < 3f && !_masqueSpawnSet && NearCenter(larg,prof,x,z)){
                        _powerUpSpawner.SetSpawnPosition("masque",unCube.transform.position);
                        _masqueSpawnSet = true;
                        Debug.Log("masque spawn set");
                    }

                    // ajoute un item possible en enfant al??atoirement
                    if(Random.Range(0,100) > 95 && _biomesItems.Count-1 >= quelBiome){
                        int quelItem = Random.Range(0, (_biomesItems[quelBiome].Count));
                        //modifier +.4f pour la hauteur set dans le prefab
                        GameObject item = Instantiate(_biomesItems[quelBiome][quelItem], new Vector3(unCube.transform.position.x, unCube.transform.position.y + _biomesItems[quelBiome][quelItem].transform.position.y,unCube.transform.position.z), _biomesItems[quelBiome][quelItem].transform.rotation, unCube.transform);
                        // Vector3 itemLocation = new Vector3(unCube.transform.position.x, unCube.transform.position.y + .4f,unCube.transform.position.z);
                        if(item.CompareTag("cactus")) {
                            _listeCatcus.Add(item.transform);
                        }else if(item.CompareTag("collectible")){
                            // Debug.Log(item.GetComponent<Item>().itemType);
                            if(item.GetComponent<Item>().itemType == "vases"){
                                // _listeVases.Add(item);
                                _amountVases++;
                            }else if(item.GetComponent<Item>().itemType == "pieces"){
                                // _listePieces.Add(item);
                                _amountPieces++;
                            }
                        }
                        item.name = "item";
                        item.SetActive(false);
                    }
                }
            }
        }

        foreach (GameObject ennemi in _listeEnnemis)
        {
            List<Vector3> patrolPoints = new List<Vector3>();
            for (int i = 0; i <= 4; i++){
                patrolPoints.Add(_listeCatcus[Random.Range(0,_listeCatcus.Count)].position);
            }
            ennemi.GetComponent<EnnemiEtatsManager>().patrolPoints = patrolPoints;
        }

        inventaire.SetTotalPieces(_amountPieces);
        inventaire.SetTotalVases(_amountVases);
    }

    private bool NearCenter(int maxX,int maxZ,int currentX, int currentZ){
        float divider = 4f; // doit rester plus gros que 2 sinon tout sera la fonction retournera toujours false;
        if(currentX > maxX / divider && currentX < maxX - maxX / divider && currentZ > maxZ / divider && currentZ < maxZ - maxZ / divider){
            return true;
        }
        return false;
    }

}
