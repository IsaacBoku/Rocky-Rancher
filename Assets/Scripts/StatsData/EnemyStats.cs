using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier = .4f;
    protected override void Start()
    {
        ApplyLevelManager();
        base.Start();

        enemy = GetComponent<Enemy>();
    }
    private void ApplyLevelManager()
    {
        Modify(strength);
        Modify(agility);
        Modify(vitality);
        Modify(damage);
        Modify(critChance);
        Modify(critPower);
        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
    }
    private void Modify(Stats _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percantageModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
    protected override void Die()
    {
        base.Die();

        enemy.Die();
    }
}
