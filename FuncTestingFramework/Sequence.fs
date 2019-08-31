

namespace FuncTestingFramework.Sequence
open System
open System
open FuncTestingFramework.DateTimeExtensions
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.generator
open System.Runtime.CompilerServices

[<Extension>]
type SequenceExtensions() =
    [<Extension>]
    static member UseValue(Sequance(store, func), (value: seq<'a>)) =
        Object <| (Expressions.useValue func value) :: store

    [<Extension>]
    static member Ignore(Date(store, func)) =
        Object <| (Expressions.ignore func) :: store

    [<Extension>]
    static member Min (Date(store, func)) (min: 'a) =
        Object <| (DateTimeExpressions.Interval func min DateTime.MaxValue) :: store

    [<Extension>]
    static member Max (Date(store, func)) (max: 'a) =
        Object <| (DateTimeExpressions.Interval func DateTime.MinValue max) :: store

    [<Extension>]
    static member Interval (Date(store, func)) (min: 'a) (max: 'a) =
        Object <| (DateTimeExpressions.Interval func min max) :: store


[<Extension>]
type SequencePipe() =
    [<Extension>]
    static member Interval1<'a,'seqType> (pipe:PipeSequance<'a, 'seqType>) (min: 'a) (max: 'a) =
        pipe
    
    [<Extension>]
    static member Interval11<'a,'seqType> (pipe:PipeSequance<'a, 'seqType>) (min: 'a) (max: 'a) =
        pipe
    
    [<Extension>]
    static member Pipe<'a, 'seqType>(sequance: Sequance<'a, 'seqType>,
                                     (configurationFunc: Func<PipeSequance<'a, 'seqType>, PipeSequance<'a, 'seqType>>)) =
        0
