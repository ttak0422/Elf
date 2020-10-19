module App.Porter.Receiving

open System
open Elmish
open Feliz
open Feliz.Bulma
open Elf.Porter

// If you only need 'Receiving' from your JavaScript file,
// you need to give a hint that fable compiler needs to 
// import the file in the following way.
Fable.Core.JsInterop.importAll "./receiving.js"

type State = { Time: DateTime option }

type Msg = Tick of DateTime

let init () = { Time = None }, Cmd.none

let tick, tickSub = Porter.create Tick

let update (msg: Msg) (state: State) =
    match msg with
    | Tick t -> { Time = Some t }, Cmd.none

let subscribe (state: State) = 
    tickSub
    |> Cmd.ofSub

let view (state: State) (dispatch: Dispatch<Msg>) = 
    Html.div 
        [ match state.Time with 
          | None -> Bulma.title.h2 [ prop.text "..." ]
          | Some t -> Bulma.title.h2 [ prop.text (string t) ]
        ]
