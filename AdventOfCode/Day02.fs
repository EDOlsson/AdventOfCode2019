module Day2

open Test

type Instruction =
    | Add
    | Multiply
    | Halt

let readInput =
    let rawInput = System.IO.File.ReadAllText "day-2-input"
    rawInput.Split([|','|])
    |> Array.map System.Convert.ToInt32

let private translateInstruction opCode =
    match opCode with
    | 1 -> Add
    | 2 -> Multiply
    | 99 -> Halt
    | _ -> failwith (sprintf "Bad instruction %d" opCode)

let private executeInstruction memory operands instruction =
    let add' (memory : int[]) (operands : int[]) =
        let addend1 = memory.[operands.[1]]
        let addend2 = memory.[operands.[2]]
        Array.set memory operands.[3] (addend1 + addend2)

    let multiply' (memory : int[]) (operands : int[]) =
        let multiplier = memory.[operands.[1]]
        let multiplicand = memory.[operands.[2]]
        Array.set memory operands.[3] (multiplier * multiplicand)

    match instruction with
    | Halt -> None
    | Add ->
        add' memory operands
        Some 1
    | Multiply ->
        multiply' memory operands
        Some 1

let executeProgram (memory : int[]) =
    let mutable result = Some 42
    let mutable strideIndex = 0
    while result.IsSome do
        let opCode = memory.[4 * strideIndex]
        let instruction = translateInstruction opCode

        let operands = memory.[4 * strideIndex..]

        result <- executeInstruction memory operands instruction

        strideIndex <- strideIndex + 1

let testDay2Part1 =
    let input1 = [|1; 0; 0; 0; 99|]
    executeProgram input1

    input1 |> shouldEqual [|2; 0; 0; 0; 99|]

    let input2 = [|2; 3; 0; 3; 99|]
    executeProgram input2

    input2 |> shouldEqual [|2; 3; 0; 6; 99|]

    let input3 = [|2; 4; 4; 5; 99; 0|]
    executeProgram input3

    input3 |> shouldEqual [|2; 4; 4; 5; 99; 9801|]

    let input4 = [|1; 1; 1; 4; 99; 5; 6; 0; 99|]
    executeProgram input4

    input4 |> shouldEqual [|30; 1; 1; 4; 2; 5; 6; 0; 99|]

let runDay2Part1 =
    let memory = readInput
    Array.set memory 1 12
    Array.set memory 2 2

    executeProgram memory

    memory.[0]