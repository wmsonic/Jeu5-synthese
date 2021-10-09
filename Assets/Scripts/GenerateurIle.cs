using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    

    private List<List<Material>> _biomesMats = new List<List<Material>>();

    // [SerializeField] private Material[] _biomes;
    // Materials biomes 
    // GenericPropertyJSON:{"name":"_biomes","type":-1,"arraySize":11,"arrayType":"PPtr<$Material>","children":[{"name":"Array","type":-1,"arraySize":11,"arrayType":"PPtr<$Material>","children":[{"name":"size","type":12,"val":11},{"name":"data","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"21a126cbc2741704aaaa8be7392b8fa5\",\"localId\":2100000,\"type\":2,\"instanceID\":24560}"},{"name":"data","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"21a126cbc2741704aaaa8be7392b8fa5\",\"localId\":2100000,\"type\":2,\"instanceID\":24560}"},{"name":"data","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"485382d9211a4054dab83440546e068c\",\"localId\":2100000,\"type\":2,\"instanceID\":24562}"},{"name":"data","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"485382d9211a4054dab83440546e068c\",\"localId\":2100000,\"type\":2,\"instanceID\":24562}"},{"name":"data","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"5c6872f53900e2a4daebad3b99630d4e\",\"localId\":2100000,\"type\":2,\"instanceID\":24564}"},{"name":"data","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"5c6872f53900e2a4daebad3b99630d4e\",\"localId\":2100000,\"type\":2,\"instanceID\":24564}"},{"name":"data","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"af837b00ddeb6084f8fa9f429675732e\",\"localId\":2100000,\"type\":2,\"instanceID\":24566}"},{"name":"data","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"af837b00ddeb6084f8fa9f429675732e\",\"localId\":2100000,\"type\":2,\"instanceID\":24566}"},{"name":"data","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"aa2be527c6e3dcc48a183ae83f697436\",\"localId\":2100000,\"type\":2,\"instanceID\":24568}"},{"name":"data","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"aa2be527c6e3dcc48a183ae83f697436\",\"localId\":2100000,\"type\":2,\"instanceID\":24568}"},{"name":"data","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"d335f336260ff484c8cd0ccf5157e355\",\"localId\":2100000,\"type\":2,\"instanceID\":24570}"}]}]}

    [SerializeField] private Color[] _biomesCacher;    
    // Couleurs pour _biomes
    // GenericPropertyJSON:{"name":"_biomes","type":-1,"arraySize":11,"arrayType":"Color","children":[{"name":"Array","type":-1,"arraySize":11,"arrayType":"Color","children":[{"name":"size","type":12,"val":11},{"name":"data","type":4,"children":[{"name":"r","type":2,"val":0.3890174},{"name":"g","type":2,"val":0.6127156},{"name":"b","type":2,"val":0.8867924},{"name":"a","type":2,"val":0}]},{"name":"data","type":4,"children":[{"name":"r","type":2,"val":0.9150943},{"name":"g","type":2,"val":0.914182365},{"name":"b","type":2,"val":0.5050285},{"name":"a","type":2,"val":0}]},{"name":"data","type":4,"children":[{"name":"r","type":2,"val":0.5372549},{"name":"g","type":2,"val":0.411764741},{"name":"b","type":2,"val":0.09411766},{"name":"a","type":2,"val":0}]},{"name":"data","type":4,"children":[{"name":"r","type":2,"val":0.782593369},{"name":"g","type":2,"val":1},{"name":"b","type":2,"val":0.438679218},{"name":"a","type":2,"val":0}]},{"name":"data","type":4,"children":[{"name":"r","type":2,"val":0.293154836},{"name":"g","type":2,"val":0.8490566},{"name":"b","type":2,"val":0.1802243},{"name":"a","type":2,"val":0}]},{"name":"data","type":4,"children":[{"name":"r","type":2,"val":0},{"name":"g","type":2,"val":0.5377358},{"name":"b","type":2,"val":0.0179727152},{"name":"a","type":2,"val":0}]},{"name":"data","type":4,"children":[{"name":"r","type":2,"val":0.292141348},{"name":"g","type":2,"val":0.9528302},{"name":"b","type":2,"val":0.6047849},{"name":"a","type":2,"val":0}]},{"name":"data","type":4,"children":[{"name":"r","type":2,"val":0.194686741},{"name":"g","type":2,"val":0.235849082},{"name":"b","type":2,"val":0.227028579},{"name":"a","type":2,"val":0}]},{"name":"data","type":4,"children":[{"name":"r","type":2,"val":0.459460676},{"name":"g","type":2,"val":0.5566038},{"name":"b","type":2,"val":0.5350164},{"name":"a","type":2,"val":0}]},{"name":"data","type":4,"children":[{"name":"r","type":2,"val":0.9056604},{"name":"g","type":2,"val":0.9056604},{"name":"b","type":2,"val":0.9056604},{"name":"a","type":2,"val":0}]},{"name":"data","type":4,"children":[{"name":"r","type":2,"val":0.9056604},{"name":"g","type":2,"val":0.9056604},{"name":"b","type":2,"val":0.9056604},{"name":"a","type":2,"val":0}]}]}]}
    
    void Start(){
        genererListeMateriauxBiomes();
        creerMap();
    }

    void genererListeMateriauxBiomes(){
        int biomeId=1;
        int variantBiomeId=1;
        bool resteMateriaux = true;
        List<Material> biomeTemp = new List<Material>();
        do{

            Material variantBiome = (Material)Resources.Load("B"+biomeId+"_"+variantBiomeId+""); //load le material
            if(variantBiome){ //si le variant de biome qu'on load n'est pas null
                biomeTemp.Add(variantBiome); //ajoute le variant a la liste de biome temporaire
                variantBiomeId++;
                // Debug.Log(variantBiome.name);
            }else{ // variant loader n'est pas définie
                if(variantBiomeId==1){ //si notre id de variant est a 1 mais que le material est null on peu assumer qu'il n'y a plus de materials dans les ressources donc on désactive le while
                    resteMateriaux = false;
                    Debug.Log(_biomesMats.Count);
                }else{
                    _biomesMats.Add(biomeTemp); // ajoute liste de variants de biomes à notre liste de listes de biomes
                    biomeTemp = new List<Material>(); //reset liste temporaire de biomes
                    variantBiomeId = 1; //reset id de variant de biome
                    biomeId++; //passe au prochain biome
                }
            }
            
        }while(resteMateriaux);
    }

    float Sigmoid(float value){
        float k = 20f; // Intensiter de la courbe
        float c = .7f; // Grosseur de base
        return 1/(1+Mathf.Exp(-k*(value-c)));
    }
    
    void creerMap(){
        // float[,] map = GenererBordureEau(_largeurIle,_profondeurIle);
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
    }

    // Devoir innondation Circulaire 
    // Auteur : William Morin
    private float[,] GenererInnondationCirculaire(int maxX, int maxZ, float rayonEau){
        float[,] ocean = new float[maxX,maxZ]; // Declaration d'un tableau de float a 2 dimensions\
        float cX = maxX/2; //centre du cercle en x
        float cZ = maxZ/2; //centre du cercle en z
        float val; // Declaration de la variable qui sera la distance entre notre position dans la boucle et le centre du cercle
        for(int x = 0; x < maxX; x++){
            for(int z = 0; z < maxZ; z++){
                // équation de distance entre deux point (pythagore). On l'utilise pour calculez la distance entre notre position courante et le centre du cercle
                val = Mathf.Sqrt(Mathf.Pow(x - cX,2) + Mathf.Pow(z - cZ,2)); 
                // division de "val" par le rayon permet de rapporter la valeur de "val" entre 0 et 1
                // puisque pour obtenir une valeur entre 0 et 1 il ne suffit qu'à diviser la valeur par sa valeur maximal possible.
                float y = val/rayonEau;
                ocean[x,z]=Sigmoid(y);
            }
        }
        return ocean;
    }

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

    private float[,] GenererTerrain(int maxX,int maxZ, float attenuateur, float[,] mapBordure){
        int bruitAleatoire = 0;
        if(_desactiverRandom == false){
            bruitAleatoire = Random.Range(0,100000);
        }
        float[,] terrain = new float[maxX,maxZ]; // Declaration d'un tableau de float a 2 dimensions
        for(int x = 0; x<maxX; x++){
            for(int z = 0; z<maxZ; z++){
                // float y = Random.Range(0f,1f);
                float y = Mathf.PerlinNoise(x/attenuateur + bruitAleatoire,z/attenuateur + bruitAleatoire);
                float yBordure = mapBordure[x,z];
                terrain[x,z] = Mathf.Clamp01(y - yBordure);
            }
        }
        return terrain;
    }


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

    void GenererIle(float[,] map){
        int larg = map.GetLength(0);
        int prof = map.GetLength(1);
        int QuantiterBiome = _biomesMats.Count;
        for(int x = 0; x<larg; x++){
            for(int z = 0; z<prof; z++){
                float y = map[x,z];
                if(y>0f){
                    GameObject unCube = Instantiate(_cube, new Vector3(x-(larg/2),y*_coefAltitude,z-(prof/2)),Quaternion.identity);
                    int quelBiome = Mathf.FloorToInt(map[x,z]*QuantiterBiome);
                    int quelVariant = Random.Range(0, (_biomesMats[quelBiome].Count -1));

                    unCube.transform.parent = transform;
                    unCube.GetComponent<BiomesEtatsManager>().biomeMateriel = _biomesMats[quelBiome][quelVariant];
                    // if(_inexplorer){
                    //     unCube.GetComponent<Renderer>().material.color = _biomesCacher[quelBiome];
                    // }else{
                    //     unCube.GetComponent<Renderer>().material = _biomesMats[quelBiome][quelVariant];

                    // }
                }
            }
        }
    }

}
