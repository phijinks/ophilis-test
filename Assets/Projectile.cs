using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	float dx; // x speed
	private ParticleSystem particles;
	private GameObject player;
	Vector3 prevPos;
	float totalDist = 0; // distance the projectile has travelled
	float travelDist = 20;

	void Start () {
		particles = GetComponent<ParticleSystem>();
		player = GameObject.Find("Player");
		prevPos = player.transform.position;
		dx = (player.transform.localScale.x * 1.5f);
	}
	
	void Update () {
		float playerSpeed = (player.transform.position - prevPos).x;
		prevPos = player.transform.position;

		transform.Translate(new Vector3(dx + playerSpeed*0.6f, 0, 0));
		totalDist += dx;

		if(Mathf.Abs(totalDist) > travelDist) {
			particles.Stop();
			dx = 0;
			Destroy(GetComponent<Collider2D>());
			Destroy(gameObject, 5f);
		}
	}
}
