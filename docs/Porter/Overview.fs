module App.Porter.Overview

open Elmish.React
open App.Markdown

let view = lazyView loadMd "https://raw.githubusercontent.com/ttak0422/Elf/main/src/Elf.Porter/README.md"