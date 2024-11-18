using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어와 몬스터 공통적인 기능
public class TopDownController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent; // void만 반환 아니면 Func
    public event Action<Vector2> OnLookEvent;
    public event Action<AttackSO> OnAttackEvent;

    protected bool IsAttacking {  get; set; }

    private float timeSinceLastAttack = float.MaxValue;

    // protected 이유: 상속받는 클래스도 볼 수 있도록
    protected CharacterStatHandler stats {  get; private set; }

    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStatHandler>();
    }

    private void Update()
    {
        handleAttackDelay();
    }

    private void handleAttackDelay()
    {
        if(timeSinceLastAttack < stats.CurrentStat.attackSO.delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
        else if(IsAttacking && timeSinceLastAttack >= stats.CurrentStat.attackSO.delay)
        {
            timeSinceLastAttack = 0f;
            CallAttackEvent(stats.CurrentStat.attackSO);
        }
    }

    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }

    public void CallAttackEvent(AttackSO attackSO)
    {
        OnAttackEvent?.Invoke(attackSO);
    }
}
