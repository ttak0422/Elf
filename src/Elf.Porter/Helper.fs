namespace Elf.Porter

open Fable.Core

[<AutoOpen>]
module Util =
    /// **Description**
    ///
    /// Used when defining the outgoing function.
    ///
    /// e.g.
    /// ```fsharp
    /// [&#60;Porter("./main.js")&#62;]
    /// let someFunction: int -> unit = jsPorter
    /// ```
    ///
    /// Alias for ``Fable.Core.jsNative``.
    let inline jsPorter<'a> : 'a = jsNative<'a>

[<RequireQualifiedAccessAttribute>]
module internal Event =
    /// **Description**
    ///
    /// create event handler and event.
    let create<'a> (): ('a -> unit) * IEvent<'a> =
        let ev = Event<'a>()
        ev.Trigger, ev.Publish

[<RequireQualifiedAccessAttribute>]
module internal Timer =
    open System.Timers

    let delay interval callback =
        let t =
            new Timer(float interval, AutoReset = false)

        t.Elapsed.Add callback
        t.Enabled <- true
        t.Start()
