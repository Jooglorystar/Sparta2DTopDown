using System.Collections.Generic;
using UnityEngine;

public class PickupStatModifiers : PickupItem
{
    [SerializeField] List<CharacterStat> statsModifier = new List<CharacterStat>();
    protected override void OnPickedUp(GameObject go)
    {
        CharacterStatHandler statHandler = go.GetComponent<CharacterStatHandler>();

        foreach (CharacterStat modifier in statsModifier)
        {
            statHandler.AddStatModifier(modifier);
        }

        // 체력 올리는 경우
        HealthSystem healthSystem = go.GetComponent<HealthSystem>();
        healthSystem.ChangeHealth(0);
    }
}
