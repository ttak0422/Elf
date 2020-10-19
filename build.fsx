#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake
open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators

let publish path = 
    !! (path </> "bin")
    ++ (path </> "obj")
    |> Shell.cleanDirs
    DotNet.restore (fun setParams -> { setParams with NoCache = true }) path 
    DotNet.pack (fun setParams -> 
        { setParams with 
            Configuration = DotNet.Release 
            Common = { setParams.Common with DotNetCliPath = "dotnet" }}) path
    let nugetKey = 
        match Environment.environVarOrNone "NUGET_KEY" with
        | Some k -> k 
        | _ -> failwith "The Nuget API key must be set in a NUGET_KEY environment variable."
    Directory.tryFindFirstMatchingFile ("*.nupkg") (path </> "bin" </> "Release")
    |> function 
    | Some nupkg -> 
        DotNet.nugetPush (fun setParams -> 
            { setParams with 
                PushParams = { 
                    setParams.PushParams with 
                        ApiKey = Some nugetKey
                        Source = Some "nuget.org"
                }
            }) nupkg
    | _ -> failwith "nupkg not found."

Target.initEnvironment ()

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    |> Shell.cleanDirs 
)

Target.create "Build" (fun _ ->
    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build id)
)

Target.create "PublishPorter" (fun _ -> 
    publish "./src/Elf.Porter"
)

Target.create "All" ignore

"Clean"
  ==> "Build"
  ==> "All"

Target.runOrDefault "All"
