using Assets.Resources.Scripts.Cubes;
using System.Collections.Generic;

namespace Game.Cubes
{
    public interface ICubeRepository
    {
        void Register(Cube cube);
        void Unregister(Cube cube);
        (Cube cubeA, Cube cubeB)? GetFirstMergeablePair();
        IReadOnlyList<Cube> GetAll();
    }
}