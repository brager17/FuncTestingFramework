module FuncTestingFramework.Generator
open System
open System.Collections
open System.Linq

type TupleElementTypes = Type array

[<RequireQualifiedAccess>]
module generator =
    let newNextArgs i j = Random().Next(i, j)
    let _int_32() = Random().Next()
    let _int_64(): int64 = int64 (_int_32()) * int64 (_int_32())
    let rec intervalInt64 (min: int64) (max: int64): int64 =
        if (min < 0L && max > Int64.MaxValue + min)
        then if Random().Next() % 2 = 0 then intervalInt64 min 0L else intervalInt64 0L max
        else int64 (double (max - min) * (Random().NextDouble())) + min
    let char() = Convert.ToChar (if _int_32() % 2 = 0 then newNextArgs 65 90 else newNextArgs 97 122)
    let string() = seq { for _ in 0..36 -> char() } |> String.Concat
    let bool() = _int_32() % 2 = 0
    let _decimal() = decimal (_int_32()) * decimal (Random().NextDouble())
    let rec generateDecimal (min: decimal) (max: decimal): decimal =
        if (min < 0M && max > Decimal.MaxValue + min)
        then if Random().Next() % 2 = 0 then generateDecimal 0M max else generateDecimal min 0M
        else (max - min) * (decimal (Random().NextDouble())) + min
    let date() = DateTime(intervalInt64 DateTime.MinValue.Ticks DateTime.MaxValue.Ticks)

[<RequireQualifiedAccess>]
module FunctionTester =
    type PrimitiveType =
        | Int32'
        | Int64'
        | String'
        | DateTime'
        | Decimal'
        | Boolean'
        | Class
        | IEnumerable of Type


    let rec (|GetType|_|) type' =
            match type' with
            | t when t = typeof<Int32> -> Some Int32'
            | t when t = typeof<Int64> -> Some Int64'
            | t when t = typeof<string> -> Some String'
            | t when t = typeof<DateTime> -> Some DateTime'
            | t when t = typeof<Decimal> -> Some Decimal'
            | t when t = typeof<bool> -> Some Boolean'
            | t when t.IsGenericType && t.GetInterfaces() |> Seq.exists (fun inf -> inf = typeof<IEnumerable>) ->
                let generic = t |> (fun x -> x.GenericTypeArguments |> Array.head)
                Some <| IEnumerable generic
            | t when t.IsClass -> Some <| Class
            | _ -> None

    let enumerableCast toType (enumerable: IEnumerable) =
        typeof<Enumerable>.GetMethod("Cast").MakeGenericMethod([| toType |]).Invoke(null, [| enumerable |])

    let rec generateByType (tp: Type) =
        match tp with
        | GetType Int32' -> (generator._int_32()) :> obj
        | GetType Int64' -> generator._int_64() :> obj
        | GetType String' -> generator.string() :> obj
        | GetType DateTime' -> generator.date() :> obj
        | GetType Boolean' -> generator.bool() :> obj
        | GetType Decimal' -> generator._decimal() :> obj
        | GetType(IEnumerable classType) -> enumerableCast classType (Seq.init 100 (fun _ -> generateByType classType))
        | GetType Class ->
            let new' = Activator.CreateInstance(tp)
            new'.GetType().GetProperties()
            |> Seq.iter (fun p ->
                let newValue = generateByType p.PropertyType
                p.SetValue(new', newValue))
            new'
        | _ -> failwith "Not Supported"

