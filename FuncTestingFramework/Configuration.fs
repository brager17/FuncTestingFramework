module FuncTestingFramework.Configuration
open FSharp.Quotations.Evaluator.QuotationEvaluationExtensions
open FuncTestingFramework.Generator
open FuncTestingFramework.ObjectExtensions

module Configuration =
    let Build<'a>() = Object<'a>([])

    let gen<'a> (obj: Object<'a>) =
        let v = FunctionTester.generateByType typeof<'a> :?> 'a
        obj.Storage
        |> Seq.map (fun x -> x.CompileUntyped() :?> 'a -> 'a)
        |> Seq.map (fun t -> t v)
        |> Seq.toList
        |> ignore
        v