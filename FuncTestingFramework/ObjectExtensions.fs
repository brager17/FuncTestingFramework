namespace FuncTestingFramework.ObjectExtensions
open System
open System.Linq.Expressions
open FuncTestingFramework.generator
open System.Runtime.CompilerServices

[<Extension>]
type  ObjectExtensions() =
    [<Extension>]
    static member  For<'a>(Object list,(expression:Expression<Func<'a,string>>)) :String<'a>  =
        String (list,expression)
            
    [<Extension>]
    static member For<'a>(Object list,(expression:Expression<Func<'a,int>>)) :Int<'a> =
        Int (list,expression)
