using System.Collections;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
}
public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stats strength; //1 point increase damage by 1 and crit.power by 1%
    public Stats agility;   //1 point increase invasion by 1 and crit.chance by 1%
    public Stats vitality; // 1 point incredase health by 3 or 5 points

    [Header("Offensive stats")]
    public Stats damage;
    public Stats critChance;
    public Stats critPower;

    [Header("Defensive stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;

    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; }
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
    }
    protected virtual void Update()
    {
       /* if (Input.GetKeyUp(KeyCode.M))
            DecreaseHealthBy(10);*/
    }
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stats _statToModify)
    {
        // start corototuine for stat increase
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }
    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stats _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;
        int totalDamage = damage.GetValue() + strength.GetValue();
        if (CanCrit())
        {
            totalDamage = CalculatedCriticalDamage(totalDamage);
        }
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        if (currentHealth < 0 && !isDead)
            Die();
    }
    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
    }
    public virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }
    protected virtual void Die()
    {
        isDead = true;
    }
    #region StatsCalculate
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }
    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }
    private int CalculatedCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion
    public Stats GetStat(StatType _statType)
    {
        if (_statType == StatType.strength) return strength;
        else if (_statType == StatType.agility) return agility;
        else if (_statType == StatType.vitality) return vitality;
        else if (_statType == StatType.damage) return damage;
        else if (_statType == StatType.critChance) return critChance;
        else if (_statType == StatType.critPower) return critPower;
        else if (_statType == StatType.health) return maxHealth;
        else if (_statType == StatType.armor) return armor;
        else if (_statType == StatType.evasion) return evasion;

        return null;
    }

}
