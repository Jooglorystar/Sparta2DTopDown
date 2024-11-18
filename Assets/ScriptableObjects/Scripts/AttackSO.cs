using UnityEngine;

[CreateAssetMenu(fileName = "DefaltAttackSO", menuName = "TopDownController/Attacks/Default", order =0)]
public class AttackSO : ScriptableObject
{
    // 카테고리화 해주는 것이 좋다.
    [Header("Attack Info")]
    public float size;
    public float delay;
    public float power;
    public float speed;
    public LayerMask target;

    [Header("Knock Back Info")]
    public bool isOnKnockBack;
    public float knockbackPower;
    public float knockbackTime;
}
