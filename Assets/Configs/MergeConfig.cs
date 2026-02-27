using UnityEngine;

[CreateAssetMenu(menuName = "Configs/MergeConfig")]
public class MergeConfig : ScriptableObject
{
    [Min(0f)]
    public float MinImpulse = 2.5f;
}