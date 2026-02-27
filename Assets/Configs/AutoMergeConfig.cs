using UnityEngine;

[CreateAssetMenu(menuName = "Configs/AutoMerge")]
public class AutoMergeConfig : ScriptableObject
{
    public float liftHeight = 3f;
    public float liftDuration = 0.4f;
    public float swingOffset = 1f;
    public float swingDuration = 0.2f;
    public float mergeDuration = 0.3f;
}