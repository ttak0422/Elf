# Elf.Porter [![NuGet version](https://badge.fury.io/nu/Elf.Porter.svg)](https://badge.fury.io/nu/Elf.Porter)

A library inspired by Elm ports.

This is to simplify the interaction between JavaScript and F#.

```fsharp
(* F# Code *)

// ...

type Msg 
    = GetText of string
    | ShowPrompt

[<Porter("/main.js")>]
let inputText: unit -> unit = jsPorter

let getText, getTextSub = Porter.create GetText

let update (msg: Msg) (state: State) =
    match msg with
    | ShowPrompt -> state, Cmd.port inputText ()
    | GetText text -> { state with Input = text }, Cmd.none

let subscribe (state: State) = 
    getTextSub
    |> Cmd.ofSub

/// ...
```

```javascript
/* JavaScript Code */
import { getText } from "./Sample.fs";
export { inputText }

function inputText() {
    const text = prompt("input: ");
    getText(text);
}
```