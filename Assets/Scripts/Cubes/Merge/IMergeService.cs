using Assets.Resources.Scripts.Cubes;

public interface IMergeService
{
    MergeResult MergeCubes(Cube a, Cube b);
    bool CanMerge(Cube a, Cube b);
}