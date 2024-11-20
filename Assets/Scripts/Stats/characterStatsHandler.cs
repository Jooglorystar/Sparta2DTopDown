using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using UnityEngine;

public class CharacterStatHandler : MonoBehaviour
{
    // 기본스탯, 추가스탯 계산해서 최종스탯 계산하기
    // 일단 기본 스탯만

    [SerializeField] private CharacterStat baseStat;
    public CharacterStat CurrentStat { get; private set; } = new();

    public List<CharacterStat> statModifiers = new List<CharacterStat>();

    private readonly float minAttackDelay = 0.03f;
    private readonly float minAttackPower = 0.5f;
    private readonly float minAttackSize = 0.4f;
    private readonly float minAttackSpeed = 0.1f;

    private readonly float minSpeed = 0.8f;

    private readonly int minMaxHealth = 5;

    private void Awake()
    {
        UpdateCharacterStat();

        Initiate();
    }

    private void Initiate()
    {
        if (baseStat.attackSO != null)
        {
            baseStat.attackSO = Instantiate(baseStat.attackSO);
            CurrentStat.attackSO = Instantiate(baseStat.attackSO);
        }
    }

    private void UpdateCharacterStat()
    {
        CurrentStat.statsChangeType = baseStat.statsChangeType;
        CurrentStat.maxHealth = baseStat.maxHealth;
        CurrentStat.speed = baseStat.speed;

        ApplyStatModifier(baseStat);

        foreach (CharacterStat stat in statModifiers.OrderBy(o => o.statsChangeType))
        {
            ApplyStatModifier(stat);
        }
    }


    public void AddStatModifier(CharacterStat modifier)
    {
        statModifiers.Add(modifier);
        UpdateCharacterStat();
    }

    public void RemoveStatModifier(CharacterStat modifier)
    {
        statModifiers.Remove(modifier);
        UpdateCharacterStat();
    }

    private void ApplyStatModifier(CharacterStat modifier)
    {
        Func<float, float, float> operation = StatModifierSwitch(modifier);

        UpdateBasicStats(operation, modifier);
        UpdateAttackStats(operation, modifier);
        if (CurrentStat.attackSO is RangedAttackSO currentRanged && modifier.attackSO is RangedAttackSO newRanged)
        {
            UpdateRangedAttackStats(operation, currentRanged, newRanged);
        }
    }

    private Func<float, float, float> StatModifierSwitch(CharacterStat modifier)
    {
        Func<float, float, float> operation = modifier.statsChangeType switch
        {
            StatsChangeType.Add => (current, change) => current + change,
            StatsChangeType.Multiple => (current, change) => current * change,
            _ => (current, change) => change
        };

        return operation;
    }

    private void UpdateRangedAttackStats(Func<float, float, float> operation, RangedAttackSO currentRanged, RangedAttackSO newRanged)
    {
        currentRanged.multipleProjectilesAngle = operation(currentRanged.multipleProjectilesAngle, newRanged.multipleProjectilesAngle);
        currentRanged.spread = operation(currentRanged.spread, newRanged.spread);
        currentRanged.duration = operation(currentRanged.duration, newRanged.duration);
        currentRanged.numberOfProjectilesPerShot = Mathf.CeilToInt(operation(currentRanged.numberOfProjectilesPerShot, newRanged.numberOfProjectilesPerShot));
        currentRanged.projectileColor = UpdateColor(operation, currentRanged.projectileColor, newRanged.projectileColor);
    }

    private Color UpdateColor(Func<float, float, float> operation, Color current, Color modifier)
    {
        return new Color(
            operation(current.r, modifier.r),
            operation(current.g, modifier.g),
            operation(current.b, modifier.b),
            operation(current.a, modifier.a));
    }

    private void UpdateAttackStats(Func<float, float, float> operation, CharacterStat modifier)
    {
        if (CurrentStat.attackSO == null || modifier.attackSO == null) return;

        var currenAttack = CurrentStat.attackSO;
        var newAttack = modifier.attackSO;

        currenAttack.delay = Mathf.Max(operation(currenAttack.delay,newAttack.delay), minAttackDelay);
        currenAttack.power = Mathf.Max(operation(currenAttack.power, newAttack.power), minAttackPower);
        currenAttack.size = Mathf.Max(operation(currenAttack.size, newAttack.size), minAttackSize);
        currenAttack.speed = Mathf.Max(operation(currenAttack.speed, newAttack.speed), minAttackSpeed);

    }

    private void UpdateBasicStats(Func<float, float, float> operation, CharacterStat modifier)
    {
        CurrentStat.maxHealth = Mathf.Max((int)operation(CurrentStat.maxHealth, modifier.maxHealth),minMaxHealth);
        CurrentStat.speed = Mathf.Max((int)operation(CurrentStat.speed,modifier.speed),minSpeed);
    }
}