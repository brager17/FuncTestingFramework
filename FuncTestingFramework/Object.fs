namespace rec FuncTestingFramework.ObjectExtensions
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.Generator
open System
open System.Collections.Generic
open System.Linq.Expressions
open FSharp.Quotations.Evaluator.QuotationEvaluationExtensions
open FuncTestingFramework.Types

type Object<'a>(storage: Storage<'a>) =
    member internal __.Storage = storage

    member __.For(path: Path<'a, decimal>): DecimalT<'a> =
        DecimalT(storage, path)

    member __.For(path: Expression<Func<'a, string>>) =
       StringT(storage, path);

    member __.For(path: Expression<Func<'a, int>>) =
       IntT(storage, path);

    member __.For(path: Expression<Func<'a, bool>>) =
       BooleanT(storage, path);

    member __.For(path: Expression<Func<'a, IEnumerable<bool>>>) =
       BooleanSequence(storage, path);

    member __.For(path: Expression<Func<'a, DateTime>>) =
       DateTimeT(storage, path);

    member __.For(path: Expression<Func<'a, IEnumerable<DateTime>>>) =
       DateTimeSequence(storage, path);

    member __.For(path: Expression<Func<'a, IEnumerable<decimal>>>) =
       SequenceDecimal(storage, path);

    member __.For(path: Expression<Func<'a, IEnumerable<string>>>) =
       SequenceString(storage, path);

    member __.For(path: Expression<Func<'a, IEnumerable<int>>>) =
       SequenceInt(storage, path);

    member __.For(path: Expression<Func<'a, 'c>>) =
        ClassT<'a, 'c>([], path)

type ClassT<'a, 'd>(store: Storage<'a>, path: Path<'a, 'd>) =
    inherit Object<'a>(store)
    member __.UseValue((value: 'd)) =
        ClassT((ObjectExpressions.useValue path value) :: store, path)

    member __.Ignore() =
        ClassT((ObjectExpressions.ignore path) :: store, path)

    member __.ForNested(deepPath: Func<Object<'d>, Object<'d>>): Object<'a> =
        let func =
            <@ fun x ->
                let objD = deepPath.Invoke(Object<'d>([]))
                let v = FunctionTester.generateByType typeof<'d> :?> 'd
                objD.Storage
                    |> Seq.map (fun x -> x.CompileUntyped() :?> 'd -> 'd)
                    |> Seq.map (fun t -> t v)
                    |> Seq.toList
                    |> ignore

                let a = assign path.Body (cons (v))
                let lambda = lambda<Action<'a>> a (path.Parameters |> Seq.toArray)
                lambda.Compile().Invoke(x);
                x
             @>
        Object<'a>(func :: store)



type DecimalT<'a>(store: Storage<'a>, path: Path<'a, decimal>) =
     inherit Object<'a>(store)
     member __.UseValue(value: decimal) =
        DecimalT(((ObjectExpressions.useValue path value) :: store), path)

     member __.Ignore() =
        DecimalT(((ObjectExpressions.ignore path) :: store), path)

     member __.Min(min: decimal) =
        DecimalT((DecimalExpressions.Interval path min Decimal.MaxValue) :: store, path)

     member __.Max(max: decimal) =
        DecimalT((DecimalExpressions.Interval path Decimal.MinValue max) :: store, path)

     member __.Interval (min: decimal) (max: decimal) =
        DecimalT((DecimalExpressions.Interval path min max) :: store, path)


type DecimalItemConfiguration = Func<DecimalT<Value<decimal>>, DecimalT<Value<decimal>>>

type SequenceDecimal<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<decimal>>) =
     inherit Object<'a>(store)

     member __.UseValue(value: IEnumerable<decimal>) =
        SequenceDecimal(((ObjectExpressions.useValue path value) :: store), path)

     member __.Ignore() =
        SequenceDecimal(((ObjectExpressions.ignore path) :: store), path)

     member __.ForItem(storage: DecimalItemConfiguration) =
        let mutableFunc =
            let nullElement = DecimalT([], Value<decimal>.GetLambda())
            let storage = storage.Invoke(nullElement).Storage
            ObjectExpressions.forItem path storage
        SequenceDecimal(mutableFunc :: store, path)


type StringT<'a>(store: Storage<'a>, expr: Path<'a, string>) =
     inherit Object<'a>(store)

     member __.UseValue(value: string) =
        StringT((ObjectExpressions.useValue expr value) :: store, expr)

     member __.Ignore() =
        StringT((ObjectExpressions.ignore expr) :: store, expr)

     member __.Length(value: int) =
        StringT((StringExpressions.intervalLengthStringMethod expr value value) :: store, expr)

     member __.MinLength(value: int) =
        if value > 10000 || value < 0 then failwith "value must be less 10000"
        StringT((StringExpressions.intervalLengthStringMethod expr value 1000) :: store, expr)

     member __.MaxLength(value: int) =
        if value > 10000 || value < 0 then failwith "value must be less 10000"
        StringT((StringExpressions.intervalLengthStringMethod expr 0 value) :: store, expr)


type StringItemConfiguration = Func<StringT<Value<string>>, StringT<Value<string>>>

type SequenceString<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<string>>) =
      inherit Object<'a>(store)

      member __.UseValue(value: IEnumerable<string>) =
        SequenceString(((ObjectExpressions.useValue path value) :: store), path)

      member __.Ignore() =
        SequenceString(((ObjectExpressions.ignore path) :: store), path)

      member __.ForItem(storage: StringItemConfiguration) =
        let mutableFunc =
            let nullElement = StringT([], Value<string>.GetLambda())
            let storage = storage.Invoke(nullElement).Storage
            ObjectExpressions.forItem path storage
        SequenceString(mutableFunc :: store, path)


type IntT<'a>(store: Storage<'a>, path: Path<'a, int>) =
    inherit Object<'a>(store)

    member __.Between((l: int), (r: int)): IntT<'a> =
        if l >= r then failwith "left number must be less than rigth number"
        IntT((IntExpressions.Interval path l r :: store), path)

    member __.Min((l: int)): IntT<'a> =
        IntT((IntExpressions.Interval path l Int32.MaxValue :: store), path)

    member __.Max((r: int)): IntT<'a> =
        IntT((IntExpressions.Interval path Int32.MinValue r :: store), path)

    member __.Negative(): IntT<'a> =
        IntT((IntExpressions.Interval path Int32.MinValue 0 :: store), path)

    member __.Positive(): IntT<'a> =
        IntT((IntExpressions.Interval path 0 Int32.MaxValue :: store), path)

    member __.UseValue(v: int): IntT<'a> =
         IntT(ObjectExpressions.useValue path v :: store, path)

    member __.Ignore(): IntT<'a> =
         IntT(ObjectExpressions.ignore path :: store, path)


type SequenceIntConfiguration = Func<IntT<Value<int>>, IntT<Value<int>>>

type SequenceInt<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<int>>) =
      inherit Object<'a>(store)

      member __.UseValue(value: IEnumerable<int>) =
        SequenceInt(((ObjectExpressions.useValue path value) :: store), path)

      member __.Ignore() =
        SequenceInt(((ObjectExpressions.ignore path) :: store), path)

      member __.ForItem(storage: SequenceIntConfiguration) =
        let mutableFunc =
            let nullElement = IntT([], Value<int>.GetLambda())
            let storage = storage.Invoke(nullElement).Storage
            ObjectExpressions.forItem path storage
        SequenceInt(mutableFunc :: store, path)


type DateTimeT<'a>(store: Storage<'a>, expr: Path<'a, DateTime>) =
    inherit Object<'a>(store)
     member __.UseValue(value: DateTime) =
        DateTimeT(((ObjectExpressions.useValue expr value) :: store), expr)

    member __.Ignore() =
        DateTimeT((ObjectExpressions.ignore expr) :: store, expr)

    member __.Min(min: DateTime) =
        DateTimeT((DateTimeExpressions.Interval expr min DateTime.MaxValue) :: store, expr)

    member __.Max(max: DateTime) =
        DateTimeT((DateTimeExpressions.Interval expr DateTime.MinValue max) :: store, expr)

    member __.Interval (min: DateTime) (max: DateTime) =
        DateTimeT((DateTimeExpressions.Interval expr min max) :: store, expr)


type SequenceDateTimeConfiguration = Func<DateTimeT<Value<DateTime>>, DateTimeT<Value<DateTime>>>

type DateTimeSequence<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<DateTime>>) =
         inherit Object<'a>(store)

         member __.UseValue(value: IEnumerable<DateTime>) =
             DateTimeSequence(((ObjectExpressions.useValue path value) :: store), path)

         member __.Ignore() =
            DateTimeSequence(((ObjectExpressions.ignore path) :: store), path)

         member __.ForItem(storage: SequenceDateTimeConfiguration) =
           let mutableFunc =
               let nullElement = DateTimeT([], Value<DateTime>.GetLambda())
               let storage = storage.Invoke(nullElement).Storage
               ObjectExpressions.forItem path storage
           DateTimeSequence(mutableFunc :: store, path)

type BooleanT<'a>(store: Storage<'a>, path: Path<'a, bool>) =
    inherit Object<'a>(store)
    member __.UseValue(value: bool) =
             BooleanT(((ObjectExpressions.useValue path value) :: store), path)

    member __.Ignore() =
            BooleanT(((ObjectExpressions.ignore path) :: store), path)


type BooleanSequenceConfiguration = Func<BooleanT<Value<bool>>, BooleanT<Value<bool>>>


type BooleanSequence<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<bool>>) =
         inherit Object<'a>(store)

         member __.UseValue(value: IEnumerable<bool>) =
             BooleanSequence(((ObjectExpressions.useValue path value) :: store), path)

         member __.Ignore() =
            BooleanSequence(((ObjectExpressions.ignore path) :: store), path)

         member __.ForItem(storage: BooleanSequenceConfiguration) =
           let mutableFunc =
               let nullElement = BooleanT([], Value<bool>.GetLambda())
               let storage = storage.Invoke(nullElement).Storage
               ObjectExpressions.forItem path storage
           BooleanSequence(mutableFunc :: store, path)


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
