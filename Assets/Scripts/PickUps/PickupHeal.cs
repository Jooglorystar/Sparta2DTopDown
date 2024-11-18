using UnityEngine;

public class PickupHeal : PickupItem
{
    [SerializeField] int healValue = 10;
    protected override void OnPickedUp(GameObject go)
    {
        HealthSystem healthSystem = go.GetComponent<HealthSystem>();
        healthSystem.ChangeHealth(healValue);
    }
}