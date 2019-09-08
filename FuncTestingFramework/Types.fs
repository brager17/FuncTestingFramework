namespace FuncTestingFramework.Types
open System
open System.Linq.Expressions

type Storage<'a> = Quotations.Expr<'a -> 'a> list

type Path<'a, 't> = Expression<Func<'a, 't>>
