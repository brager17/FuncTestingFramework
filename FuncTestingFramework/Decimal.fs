namespace FuncTestingFramework.Decimal
open System
open System.Linq.Expressions
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.generator
open System.Runtime.CompilerServices

 module DecimalExpressions =
    let Interval (e: Expression<Func<'a, decimal>>) (min: decimal) (max: decimal) : Expression<Action<'a>>=
        let f = new Func<decimal, decimal, decimal>(generator.generateDecimal)
        let call' = call (cons f.Target) f.Method [ cons min; cons max ]
        Expressions.buildMutableAction call' e



[<Extension>]
type DecimalExtensions() =
    [<Extension>]
    static member UseValue(Decimal(store, func), (value: decimal)) =
        Object <| (Expressions.useValue func value) :: store

    [<Extension>]
    static member Ignore(Decimal(store, func)) =
        Object <| (Expressions.ignore func) :: store

    [<Extension>]
    static member Min (Decimal(store, func)) (min: decimal) =
        Object <| (DecimalExpressions.Interval func min Decimal.MaxValue) :: store

    [<Extension>]
    static member Max (Decimal(store, func)) (max: decimal) =
        Object <| (DecimalExpressions.Interval func Decimal.MinValue max) :: store

    [<Extension>]
    static member Interval (Decimal(store, func)) (min: decimal) (max: decimal) =
        Object <| (DecimalExpressions.Interval func min max) :: store
