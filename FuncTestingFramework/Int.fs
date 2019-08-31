namespace Extensions
open System
open System.Linq.Expressions
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.Extensions
open FuncTestingFramework.generator
open System.Runtime.CompilerServices

module IntExpressions =
    let private rnd = new Random();
    let private callNext_2 (l:int) (r:int) = call (cons rnd) (mtdMyNameAndArgsCount<Random> "Next" 2) [ cons l; cons r ]

    let between (e: Expression<Func<'a, int>>) l r: Expression<Action<'a>> =
        let exprCall = callNext_2 l r
        Expressions.buildMutableAction exprCall e


    let min (e: Expression<Func<'a, int>>) l: Expression<Action<'a>> =
        between e l Int32.MaxValue

    let max (e: Expression<Func<'a, int>>) r: Expression<Action<'a>> =
        between e Int32.MinValue r

    let negative (e: Expression<Func<'a, int>>): Expression<Action<'a>> =
        between e Int32.MinValue 0

    let positive (e: Expression<Func<'a, int>>): Expression<Action<'a>> =
        between e 0 Int32.MaxValue


[<Extension>]
type IntExtensions() =
    [<Extension>]
    static member Between(Int(list, exp), (l: int32), (r: int32)): Object<'a> =
        if l >= r then failwith "left number must be less than rigth number"
        Object(IntExpressions.between exp l r :: list)

    [<Extension>]
    static member Min(Int(list, exp), (l: int32)): Object<'a> =
        Object(IntExpressions.min exp l :: list)

    [<Extension>]
    static member Max(Int(list, exp), (r: int32)): Object<'a> =
        Object(IntExpressions.max exp r :: list)


    [<Extension>]
    static member Negative(Int(list, exp)): Object<'a> =
        Object(IntExpressions.negative exp :: list)


    [<Extension>]
    static member Positive(Int(list, exp)): Object<'a> =
        Object(IntExpressions.positive exp :: list)


    [<Extension>]
    static member UseValue (Int(list, exp)) (v:int): Object<'a> =
         Object(Expressions.useValue exp v :: list)

    [<Extension>]
    static member Ignore(Int(list, exp)): Object<'a> =
         Object(Expressions.ignore exp :: list)


