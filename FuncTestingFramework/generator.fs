
module FuncTestingFramework.generator
open System
open System
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

    let rec intervalInt64 (min: int64) (max: int64): int64 =
        if (min < 0L && max > Int64.MaxValue + min)
        then if Random().Next() % 2 = 0 then intervalInt64 min 0L else intervalInt64 0L max
        else int64 (double (max - min) * (Random().NextDouble())) + min

    let char() = Convert.ToChar (if _int_32() % 2 = 0 then newNextArgs 65 90 else newNextArgs 97 122)

    let string() = seq { for _ in 0..36 -> char() } |> String.Concat

    let stringInterval min max =
        seq { while true do yield char() }
        |> Seq.take (newNextArgs min max)
        |> String.Concat

    let bool() = _int_32() % 2 = 0

    let _decimal() = decimal (_int_32()) * decimal (Random().NextDouble())

    let rec generateDecimal (min: decimal) (max: decimal): decimal =
        if (min < 0M && max > Decimal.MaxValue + min)
        then if Random().Next() % 2 = 0 then generateDecimal 0M max else generateDecimal min 0M
        else (max - min) * (decimal (Random().NextDouble())) + min

    let _dateTime (min: DateTime) (max: DateTime) =
        let s = intervalInt64 min.Ticks max.Ticks
        DateTime(s)

    let date() = _dateTime DateTime.MinValue DateTime.MaxValue

[<RequireQualifiedAccess>]
module FunctionTester =

    type PrimitiveType =
        | Int32'
        | Int64'
        | String'
        | DateTime'
        | Decimal'
        | Boolean'
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
            | t when t = typeof<Decimal> -> Decimal'
            | t when t = typeof<bool> -> Boolean'
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
        | GetType Boolean' -> generator.bool() :> obj
        | GetType Decimal' -> generator._decimal() :> obj
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

type Decimal<'a> = Decimal of (Expression<Action<'a>> list * Expression<Func<'a, decimal>>)

type Boolean<'a> = Boolean of (Expression<Action<'a>> list * Expression<Func<'a, bool>>)

type Date<'a> = Date of (Expression<Action<'a>> list * Expression<Func<'a, DateTime>>)

type String<'a> = String of (Expression<Action<'a>> list * Expression<Func<'a, string>>)


type SequanceStore<'a,'seqType> = (Expression<Action<'a>> list * Expression<Func<'a, seq<'seqType>>>)
type Sequance<'a, 'seqType> = Sequance of SequanceStore<'a,'seqType>

//type Configuration<'a> =
//    |Object of Accumulator<'a>
//    |Int of Accumulator<'a>
//    |String of Accumulator<'a>

type ConfigurationBuilder() =
    member this.Build<'a>(): Object<'a> = Object []





