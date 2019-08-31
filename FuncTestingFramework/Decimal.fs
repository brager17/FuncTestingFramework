namespace FuncTestingFramework.Decimal
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.generator
open System.Runtime.CompilerServices
    
    
[<Extension>]
type DecimalExtensions() =
    [<Extension>]
    static member UseValue(Decimal (store,func),(value:decimal))=
        Object <| (Expressions.useValue func value)::store
        
    [<Extension>]
    static member Ignore(Decimal (store,func)) =
        Object <| (Expressions.ignore func)::store
        
    [<Extension>]
    static member Min(Decimal(store,func)) (v:decimal) =
        Object <| (Expressions.ignore func)::store
        
    [<Extension>]
    static member Max(Decimal (store,func)) (v:decimal) =
        Object <| (Expressions.ignore func)::store
        
    [<Extension>]
    static member Interval(Decimal (store,func)) (min:decimal) (max:decimal)=
        Object <| (Expressions.ignore func)::store