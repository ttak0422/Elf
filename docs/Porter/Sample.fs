module App.Porter.Sample

open Elmish
open Feliz
open Feliz.Bulma
open Elf.Porter

type State = { Input: string }

type Msg
    = GetText of string 
    | ShowPrompt

let init () = { Input = "" }, Cmd.none

[<Porter("./sample.js")>]
let inputText: unit -> unit = jsPorter

let getText, getTextSub = Porter.create GetText

let update (msg: Msg) (state: State) =
    match msg with
    | ShowPrompt -> state, Cmd.port inputText ()
    | GetText text -> { state with Input = text }, Cmd.none

let subscribe (state: State) = 
    getTextSub
    |> Cmd.ofSub

let view (state: State) (dispatch: Dispatch<Msg>) = 
    Html.div [
        prop.children 
            [ Bulma.title.h2  
                    [ prop.text state.Input
                    ]
            ; Bulma.field.div 
                [ Bulma.control.div 
                    [ Bulma.button.button 
                        [ Bulma.color.isPrimary
                        ; prop.text "Show Proompt"
                        ; prop.onClick (fun _ -> dispatch ShowPrompt)
                    ]
                ]
            ]
        ]
    ]