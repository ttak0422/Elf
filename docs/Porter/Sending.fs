module App.Porter.Sending

open Elmish
open Feliz
open Feliz.Bulma
open Elf.Porter

type State = { Input: string }

type Msg
    = OnInput of string
    | Send

let init () = { Input = "" }, Cmd.none

[<Porter("./sending.js")>]
let jsAlert: string -> unit = jsPorter

let update (msg: Msg) (state: State) =
    match msg with
    | OnInput input -> { state with Input = input }, Cmd.none
    | Send -> { state with Input = "" }, Cmd.port jsAlert state.Input

let view (state: State) (dispatch: Dispatch<Msg>) =
    Html.div 
        [ prop.children 
            [ Bulma.field.div 
                [ Bulma.label 
                    [ prop.text "Text" 
                    ]
                ; Bulma.control.div 
                    [ Bulma.input.text 
                        [ prop.valueOrDefault state.Input
                        ; prop.onChange (OnInput >> dispatch) 
                        ]
                    ] 
                ]
            ; Bulma.field.div 
                [ Bulma.control.div 
                    [ Bulma.button.button 
                        [ Bulma.color.isPrimary
                        ; prop.text "Send"
                        ; prop.disabled (state.Input = "")
                        ; prop.onClick (fun _ -> dispatch Send) 
                        ] 
                    ] 
                ]
            ] 
        ]
