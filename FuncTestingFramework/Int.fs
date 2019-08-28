namespace Extensions
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.Extensions
open FuncTestingFramework.generator
open System.Runtime.CompilerServices

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


