using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace FuncTestingFrameworkClient
{
    public static class CSBuilder
    {
        public static IConfiguration<T> GetConfiguration<T>() => null;

        public static IArrayConfiguration<T, TSeqItem> For<T, TSeqItem>(this IConfiguration<T> configuration,
            Expression<Func<T, IEnumerable<TSeqItem>>> path) => null;
    }

    [DebuggerDisplay("x={xvar} Y={yvar} Width = {widthvar} Height = {heightvar}")]
    public class IConfiguration<T>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<Expression<Action<T>>> Apply { get; }
    }

    public class IDateTimeConfiguration<T> : IConfiguration<T>
    {
        Expression<Func<T, DateTime>> Path { get; }
    }

    public class IStringConfiguration<T> : IConfiguration<T>
    {
        Expression<Func<T, string>> Path { get; }
    }

    public class IIntConfiguration<T> : IConfiguration<T>
    {
        Expression<Func<T, int>> Path { get; }
    }

    public class IDecimalConfiguration<T> : IConfiguration<T>
    {
        Expression<Func<T, decimal>> Path { get; }
    }

    public interface IArrayConfiguration<T>
    {
    }

    public class IArrayConfiguration<T, TSeqElement> : IConfiguration<T>
    {
        Expression<Func<T, IEnumerable<TSeqElement>>> Path { get; }
    }

    public class ICountConfiguration<T, TSeqElement> : IConfiguration<T>
    {
        Expression<Func<T, IEnumerable<TSeqElement>>> Path { get; }
    }

    public class IMaxConfiguration<T, TSeqElement> : IConfiguration<T>
    {
        Expression<Func<T, IEnumerable<TSeqElement>>> Path { get; }
    }

    public class IMinConfiguration<T, TSeqElement> : IConfiguration<T>
    {
        Expression<Func<T, IEnumerable<TSeqElement>>> Path { get; }
    }

    public class IIntervalConfiguration<T, TSeqElement> : IConfiguration<T>
    {
        Expression<Func<T, IEnumerable<TSeqElement>>> Path { get; }
    }


    public static class ArrayConfigurationExtensions
    {
        public static IConfiguration<T> UseValue<T, TSeqItem>(this IArrayConfiguration<T, TSeqItem> conf,
            IEnumerable<TSeqItem> value) => null;

        public static IConfiguration<T> Ignore<T, TSeqItem>(this IArrayConfiguration<T, TSeqItem> conf) => null;


        public static ICountConfiguration<T, TSeqItem> Count<T, TSeqItem>(this IArrayConfiguration<T, TSeqItem> conf,
            int count) => null;

        public static ICountConfiguration<T, TSeqItem> Count<T, TSeqItem>(this IMaxConfiguration<T, TSeqItem> conf,
            int count) => null;

        public static ICountConfiguration<T, TSeqItem> Count<T, TSeqItem>(this IMinConfiguration<T, TSeqItem> conf,
            int count) => null;

        public static ICountConfiguration<T, TSeqItem> Count<T, TSeqItem>(this IIntervalConfiguration<T, TSeqItem> conf,
            int count) => null;


        public static IMinConfiguration<T, TSeqItem>
            Min<T, TSeqItem>(this IArrayConfiguration<T, TSeqItem> conf, int min) => null;

        public static IMinConfiguration<T, TSeqItem>
            Min<T, TSeqItem>(this ICountConfiguration<T, TSeqItem> conf, int min) => null;

        public static IMaxConfiguration<T, TSeqItem>
            Max<T, TSeqItem>(this IArrayConfiguration<T, TSeqItem> conf, int max) => null;

        public static IMaxConfiguration<T, TSeqItem>
            Max<T, TSeqItem>(this ICountConfiguration<T, TSeqItem> conf, int max) => null;

        public static IIntervalConfiguration<T, TSeqItem> Interval<T, TSeqItem>(
            this IArrayConfiguration<T, TSeqItem> conf, int min,
            int max) => null;


        public static IIntervalConfiguration<T, TSeqItem> Interval<T, TSeqItem>(
            this ICountConfiguration<T, TSeqItem> conf, int min,
            int max) => null;
    }
}