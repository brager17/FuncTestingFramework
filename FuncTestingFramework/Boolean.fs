namespace FuncTestingFramework.Boolean
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.generator
open System.Runtime.CompilerServices

    
[<Extension>]
type BooleanExtensions() =
    [<Extension>]
    static member UseValue(Boolean (store,func),(value:bool))=
        Object <| (Expressions.useValue func value)::store
        
    [<Extension>]
    static member Ignore(Boolean (store,func)) =
        Object <| (Expressions.ignore func)::store
        
    