﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tonneaux : MonoBehaviour {

	public float speed;
	public float side;
	Rigidbody2D rbody;
	List<int> l = new List<int>();
	float targetside;
	float sideVelocity;
	float timeWithoutMoving = 0;
	public AudioClip deathClip;
	[Range(0,1)] public float deathVolume = 1;
	void Start () {
		targetside = side;
		rbody = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Mathf.Abs(rbody.velocity.x) < 0.01f)
			timeWithoutMoving += Time.fixedDeltaTime;
		else
			timeWithoutMoving = 0;
		if (timeWithoutMoving > 0.05f)
		{
			if (side != targetside)
				side = targetside;
			else
			{
				targetside = -targetside;
				side = targetside;
			}
		}
		side = Mathf.SmoothDamp(side, targetside, ref sideVelocity, 0.6f);
		rbody.velocity = new Vector2( side * speed, rbody.velocity.y);
	}

	IEnumerator Death()
	{
		GetComponent<Animator>()?.SetTrigger("death");
		GameObject.Destroy(GetComponent<rotateOnVX>());
		GameObject.Destroy(GetComponent<Collider2D>());
		if (deathClip)
			AudioSource.PlayClipAtPoint(deathClip, transform.position, deathVolume);
		rbody.angularVelocity = 0;
		yield return new WaitForSeconds(0.5f);
		GameObject.Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (l.Contains(other.gameObject.GetInstanceID()))
			return ;
		l.Add(other.gameObject.GetInstanceID());
		if (other.gameObject.tag == "ChangeDirTonneaux")
			targetside  = -targetside;
		else if (other.gameObject.tag == "ChangeDirTonneauxBrut" || other.gameObject.GetComponent<tonneaux>())
		{
			side = -side;
			targetside  = -targetside;
		}
		else if (other.gameObject.tag == "DestroyTonneaux")
		{
			// GameObject.Destroy(gameObject);
			StartCoroutine(Death());
		}
	}
}
