// Learn more about F# at http://fsharp.org

open System
open Day1

[<EntryPoint>]
let main argv =
    
    testFuelCalculations |> ignore

    let day1a = calculateTotalFuel

    printfn "Day 1 - part 1 >> %d" day1a

    testDay1Part2 |> ignore

    let day1b = calculateDay1Part2

    printfn "Day 1 - part 2 >> %d" day1b

    0 // return an integer exit code
