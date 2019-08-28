namespace FuncTestingFramework.Extensions
open System
open System.Linq.Expressions
open System.Runtime.CompilerServices
open FuncTestingFramework.generator
open FuncTestingFramework.ExpressionBuilder

[<Extension>]
type  ObjectExtensions() =
    [<Extension>]
    static member  For<'a>(Object list,(expression:Expression<Func<'a,string>>)) :String<'a>  =
        String (list,expression)
            
    [<Extension>]
    static member For<'a>(Object list,(expression:Expression<Func<'a,int>>)) :Int<'a> =
        Int (list,expression)

type Language =
    |English
    |Russian

[<Extension>]
type StringExtensions()=
//    [<Extension>]
//    static member UseValue<'a>((String (value,func)):String<'a>,(value:string))=
        
//    [<Extension>]
//    static member Ignore<'a>((String value):String<'a>) =
//        Object value
//        
//    [<Extension>]
//    static member Length<'a>((String value):String<'a>,(value:int)) =
//        Object value
//        
    [<Extension>]
    static member MinLength<'a>((String (store,func)):String<'a>,(value:int)) =
        Object <| ((maxLengthCallMethod<'a> func value)::store)

//        
    [<Extension>]
    static member MaxLength<'a>((String (store,func)):String<'a>,(value:int)) =
        if value>10000 || value<0  then failwith "value must be less 10000"
        Object <| ((maxLengthCallMethod<'a> func value)::store)
//        
//    [<Extension>]
//    static member UseLanguage<'a>((String value):String<'a>,(value:Language)) =
//        Object value