namespace rec FuncTestingFramework.ObjectExtensions
//open FuncTestingFramework.ExpressionBuilder
//open System
//open System
//open System
//open System.Collections
//open System.Collections.Generic
//open System.Linq.Expressions
//
//type IntItemConfiguration<'a> = Func<Int<'a>, Int<'a>>
//type DateItemConfiguration<'a> = Func<Date<'a>, Date<'a>>
//type DecimalItemConfiguration<'a> = Func<DecimalT<'a>, DecimalT<'a>>
//type ObjectItemConfiguration<'a> = Func<Object<'a>, Object<'a>>
//type BoolItemConfiguration<'a> = Func<Bool<'a>, Bool<'a>>
//
//type Sequence<'a, 't>(store: Storage<'a>, path: Path<'a, IEnumerable<'t>>) =
//    inherit Object<'a>(store)
//     member __.UseValue(value: IEnumerable<'t>) =
//        Sequence(((ObjectExpressions.useValue path value) :: store), path)
//
//     member __.Ignore() =
//        Sequence(((ObjectExpressions.ignore path) :: store), path)
//
//     member __.ForItem(conf: IntItemConfiguration<'a>) =
//        let valueTupleType = typeof<ValueTuple<int, int []>>
//        let tupleParameter = Expression.Parameter(valueTupleType)
//        let item1 = Expression.Field(tupleParameter, valueTupleType.GetField("Item1"))
//        let item2 = Expression.Field(tupleParameter, valueTupleType.GetField("Item2"))
//        let arrayAccess = Expression.ArrayAccess(item2, item1)
//        let result = Expression.Lambda<Func<ValueTuple<int, int []>, int>>(arrayAccess, tupleParameter)
//        let store = Int<ValueTuple<int,int[]>>([],result).Storage;
//        0
////
////     member __.Max (Sequance(store, func)) (max: 'a) =
////        Object <| (DateTimeExpressions.Interval func DateTime.MinValue max) :: store
////
////     member __.Interval (Sequance(store, func)) (min: 'a) (max: 'a) =
////        Object <| (DateTimeExpressions.Interval func min max) :: store
//
//
//
//
