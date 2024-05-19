﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Resource : MonoBehaviour
{
    [SerializeField] ResourceType resourceType;
    [SerializeField] int amount;
    Damageable damageable;
    public bool isHover;

    //HoverVisual
    new private Renderer renderer;
    private Color emissionColor;
    private Color emissionColor2;

    void Awake()
    {
        damageable = GetComponent<Damageable>();
        damageable.onDestroy.AddListener(GiveResource);
        damageable.onHit.AddListener(HitResource);

        renderer = GetComponent<Renderer>();
        if (renderer)
            emissionColor = renderer.material.GetColor("_GroundColor");
            emissionColor2 = renderer.material.GetColor("_TopColor");
    }

    void GiveResource()
    {
        BuildingManager.instance.AddResource(resourceType, amount);
    }

    void HitResource()
    {
        //visual
        transform.DOComplete();
        transform.DOShakeScale(.5f, .2f, 10, 90, true);
    }

    private void OnMouseEnter()
    {
        isHover = true;
        if (renderer)
            renderer.material.SetColor("_GroundColor", Color.grey);
            renderer.material.SetColor("_TopColor", Color.grey);
    }
    private void OnMouseExit()
    {
        isHover = false;
        if (renderer)
            renderer.material.SetColor("_GroundColor", emissionColor);
            renderer.material.SetColor("_TopColor", emissionColor2);
    }
}
