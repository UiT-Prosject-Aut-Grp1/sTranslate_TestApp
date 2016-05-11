module FSharpFunctions
open System
open System.IO
open System.Text
open HelperFunctions
open sTranslate_parallel.XltTool

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
        let elapsedTime = DateTime.Now.Subtract(startTime) 
        printf "\r%i%% completed, " (int pctComplete)
        printRemainingTime pctComplete elapsedTime
    printfn ""
    
    // Time the entire stresstest
    let elapsedTime = DateTime.Now.Subtract(startTime) 
    (searchCounter,elapsedTime,List.rev loopTimes)

let toSearch (line : string []) = 
    ( { FromText = line.[0]
        Context = line.[1]
        Property = line.[2]
        Criteria = line.[3]
        ToLang = line.[4] } : Search)

let StressTestFsParallel fileName numLoops =
    printfn "Using search data in: %s" fileName
    let startTime = System.DateTime.Now
    // Initialize accumulator variables
    let mutable loopTimes : List<System.TimeSpan> = []
    // Create string array of each line in the .csv file
    let lines = System.IO.File.ReadAllLines(fileName, System.Text.Encoding.GetEncoding("ISO-8859-1"))
    // Make list of searches
    let searchList = lines |> List.ofSeq |> List.map (fun line -> line.Split([|';'|])) |> List.map toSearch
    // Do the .csv search numLoops number of times
    for i in 1 .. numLoops do
        //get the result
        let loopStartTime = System.DateTime.Now
        let resultSeq = sTranslate_parallel.XltTool.GetToTextAsync searchList
        //printfn "%A" resultSeq
        // Time the individual loop
        loopTimes <- addToList loopTimes (System.DateTime.Now.Subtract(loopStartTime))
        // Track completion
        let pctComplete = Math.Floor (float i)/(float numLoops)*100.0
        let elapsedTime = System.DateTime.Now.Subtract(startTime) 
        printf "\r%i%% completed, " (int pctComplete)
        printRemainingTime pctComplete elapsedTime
    printfn ""
    
    // Time the entire stresstest
    let elapsedTime = DateTime.Now.Subtract(startTime) 
    (elapsedTime,List.rev loopTimes)