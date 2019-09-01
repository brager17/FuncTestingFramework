module FuncTestingFramework.ExpressionBuilder
open System
open System.Collections.Generic
open FuncTestingFramework.Generator
open System.Linq.Expressions

let consWithType (a: 'a) = Expression.Constant(a, typeof<'a>)
let cons = Expression.Constant

let lambda<'a> e p = Expression.Lambda<'a>(e, p)

let call ins mtd prs = Expression.Call(ins, mtd, prs |> Array.ofList)

let assign l r = Expression.Assign(l, r)

let mtdByName<'a> name =
    typeof<'a>.GetMethods() |> Array.filter (fun x -> x.Name = name) |> Array.head

let mtdMyNameAndArgsCount<'a> name countArgs =
    typeof<'a>.GetMethods() |> Array.filter (fun x -> x.Name = name && x.GetParameters().Length = countArgs) |> Array.head


module ObjectExpressions =
    let buildMutableAction (exprCall: MethodCallExpression) (e: Expression<Func<'a, 'b>>): Expression<Action<'a>> =
        let assign' = assign e.Body exprCall
        lambda assign' (e.Parameters |> Seq.toArray)

    let useValue (e: Expression<Func<'a, 'b>>) (value: 'b): Expression<Action<'a>> =
        let assign' = assign e.Body (consWithType value)
        lambda assign' (e.Parameters |> Seq.toArray)

    let ignore (e: Expression<Func<'a, 'b>>): Expression<Action<'a>> =
        let d = Unchecked.defaultof<'b>
        useValue e <| d


module DecimalExpressions =
    let Interval (e: Expression<Func<'a, decimal>>) (min: decimal) (max: decimal): Expression<Action<'a>> =
        let f = new Func<decimal, decimal, decimal>(generator.generateDecimal)
        let call' = call (cons f.Target) f.Method [ cons min; cons max ]
        ObjectExpressions.buildMutableAction call' e


module StringExpressions =
    let intervalLengthStringMethod (e: Expression<Func<'a, string>>) (l: int) (r: int): Expression<Action<'a>> =
        let intervalStringFunc = (Func<int, int, string>(generator.stringInterval));
        let callIntervalStringFunc = call (cons intervalStringFunc.Target) intervalStringFunc.Method [ cons l; cons r ]
        ObjectExpressions.buildMutableAction callIntervalStringFunc e

    let maxLengthStringCallMethod (e: Expression<Func<'a, string>>) (count: int): Expression<Action<'a>> =
        intervalLengthStringMethod e 0 count

    let minLenghtStringCallMethod (e: Expression<Func<'a, string>>) (count: int): Expression<Action<'a>> =
        intervalLengthStringMethod e count 10000

    let legnthCallMethod (e: Expression<Func<'a, string>>) (count: int) =
        intervalLengthStringMethod e count count


module IntExpressions =
    let private rnd = new Random();
    let private callNext_2 (l: int) (r: int) = call (cons rnd) (mtdMyNameAndArgsCount<Random> "Next" 2) [ cons l; cons r ]

    let between (e: Expression<Func<'a, int>>) l r: Expression<Action<'a>> =
        let exprCall = callNext_2 l r
        ObjectExpressions.buildMutableAction exprCall e

    let min (e: Expression<Func<'a, int>>) l: Expression<Action<'a>> =
        between e l Int32.MaxValue

    let max (e: Expression<Func<'a, int>>) r: Expression<Action<'a>> =
        between e Int32.MinValue r

    let negative (e: Expression<Func<'a, int>>): Expression<Action<'a>> =
        between e Int32.MinValue 0

    let positive (e: Expression<Func<'a, int>>): Expression<Action<'a>> =
        between e 0 Int32.MaxValue


module DateTimeExpressions =
    let Interval (e: Expression<Func<'a, DateTime>>) (min: DateTime) (max: DateTime) =
       let func = new Func<DateTime, DateTime, DateTime>(generator._dateTime)
       let callExpressin = call (cons func.Target) (func.Method) [ cons min; cons max ];
       ObjectExpressions.buildMutableAction callExpressin e

module SequenceExpressions =
    let getArrayAccessLambda<'a> =
        let arrayType = typeof<'a array>
        let arrayParameter = Expression.Parameter(arrayType)
        let arrayAccess = Expression.ArrayAccess(arrayParameter, Expression.Constant(0))
        let result = Expression.Lambda<Func<'a array, 'a>>(arrayAccess, arrayParameter)
        result
        
    let applyActions<'a,'t> (list : Action<'t array> list) (path:Expression<Func<'a, IEnumerable<'t>>>) = 
        let arr = Array.init 100 (fun _ -> Unchecked.defaultof<'t>)
        let r =
            arr |> Seq.collect (fun i ->
            let a = [| i |]
            list |> Seq.iter (fun f -> f.Invoke(a))
            a)
        let assign' = assign path.Body (cons r)
        Expression.Lambda<Action<'a>>(assign', path.Parameters)


