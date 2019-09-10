using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CSharpFuncTestingFramework
{
    public class ObjectT<T>
    {
        internal IEnumerable<Expression<Func<T, T>>> Storage { get; set; }

        public ObjectT(IEnumerable<Expression<Func<T, T>>> storage)
        {
            Storage = storage;
        }

        public IntT<int> For(Expression<Func<T, int>> path)
        {
            throw new NotImplementedException();
        }

        public StringT<int> For(Expression<Func<T, string>> path)
        {
            throw new NotImplementedException();
        }

        public DecimalT<T> For(Expression<Func<T, decimal>> path)
        {
            throw new NotImplementedException();
        }

        public DateTimeT<int> For(Expression<Func<T, DateTime>> path)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class PathObject<T, T1> : ObjectT<T>
    {
        protected Expression<Func<T, T1>> Path { get; set; }

        protected PathObject(Expression<Func<T, T>> storage, Expression<Func<T, T1>> path) : base(storage)
        {
            Path = path;
        }

        public abstract ObjectT<T> UseValue(T1 value);

        public abstract ObjectT<T> Ignore();
    }

    public class IntT<T> : PathObject<T, int>
    {
        public IntT(Expression<Func<T, T>> storage, Expression<Func<T, int>> path) : base(storage, path)
        {
        }

        public override ObjectT<T> UseValue(int value)
        {
            throw new NotImplementedException();
        }

        public override ObjectT<T> Ignore()
        {
            throw new NotImplementedException();
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