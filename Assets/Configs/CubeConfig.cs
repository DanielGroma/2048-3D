using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "CubeConfig", menuName = "Configs/CubeConfig")]
public class CubeConfig : ScriptableObject
{
    [System.Serializable]
    public class CubeData
    {
        public int value;
        public Color color;
    }

    public CubeData[] cubes;
}