module Kimedics.FSTests
open System.Collections.Generic

open System.Collections

open System.Collections

open System.Collections

open System.Collections.Generic

open FuncTestingFramework.ObjectExtensions
open FuncTestingFramework.Generator
let gen<'a> (obj: Object<'a>) =
    let v = FunctionTester.generateByType typeof<'t> :?> 't
    obj.Storage |> Seq.map ((fun x -> x.Compile()) >> (fun action -> action.Invoke(v))) |> Seq.toList |> ignore
    v

[<EntryPoint>]
let main (args) =
   let typ = typeof<IEnumerable<int>>
   let IEnumerable = typ.IsGenericType
   0


