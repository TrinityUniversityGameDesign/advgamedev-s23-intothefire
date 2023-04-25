using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;

[CreateAssetMenu(menuName="Into the Fire/Weapon")]
public class WeaponData : ScriptableObject
{
    // Start is called before the first frame update
    public enum Type
    {
        Hammer, Swords, FryingPan, Scythe, Boots, Gauntlets, Whip, Jetpack
    }
    [Header("Weapon Settings")]
    public Type WeaponType;
    public string Description;
    public Sprite Icon;

    public Weapon Create()
    {
        switch (WeaponType)
        {
            case Type.Hammer:
                return new Hammer();
            case Type.Swords:
                return new Swords();
            case Type.FryingPan:
                return new FryingPan();
            case Type.Scythe:
                return new Scythe();
            case Type.Boots:
                return new Boots();
            case Type.Gauntlets:
                return new Gauntlets();
            case Type.Whip:
                return new Whip();
            case Type.Jetpack:
                return new Jetpack();
            default:
                Debug.LogError("Nothing was selected");
                return new Weapon();
        }
    }
}
