using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Items
{
    //Offensive Items
    AttackSpeed,
    Crit,
    Damage,
    DamageOverTime,
    Knockback,
    Lifesteal,

    //Defensive Items
    Armor,
    Health,
    KnockbackRes,
    Lifegain,

    //Utility Items
    JumpHeight,
    MaxJumps,
    Speed,
    MaxSpecials
}

public class Item
{
    // Start is called before the first frame update
    public float value = 0f;
    public float time = 0f;
    public float timer = 0f;
    public GameObject player;
    public Sprite icon;
    public string name = "";
    public string description = "";
    public Item()
    {

    }

    public static Item GrantNewRandomItem()
    {
        int numOptions = System.Enum.GetNames(typeof(Items)).Length;
        int randomIndex = Random.Range(0, numOptions);
        Items newItem = (Items)randomIndex;

        switch (newItem)
        {
            case Items.Armor:
                return new ArmorItem();
            case Items.AttackSpeed:
                return new AttackSpeedItem();
            case Items.Crit:
                return new CritItem();
            case Items.Damage:
                return new DamageItem();
            case Items.DamageOverTime:
                return new DamageOverTimeItem();
            case Items.Health:
                return new HealthItem();
            case Items.JumpHeight:
                return new JumpHeightItem();
            case Items.Knockback:
                return new KnockbackItem();
            case Items.KnockbackRes:
                return new KnockbackResistanceItem();
            case Items.Lifegain:
                return new LifegainItem();
            case Items.Lifesteal:
                return new LifestealItem();
            case Items.MaxJumps:
                return new MaxJumpsItem();
            case Items.MaxSpecials:
                return new MaxSpecialsItem();
            case Items.Speed:
                return new SpeedItem();
            default:
                return null;
        }
    }

    public static Item GenerateRandomOffensiveItem()
    {
        //Choose random "Offensive" item
        int randomIndex = Random.Range(0, 6);
        Items newItem = (Items)randomIndex;
        switch (newItem)
        {
            case Items.AttackSpeed:
                return new AttackSpeedItem();
            case Items.Crit:
                return new CritItem();
            case Items.Damage:
                return new DamageItem();
            case Items.DamageOverTime:
                return new DamageOverTimeItem();
            case Items.Knockback:
                return new KnockbackItem();
            case Items.Lifesteal:
                return new LifestealItem();
            default:
                return null;
        }
    }

    public static Item GenerateRandomDefensiveItem()
    {
        //Choose random "Offensive" item
        int randomIndex = Random.Range(6, 10);
        Items newItem = (Items)randomIndex;
        switch (newItem)
        {
            case Items.Armor:
                return new ArmorItem();
            case Items.Health:
                return new HealthItem();
            case Items.KnockbackRes:
                return new KnockbackResistanceItem();
            case Items.Lifegain:
                return new LifegainItem();
            default:
                return null;
        }
    }

    public static Item GenerateRandomUtilityItem()
    {
        //Choose random "Offensive" item
        int randomIndex = Random.Range(10, 14);
        Items newItem = (Items)randomIndex;
        switch (newItem)
        {
            case Items.JumpHeight:
                return new JumpHeightItem();
            case Items.MaxJumps:
                return new MaxJumpsItem();
            case Items.Speed:
                return new SpeedItem();
            case Items.MaxSpecials:
                return new MaxSpecialsItem();
            default:
                return null;
        }
    }

    public Item(GameObject selectedPlayer)
    {
        player = selectedPlayer;
    }

    public Item(GameObject selectedPlayer, float itemValue)
    {
        player = selectedPlayer;
        value = itemValue;
    }
    public Item(GameObject selectedPlayer,float itemValue, float iterationTime)
    {
        player = selectedPlayer;
        value = itemValue;
        time = iterationTime;
    }

    public Item(float itemValue)
    {
        value = itemValue;
    }
    
    public Item(float itemValue, float iterationTime)
    {
        value = itemValue;
        time = iterationTime;
    }

    // Update is called once per frame
    public virtual bool UpdateItem()
    {
        timer++;
        if(timer > time)
        {
            timer = 0f;
            ItemCooldown();
        }
        return false;
    }

    public virtual bool ItemCooldown()
    {
        return false;
    }
    public virtual bool ItemPickup()
    {
        return false;
    }
    public virtual bool ItemJump()
    {
        return false;
    }
    public virtual bool ItemMove()
    {
        return false;
    }
    public virtual bool ItemSpecial()
    {
        return false;
    }
    public virtual bool ItemLight()
    {
        return false;
    }
    public virtual bool ItemHeavy()
    {
        return false;
    }

    public void AssignPlayer(GameObject play)
    {
        player = play;
    }
}
