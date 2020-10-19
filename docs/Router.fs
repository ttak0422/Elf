module App.Router

open Feliz.Router

[<RequireQualifiedAccessAttribute>]
type Url 
    = Index
    | Porter
    | PorterSample
    | PorterSending
    | PorterReceiving

[<RequireQualifiedAccessAttribute>]
type Page 
    = Index 
    | Porter
    | PorterSample
    | PorterSending
    | PorterReceiving

let parseUrl =
    function
    | [ "" ] -> Url.Index
    | [ "porter" ] -> Url.Porter
    | [ "porter"; "sample" ] -> Url.PorterSample
    | [ "porter"; "sending" ] -> Url.PorterSending
    | [ "porter"; "receiving" ] -> Url.PorterReceiving
    | _ -> Url.Index

let toHref =
    function
    | Url.Index -> Router.format ("")
    | Url.Porter -> Router.format ("porter")
    | Url.PorterSample -> Router.format ("porter", "sample")
    | Url.PorterSending -> Router.format ("porter", "sending")
    | Url.PorterReceiving -> Router.format ("porter", "receiving")
