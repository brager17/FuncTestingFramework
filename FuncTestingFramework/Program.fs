module Kimedics.FSTests
open FuncTestingFramework.generator
open System
open System.Linq.Expressions
open System.Runtime.CompilerServices
open Microsoft.FSharp.Reflection

type TupleElementTypes = Type array

[<RequireQualifiedAccess>]
module generator =
    let newNextUnit() = ()
    let newNextArg i = Random().Next i
    let newNextArgs i j = Random().Next(i, j)
    let _int_32() = Random().Next()
    let _int_64(): int64 =
     int64 (_int_32()) * int64 (_int_32())

    let date() =
        let v = _int_64()
        DateTime <| (if v < 0L then (-1L * v) else v) % DateTime.MaxValue.Ticks

[<RequireQualifiedAccess>]
module FunctionTester =

    type PrimitiveType =
        | Int32'
        | Int64'
        | String'
        | DateTime'
        | Object'
        | Tuple of TupleElementTypes
        | Class
        | Record
        | NotSupported

    let (|GetType|) type' =
            printf "type:%A" type'
            match type' with
            | t when t = typeof<Int32> -> Int32'
            | t when t = typeof<Int64> -> Int64'
            | t when t = typeof<string> -> String'
            | t when t = typeof<DateTime> -> DateTime'
            | t when t = typeof<Tuple> -> Tuple <| FSharpType.GetTupleElements type'
            | t when t.IsClass -> Class
            | t when FSharpType.IsRecord t -> Record
            | _ -> NotSupported

    let unit f = f()

    let rec generateByType (tp: Type) =
        printf "generateByType"
        (match tp with
        | GetType Int32' -> (generator._int_32()) :> obj
        | GetType Int64' -> generator._int_64() :> obj
        | GetType String' -> generator.string() :> obj
        | GetType DateTime' -> generator.date() :> obj
        | GetType(Tuple items) ->
            let values = items |> Seq.map (fun x -> generateByType x) |> Seq.toArray
            FSharpValue.MakeTuple(values, tp)
        | GetType Class ->
            let new' = Activator.CreateInstance(tp)
            new'.GetType().GetProperties()
            |> Seq.iter (fun p ->
                let newValue = generateByType p.PropertyType
                p.SetValue(new', newValue))
            new'
        | _ -> failwith "not supported type")

    let generate<'a> =
        generateByType typeof<'a> :?> 'a

    let primitive<'a, 'b> (f: 'a -> 'b) =
        let randomValue = generate<'a>
        f randomValue

    let record<'a, 'b> (f: 'a -> 'b) =
        let props = typeof<'a>.GetProperties()
                    |> Seq.map (fun t -> generateByType t.PropertyType)
                    |> Seq.toArray
        let record = FSharpValue.MakeRecord(typeof<'a>, props) :?> 'a
        f record

    let class'<'a, 'b> (f: 'a -> 'b) =
        let changePublicPropertyValue (it: 'a) =
            typeof<'a>.GetProperties()
             |> Seq.filter (fun prop -> prop.CanWrite)
             |> Seq.iter (fun prop -> prop.SetValue(it, generateByType prop.PropertyType))

        let createdObjects =
            typeof<'a>.GetConstructors()
            |> Seq.map (fun ctor -> (ctor, ctor.GetParameters() |> Seq.map (fun x -> generateByType x.ParameterType)))
            |> Seq.map (fun (ctor, values) -> ctor.Invoke(values |> Seq.toArray))
            |> Seq.map (fun object -> changePublicPropertyValue (downcast object); downcast object)
        createdObjects |> Seq.map (f)

    let loadTest (f: 'a -> 'b) =
       let chooseFn =
        match typeof<'a> with
        | GetType Int32'
        | GetType Int64'
        | GetType String'
        | GetType DateTime'
        | GetType(Tuple _) -> primitive
        // todo
        | GetType Class -> class' >> Seq.head
        | GetType record' -> record
       for i in [ 1..100 ] do
            chooseFn f |> ignore

       chooseFn f

    let gen<'t> (conf: Object<'t>) =
      let (Object list) = conf
      let v = FunctionTester.generateByType typeof<'t> :?> 't
      list |> Seq.map ((fun x -> x.Compile()) >> (fun action -> action.Invoke(v))) |> Seq.toList |> ignore
      v

type Accumulator<'a> = Expression<Action<'a>> list

type public FSharpFuncUtil =
    [<Extension>]
    static member ToFSharpFunc<'a, 'b>(func: System.Func<'a, 'b>) = fun x -> func.Invoke(x)

open System
open System.Threading.Tasks
open System.Collections.Concurrent
open System.Collections.Generic
open System.Reflection

[<EntryPoint>]
let main (args) =
    
   0


