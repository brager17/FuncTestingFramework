namespace rec FuncTestingFramework.ObjectExtensions
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.Generator
open System
open System.Collections.Generic
open System.Collections.Generic
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

    member __.For(path: Expression<Func<'a, IEnumerable<'c>>>) =
        SequenceClassT<'a, 'c>([], path)

type PathObject<'a, 'd>(store: Storage<'a>, path: Path<'a, 'd>) =
    inherit Object<'a>(store)
    abstract UseValue: 'd -> Object<'a>
    abstract Ignore: unit -> Object<'a>
    default  __.UseValue(value: 'd) =
        Object<'a>((ObjectExpressions.useValue path value) :: store)
    default __.Ignore() =
        Object<'a>((ObjectExpressions.ignore path) :: store)
        
type NestedClassConfiguration<'a> = Func<Object<'a>, Object<'a>>
type ClassT<'a, 'd>(store: Storage<'a>, path: Path<'a, 'd>) =
    inherit PathObject<'a, 'd>(store, path)
    override  __.UseValue(value: 'd) = base.UseValue(value)
    override __.Ignore() = base.Ignore()
    member __.ForNested(deepPath: NestedClassConfiguration<'d>): Object<'a> =
        let objD = deepPath.Invoke(Object<'d>([]))
        let func = ObjectExpressions.forNested path objD.Storage
        Object<'a>(func :: store)

type SequenceNestedClassConfiguration<'a> = Func<Object<'a>, Object<'a>>
type SequenceClassT<'a, 'd>(store: Storage<'a>, path: Path<'a, IEnumerable<'d>>) =
    inherit PathObject<'a, IEnumerable<'d>>(store, path)
    override  __.UseValue(value: IEnumerable<'d>) = base.UseValue(value)
    override __.Ignore() = base.Ignore()

    member __.ForItem(storage: SequenceNestedClassConfiguration<'d>) =
        let mutableFunc =
            let nullElement = Object<'d>([])
            let storage = storage.Invoke(nullElement).Storage |> Seq.map (fun x -> x.Compile()) |> Seq.toList
            <@
            fun x ->
               let res = path.Compile().Invoke(x)
                       |> Seq.collect (fun x -> storage |> Seq.map (fun f -> f x))
                       |> Seq.toList
               let lambda = lambda<Action<'a>> (assign path.Body (cons res)) (path.Parameters |> Seq.toArray)
               lambda.Compile().Invoke(x)
               x
            @>
        SequenceClassT(mutableFunc :: store, path)

type DecimalT<'a>(store: Storage<'a>, path: Path<'a, decimal>) =
    inherit PathObject<'a, decimal>(store, path)
    override  __.UseValue(value: decimal) = base.UseValue(value)
    override __.Ignore() = base.Ignore()

     member __.Min(min: decimal) =
        DecimalT((DecimalExpressions.Interval path min Decimal.MaxValue) :: store, path)

     member __.Max(max: decimal) =
        DecimalT((DecimalExpressions.Interval path Decimal.MinValue max) :: store, path)

     member __.Interval (min: decimal) (max: decimal) =
        DecimalT((DecimalExpressions.Interval path min max) :: store, path)


type DecimalItemConfiguration = Func<DecimalT<Value<decimal>>, DecimalT<Value<decimal>>>

type SequenceDecimal<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<decimal>>) =
    inherit PathObject<'a, IEnumerable<decimal>>(store, path)
    override  __.UseValue(value: IEnumerable<decimal>) = base.UseValue(value)
    override __.Ignore() = base.Ignore()

     member __.ForItem(storage: DecimalItemConfiguration) =
        let mutableFunc =
            let nullElement = DecimalT([], Value<decimal>.GetLambda())
            let storage = storage.Invoke(nullElement).Storage
            ObjectExpressions.forItem path storage
        SequenceDecimal(mutableFunc :: store, path)


type StringT<'a>(store: Storage<'a>, path: Path<'a, string>) =
    inherit PathObject<'a, string>(store, path)
    override  __.UseValue(value: string) = base.UseValue(value)
    override __.Ignore() = base.Ignore()

     member __.Length(value: int) =
        StringT((StringExpressions.intervalLengthStringMethod path value value) :: store, path)

     member __.MinLength(value: int) =
        if value > 10000 || value < 0 then failwith "value must be less 10000"
        StringT((StringExpressions.intervalLengthStringMethod path value 1000) :: store, path)

     member __.MaxLength(value: int) =
        if value > 10000 || value < 0 then failwith "value must be less 10000"
        StringT((StringExpressions.intervalLengthStringMethod path 0 value) :: store, path)


type StringItemConfiguration = Func<StringT<Value<string>>, StringT<Value<string>>>

type SequenceString<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<string>>) =
    inherit PathObject<'a, IEnumerable<string>>(store, path)
    override  __.UseValue(value: IEnumerable<string>) = base.UseValue(value)
    override __.Ignore() = base.Ignore()

      member __.ForItem(storage: StringItemConfiguration) =
        let mutableFunc =
            let nullElement = StringT([], Value<string>.GetLambda())
            let storage = storage.Invoke(nullElement).Storage
            ObjectExpressions.forItem path storage
        SequenceString(mutableFunc :: store, path)


type IntT<'a>(store: Storage<'a>, path: Path<'a, int>) =
    inherit PathObject<'a, int>(store, path)
    override  __.UseValue(value: int) = base.UseValue(value)
    override __.Ignore() = base.Ignore()

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


type SequenceIntConfiguration = Func<IntT<Value<int>>, IntT<Value<int>>>

type SequenceInt<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<int>>) =
    inherit PathObject<'a, IEnumerable<int>>(store, path)
    override  __.UseValue(value: IEnumerable<int>) = base.UseValue(value)
    override __.Ignore() = base.Ignore()

    member __.ForItem(storage: SequenceIntConfiguration) =
        let mutableFunc =
          let nullElement = IntT([], Value<int>.GetLambda())
          let storage = storage.Invoke(nullElement).Storage
          ObjectExpressions.forItem path storage
        SequenceInt(mutableFunc :: store, path)


type DateTimeT<'a>(store: Storage<'a>, path: Path<'a, DateTime>) =
    inherit PathObject<'a, DateTime>(store, path)
    override  __.UseValue(value: DateTime) = base.UseValue(value)
    override __.Ignore() = base.Ignore()

    member __.Min(min: DateTime) =
        DateTimeT((DateTimeExpressions.Interval path min DateTime.MaxValue) :: store, path)

    member __.Max(max: DateTime) =
        DateTimeT((DateTimeExpressions.Interval path DateTime.MinValue max) :: store, path)

    member __.Interval (min: DateTime) (max: DateTime) =
        DateTimeT((DateTimeExpressions.Interval path min max) :: store, path)


type SequenceDateTimeConfiguration = Func<DateTimeT<Value<DateTime>>, DateTimeT<Value<DateTime>>>

type DateTimeSequence<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<DateTime>>) =
     inherit PathObject<'a, IEnumerable<DateTime>>(store, path)
     override  __.UseValue(value: IEnumerable<DateTime>) = base.UseValue(value)
     override __.Ignore() = base.Ignore()

     member __.ForItem(storage: SequenceDateTimeConfiguration) =
       let mutableFunc =
           let nullElement = DateTimeT([], Value<DateTime>.GetLambda())
           let storage = storage.Invoke(nullElement).Storage
           ObjectExpressions.forItem path storage
       DateTimeSequence(mutableFunc :: store, path)

type BooleanT<'a>(store: Storage<'a>, path: Path<'a, bool>) =
    inherit PathObject<'a, bool>(store, path)
    override __.UseValue(value: bool) = __.UseValue(value)
    override __.Ignore() = __.Ignore()


type BooleanSequenceConfiguration = Func<BooleanT<Value<bool>>, BooleanT<Value<bool>>>

type BooleanSequence<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<bool>>) =
     inherit PathObject<'a, IEnumerable<bool>>(store, path)
     override  __.UseValue(value: IEnumerable<bool>) = base.UseValue(value)
     override __.Ignore() = base.Ignore()

     member __.ForItem(storage: BooleanSequenceConfiguration) =
       let mutableFunc =
           let nullElement = BooleanT([], Value<bool>.GetLambda())
           let storage = storage.Invoke(nullElement).Storage
           ObjectExpressions.forItem path storage
       BooleanSequence(mutableFunc :: store, path)

