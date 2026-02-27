using System;

namespace Assets.Resources.Scripts.Cubes
{
    [Serializable]
    public struct CubeValue
    {
        public int Value { get; private set; }

        public CubeValue(int value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is CubeValue other)
                return Value == other.Value;

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CubeValue left, CubeValue right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CubeValue left, CubeValue right)
        {
            return !(left == right);
        }

        public int GetMergeScore() => Value / 2;

        public CubeValue NextValue() => new CubeValue(Value * 2);

        public override string ToString() => Value.ToString();
    }
}