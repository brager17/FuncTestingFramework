open System
open System.Linq.Expressions

open Microsoft.FSharp.Quotations


type Person(name: string, surname: string) =
    let mutable name = ""
    let mutable surname = ""
    member this.Name with get () = name and set value = name <- value
    member this.Surname = surname

type NestedPerson(p: Person) =
    member this.P = p

let incrementName = <@@
                        Func<_, _>(fun (person: Person) -> person.Name <- "n"; person)
                    @@>

type Value<'a>(v: 'a) =
   let mutable _v = v
   member this.V with get () = _v and set (v) = _v <- v;
let person = Person("name", "surname")
let nested = NestedPerson(person)
(incrementName.CompileUntyped() :?> Delegate).DynamicInvoke(person) :?> Person

let assign<'T> (e:Expression<Func<'T,string>>) (e1:'T) value =
    e.Compile().Invoke(e1) <- value

let x expression  = <@@
                        fun x -> assign expression x (Guid.NewGuid().ToString());x
                    @@>



<@ Func<_,_>(fun (x: Value<decimal>) -> x.V) @>.ToLinqExpressionUntyped() :?> Expression<Func<Value<decimal>,decimal>>






