using Assets.Resources.Scripts.Cubes;

public class MergeValidator
{
    public bool CanMerge(Cube a, Cube b)
    {
        if (a == null || b == null) return false;
        return a.Value.Value == b.Value.Value;
    }
}