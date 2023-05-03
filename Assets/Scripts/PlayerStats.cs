using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public enum StatType
{
    Health, MaxHealth, Armor, AttackSpeed, Critical, Damage, DamageOverTime, KnockBack, 
    KnockBackResist, LifeGain, LifeSteal, MaxJumps, MaxSpecials, Speed
}

public enum AbilityType
{
    Light, Heavy, Jump, Move, Cooldown, Special, None
}
public class PlayerStats
{
    public struct PlayerStat
    {
        private float _value;
        private float _bonus;

        public PlayerStat(float value, float bonus = 0f)
        {
            _value = value;
            _bonus = bonus;
        }
        public float TotalValue() => _value + _bonus;
        public void SetValue(float value) => _value = value;
        public void AddValue(float value) => _value += value;
        public void RemoveValue(float value) => _value -= value;
        public void AddBonus(float bonus) => _bonus = bonus;
        public void AddBonus(float bonus, float maxValue)
        {
            _bonus = Math.Min(maxValue - _value, bonus);
        }
        public float BonusValue() => _bonus;
    }

    public Dictionary<StatType, PlayerStat> Stats { get; private set; }

    public PlayerStats(Dictionary<StatType, float> defaultStats)
    {
        Stats = new Dictionary<StatType, PlayerStat>();
        foreach (var (k, v) in defaultStats)
        {
            Stats.Add(k, new PlayerStat(v));
        }
    }

    public void SetStat(StatType type, float value)
    {
        if (Stats.ContainsKey(type)) Stats[type].SetValue(value);
    }
    
    public bool SetStat(string type, float value)
    {
        StatType parsedEnum = (StatType)System.Enum.Parse(typeof(StatType), type);
        if (Stats.ContainsKey(parsedEnum))
        {
            Stats[parsedEnum].SetValue(value);
            return true;
        }
        return false;
    }

    public void AddStat(StatType type, float bonusValue)
    {
        if (Stats.ContainsKey(type)) Stats[type].AddBonus(bonusValue);
    }

    public bool AddStat(string type, float bonusValue)
    {
        StatType parsedEnum = (StatType)System.Enum.Parse(typeof(StatType), type);
        if (Stats.ContainsKey(parsedEnum))
        {
            Stats[parsedEnum].AddBonus(bonusValue);
            return true;
        }
        return false;
    }

    public void AddItem(ItemData itemData)
    {
        StatType type = itemData.statType;
        if (Stats.ContainsKey(type))
        {
            Stats[type].AddBonus(itemData.value);
        }
    }

    public void SetHealth(float value)
    {
        Stats[StatType.Health].SetValue(value);
    }

    public void TakeDamage(float value)
    {
        Stats[StatType.Health].RemoveValue(value);
    }

    public void Heal(float value)
    {
        Stats[StatType.Health].AddValue(value);
    }

    public float Health => Stats[StatType.Health].TotalValue();
    public float MaxHealth => Stats[StatType.MaxHealth].TotalValue();
    public float Armor => Stats[StatType.Armor].TotalValue();
    public float AttackSpeed => Stats[StatType.AttackSpeed].TotalValue();
    public float Critical => Stats[StatType.Critical].TotalValue();
    public float Damage => Stats[StatType.Damage].TotalValue();
    public float DamageOverTime => Stats[StatType.DamageOverTime].TotalValue();
    public float KnockBack => Stats[StatType.KnockBack].TotalValue();
    public float KnockBackResist => Stats[StatType.KnockBackResist].TotalValue();
    public float LifeGain => Stats[StatType.LifeGain].TotalValue();
    public float LifeSteal => Stats[StatType.LifeSteal].TotalValue();
    public float MaxJumps => Stats[StatType.MaxJumps].TotalValue();
    public float MaxSpecials => Stats[StatType.MaxSpecials].TotalValue();
    public float Speed => Stats[StatType.Speed].TotalValue();
}
