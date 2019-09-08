module FuncTestingFramework.ExpressionBuilder
open System
open System.Collections.Generic
open System.Linq.Expressions
open FSharp.Quotations.Evaluator.QuotationEvaluationExtensions
open FuncTestingFramework.Generator
type Storage<'a> = Quotations.Expr<'a -> 'a> list
type Path<'a, 't> = Expression<Func<'a, 't>>
type Value<'a>(v: 'a) =
   let mutable _v = v
   member this.V with get () = _v and set (v) = _v <- v;

let cons = Expression.Constant
let lambda<'a> e p = Expression.Lambda<'a>(e, p)
let assign l r =
    try Expression.Assign(l, r)
    with e -> failwith e.Message

module ObjectExpressions =
    let useValue (e: Expression<Func<'a, 'b>>) (value: 'b): Quotations.Expr<'a -> 'a> =
        let assign' = assign e.Body (<@ value @>.ToLinqExpressionUntyped() :?> LambdaExpression).Body
        let l = (lambda<Action<'a>> assign' (e.Parameters |> Seq.toArray))
        <@ fun x -> l.Compile().Invoke(x); x @>

    let ignore (e: Expression<Func<'a, 'b>>): Quotations.Expr<'a -> 'a> =
        let d = Unchecked.defaultof<'b>
        useValue e <| d

    let forItem (path: Path<'a, IEnumerable<'t>>) (storage: Storage<Value<'t>>)
        : Quotations.Expr<'a -> 'a> =
            <@
            fun x ->
               let func = storage
                          |> Seq.map (fun x -> x.CompileUntyped() :?> Value<'t> -> Value<'t>)
                          |> Seq.toList

               let res = path.Compile().Invoke(x)
                       |> Seq.mapi (fun _ i -> Value<'t>(i))
                       |> Seq.collect (fun x -> func |> Seq.map (fun f -> f x))
                       |> Seq.map (fun x -> x.V)
                       |> Seq.toList

               let lambda = lambda<Action<'a>> (assign path.Body (cons res)) (path.Parameters |> Seq.toArray)
               lambda.Compile().Invoke(x)
               x
          @>

module StringExpressions =
    let intervalLengthStringMethod (e: Path<'a, string>) (l: int) (r: int): Quotations.Expr<'a -> 'a> =
          <@
             fun x ->
                let str =
                     seq { while true do yield generator.char() }
                        |> Seq.take (generator.newNextArgs l r)
                        |> Seq.toList
                        |> String.Concat
                let mutableAction = Expression.Lambda<Action<'a>>(Expression.Assign(e.Body, Expression.Constant(str)), e.Parameters)
                mutableAction.Compile().Invoke(x)
                x
          @>

module DecimalExpressions =
    let Interval (e: Expression<Func<'a, decimal>>) (min: decimal) (max: decimal): Quotations.Expr<'a -> 'a> =
        <@
          fun x ->
              let assign' = assign e.Body (cons (generator.generateDecimal min max))
              let l = lambda<Action<'a>> assign' (e.Parameters |> Seq.toArray)
              l.Compile().Invoke(x)
              x
        @>

module IntExpressions =
    let Interval (e: Path<'a, int>) (min: int) (max: int) =
       <@
          fun x ->
              let assign' = assign e.Body (cons (generator.generateDecimal (decimal (min)) (decimal (max)) |> int))
              let l = lambda<Action<'a>> assign' (e.Parameters |> Seq.toArray)
              l.Compile().Invoke(x)
              x
        @>

module DateTimeExpressions =
    let Interval (e: Path<'a, DateTime>) (min: DateTime) (max: DateTime) =
       <@
          // overhead for compiler errors
          let min = min
          let max = max
          fun x ->
              let value = DateTime(int64 (generator.generateDecimal (decimal (min.Ticks)) (decimal ((max.Ticks)))))
              let assign' = assign e.Body (cons value)
              let l = lambda<Action<'a>> assign' (e.Parameters |> Seq.toArray)
              l.Compile().Invoke(x)
              x
       @>

