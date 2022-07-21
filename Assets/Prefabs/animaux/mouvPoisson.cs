using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouvPoisson : MonoBehaviour {
	
	public poisson myManager;
	float speed;

	private Vector3 home = new Vector3();

	// Use this for initialization
	void Start () {
		//calibrer la vitesse des poissons
		speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
		home = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//faire bouger les poissons en z
		transform.Translate(0, 0, Time.deltaTime * speed);
		ApplyRules();
		//distance max avant qu'ils revniennent à leurs position de départ
		if(Vector3.Distance(this.transform.position,home)>60f){
			this.transform.LookAt(home);
		}
	}
	void ApplyRules()
	{
		//le groupe de poissons
		GameObject[] gos;
		gos = myManager.toutPoissons;

		//trouver le centre du groupe de poissons
		Vector3 vcentre = Vector3.zero;
		Vector3 vavoid = Vector3.zero;

		//calculer la distance d'un poisson avec un groupe
		float nDistance;
		int groupSize = 0;

		foreach (GameObject go in gos) 
		{
			if(go != this.gameObject)
			{	
				nDistance = Vector3.Distance(go.transform.position,this.transform.position);
				if(nDistance <= myManager.neighbourDistance)
				{
					//position moyenne du groupe
					vcentre += go.transform.position;	
					groupSize++;	
					
					if(nDistance < 1.0f)		
					{
						vavoid = vavoid + (this.transform.position - go.transform.position);
					}
				}
			}
		} 
		
		//si les poissons sont dans un groupe
		if(groupSize > 0)
		{
			//fait tourner les poissons en tenant compte du centredu groupe
			vcentre = vcentre/groupSize;
			Vector3 direction = (vcentre + vavoid) - transform.position;
			if(direction != Vector3.zero)
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
		}
	}
}
//tutoriel utilisé : https://www.intel.com/content/www/us/en/developer/articles/case-study/fish-flocking-with-unitysimulating-the-behavior-of-object-moving-with-ai.html