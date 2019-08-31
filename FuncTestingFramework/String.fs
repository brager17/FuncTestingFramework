namespace FuncTestingFramework.Extensions
open System
open System.Linq.Expressions
open System.Runtime.CompilerServices
open FuncTestingFramework.generator
open FuncTestingFramework.ExpressionBuilder

module StringExpressions =
    let intervalLengthStringMethod (e: Expression<Func<'a, string>>) l r : Expression<Action<'a>> =
        let intervalStringFunc = (Func<int,int,string>(generator.stringInterval));
        let callIntervalStringFunc = call (cons intervalStringFunc.Target) intervalStringFunc.Method [cons l;cons r]
        let assign' = assign e.Body callIntervalStringFunc
        let lambda' = lambda<Action<'a>> assign' (e.Parameters|> Seq.toArray)
        lambda'
    
    let maxLengthStringCallMethod<'a> (e: Expression<Func<'a, string>>) (count:int): Expression<Action<'a>> =
        intervalLengthStringMethod e 0 count
     
    let minLenghtStringCallMethod<'a> (e:Expression<Func<'a,string>>) (count:int):Expression<Action<'a>> =
        intervalLengthStringMethod e count 10000

    let legnthCallMethod<'a> (e:Expression<Func<'a,string>>) (count:int) =
        intervalLengthStringMethod e count count



[<Extension>]
type StringExtensions()=
    [<Extension>]
    static member UseValue<'a>((String (store,func)):String<'a>,(value:string))=
        Object <| (Expressions.useValue func value)::store
        
    [<Extension>]
    static member Ignore<'a>((String (store,func)):String<'a>) =
        Object <| (Expressions.ignore func)::store

        
    [<Extension>]
    static member Length<'a>((String (store,func)):String<'a>,(value:int)) =
        Object <| (StringExpressions.legnthCallMethod<'a> func value)::store
        
    [<Extension>]
    static member MinLength<'a>((String (store,func)):String<'a>,(value:int)) =
        Object <| (StringExpressions.minLenghtStringCallMethod<'a> func value)::store

    [<Extension>]
    //todo собрать все условия в одном месте
    static member MaxLength<'a>((String (store,func)):String<'a>,(value:int)) =
        if value>10000 || value<0  then failwith "value must be less 10000"
        Object <| (StringExpressions.maxLengthStringCallMethod<'a> func value)::store
