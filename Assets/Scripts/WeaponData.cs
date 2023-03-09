using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

    public Animation lightAttack;
    public float attackDamage = 1f;
    public float lightAttackDamage = 0.5f;
    public float attackSpeed = 1f;
    public float coolDown = 0.5f;

    public void LightAttack()
    {
    }
}
