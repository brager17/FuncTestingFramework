

namespace FuncTestingFramework.Sequence
open System
open System
open System.Linq.Expressions
open FuncTestingFramework.DateTimeExtensions
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.generator
open System.Runtime.CompilerServices

type SequanceCount<'a, 'seqType> = SequanceCount of SequanceStore<'a, 'seqType>
type MaxSequance<'a, 'seqType> = MaxSequance of SequanceStore<'a, 'seqType>
type MinSequance<'a, 'seqType> = MinSequance of SequanceStore<'a, 'seqType>
type IntervalSequance<'a, 'seqType> = IntervalSequance of SequanceStore<'a, 'seqType>


[<Extension>]
type SequenceExtensions() =
    [<Extension>]
    static member UseValue(Sequance(store, func), (value: seq<'a>)) =
        Object <| (Expressions.useValue func value) :: store

    [<Extension>]
    static member Ignore(Sequance(store, func)) =
        Object <| (Expressions.ignore func) :: store

    [<Extension>]
    static member Min (Sequance(store, func)) (min: 'a) =
        Object <| (DateTimeExpressions.Interval func min DateTime.MaxValue) :: store

    [<Extension>]
    static member Max (Sequance(store, func)) (max: 'a) =
        Object <| (DateTimeExpressions.Interval func DateTime.MinValue max) :: store

    [<Extension>]
    static member Interval (Sequance(store, func)) (min: 'a) (max: 'a) =
        Object <| (DateTimeExpressions.Interval func min max) :: store

    [<Extension>]
    static member For (ent:'t) (min: 'a) (max: 'a) =
        Object <| (DateTimeExpressions.Interval func min max) :: store

   

