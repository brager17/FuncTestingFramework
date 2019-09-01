namespace FuncTestingFramework.ObjectExtensions
open System
open System.Collections.Generic
open System.Linq.Expressions
open FuncTestingFramework.generator
open System.Runtime.CompilerServices

type ObjectExtensions() =
    member __.For(Object list, (expression: Expression<Func<'a, string>>)): String<'a> =
        String(list, expression)

    member __.For(Object list, (expression: Expression<Func<'a, int>>)): Int<'a> =
        Int(list, expression)

    member __.For(Object list, (expression: Expression<Func<'a, Boolean>>)): Boolean<'a> =
        Boolean(list, expression)

    member __.For(Object list, (expression: Expression<Func<'a, Decimal>>)): Decimal<'a> =
        Decimal(list, expression)

    member __.For(Object list, (expression: Expression<Func<'a, DateTime>>)): Date<'a> =
        Date(list, expression)

    member __.For(Object list, (expression: Expression<Func<'a, IEnumerable<'b>>>)): Sequance<'a, 'b> =
        Sequance.Sequance(list, expression)
