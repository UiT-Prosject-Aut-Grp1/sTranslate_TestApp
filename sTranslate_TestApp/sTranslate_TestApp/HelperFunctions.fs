module HelperFunctions
open System

let addToList (myList:List<'a>) element = element::myList

let printRemainingTime (pctComplete : float) (elapsedTime : System.TimeSpan) =
    let remainingRatio = 100.0/pctComplete
    let totalProjectedTime = System.TimeSpan.FromTicks(int64(float(elapsedTime.Ticks)*remainingRatio))
    let remainingTime = totalProjectedTime - elapsedTime
    printf " time remaining: %A" remainingTime
    ()