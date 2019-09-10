using System;
using System.Linq.Expressions;

namespace CSharpFuncTestingFramework
{
    public class ObjectT<T>
    {
        internal Expression<Func<T, T>> Storage { get; set; }

        public ObjectT(Expression<Func<T, T>> storage)
        {
            Storage = storage;
        }
    }

    public abstract class PathObject<T, T1> : ObjectT<T>
    {
        protected Expression<Func<T, T1>> Path { get; set; }

        protected PathObject(Expression<Func<T, T>> storage, Expression<Func<T, T1>> path) : base(storage)
        {
            Path = path;
        }
    }

    public class IntT<T> : PathObject<T, int>
    {
        public IntT(Expression<Func<T, T>> storage, Expression<Func<T, int>> path) : base(storage, path)
        {
        }
    }

    public class StringT<T> : PathObject<T, string>
    {
        public StringT(Expression<Func<T, T>> storage, Expression<Func<T, string>> path) : base(storage, path)
        {
        }
    }

    public class DecimalT<T> : PathObject<T, decimal>
    {
        public DecimalT(Expression<Func<T, T>> storage, Expression<Func<T, decimal>> path) : base(storage, path)
        {
        }
    }

    public class DateTimeT<T> : PathObject<T, DateTime>
    {
        public DateTimeT(Expression<Func<T, T>> storage, Expression<Func<T, DateTime>> path) : base(storage, path)
        {
        }
    }
}