namespace Elf.Porter

open Elmish
open Fable.Core

type Porter = ImportMemberAttribute

[<RequireQualifiedAccessAttribute>]
module Porter =

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

    let port (f: 'a -> _) (arg: 'a): Cmd<'msg> =
        let bind (dispatch: Dispatch<'msg>) = async { f arg |> ignore }
        [ bind >> start ]
