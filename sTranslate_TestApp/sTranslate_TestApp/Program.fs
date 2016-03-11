open System
open System.IO
open System.Text

let logFileCSharpNoCache = __SOURCE_DIRECTORY__ + @"\logCSharpNoCache.csv"
let logFileCSharpCache = __SOURCE_DIRECTORY__ + @"\logCSharpWithCache.csv"
let logFileFSharpDirectNoCache = __SOURCE_DIRECTORY__ + @"\logFSharpDirectNoCache.csv"
let logFileFSharpDirectCache = __SOURCE_DIRECTORY__ + @"\logFSharpDirectWithCache.csv"
let logFileFSharpNoCache = __SOURCE_DIRECTORY__ + @"\logFSharpNoCache.csv"
let logFileFSharpCache = __SOURCE_DIRECTORY__ + @"\logFSharpCache.csv"

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

let StressTestCsGetToText fileName numLoops = 
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
            let toText = sTranslate.Tools.XltTool.GetToText(criteria, fromText, property, context, toLang)
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
    let numLoops = 100
    let fileName = testFile    
    
    //let (searchCounter,elapsedTime,loopTimes) = StressTestCsGetToText fileName numLoops
    //let (searchCounter,elapsedTime,loopTimes) = StressTestCsToText fileName numLoops
    //let (searchCounter,elapsedTime,loopTimes) = StressTest sTranslate_direct.XltTool.GetToText sTranslate_direct.XltEnums.ToPropertyType sTranslate_direct.XltEnums.ToCriteria fileName numLoops
    //let (searchCounter,elapsedTime,loopTimes) = StressTest sTranslate_direct.XltTool.ToText sTranslate_direct.XltEnums.ToPropertyType sTranslate_direct.XltEnums.ToCriteria fileName numLoops
    //let (searchCounter,elapsedTime,loopTimes) = StressTest sTranslate_fs.XltTool.GetToText sTranslate_fs.XltEnums.ToPropertyType sTranslate_fs.XltEnums.ToCriteria fileName numLoops
    let (searchCounter,elapsedTime,loopTimes) = StressTest sTranslate_fs.XltTool.ToText sTranslate_fs.XltEnums.ToPropertyType sTranslate_fs.XltEnums.ToCriteria fileName numLoops
    
    printfn "Duration: %A sec" elapsedTime
    
    let outFile = new StreamWriter(logFileCSharpNoCache)
    let dataFrame = loopTimes
                    |> Seq.iter (fun y -> outFile.WriteLine(y.ToString().Split([|':'|]).[2]))
    outFile.Close() |> ignore
    // Keypress to close terminal
    System.Console.ReadKey() |> ignore

    0 // return an integer exit code