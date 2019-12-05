module Day4

open Test
//
// Assumes 6-digit number
//
let consecutiveRepeatedDigitRule input =
    let evaluateDigits5And6 input =
        (input / 10000) % 11 = 0

    let evaluateDigits4And5 input =
        let mask = (input / 100000 ) * 100000
        ((input - mask) / 1000) % 11 = 0

    let evaluateDigits3And4 input =
        let mask = (input / 10000) * 10000
        ((input - mask) / 100) % 11 = 0

    let evaluateDigits2And3 input =
        let mask = (input / 1000) * 1000
        ((input - mask) / 10) % 11 = 0

    let evaluateDigits1And2 input =
        let mask = (input / 100) * 100
        ((input - mask)) % 11 = 0

    evaluateDigits5And6 input || evaluateDigits4And5 input || evaluateDigits3And4 input || evaluateDigits2And3 input || evaluateDigits1And2 input

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
    evaluateInput 112345 |> shouldEqual true
    evaluateInput 111123 |> shouldEqual true
    evaluateInput 135679 |> shouldEqual false
    evaluateInput 111111 |> shouldEqual true
    evaluateInput 223450 |> shouldEqual false
    evaluateInput 123789 |> shouldEqual false

let day4Part1 =
    [307237..769058]
    |> List.map (fun n -> (n, evaluateInput n))
    |> List.filter (fun (_, isCandidate) -> isCandidate)
    |> List.length
