module FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.generator
open System
open System.Linq.Expressions

let cons a = Expression.Constant(a)

let lambda<'a> e p = Expression.Lambda<'a>(e, p)

let call ins mtd prs = Expression.Call(ins, mtd, prs |> Array.ofList)

let assign l r = Expression.Assign(l, r)

let mtdByName name =
    typeof<'a>.GetMethods() |> Array.filter (fun x -> x.Name = name) |> Array.head

let mtdMyNameAndArgsCount<'a> name countArgs =
    typeof<'a>.GetMethods() |> Array.filter (fun x -> x.Name = name && x.GetParameters().Length = countArgs) |> Array.head

[<Literal>]
let countSymbolsInGuid = 36;

// выделить отдельный метод генерирущий строку определенной длины
// выделить UseValue Ignore в отдельные методы
let maxLengthCallMethod<'a> (e: Expression<Func<'a, string>>) count: Expression<Action<'a>> =
     let maxLength =
        new Func<int, string>(fun count ->
            let steps = (Math.Floor(double (count) / double (countSymbolsInGuid)) |> int) + 1;
            seq { for _ in 1..steps -> generator.string() } |> String.Concat |> Seq.take (count) |> String.Concat)
     let maxLengthCall = Expression.Call(Expression.Constant(maxLength.Target),maxLength.Method, cons count)
     let assign' = assign e.Body maxLengthCall
     lambda assign' (e.Parameters |> Seq.toArray)


module Expressions =
    let useValue (e: Expression<Func<'a, 'b>>) value: Expression<Action<'a>> =
        let assign' = assign e.Body (cons value)
        lambda assign' (e.Parameters |> Seq.toArray)

    let ignore (e: Expression<Func<'a, 'b>>): Expression<Action<'a>> =
        useValue e <| Activator.CreateInstance<'b>()

module IntExpressions =
    let private rnd = new Random();
    let private callNext_2 l r = call (cons rnd) (mtdMyNameAndArgsCount<Random> "Next" 2) [ cons l; cons r ]

    let between (e: Expression<Func<'a, int>>) l r: Expression<Action<'a>> =
        let exprCall = callNext_2 l r
        let assign' = assign e.Body exprCall
        lambda assign' (e.Parameters |> Seq.toArray)

    let min<'a> (e: Expression<Func<'a, int>>) l: Expression<Action<'a>> =
        between e l Int32.MaxValue

    let max<'a> (e: Expression<Func<'a, int>>) r: Expression<Action<'a>> =
        between e Int32.MinValue r

    let negative (e: Expression<Func<'a, int>>): Expression<Action<'a>> =
        between e Int32.MinValue 0

    let positive (e: Expression<Func<'a, int>>): Expression<Action<'a>> =
        between e 0 Int32.MaxValue

module StringExpressions =
    let maxLength<'a> (e: Expression<Func<'a, string>>) length: Expression<Action<'a>> =
        maxLengthCallMethod e length
