module Day4

open Test
//
// Assumes 6-digit number
//
let generate2DigitGroups input =
    [
        yield (input / 10000)

        let mask1 = (input / 100000 ) * 100000
        yield ((input - mask1) / 1000)

        let mask3 = (input / 10000) * 10000
        yield ((input - mask3) / 100)

        let mask4 = (input / 1000) * 1000
        yield ((input - mask4) / 10)

        let mask5 = (input / 100) * 100
        yield ((input - mask5))
    ]

let consecutiveRepeatedDigitRule input =
    let countOfRepeatedGroups = generate2DigitGroups input
                                |> List.filter (fun g -> g % 11 = 0)
                                |> List.length
    countOfRepeatedGroups > 0

let leftToRightDigitIncreasingRule input =
    let digit6 = input / 100000
    let digit5 = (input - digit6 * 100000) / 10000
    let digit4 = (input - ((input / 10000) * 10000)) / 1000
    let digit3 = (input - ((input / 1000) * 1000)) / 100
    let digit2 = (input - ((input / 100) * 100)) / 10
    let digit1 = (input - ((input / 10) * 10))

    digit6 <= digit5 && digit5 <= digit4 && digit4 <= digit3 && digit3 <= digit2 && digit2 <= digit1

let evaluateInput input =
    consecutiveRepeatedDigitRule input && leftToRightDigitIncreasingRule input

let testDay4Part1 =
    evaluateInput 112345 |> shouldEqualWithLabel "Pt. 1 - 112345" true
    evaluateInput 111123 |> shouldEqualWithLabel "Pt. 1 - 111123" true
    evaluateInput 135679 |> shouldEqualWithLabel "Pt. 1 - 135679" false
    evaluateInput 111111 |> shouldEqualWithLabel "Pt. 1 - 111111" true
    evaluateInput 223450 |> shouldEqualWithLabel "Pt. 1 - 223450" false
    evaluateInput 123789 |> shouldEqualWithLabel "Pt. 1 - 123789" false

let day4Part1 =
    [307237..769058]
    |> List.map (fun n -> (n, evaluateInput n))
    |> List.filter (fun (_, isCandidate) -> isCandidate)
    |> List.length

let generate3DigitGroups input =
    [
        yield input / 1000

        let mask1 = (input / 100000) * 100000
        yield ((input - mask1) / 100)

        let mask2 = (input / 10000) * 10000
        yield ((input - mask2) / 10)

        let mask3 = (input / 1000) * 1000
        yield input - mask3
    ]

let consecutiveRepeatedDigitsLimitedToTwoRule input =
    let repeatedDigits = generate2DigitGroups input |> List.filter (fun g -> g % 11 = 0)

    let threeDigitRepeats = generate3DigitGroups input |> List.filter (fun g -> g % 111 = 0)

    let unique2Digits = repeatedDigits
                        |> List.filter (fun g -> not (List.contains (g * 10 + g / 10) threeDigitRepeats))
                        |> List.length

    threeDigitRepeats |> List.isEmpty || unique2Digits > 0

let evaluatePart2Input input =
    consecutiveRepeatedDigitRule input && leftToRightDigitIncreasingRule input && consecutiveRepeatedDigitsLimitedToTwoRule input

let testDay4Part2 =
    evaluatePart2Input 112345 |> shouldEqualWithLabel "Pt. 2 - 112345" true
    evaluatePart2Input 111123 |> shouldEqualWithLabel "Pt. 2 - 111123" false
    evaluatePart2Input 135679 |> shouldEqualWithLabel "Pt. 2 - 135679" false
    evaluatePart2Input 111111 |> shouldEqualWithLabel "Pt. 2 - 111111" false
    evaluatePart2Input 223450 |> shouldEqualWithLabel "Pt. 2 - 223450" false
    evaluatePart2Input 123789 |> shouldEqualWithLabel "Pt. 2 - 123789" false
    evaluatePart2Input 111122 |> shouldEqualWithLabel "Pt. 2 - 111122" true
    evaluatePart2Input 112233 |> shouldEqualWithLabel "Pt. 2 - 112233" true
    evaluatePart2Input 123444 |> shouldEqualWithLabel "Pt. 2 - 123444" false
    evaluatePart2Input 333444 |> shouldEqualWithLabel "Pt. 2 - 333444" false

let day4Part2 =
    let x = [307237..769058]
            |> List.map (fun n -> (n, evaluatePart2Input n))
            |> List.filter (fun (_, isCandidate) -> isCandidate)

    x |> List.length
