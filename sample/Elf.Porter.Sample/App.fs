module Elf.Porter.Sample

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Elf.Porter

type Model = { Input: string }

type Msg 
    = GetText of string 
    | ShowPrompt

[<Porter("./app.js")>]
let inputText: unit -> unit = jsPorter

let getText, getTextSub = Porter.create GetText

let init (): Model * Cmd<Msg> =
    { Input = "" }, Cmd.none 

let update (msg: Msg) (model: Model): Model * Cmd<Msg> = 
    match msg with 
    | GetText text -> { model with Input = text }, Cmd.none
    | ShowPrompt -> model, Cmd.port inputText ()

let subscription (model: Model): Cmd<Msg> =
    Cmd.ofSub getTextSub

let view (model: Model) (dispatch: Dispatch<Msg>): ReactElement = 
    div [] 
        [ button 
            [ OnClick(fun _ -> dispatch ShowPrompt) ] 
            [ str "Show Prompt" ]
        ; str model.Input
        ]

Program.mkProgram init update view 
|> Program.withSubscription subscription
|> Program.withReactBatched "app"
|> Program.run
