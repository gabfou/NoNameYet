﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Vaguator : MonoBehaviour
{
    public List<GameObject> listVague;
    public List<Agro> currentAgro = new List<Agro>();
    Transform player;

    void GetCurrentAgro()
    {
        currentAgro.Clear();
        for (int i = 0; i < listVague.First().transform.childCount; i++)
            currentAgro.Add(listVague.First().transform.GetChild(i).GetComponent<Agro>());
        foreach (Agro a in currentAgro)
            if (a) a.Cible = player;
    }

    void Start()
    {
        player = GameManager.instance.player.transform;
        listVague.First().SetActive(true);
        GetCurrentAgro();
    }

   void NextVague()
    {
        listVague.RemoveAt(0);
        if (listVague.Count < 1)
        {
            GameObject.Destroy(this);
            return ;
        }
        listVague.First().SetActive(true);
        GetCurrentAgro();
    }

    void checkVague()
    {
        if (currentAgro.TrueForAll(a => a == null || a.life < 1))
            NextVague();
    }

    // Update is called once per frame
    void Update()
    {
        checkVague();
    }
}
