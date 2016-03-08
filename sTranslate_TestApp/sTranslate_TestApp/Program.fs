open System
open System.IO
open System.Text

let logFileCSharp = __SOURCE_DIRECTORY__ + @"\logCSharp.csv"
let logFileFSharp = __SOURCE_DIRECTORY__ + @"\logFSharp.csv"
let logFileFSharpDirect = __SOURCE_DIRECTORY__ + @"\logFSharpDirect.csv"
let testFile = __SOURCE_DIRECTORY__ + @"\StressTest.csv"

let addToList (myList:List<'a>) element = element::myList

let StressTest translateFunction propertyFunction criteriaFunction fileName numLoops = 
    printfn "Using search data in: %s" fileName
    let startTime = DateTime.Now
    // Initialize accumulator variables
    let mutable searchCounter = 0
    let mutable loopTimes : List<TimeSpan> = []
    // Create string array of each line in the .csv file
    let lines = System.IO.File.ReadAllLines(fileName, Encoding.GetEncoding("ISO-8859-1"))
    // Do the .csv search numLoops number of times
    for i in 1 .. numLoops do
        let loopStartTime = DateTime.Now
        for line in lines do
            // Get search criteria from the current line
            let splitLine = line.Split([|';'|])
            let fromText = splitLine.[0]
            let context = splitLine.[1]
            let property = propertyFunction splitLine.[2]
            let criteria = criteriaFunction splitLine.[3]
            let toLang = splitLine.[4]
            // Call the translation function
            let toText = translateFunction criteria fromText property context toLang
            toText |> ignore
            //printfn "EN: %s; NO: %s" fromText toText
            // Track number of individual searches
            searchCounter <- searchCounter+1
        // Time the individual loop
        loopTimes <- addToList loopTimes (DateTime.Now.Subtract(loopStartTime))
        // Track completion
        let pctComplete = Math.Floor (float i)/(float numLoops)*100.0
        printf "\r%i%%" (int pctComplete)
    printfn ""
    // Time the entire stresstest
    let elapsedTime = DateTime.Now.Subtract(startTime) 
    (searchCounter,elapsedTime,List.rev loopTimes)


let StressTestCsToText fileName numLoops = 
    printfn "Using search data in: %s" fileName
    let startTime = DateTime.Now
    // Initialize accumulator variables
    let mutable searchCounter = 0
    let mutable loopTimes : List<TimeSpan> = []
    // Create string array of each line in the .csv file
    let lines = System.IO.File.ReadAllLines(fileName, Encoding.GetEncoding("ISO-8859-1"))
    // Do the .csv search numLoops number of times
    for i in 1 .. numLoops do
        let loopStartTime = DateTime.Now
        for line in lines do
            // Get search criteria from the current line
            let splitLine = line.Split([|';'|])
            let fromText = splitLine.[0]
            let context = splitLine.[1]
            let property = sTranslate.Tools.EnumsXlt.ToPropertyType(splitLine.[2])
            let criteria = sTranslate.Tools.EnumsXlt.ToCriteria(splitLine.[3])
            let toLang = splitLine.[4]
            // Call the translation function
            let toText = sTranslate.Tools.XltTool.ToText(criteria, fromText, property, context, toLang)
            toText |> ignore
            //printfn "EN: %s; NO: %s" fromText toText
            // Track number of individual searches
            searchCounter <- searchCounter+1
        // Time the individual loop
        loopTimes <- addToList loopTimes (DateTime.Now.Subtract(loopStartTime))
        // Track completion
        let pctComplete = Math.Floor (float i)/(float numLoops)*100.0
        printf "\r%i%%" (int pctComplete)
    printfn ""
    // Time the entire stresstest
    let elapsedTime = DateTime.Now.Subtract(startTime) 
    (searchCounter,elapsedTime,List.rev loopTimes)

[<EntryPoint>]
let main argv = 
    
    // Call the stresstest
    let numLoops = 20
    let fileName = testFile
    
    let (searchCounter1,elapsedTime1,loopTimes1) = StressTest sTranslate_fs.XltTool.ToText sTranslate_fs.XltEnums.ToPropertyType sTranslate_fs.XltEnums.ToCriteria fileName numLoops
    let (searchCounter2,elapsedTime2,loopTimes2) = StressTest sTranslate_direct.XltTool.ToText sTranslate_direct.XltEnums.ToPropertyType sTranslate_direct.XltEnums.ToCriteria fileName numLoops
    let (searchCounter3,elapsedTime3,loopTimes3) = StressTestCsToText fileName numLoops
    
    // Print test results
    printfn "Duration: %A sec" elapsedTime1
    printfn "Duration: %A sec" elapsedTime2
    printfn "Duration: %A sec" elapsedTime3
    
    // Saves to file
    let outFile = new StreamWriter(logFileFSharp)
    let dataFrame = loopTimes1
                    |> Seq.iter (fun y -> outFile.WriteLine(y.ToString()))
    outFile.Close() |> ignore
    
    let outFile = new StreamWriter(logFileFSharpDirect)
    let dataFrame = loopTimes2
                    |> Seq.iter (fun y -> outFile.WriteLine(y.ToString()))
    outFile.Close() |> ignore
    
    let outFile = new StreamWriter(logFileCSharp)
    let dataFrame = loopTimes3
                    |> Seq.iter (fun y -> outFile.WriteLine(y.ToString()))
    outFile.Close() |> ignore

    // Keypress to close terminal
    System.Console.ReadKey() |> ignore

    0 // return an integer exit code