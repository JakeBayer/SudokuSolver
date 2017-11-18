using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Square : IGridElement
    {
        private Dictionary<StepType, bool> _dirties = new Dictionary<StepType, bool>();
        private int? _value = null;
        public int x { get; protected set; }
        public int y { get; protected set; }

        public HashSet<int> PossibleValues;

        public int? Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public void SetValue(int value)
        {
            _value = value;
            PossibleValues.Clear();
            PossibleValues.Add(value);
            Clean();
        }

        public bool HasValue { get { return _value.HasValue; } }

        public Square(int _x, int _y) 
        {
            x = _x;
            y = _y;
            PossibleValues = new HashSet<int>();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Square;
            if (other == null) return false;
            return this.x == other.x && this.y == other.y;
        }

        public void SetDirty(StepType stepType)
        {
            if (!_dirties.ContainsKey(stepType))
            {
                _dirties.Add(stepType, !HasValue);
            }
            else
            {
                _dirties[stepType] = !HasValue;
            }
        }

        public bool IsDirty(StepType stepType)
        {
            if (!_dirties.ContainsKey(stepType))
            {
                _dirties.Add(stepType, !HasValue);
            }
            return _dirties[stepType];
        }

        public void IsDirty()
        {
            if (!HasValue)
            {
                var keys = new List<StepType>(_dirties.Keys);
                foreach (var stepType in keys)
                {
                    _dirties[stepType] = true;
                }
            }
        }

        public bool IsAnyDirty { get { return _dirties.Any() ? _dirties.Any(kvp => kvp.Value) : !HasValue; } }

        private void Clean()
        {
            var keys = new List<StepType>(_dirties.Keys);
            foreach (var stepType in keys)
            {
                _dirties[stepType] = false;
            }
        }

        public void Clean(StepType stepType)
        {
            if (!_dirties.ContainsKey(stepType))
            {
                _dirties.Add(stepType, false);
            }
            else
            {
                _dirties[stepType] = false;
            }
        } 

        public bool RemoveValue(int value)
        {
            if (PossibleValues.Contains(value))
            {
                PossibleValues.Remove(value);
                this.IsDirty();
                if (!PossibleValues.Any())
                {
                    throw new InvalidOperationException(
                        string.Format("Square ({0}, {1}) has no possible values. Just removed {2}", x, y, value));
                }
                return true;
            }
            return false;
        }
    }
}
