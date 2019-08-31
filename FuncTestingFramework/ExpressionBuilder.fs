module FuncTestingFramework.ExpressionBuilder
open System
open System
open System
open System.Linq.Expressions

let consWithType (a:'a) = Expression.Constant (a,typeof<'a>)
let cons = Expression.Constant

let lambda<'a> e p = Expression.Lambda<'a>(e, p)

let call ins mtd prs = Expression.Call(ins, mtd, prs |> Array.ofList)

let assign l r = Expression.Assign(l, r)

let mtdByName<'a> name =
    typeof<'a>.GetMethods() |> Array.filter (fun x -> x.Name = name) |> Array.head

let mtdMyNameAndArgsCount<'a> name countArgs =
    typeof<'a>.GetMethods() |> Array.filter (fun x -> x.Name = name && x.GetParameters().Length = countArgs) |> Array.head



module Expressions =
    
    let buildMutableAction (exprCall:MethodCallExpression) (e: Expression<Func<'a, 'b>>) :Expression<Action<'a>> =
        let assign' = assign e.Body exprCall
        lambda assign' (e.Parameters |> Seq.toArray)
    
    let useValue (e: Expression<Func<'a, 'b>>) (value:'b): Expression<Action<'a>> =
        let assign' = assign e.Body (consWithType value)
        lambda assign' (e.Parameters |> Seq.toArray)

    let ignore (e: Expression<Func<'a, 'b>>): Expression<Action<'a>> =
        let d = Unchecked.defaultof<'b>
        useValue e <| d 


