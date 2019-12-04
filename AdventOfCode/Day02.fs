module Day2

open Test

type OpCode =
    | Add of int * int * int
    | Multiply of int * int * int
    | Halt

type Computer = {
    memory : int[]
    instructionPointer : int
}

let readInput =
    let rawInput = System.IO.File.ReadAllText "day-2-input"
    rawInput.Split([|','|])
    |> Array.map System.Convert.ToInt32

let private translateInstruction computer =
    let readMemoryOffset computer offset =
        computer.memory.[computer.instructionPointer + offset]

    match readMemoryOffset computer 0 with
    | 1 -> Add (readMemoryOffset computer 1, readMemoryOffset computer 2, readMemoryOffset computer 3)
    | 2 -> Multiply (readMemoryOffset computer 1, readMemoryOffset computer 2, readMemoryOffset computer 3)
    | 99 -> Halt
    | _ -> failwith (sprintf "Bad instruction %d" (readMemoryOffset computer 0))

let rec private executeInstruction computer =
    let executeBinaryInstruction computer operation param1 param2 destination =
        let p1 = computer.memory.[param1]
        let p2 = computer.memory.[param2]
        let result = operation p1 p2
        let memory' = Array.copy computer.memory
        Array.set memory' destination result
        { computer with memory = memory'; instructionPointer = computer.instructionPointer + 4 }

    let instruction = translateInstruction computer

    match instruction with
    | Halt -> computer
    | Add (param1, param2, destination) ->
        let result = executeBinaryInstruction computer (+) param1 param2 destination
        executeInstruction result

    | Multiply (param1, param2, destination) ->
        let result = executeBinaryInstruction computer (*) param1 param2 destination
        executeInstruction result

let executeProgram (memory : int[]) =
    let computer = executeInstruction { memory = memory; instructionPointer = 0 }

    computer.memory

let testDay2Part1 =
    let input1 = [|1; 0; 0; 0; 99|]
    let result1 = executeProgram input1

    result1 |> shouldEqual [|2; 0; 0; 0; 99|]

    let input2 = [|2; 3; 0; 3; 99|]
    let result2 = executeProgram input2

    result2 |> shouldEqual [|2; 3; 0; 6; 99|]

    let input3 = [|2; 4; 4; 5; 99; 0|]
    let result3 = executeProgram input3

    result3 |> shouldEqual [|2; 4; 4; 5; 99; 9801|]

    let input4 = [|1; 1; 1; 4; 99; 5; 6; 0; 99|]
    let result4 = executeProgram input4

    result4 |> shouldEqual [|30; 1; 1; 4; 2; 5; 6; 0; 99|]

let private initializeMemory noun verb (memory : int[]) =
    let memory' = Array.copy memory

    Array.set memory' 1 noun
    Array.set memory' 2 verb

    memory'

let runDay2Part1 =
    let memory = readInput
    let initializedMemory = initializeMemory 12 2 memory

    let result = executeProgram initializedMemory

    result.[0]

let runDay2Part2 =
    let memory = readInput

    seq { 
        for noun in 0..99 do
        for verb in 0..99 do
        yield (100 * noun + verb, (executeProgram (initializeMemory noun verb memory)))
     }
     |> Seq.filter (fun (_, memory) -> memory.[0] = 19690720 )
     |> Seq.map (fun (answer, _) -> answer)
     |> Seq.take 1
     |> Seq.head