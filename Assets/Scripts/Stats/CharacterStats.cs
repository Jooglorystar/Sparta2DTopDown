using UnityEngine;

public enum StatsChangeType
{
    Add,
    Multiple,
    Override
}

// 데이터 폴더처럼 사용할 수 있게 만들어주는 Attribute
[System.Serializable]
public class CharacterStat
{
    public StatsChangeType statsChangeType;
    [Range(0, 100)] public int maxHealth;
    [Range(0f, 20f)] public float speed;
    public AttackSO attackSO;
}
