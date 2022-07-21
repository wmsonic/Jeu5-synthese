using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poisson : MonoBehaviour {
	public GameObject poissonPrefab;
	public int nbPoissons = 20;
	public GameObject[] toutPoissons;
	public Vector3 swimLimits = new Vector3(5,5,5);
	
	[Header("Fish Settings")]
	[Range(0.0f, 5.0f)]
	public float minSpeed;
	[Range(0.0f, 5.0f)]
	public float maxSpeed;
	[Range(1.0f, 10.0f)]
	public float neighbourDistance;
	[Range(0.0f, 5.0f)]
	public float rotationSpeed;

	// Use this for initialization
	void Start () {
		toutPoissons = new GameObject[nbPoissons];
		for(int i = 0; i < nbPoissons; i++)
		{
			//possibilité de mouvement des poissons
			Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x,swimLimits.x), Random.Range(-swimLimits.y,swimLimits.y) ,Random.Range(-swimLimits.z,swimLimits.z));
			GameObject newPoisson = Instantiate(poissonPrefab, pos, Quaternion.identity, this.transform);
			newPoisson.GetComponent<mouvPoisson>().myManager = this;
			toutPoissons[i] = newPoisson;
		}
	}
}