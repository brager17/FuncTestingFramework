namespace FuncTestingFramework.DateTimeExtensions
open System
open System.Runtime.CompilerServices
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.generator

    
[<Extension>]
type DecimalExtensions() =
    [<Extension>]
    static member UseValue(Date (store,func),(value:DateTime))=
        Object <| (Expressions.useValue func value)::store
        
    [<Extension>]
    static member Ignore(Date (store,func)) =
        Object <| (Expressions.ignore func)::store
        
    [<Extension>]
    static member Min(Date(store,func)) (v:decimal) =
        Object <| (Expressions.ignore func)::store
        
    [<Extension>]
    static member Max(Date (store,func)) (v:decimal) =
        Object <| (Expressions.ignore func)::store
        
    [<Extension>]
    static member Interval(Date (store,func)) (min:decimal) (max:decimal)=
        Object <| (Expressions.ignore func)::store

