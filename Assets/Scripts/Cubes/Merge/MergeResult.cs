using Assets.Resources.Scripts.Cubes;

public class MergeResult
{
    public Cube OriginalA { get; }
    public Cube OriginalB { get; }
    public Cube NewCube { get; }

    public MergeResult(Cube a, Cube b, Cube newCube)
    {
        OriginalA = a;
        OriginalB = b;
        NewCube = newCube;
    }
}