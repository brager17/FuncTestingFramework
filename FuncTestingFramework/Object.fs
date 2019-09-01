namespace rec FuncTestingFramework.ObjectExtensions
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.ExpressionBuilder
open FuncTestingFramework.ExpressionBuilder
open System
open System.Collections
open System.Collections.Generic
open System.Collections.Generic
open System.Linq.Expressions

type Storage<'a> = Expression<Action<'a>> list
type Path<'a, 't> = Expression<Func<'a, 't>>


type Object<'a>(storage: Storage<'a>) =
    member internal __.Storage = storage

    member __.For(path: Path<'a, string>): StringT<'a> =
        StringT(storage, path)

    member __.For(path: Path<'a, int>): Int<'a> =
        Int(storage, path)

    member __.For(path: Path<'a, bool>): Bool<'a> =
        Bool(storage, path)

    member __.For(path: Path<'a, decimal>): DecimalT<'a> =
        DecimalT(storage, path)

    member __.For(path: Path<'a, DateTime>): Date<'a> =
        Date(storage, path)

    member __.For((expression: Expression<Func<'a, IEnumerable<int>>>)): SequenceInt<'a> =
        SequenceInt([], expression)

    member __.For((expression: Expression<Func<'a, IEnumerable<DateTime>>>)): SequenceDateTime<'a> =
        SequenceDateTime([], expression)

    member __.For((expression: Expression<Func<'a, IEnumerable<decimal>>>)): SequenceDecimal<'a> =
        SequenceDecimal([], expression)


type DecimalT<'a>(store: Expression<Action<'a>> list, expr: Expression<Func<'a, decimal>>) =
     inherit Object<'a>(store)
     member __.UseValue((value: decimal)) =
        DecimalT((ObjectExpressions.useValue expr value) :: store, expr)

     member __.Ignore() =
        DecimalT((ObjectExpressions.ignore expr) :: store, expr)

     member __.Min(min: decimal) =
        DecimalT((DecimalExpressions.Interval expr min Decimal.MaxValue) :: store, expr)

     member __.Max(max: decimal) =
        DecimalT((DecimalExpressions.Interval expr Decimal.MinValue max) :: store, expr)

     member __.Interval (min: decimal) (max: decimal) =
        DecimalT((DecimalExpressions.Interval expr min max) :: store, expr)

type StringT<'a>(store: Expression<Action<'a>> list, expr: Expression<Func<'a, string>>) =
     inherit Object<'a>(store)
     member __.UseValue(value: string) =
        StringT((ObjectExpressions.useValue expr value) :: store, expr)

     member __.Ignore() =
        StringT((ObjectExpressions.ignore expr) :: store, expr)

     member __.Length(value: int) =
        StringT((StringExpressions.legnthCallMethod expr value) :: store, expr)

     member __.MinLength(value: int) =
        StringT((StringExpressions.minLenghtStringCallMethod expr value) :: store, expr)

    //todo собрать все условия в одном месте
     member __.MaxLength(value: int) =
        if value > 10000 || value < 0 then failwith "value must be less 10000"
        StringT((StringExpressions.maxLengthStringCallMethod expr value) :: store, expr)

type Int<'a>(store: Expression<Action<'a>> list, expr: Expression<Func<'a, int>>) =
    inherit Object<'a>(store)
    member __.Between((l: int32), (r: int32)): Int<'a> =
        if l >= r then failwith "left number must be less than rigth number"
        Int((IntExpressions.between expr l r :: store), expr)

    member __.Min((l: int32)): Int<'a> =
        Int(IntExpressions.min expr l :: store, expr)

    member __.Max((r: int32)): Int<'a> =
        Int(IntExpressions.max expr r :: store, expr)

    member __.Negative(): Int<'a> =
        Int(IntExpressions.negative expr :: store, expr)

    member __.Positive(): Int<'a> =
        Int(IntExpressions.positive expr :: store, expr)

    member __.UseValue(v: int): Int<'a> =
         Int(ObjectExpressions.useValue expr v :: store, expr)

    member __.Ignore(): Int<'a> =
         Int(ObjectExpressions.ignore expr :: store, expr)

type Date<'a>(store: Expression<Action<'a>> list, expr: Expression<Func<'a, DateTime>>) =
    inherit Object<'a>(store)
    member __.UseValue(value: DateTime) =
        Date((ObjectExpressions.useValue expr value) :: store, expr)

     member __.Ignore() =
        Date((ObjectExpressions.ignore expr) :: store, expr)

     member __.Min(min: DateTime) =
        Date((DateTimeExpressions.Interval expr min DateTime.MaxValue) :: store, expr)

     member __.Max(max: DateTime) =
        Date((DateTimeExpressions.Interval expr DateTime.MinValue max) :: store, expr)

     member __.Interval (min: DateTime) (max: DateTime) =
        Date((DateTimeExpressions.Interval expr min max) :: store, expr)

type Bool<'a>(store: Expression<Action<'a>> list, expr: Expression<Func<'a, bool>>) =
     inherit Object<'a>(store)
     member __.UseValue(value: bool) =
        Bool((ObjectExpressions.useValue expr value) :: store, expr)

     member __.Ignore(store, func) =
        Bool((ObjectExpressions.ignore expr) :: store, expr)

type ObjectItemConfiguration<'a> = Func<Object<'a>, Object<'a>>
type BoolItemConfiguration<'a> = Func<Bool<'a>, Bool<'a>>

type IntItemConfiguration = Func<Int<int []>, Int<int []>>
type SequenceInt<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<int>>) =
    inherit Object<'a>(store)
     member __.UseValue(value: IEnumerable<int>) =
        SequenceInt(((ObjectExpressions.useValue path value) :: store), path)

     member __.Ignore() =
        SequenceInt(((ObjectExpressions.ignore path) :: store), path)

     member __.ForItem(conf: IntItemConfiguration) =
        let arrayAccessLambda = SequenceExpressions.getArrayAccessLambda<int>
        let int = Int([], arrayAccessLambda)
        let compile = conf.Invoke(int).Storage |> Seq.map (fun x -> x.Compile()) |> Seq.toList;
        let lambda = SequenceExpressions.applyActions<'a, int> compile (path)
        SequenceInt(lambda :: store, path)


type DateItemConfiguration = Func<Date<DateTime []>, Date<DateTime []>>
type SequenceDateTime<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<DateTime>>) =
     inherit Object<'a>(store)
     member __.UseValue(value: IEnumerable<DateTime>) =
        SequenceDateTime(((ObjectExpressions.useValue path value) :: store), path)
     member __.Ignore() =
        SequenceDateTime(((ObjectExpressions.ignore path) :: store), path)

     member __.ForItem(conf: DateItemConfiguration) =
        let arrayAccessLambda = SequenceExpressions.getArrayAccessLambda<DateTime>
        let date = Date([], arrayAccessLambda)
        let compile = conf.Invoke(date).Storage |> Seq.map (fun x -> x.Compile()) |> Seq.toList;
        let lambda = SequenceExpressions.applyActions<'a, DateTime> compile (path)
        SequenceDateTime(lambda :: store, path)


type DecimalItemConfiguration = Func<DecimalT<decimal []>, DecimalT<decimal []>>
type SequenceDecimal<'a>(store: Storage<'a>, path: Path<'a, IEnumerable<decimal>>) =
     inherit Object<'a>(store)
     member __.UseValue(value: IEnumerable<decimal>) =
        SequenceDecimal(((ObjectExpressions.useValue path value) :: store), path)
     member __.Ignore() =
        SequenceDecimal(((ObjectExpressions.ignore path) :: store), path)

     member __.ForItem(conf: DecimalItemConfiguration) =
        let arrayAccessLambda = SequenceExpressions.getArrayAccessLambda<decimal>
        let date = DecimalT([], arrayAccessLambda)
        let compile = conf.Invoke(date).Storage |> Seq.map (fun x -> x.Compile()) |> Seq.toList;
        let lambda = SequenceExpressions.applyActions<'a, decimal> compile (path)
        SequenceDecimal(lambda :: store, path)

module Configuration =
    let Build<'a>() = Object<'a>([])

