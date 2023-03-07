using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Items/Weapon")]
public class WeaponData : ItemData
{
    public enum ToolType
    {
        Sword, Bow, Gun, Axe
    }
    
    [Header("Weapon Settings")]
    public ToolType toolType;
    public float attackDamage = 1f;
    public float attackSpeed = 1f;
    public float coolDown = 0.5f;
}
