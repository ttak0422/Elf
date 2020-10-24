<div align="center">
<h1> Elf.Porter</h1>
<p>
A library inspired by Elm ports.
This is to simplify the interaction between JavaScript and F#.
</p>
<a href="https://www.nuget.org/packages/Elf.Porter">
<img src="https://img.shields.io/nuget/v/Elf.Porter?style=for-the-badge">
</a>
</div>

## Type Table

<table>
    <tr>
        <th align="center">F#</th>
        <th align="center">JavaScript</th>
    </tr>
    <tr>
        <td align="center">Number ( int, float, ... )</td>
        <td align="center">number</td>
    </tr>
    <tr>
        <td align="center">string</td>
        <td align="center">string</td>
    </tr>
    <tr>
        <td align="center">bool</td>
        <td align="center">boolean</td>
    </tr>
    <tr>
        <td align="center">DateTime</td>
        <td align="center">Date</td>
    </tr>
    <tr>
        <td align="center">a' option ( int option, ... )</td>
        <td align="center">T | null ( number | null, ... )</td>
    </tr>
    <tr>
        <td align="center">array</td>
        <td align="center">Array</td>
    </tr>
    <tr>
        <td align="center">Record</td>
        <td align="center">Object</td>
    </tr>
</table>

## Usage

```fsharp
(* F# Code [/sample/Elf.Porter.Sample/App.fs] *)

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
```

```javascript
/* JavaScript Code [/sample/Elf.Porter.Sample/app.js] */
import { getText } from "./App.fs";

export function inputText() {
    const text = prompt("input: ");
    getText(text);
}
```