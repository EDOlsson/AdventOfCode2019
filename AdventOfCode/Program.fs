// Learn more about F# at http://fsharp.org

open System
open Day1
open Day2
open Day3
open Day4
open Day5

[<EntryPoint>]
let main argv =
    
    //
    // Day 1
    //
    // testFuelCalculations |> ignore

    // let day1a = calculateTotalFuel

    // printfn "Day 1 - part 1 >> %d" day1a

    // testDay1Part2 |> ignore

    // let day1b = calculateDay1Part2

    // printfn "Day 1 - part 2 >> %d" day1b

    //
    // Day 2
    //
    // testDay2Part1 |> ignore

    // let day2a = runDay2Part1
    // printfn "Day 2 - part 1 >> %d" day2a

    // let day2b = runDay2Part2
    // printfn "Day 2 - part 2 >> %d" day2b

    //
    // Day 3
    //
    // testDay3Part1 |> ignore
    // let day3a = day3Part1

    // printfn "Day 3 - part 1 >> %d" day3a

    // testDay3Part2 |> ignore
    // printfn "Day 3 - part 2 >> %d" <| day3Part2

    //
    // Day 4
    //
    // testDay4Part1 |> ignore
    // printfn "Day 4 - part 1 >> %d" <| day4Part1

    // testDay4Part2 |> ignore
    // printfn "Day 4 - part 2 >> %d" <| day4Part2

    //
    // Day 5
    //
    let day5Computer = day5Part1
    printfn "Day 5 - part 1 >> %d" <| (day5Computer.outputs |> List.head)

    0 // return an integer exit code
