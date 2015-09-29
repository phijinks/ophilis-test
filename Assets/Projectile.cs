using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	float dx; // x speed
	private ParticleSystem particles;
	private GameObject player;
	float totalDist = 0; // distance the projectile has travelled
	float travelDist = 20;

	void Start () {
		particles = GetComponent<ParticleSystem>();
		player = GameObject.Find("Player");
		dx = player.transform.localScale.x * 1.2f;
	}
	
	void Update () {
		transform.Translate(new Vector3(dx, 0, 0));
		totalDist += dx;

		if(Mathf.Abs(totalDist) > travelDist) {
			particles.Stop();
			dx = 0;
			Destroy(GetComponent<Collider2D>());
			Destroy(gameObject, 5f);
		}
	}
}
