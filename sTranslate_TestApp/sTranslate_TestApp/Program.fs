open System
open System.IO
open System.Text
open FSharpFunctions
open CSharpFunctions

let logFileCSharpNoCache = __SOURCE_DIRECTORY__ + @"\logCSharpNoCache.csv"
let logFileCSharpCache = __SOURCE_DIRECTORY__ + @"\logCSharpWithCache.csv"
let logFileFSharpDirectNoCache = __SOURCE_DIRECTORY__ + @"\logFSharpDirectNoCache.csv"
let logFileFSharpDirectCache = __SOURCE_DIRECTORY__ + @"\logFSharpDirectWithCache.csv"
let logFileFSharpNoCache = __SOURCE_DIRECTORY__ + @"\logFSharpNoCache.csv"
let logFileFSharpCache = __SOURCE_DIRECTORY__ + @"\logFSharpWithCache.csv"

let testFile = __SOURCE_DIRECTORY__ + @"\StressTest.csv"

[<EntryPoint>]
let main argv = 
    
    // Call the stresstest
    let numLoops = 100
    let fileName = testFile    
    
//    let (searchCounter,elapsedTime,loopTimes) = StressTestCsGetToText fileName numLoops
//    let logFile = logFileCSharpNoCache
    let (searchCounter,elapsedTime,loopTimes) = StressTestCsToText fileName numLoops
    let logFile = logFileCSharpCache
//    let (searchCounter,elapsedTime,loopTimes) = StressTest sTranslate_direct.XltTool.GetToText sTranslate_direct.XltEnums.ToPropertyType sTranslate_direct.XltEnums.ToCriteria fileName numLoops
//    let logFile = logFileFSharpDirectNoCache
//    let (searchCounter,elapsedTime,loopTimes) = StressTest sTranslate_direct.XltTool.ToText sTranslate_direct.XltEnums.ToPropertyType sTranslate_direct.XltEnums.ToCriteria fileName numLoops
//    let logFile = logFileFSharpDirectCache
//    let (searchCounter,elapsedTime,loopTimes) = StressTest sTranslate_fs.XltTool.GetToText sTranslate_fs.XltEnums.ToPropertyType sTranslate_fs.XltEnums.ToCriteria fileName numLoops
//    let logFile = logFileFSharpNoCache
//    let (searchCounter,elapsedTime,loopTimes) = StressTest sTranslate_fs.XltTool.ToText sTranslate_fs.XltEnums.ToPropertyType sTranslate_fs.XltEnums.ToCriteria fileName numLoops
//    let logFile = logFileFSharpCache

//    let fromText = "Edit"
//    let context = "lblTitle"
//    let property = sTranslate_fs.XltEnums.ToPropertyType "Id"
//    let criteria = sTranslate_fs.XltEnums.ToCriteria "Contains"
//    let toLang = "no"
//    let toText = sTranslate_fs.XltTool.ToText criteria fromText property context toLang
//    printfn "%s" toText

    printfn "Duration: %A sec" elapsedTime
    
    let outFile = new StreamWriter(logFile)
    let dataFrame = loopTimes
                    |> Seq.iter (fun y -> outFile.WriteLine(y.ToString().Split([|':'|]).[2]))
    outFile.Close() |> ignore
    // Keypress to close terminal
    System.Console.ReadKey() |> ignore

    0 // return an integer exit code