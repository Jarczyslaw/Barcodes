using System;

namespace JToolbox.Core.Results
{
    public class ValueResult<T> : Result
    {
        public ValueResult()
        {
        }

        public ValueResult(Result other)
            : base(other)
        {
        }

        public ValueResult(ValueResult<T> other)
            : base(other)
        {
            Value = other.Value;
        }

        public ValueResult(T value)
        {
            Value = value;
        }

        public new static ValueResult<T> AsError(string error)
        {
            var result = new ValueResult<T>();
            result.Messages.AddError(error);
            return result;
        }

        public new static ValueResult<T> AsError(Exception exc)
        {
            var result = new ValueResult<T>();
            result.Messages.AddError(exc);
            return result;
        }

        public T Value { get; set; }

        public override void Clear()
        {
            base.Clear();
            Value = default(T);
        }
    }
}