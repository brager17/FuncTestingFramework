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
        
    [<Extension>]
    static member For<'a>(Object list,(expression:Expression<Func<'a,Boolean>>)) :Boolean<'a> =
        Boolean (list,expression)
        
    [<Extension>]
    static member For<'a>(Object list,(expression:Expression<Func<'a,Decimal>>)) :Decimal<'a> =
        Decimal (list,expression)
        
    [<Extension>]
    static member For<'a>(Object list,(expression:Expression<Func<'a,DateTime>>)) :DateTime<'a> =
        DateTime (list,expression)
