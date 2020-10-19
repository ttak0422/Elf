namespace Elf.Porter

open Fable.Core

[<AutoOpen>]
module Util =
    let inline jsPorter<'a> : 'a = jsNative<'a>

[<RequireQualifiedAccessAttribute>]
module Event =
    let create<'a> (): ('a -> unit) * IEvent<'a> =
        let ev = Event<'a>()
        ev.Trigger, ev.Publish

[<RequireQualifiedAccessAttribute>]
module internal Timer =
    open System.Timers
    let delay interval callback =
        let t = new Timer(float interval, AutoReset = false)
        t.Elapsed.Add callback
        t.Enabled <- true
        t.Start()