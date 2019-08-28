
module FuncTestingFramework.generator
open System
open System.Linq.Expressions
open Microsoft.FSharp.Reflection

type TupleElementTypes = Type array

[<RequireQualifiedAccess>]
module generator =
    let newNextUnit() = ()
    let newNextArg i = Random().Next i
    let newNextArgs i j = Random().Next(i, j)
    let _int_32() = Random().Next()
    let _int_64(): int64 = int64 (_int_32()) * int64 (_int_32())
    let string() = Guid.NewGuid().ToString()
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


type Object<'a> = Object of Expression<Action<'a>> list

type Int<'a> = Int of (Expression<Action<'a>> list * Expression<Func<'a, int>>)

type String<'a> = String of (Expression<Action<'a>> list * Expression<Func<'a, string>>)

//type Configuration<'a> =
//    |Object of Accumulator<'a>
//    |Int of Accumulator<'a>
//    |String of Accumulator<'a>

type ConfigurationBuilder() =
    member this.Build<'a>(): Object<'a> = Object []





