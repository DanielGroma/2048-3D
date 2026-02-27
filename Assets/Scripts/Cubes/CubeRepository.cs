using Assets.Resources.Scripts.Cubes;
using System.Collections.Generic;

namespace Game.Cubes
{
    public class CubeRepository : ICubeRepository
    {
        private readonly List<Cube> _cubes = new();

        public void Register(Cube cube)
        {
            if (!_cubes.Contains(cube))
                _cubes.Add(cube);
        }

        public void Unregister(Cube cube)
        {
            _cubes.Remove(cube);
        }

        public IReadOnlyList<Cube> GetAll() => _cubes;

        public (Cube cubeA, Cube cubeB)? GetFirstMergeablePair()
        {
            for (int i = 0; i < _cubes.Count; i++)
            {
                if (!_cubes[i].GetComponent<CubeLauncher>().IsLaunched)
                    continue;

                for (int j = i + 1; j < _cubes.Count; j++)
                {
                    if (!_cubes[j].GetComponent<CubeLauncher>().IsLaunched)
                        continue;

                    if (_cubes[i].Value == _cubes[j].Value)
                        return (_cubes[i], _cubes[j]);
                }
            }

            return null;
        }
    }
}