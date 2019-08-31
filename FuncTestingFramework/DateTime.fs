namespace FuncTestingFramework.DateTimeExtensions
open FuncTestingFramework.Decimal
open System
open System.Linq.Expressions
open System.Runtime.CompilerServices
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.generator
open FuncTestingFramework.generator

module DateTimeExpressions =
    let Interval (e: Expression<Func<'a, DateTime>>) (min: DateTime) (max: DateTime) =
       let func = new Func<DateTime, DateTime, DateTime>(generator._dateTime)
       let callExpressin = call (cons func.Target) (func.Method) [ cons min; cons max ];
       Expressions.buildMutableAction callExpressin e


[<Extension>]
type DecimalExtensions() =
    [<Extension>]
    static member UseValue(Date(store, func), (value: DateTime)) =
        Object <| (Expressions.useValue func value) :: store

    [<Extension>]
    static member Ignore(Date(store, func)) =
        Object <| (Expressions.ignore func) :: store

    [<Extension>]
    static member Min (Date(store, func)) (min: DateTime) =
        Object <| (DateTimeExpressions.Interval func min DateTime.MaxValue) :: store

    [<Extension>]
    static member Max (Date(store, func)) (max: DateTime) =
        Object <| (DateTimeExpressions.Interval func DateTime.MinValue max) :: store

    [<Extension>]
    static member Interval (Date(store, func)) (min: DateTime) (max: DateTime) =
        Object <| (DateTimeExpressions.Interval func min max) :: store

