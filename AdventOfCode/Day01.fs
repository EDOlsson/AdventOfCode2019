module Day1

open Test

let private calculateFuel weight =
    (weight / 3 ) - 2

let testFuelCalculations =
    let testCase1 = calculateFuel 1969
    let testCase2 = calculateFuel 100756

    testCase1 |> shouldEqual 654
    testCase2 |> shouldEqual 33583

let private calculateTotalFuelForMass calculateFuelForMass masses =
    Array.sumBy calculateFuelForMass masses

let calculateTotalFuel =
    System.IO.File.ReadAllLines "day-1-input"
    |> Array.map System.Convert.ToInt32
    |> calculateTotalFuelForMass calculateFuel

let private calculateFuelForMass mass =
    let rec calc' currentFuel newMass =
        let fuelForMass = calculateFuel newMass
        if fuelForMass <= 0 then currentFuel else calc' (currentFuel + fuelForMass) fuelForMass

    calc' 0 mass

let testDay1Part2 =
    let testCase1 = calculateFuelForMass 1969
    let testCase2 = calculateFuelForMass 100756

    testCase1 |> shouldEqual 966
    testCase2 |> shouldEqual 50346

let calculateDay1Part2 =
    System.IO.File.ReadAllLines "day-1-input"
    |> Array.map System.Convert.ToInt32
    |> calculateTotalFuelForMass calculateFuelForMass