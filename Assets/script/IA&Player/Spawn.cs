﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

	public GameObject gameObject = null;
	public Transform parent = null;
	public float cd =1;
	public float decalage = 0;
	public float randomFrom0 = 0;
	float actualcd;
	public bool onlyIfLastDie = false;
	GameObject last = null;
	public float distToplayer = -1;
	// public float distMax = 300;
	Transform player;
	void Start () {
		actualcd = cd + Random.Range(0, randomFrom0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (decalage > 0 && (onlyIfLastDie == false || last == null))
		{
			decalage -= Time.fixedDeltaTime;
			return ;
		}
		actualcd -= Time.fixedDeltaTime;
		if (actualcd <= 0 && (onlyIfLastDie == false || last == null) && (distToplayer <= 0 || distToplayer < Vector2.Distance(GameManager.instance.player.transform.transform.position, transform.position)))
		{
			actualcd = cd + Random.Range(0, randomFrom0);
			last = GameObject.Instantiate(gameObject, transform.position, Quaternion.identity, parent);
		}
	}
}
