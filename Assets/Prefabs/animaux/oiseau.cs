using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oiseau : MonoBehaviour {
	public GameObject oiseauPrefab;
	public int nbOiseaux = 20;
	public GameObject[] toutOiseaux;
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
		toutOiseaux = new GameObject[nbOiseaux];
		for(int i = 0; i < nbOiseaux; i++)
		{
			Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x,swimLimits.x),
				                      							Random.Range(-swimLimits.y,swimLimits.y),
				                      							Random.Range(-swimLimits.z,swimLimits.z));
			GameObject newOiseau = Instantiate(oiseauPrefab, pos, Quaternion.identity, this.transform);
			newOiseau.GetComponent<mouvOiseau>().myManager = this;
			toutOiseaux[i] = newOiseau;
		}
	}
}