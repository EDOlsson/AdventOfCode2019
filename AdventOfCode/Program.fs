// Learn more about F# at http://fsharp.org

open System
open Day1
open Day2

[<EntryPoint>]
let main argv =
    
    // testFuelCalculations |> ignore

    // let day1a = calculateTotalFuel

    // printfn "Day 1 - part 1 >> %d" day1a

    // testDay1Part2 |> ignore

    // let day1b = calculateDay1Part2

    // printfn "Day 1 - part 2 >> %d" day1b

    testDay2Part1 |> ignore

    let day2a = runDay2Part1
    printfn "Day 2 - part 1 >> %d" day2a

    let day2b = runDay2Part2
    printfn "Day 2 - part 2 >> %d" day2b

    0 // return an integer exit code
