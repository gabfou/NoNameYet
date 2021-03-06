﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Random;

public class Throwthing : MonoBehaviour {
    public Rigidbody2D Projectile;
    public float angleprecision = 0;
    public float nbofshotbyburst = 1;
    public float timetoshoot = 1;
    public float timetoshootRand = 0;
    public float cd = 1;
    public float distanceMin = 2;
    public float impulsionForce = 10;
    public bool cannotLoseSight = false;
    public bool willShootStopMove = true;


    float delay = 0;
    Animator anim = null;
    bool willShoot = false;
    Agro agro = null;
    bool playerInSight = false;
    Transform cible = null;
    Transform parent;
    Vector3 savelocalScale;

    // Use this for initialization
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        actualTimeToShoot = timetoshoot + Random.Range(0, timetoshootRand);
        delay = actualTimeToShoot;
        agro = GetComponentInParent<Agro>();
        if (transform.parent.GetInstanceID() != agro.transform.GetInstanceID())
            parent = transform.parent;
    }
    float actualTimeToShoot;
    // Update is called once per frame
    void Update()
    {
        if (agro.isDead || cible == null)
            return ;
        if (willShootStopMove)
            agro.cannotmove = (agro.cannotmove) ? willShoot : false;
        // if ((delay > 0 && willShoot) || delay > actualTimeToShoot)
            delay -= Time.deltaTime;
        if (agro && delay < actualTimeToShoot && ((agro.grounded == false && agro.flying == false)
                                            || agro.IsOuchstun == true
                                            || !cible
                                            || distanceMin > Vector2.Distance(transform.position, cible.position)))
            delay = actualTimeToShoot;

        if (!playerInSight)
            return;

        if ( willShoot == false && delay <= actualTimeToShoot)
        {
            willShoot = true;
            anim.SetBool("willshoot", true);
            if (parent)
                savelocalScale = transform.parent.localScale;
        }
        if (parent)
        {
            if (agro && !agro.IsFacingPlayer())
                parent.localScale = new Vector3(-savelocalScale.x,savelocalScale.y,savelocalScale.z);
            else
                parent.localScale = savelocalScale;
        }
        if (delay > 0)
            return;

        if (parent)
            parent.localScale = savelocalScale;
        willShoot = false;
        anim.SetBool("willshoot", false);
        actualTimeToShoot = timetoshoot + Random.Range(0, timetoshootRand);
        delay = cd + actualTimeToShoot;
        shoot(cible.position);
    }

    [HideInInspector] public delegate void ModifProjectile(Rigidbody2D rb);
    [HideInInspector]public ModifProjectile modifProjectile = null; 

    [HideInInspector] public Rigidbody2D lastmp;
    public void shoot(Vector2 cible)
    {

        anim.SetTrigger("shooting");
        for (int i = 0; i < nbofshotbyburst; i++)
        {

            lastmp = GameObject.Instantiate(Projectile, transform.position, Quaternion.identity);
            lastmp.transform.right = (cible - (Vector2)transform.position).normalized;
            lastmp.transform.Rotate(transform.forward, Random.Range(-angleprecision / 2, angleprecision / 2));
            lastmp.AddForce(lastmp.transform.right * impulsionForce, ForceMode2D.Impulse);
            if (modifProjectile != null)
                modifProjectile(lastmp);    
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInSight = true;
            cible = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!cannotLoseSight && collision.tag == "Player")
        {
            playerInSight = false;
            willShoot = false;
            anim.SetBool("willshoot", false);
            delay = cd + timetoshoot;
            cible = null;
        }
    }
    private void OnDisable() {
        if (anim)
            anim.SetBool("willshoot", false);
    }
}
