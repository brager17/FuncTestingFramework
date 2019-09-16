using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using static CSharpFuncTestingFramework.ExpressionBuildExtensions;

namespace CSharpFuncTestingFramework
{
    public class ObjectT<T>
    {
        public IEnumerable<Expression<Func<T, T>>> Storage { get; set; }

        public ObjectT(IEnumerable<Expression<Func<T, T>>> storage)
        {
            Storage = storage;
        }

        public IntT<T> For(Expression<Func<T, int>> path)
        {
            return new IntT<T>(Storage, path);
        }

        public StringT<T> For(Expression<Func<T, string>> path)
        {
            return new StringT<T>(Storage, path);
        }

        public DecimalT<T> For(Expression<Func<T, decimal>> path)
        {
            return new DecimalT<T>(Storage, path);
        }

        public DateTimeT<T> For(Expression<Func<T, DateTime>> path)
        {
            return new DateTimeT<T>(Storage, path);
        }
    }

    public abstract class PathObject<T, T1> : ObjectT<T>
    {
        protected Expression<Func<T, T1>> Path { get; set; }

        protected PathObject(IEnumerable<Expression<Func<T, T>>> storage, Expression<Func<T, T1>> path) : base(storage)
        {
            Path = path;
        }

        public ObjectT<T> UseValue(T1 value)
        {
            return MemberInit(Path, value)
                .Add(Storage)
                .PipeTo(x => new ObjectT<T>(x));
        }

        public ObjectT<T> Ignore() =>
            MemberInit(Path, default(T1))
                .Add(Storage)
                .PipeTo(x => new ObjectT<T>(x));
    }

    public class IntT<T> : PathObject<T, int>
    {
        public IntT(IEnumerable<Expression<Func<T, T>>> storage, Expression<Func<T, int>> path) : base(storage, path)
        {
        }

        public IntT<T> Min(int min)
        {
            return MemberInit(Path, RandomGeneratorMeta.GenerateIntCall(min, int.MaxValue))
                .Add(Storage)
                .PipeTo(x => new IntT<T>(x, Path));
        }

        public IntT<T> Max(int max)
        {
            return MemberInit(Path, RandomGeneratorMeta.GenerateIntCall(int.MinValue, max))
                .Add(Storage)
                .PipeTo(x => new IntT<T>(x, Path));
        }

        public IntT<T> Interval(int min, int max)
        {
            return MemberInit(Path, RandomGeneratorMeta.GenerateIntCall(min, max))
                .Add(Storage)
                .PipeTo(x => new IntT<T>(x, Path));
        }

        public IntT<T> Positive()
        {
            return MemberInit(Path, RandomGeneratorMeta.GenerateIntCall(0, int.MaxValue))
                .Add(Storage)
                .PipeTo(x => new IntT<T>(x, Path));
        }


        public IntT<T> Negative()
        {
            return MemberInit(Path, RandomGeneratorMeta.GenerateIntCall(int.MinValue, 0))
                .Add(Storage)
                .PipeTo(x => new IntT<T>(x, Path));
        }
    }

    public class StringT<T> : PathObject<T, string>
    {
        public StringT(IEnumerable<Expression<Func<T, T>>> storage, Expression<Func<T, string>> path) : base(storage,
            path)
        {
        }

        public StringT<T> MinLength(int min)
        {
            if (min > 0)
                throw new ArgumentException("length > 0");

            return MemberInit(Path, RandomGeneratorMeta.GenerateStringCall(min, 10000))
                .Add(Storage)
                .PipeTo(x => new StringT<T>(x, Path));
        }

        public StringT<T> MaxLength(int max)
        {
            Contract.Assert(max < 10000, "length < 10000");
            return MemberInit(Path, RandomGeneratorMeta.GenerateStringCall(0, max))
                .Add(Storage)
                .PipeTo(x => new StringT<T>(x, Path));
        }

        public StringT<T> Length(int length)
        {
            if (length > 0)
                throw new ArgumentException("length > 0");

            if (length < 10000)
                throw new ArgumentException("length < 10000");

            return MemberInit(Path, RandomGeneratorMeta.GenerateStringCall(length, length))
                .Add(Storage)
                .PipeTo(x => new StringT<T>(x, Path));
        }
    }

    public class DecimalT<T> : PathObject<T, decimal>
    {
        public DecimalT(IEnumerable<Expression<Func<T, T>>> storage, Expression<Func<T, decimal>> path) : base(storage,
            path)
        {
        }
    }

    public class DateTimeT<T> : PathObject<T, DateTime>
    {
        public DateTimeT(IEnumerable<Expression<Func<T, T>>> storage, Expression<Func<T, DateTime>> path) : base(
            storage, path)
        {
        }

        public DateTimeT<T> Min(DateTime min)
        {
            return MemberInit(Path, RandomGeneratorMeta.GenerateDateTimeCall(min, DateTime.MaxValue))
                .Add(Storage)
                .PipeTo(x => new DateTimeT<T>(x, Path));
        }

        public DateTimeT<T> Max(DateTime max)
        {
            return MemberInit(Path, RandomGeneratorMeta.GenerateDateTimeCall(DateTime.MinValue, max))
                .Add(Storage)
                .PipeTo(x => new DateTimeT<T>(x, Path));
        }

        public DateTimeT<T> Interval(DateTime left, DateTime right)
        {
            return MemberInit(Path, RandomGeneratorMeta.GenerateDateTimeCall(left, right))
                .Add(Storage)
                .PipeTo(x => new DateTimeT<T>(x, Path));
        }
    }


    public static class Configuration
    {
        public static ObjectT<T> Build<T>() => new ObjectT<T>(new List<Expression<Func<T, T>>>());

        public static T gen<T>([CanBeNull] ObjectT<T> obj = null)
        {
            var value = (T) Activator.CreateInstance(typeof(T));

            if (obj == null)
                return value;

            foreach (var func in obj.Storage.Select(x => x.Compile()))
            {
                func.Invoke(value);
            }

            return value;
        }
    }
}