open System
open System.IO
open System.Net
Environment.CurrentDirectory = __SOURCE_DIRECTORY__
#r "FAKE/tools/FakeLib.dll"
open Fake
open Fake.Testing.XUnit2

let buildDir = "../KataTennis/bin/Debug"
let objDir = "../KataTennis/obj/Debug"

Target "Clean" (fun _ ->
    CleanDirs [objDir; buildDir] 
)

Target "RestorePackages" (fun _ ->
    "../KataTennis/packages.config"
    |> RestorePackage (fun p ->
        { p with
            Retries = 4
            OutputPath = "../packages"
            ToolPath = "nuget.exe" })
)

Target "Build" (fun _ -> 
    let setParams defaults = 
          { defaults with 
              Verbosity = Some(Quiet)
              Targets = ["Build"] }
    build setParams "../KataTennis/KataTennis.fsproj"
          |> DoNothing
)

Target "Test" (fun _ -> 
    !! (buildDir @@ "KataTennis.dll")
    |> xUnit2 (fun p -> p)
)

"Clean"
  ==> "RestorePackages"
  ==> "Build"
  ==> "Test"

Run "Test"


