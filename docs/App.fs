module App

open Fable
open Feliz
open Feliz.Router
open Feliz.Bulma
open Feliz.Markdown
open App.Router
open Elmish
open Elmish.React
open App

type State =
    { CurrentUrl: Url
    ; CurrentPage: Page
    ; PorterSample: Porter.Sample.State
    ; PorterSending: Porter.Sending.State
    ; PorterReceiving: Porter.Receiving.State 
    }

type Msg =
    | UrlChanged of Url
    | PorterSampleMsg of Porter.Sample.Msg
    | PorterSendingMsg of Porter.Sending.Msg
    | PorterReceivingMsg of Porter.Receiving.Msg

let getPage = 
    function 
    | Url.Index -> Page.Index
    | Url.Porter ->  Page.Porter
    | Url.PorterSample -> Page.PorterSample
    | Url.PorterSending -> Page.PorterSending
    | Url.PorterReceiving -> Page.PorterReceiving

let init () =
    let url = parseUrl (Router.currentUrl ())
    let page = getPage url
    let porterSampleState, porterSampleCmd = Porter.Sample.init ()
    let porterSendingState, porterSendingCmd = Porter.Sending.init ()
    let porterReceivingState, porterReceivingCmd = Porter.Receiving.init ()
    { CurrentUrl = url
    ; CurrentPage = page
    ; PorterSample = porterSampleState
    ; PorterSending = porterSendingState
    ; PorterReceiving = porterReceivingState 
    },
    Cmd.batch 
        [ Cmd.map PorterSampleMsg porterSampleCmd
        ; Cmd.map PorterSendingMsg porterSendingCmd
        ; Cmd.map PorterReceivingMsg porterReceivingCmd 
        ]

let update (msg: Msg) (state: State) =

    match msg, state.CurrentPage with
    | PorterSampleMsg lMsg, _ -> 
        let lState', lCmd = Porter.Sample.update lMsg state.PorterSample
        { state with PorterSample = lState' }, Cmd.map PorterSampleMsg lCmd
    | PorterSendingMsg lMsg, _ ->
        let lState', lCmd = Porter.Sending.update lMsg state.PorterSending
        { state with PorterSending = lState' }, Cmd.map PorterSendingMsg lCmd
    | PorterReceivingMsg lMsg, _ ->
        let lState', lCmd = Porter.Receiving.update lMsg state.PorterReceiving
        { state with PorterReceiving = lState' }, Cmd.map PorterReceivingMsg lCmd
    | UrlChanged url, _ ->
        let f page =
            { state with
                CurrentPage = page
                CurrentUrl = url 
            }
        let page = getPage url 
        f page, Cmd.none 

let subscription (state: State) =
    Cmd.batch 
        [ Cmd.map PorterSampleMsg (Porter.Sample.subscribe state.PorterSample)
        ; Cmd.map PorterReceivingMsg (Porter.Receiving.subscribe state.PorterReceiving) 
        ]

let menu (currentPage: Url) =
    let item (text: string) (url: Url) =
        Bulma.menuItem.a 
            [ prop.text text
            ; prop.href (toHref url)
            ; if currentPage = url then 
                helpers.isActive 
                color.hasBackgroundPrimary 
            ]
            
    Bulma.menu 
        [ Bulma.menuLabel "Elf"
        ; Bulma.menuList [ item "Overview" Url.Index ]
        ; Bulma.menuLabel "Elf.Porter"
        ; Bulma.menuList 
            [ item "Obserview" Url.Porter
            ; item "Sample" Url.PorterSample
            ; item "Sending" Url.PorterSending
            ; item "Receiving" Url.PorterReceiving 
            ] 
        ]


let view (state: State) (dispatch: Dispatch<Msg>) =
    let nav = 
        Bulma.navbar 
            [ color.isDark 
            ; prop.children 
                [ Bulma.container 

                    [ Bulma.navbarBrand.div 
                        [ Bulma.navbarItem.a
                            [ prop.text "Elf" 
                            ; prop.href (toHref Url.Index)
                            ; Bulma.navbarItem.isHoverable
                            ]
                        ]
                    ]
                ]
            ]

    let pageContent =
        function
        | Page.Index -> lazyView Markdown.loadMd "https://raw.githubusercontent.com/ttak0422/Elf/main/README.md"
        | Page.Porter -> Porter.Overview.view
        | Page.PorterSample -> Porter.Sample.view state.PorterSample (PorterSampleMsg >> dispatch)
        | Page.PorterSending -> Porter.Sending.view state.PorterSending (PorterSendingMsg >> dispatch)
        | Page.PorterReceiving -> Porter.Receiving.view state.PorterReceiving (PorterReceivingMsg >> dispatch)

    let content =
        
        Bulma.container 
            [ Bulma.section 
                [ Bulma.tile 
                    [ tile.isAncestor
                    ; prop.children 
                        [ Bulma.tile 
                            [ tile.is2
                            ; prop.children (menu state.CurrentUrl) 
                            ]
                        ; Bulma.tile (pageContent state.CurrentPage) 
                        ] 
                    ] 
                ] 
            ]

    React.router 
        [ router.onUrlChanged (parseUrl >> UrlChanged >> dispatch)
        ; router.children [ nav; content ] 
        ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
|> Program.withSubscription subscription
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
