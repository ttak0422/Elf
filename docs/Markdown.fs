module App.Markdown 

open Elmish
open Fable.Core.JsInterop
open Fable.SimpleHttp
open Feliz
open Feliz.Markdown
open Feliz.Bulma
open Feliz.ElmishComponents

type HighLight = 
    static member inline highLight (props: IReactProperty list): ReactElement = 
        Interop.reactApi.createElement (importDefault "react-highlight", createObj !! props)

type State 
    = Initial 
    | Loading 
    | Failed of string 
    | Loaded of string 

type Msg 
    = StartLoading of path: string 
    | Loaded of Result<string, int * string>

let init (url: string) = Initial, Cmd.ofMsg (StartLoading url)

let update (msg: Msg) (state: State) =
    match msg with 
    | StartLoading url ->
        let loadMdAsync () =
            async {
                let! (statusCode, response) = Http.get url 
                if statusCode = 200 
                then return Loaded(Ok response)
                else return Loaded(Error (statusCode, response))
            }
        Loading, Cmd.OfAsync.perform loadMdAsync () id 
    | Loaded (Ok content) -> State.Loaded content, Cmd.none 
    | Loaded (Error (statusCode, err)) -> State.Loaded (sprintf "%s." err), Cmd.none

let renderMd (content: string) = 
    Bulma.container 
        [ prop.children 
            [ Markdown.markdown 
                [ markdown.source content 
                ; markdown.escapeHtml false 
                ; markdown.renderers
                    [ markdown.renderers.code (fun props -> 
                        Html.div 
                            [ HighLight.highLight 
                                [ prop.className "fsharp"
                                ; prop.text (props.value)
                                ]
                            ])
                    ]
                ]
            ]
        ]

let renderErr (err: string) =
    Html.h1 
        [ prop.style [ style.color.crimson ]
        ; prop.text err 
        ]

let view (state: State) (dispatch: Dispatch<Msg>) = 
    match state with 
    | State.Initial -> Html.none 
    | State.Loading -> Html.div [ prop.text "..." ]
    | State.Loaded content -> renderMd content 
    | State.Failed err -> renderErr err 

let loadMd (url: string) = React.elmishComponent ("LoadMarkdown", init url, update, view, key = url)