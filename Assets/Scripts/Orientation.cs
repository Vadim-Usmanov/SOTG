using System.Collections.Generic;
using UnityEngine;
public class Orientation
{
    static private Orientation _up = new Orientation(Value.Up, Value.Left, Value.Right);
    static private Orientation _right = new Orientation(Value.Right, Value.Up, Value.Down);
    static private Orientation _down = new Orientation(Value.Down, Value.Right, Value.Left);
    static private Orientation _left = new Orientation(Value.Left, Value.Down, Value.Up);
    static private Orientation _indifferent = new Orientation(Value.Indifferent, Value.Indifferent, Value.Indifferent);
    static private Dictionary<Value, Orientation> _dictionary = new Dictionary<Value, Orientation>(ValueOrientationPairCollection());
    static private Dictionary<Value, Vector3> _vectors = new Dictionary<Value, Vector3>(ValueVector3PairCollection());
    private Value _value;
    private Value _clockwise;
    private Value _counterClockwise;
    private Orientation(Value val, Value counterClockwise, Value clockWise)
    {
        _value = val;
        _clockwise = clockWise;
        _counterClockwise = counterClockwise;
    }
    static public Orientation Up { get => _up; }
    static public Orientation Right { get => _right; }
    static public Orientation Down { get => _down; }
    static public Orientation Left { get => _left; }
    static public Orientation Indifferent { get => _indifferent; }
    public Orientation Clockwise { get => _dictionary[_clockwise]; }
    public Orientation CounterClockwise { get => _dictionary[_counterClockwise]; }
    public enum Value { Up, Right, Down, Left, Indifferent };
    public Value Val { get => _value; }
    public override bool Equals(object obj)
    {
        return base.Equals(obj as Orientation);
    }
    public bool Equals(Orientation other)
    {
        return this._value == other._value;
    }
    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
    public static bool operator ==(Orientation left, Orientation rigth)
    {
        return left.Equals(rigth);
    }
    public static bool operator !=(Orientation left, Orientation right)
    {
        return !left.Equals(right);
    }
    public Vector3 ToVector3()
    {
        return _vectors[_value];
    }
    static private IEnumerable<KeyValuePair<Value, Orientation>> ValueOrientationPairCollection()
    {
        return new KeyValuePair<Value, Orientation>[] 
        {
            new KeyValuePair<Value, Orientation>(Value.Up, _up),
            new KeyValuePair<Value, Orientation>(Value.Right, _right),
            new KeyValuePair<Value, Orientation>(Value.Down, _down),
            new KeyValuePair<Value, Orientation>(Value.Left, _left)
        };
    }
    static private IEnumerable<KeyValuePair<Value, Vector3>> ValueVector3PairCollection()
    {
        return new KeyValuePair<Value, Vector3>[]
        {
            new KeyValuePair<Value, Vector3>(Value.Up, Vector3.forward),
            new KeyValuePair<Value, Vector3>(Value.Right, Vector3.right),
            new KeyValuePair<Value, Vector3>(Value.Down, Vector3.back),
            new KeyValuePair<Value, Vector3>(Value.Left, Vector3.left)
        };
    }
}
