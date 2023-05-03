using System;
using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;

[CreateAssetMenu(menuName="Into the Fire/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Settings")]
    public StatType statType;
    public AbilityType abilityType = AbilityType.None;
    public string description;
    public float value = 0f;
    public float cooldown = 0f;
    
    [Header("Item Files")]
    public Sprite icon;
    public GameObject gameObject;

    public GameObject Create()
    {
        return Instantiate(gameObject);
    }
}
