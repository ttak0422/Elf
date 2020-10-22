namespace Elf.Porter

open Elmish
open Fable.Core

/// **Description**
///
/// Used when defining the outgoing function.
///
/// e.g.
/// ```fsharp
/// [&#60;Porter("./main.js")&#62;]
/// let someFunction: int -> unit = jsPorter
/// ```
///
/// Alias for ``Fable.Core.ImportMember``
type Porter = ImportMemberAttribute

[<RequireQualifiedAccessAttribute>]
module Porter =

    /// **Desciption**
    ///
    /// Used when defining the incoming function.
    ///
    /// **Parameters**
    /// - `toMsg` : The function that receives a value and converts it into a msg.
    ///
    /// e.g.
    /// ```fsharp
    /// // 1st function is for JavaScript,
    /// // 2nd function is for F#.
    /// let someFunction, someFunctionSub = Porter.create SomeMsg
    /// ```
    let create (toMsg: 'a -> 'msg): ('a -> unit) * Sub<'msg> =
        let (handler, event) = Event.create<'a> ()
        handler,
        (fun dispatch ->
            event
            |> Observable.subscribe (toMsg >> dispatch)
            |> ignore)

[<RequireQualifiedAccessAttribute>]
module Cmd =
    let private start (x: Async<unit>): unit =
        Timer.delay 0 (fun _ -> Async.StartImmediate x)

    /// **Description**
    ///
    /// Run outgoing function asynchronously.
    ///
    /// **Parameters**
    /// - `f`: outgoing function.
    /// - `arg`: argment.
    let port (f: 'a -> _) (arg: 'a): Cmd<'msg> =
        let bind (dispatch: Dispatch<'msg>) = async { f arg |> ignore }
        [ bind >> start ]
