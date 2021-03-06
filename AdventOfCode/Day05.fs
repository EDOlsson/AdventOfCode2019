module Day5

type Computer = {
    memory : int[]
    instructionPointer : int
    inputs : int list
    outputs : int list
}

type OpCodeParameterPosition =
    | First
    | Second
    | Third

type OpCodeParameterMode =
    | Position
    | Immediate

type OpCodeParameter = {
    position : OpCodeParameterPosition
    mode : OpCodeParameterMode
    value : int
}

type OpCode =
    | Add of OpCodeParameter * OpCodeParameter * OpCodeParameter
    | Multiply of OpCodeParameter * OpCodeParameter * OpCodeParameter
    | Input of OpCodeParameter
    | Output of OpCodeParameter
    | JumpIfTrue of OpCodeParameter * OpCodeParameter
    | JumpIfFalse of OpCodeParameter * OpCodeParameter
    | LessThan of OpCodeParameter * OpCodeParameter * OpCodeParameter
    | Equals of OpCodeParameter * OpCodeParameter * OpCodeParameter
    | Halt

let readInput =
    let rawInput = System.IO.File.ReadAllText "day-5-input"
    rawInput.Split([|','|])
    |> Array.map System.Convert.ToInt32

let private translateOpCode value =
    let parseParameterMode mode =
        match mode with
        | 0 -> Position
        | 1 -> Immediate
        | _ -> failwith <| sprintf "Invalid parameter mode %d" mode

    let opCodeValue = value - ((value / 100) * 100)
    let parameter1Mode = (value / 100) - ((value / 1000) * 10)
    let parameter2Mode = (value / 1000) - ((value / 10000) * 10)
    let parameter3Mode = (value / 10000) - ((value / 100000) * 10)

    match opCodeValue with
    | 1 -> Add         ({ position = First; mode = parseParameterMode parameter1Mode; value = 0 }, { position = Second; mode = parseParameterMode parameter2Mode; value = 0 }, { position = Third; mode = parseParameterMode parameter3Mode; value = 0 })
    | 2 -> Multiply    ({ position = First; mode = parseParameterMode parameter1Mode; value = 0 }, { position = Second; mode = parseParameterMode parameter2Mode; value = 0 }, { position = Third; mode = parseParameterMode parameter2Mode; value = 0 })
    | 3 -> Input        { position = First; mode = parseParameterMode parameter1Mode; value = 0 }
    | 4 -> Output       { position = First; mode = parseParameterMode parameter1Mode; value = 0 }
    | 5 -> JumpIfTrue  ({ position = First; mode = parseParameterMode parameter1Mode; value = 0 }, { position = Second; mode = parseParameterMode parameter2Mode; value = 0 })
    | 6 -> JumpIfFalse ({ position = First; mode = parseParameterMode parameter1Mode; value = 0 }, { position = Second; mode = parseParameterMode parameter2Mode; value = 0 })
    | 7 -> LessThan    ({ position = First; mode = parseParameterMode parameter1Mode; value = 0 }, { position = Second; mode = parseParameterMode parameter2Mode; value = 0 }, { position = Third; mode = parseParameterMode parameter3Mode; value = 0 })
    | 8 -> Equals      ({ position = First; mode = parseParameterMode parameter1Mode; value = 0 }, { position = Second; mode = parseParameterMode parameter2Mode; value = 0 }, { position = Third; mode = parseParameterMode parameter3Mode; value = 0 })
    | 99 -> Halt
    | _ -> failwith <| sprintf "Unknown opcode %d" opCodeValue

let private translateInstruction computer =
    let readMemoryOffset computer offset =
        computer.memory.[computer.instructionPointer + offset]

    let supplyParameterValue mode offset computer =
        let parameterValue = computer.memory.[computer.instructionPointer + offset]
        match mode with
        | Position -> computer.memory.[parameterValue]
        | Immediate -> parameterValue

    let setOpCodeValue opCode computer =
        match opCode with
        | Add (p1, p2, p3) -> Add ({ p1 with value = supplyParameterValue p1.mode 1 computer }, { p2 with value = supplyParameterValue p2.mode 2 computer }, { p3 with value = readMemoryOffset computer 3 })
        | Multiply (p1, p2, p3) -> Multiply ({ p1 with value = supplyParameterValue p1.mode 1 computer }, { p2 with value = supplyParameterValue p2.mode 2 computer }, { p3 with value = readMemoryOffset computer 3 })
        | Input p -> Input { p with value = readMemoryOffset computer 1 }
        | Output p -> Output { p with value = supplyParameterValue p.mode 1 computer }
        | JumpIfTrue (p1, p2) -> JumpIfTrue ({ p1 with value = supplyParameterValue p1.mode 1 computer }, { p2 with value = supplyParameterValue p2.mode 2 computer })
        | JumpIfFalse (p1, p2) -> JumpIfFalse ({ p1 with value = supplyParameterValue p1.mode 1 computer }, { p2 with value = supplyParameterValue p2.mode 2 computer })
        | LessThan (p1, p2, p3) -> LessThan ({ p1 with value = supplyParameterValue p1.mode 1 computer }, {p2 with value = supplyParameterValue p2.mode 2 computer }, { p3 with value = readMemoryOffset computer 3})
        | Equals (p1, p2, p3) -> Equals ({ p1 with value = supplyParameterValue p1.mode 1 computer }, { p2 with value = supplyParameterValue p2.mode 2 computer }, { p3 with value = readMemoryOffset computer 3 })
        | Halt -> Halt

    let opCodeSansValue = translateOpCode (readMemoryOffset computer 0)

    setOpCodeValue opCodeSansValue computer

let rec private executeInstruction computer =
    let executeBinaryInstruction computer operation param1 param2 destination =
        let result = operation param1 param2
        let memory' = Array.copy computer.memory
        Array.set memory' destination result
        { computer with memory = memory'; instructionPointer = computer.instructionPointer + 4 }

    let executeInput computer param =
        let inputValue = computer.inputs |> List.head
        let memory' = Array.copy computer.memory
        Array.set memory' param inputValue
        { computer with memory = memory'; instructionPointer = computer.instructionPointer + 2; inputs = (computer.inputs |> List.tail) }

    let executeOutput computer param =
        { computer with outputs = (param :: computer.outputs); instructionPointer = computer.instructionPointer + 2 }

    let executeConditionalJump computer param shouldJump =
        if (shouldJump) then { computer with instructionPointer = param } else { computer with instructionPointer = computer.instructionPointer + 3 }

    let executeComparison computer param1 param2 op param3 =
        let setMemory computer location value =
            let memory' = Array.copy computer.memory
            Array.set memory' location value
            { computer with memory = memory' }

        let c' = setMemory computer param3 (if (op param1 param2) then 1 else 0)

        { c' with instructionPointer = computer.instructionPointer + 4 }

    let instruction = translateInstruction computer

    match instruction with
    | Halt -> computer
    | Add (param1, param2, destination) ->
        let result = executeBinaryInstruction computer (+) param1.value param2.value destination.value
        executeInstruction result

    | Multiply (param1, param2, destination) ->
        let result = executeBinaryInstruction computer (*) param1.value param2.value destination.value
        executeInstruction result

    | Input (p) ->
        let result = executeInput computer p.value
        executeInstruction result

    | Output (p) ->
        let result = executeOutput computer p.value
        executeInstruction result

    | JumpIfTrue (p1, p2) ->
        let result = executeConditionalJump computer p2.value (p1.value <> 0)
        executeInstruction result

    | JumpIfFalse (p1, p2) ->
        let result = executeConditionalJump computer p2.value (p1.value = 0)
        executeInstruction result

    | LessThan (p1, p2, p3) ->
        let result = executeComparison computer p1.value p2.value (<) p3.value
        executeInstruction result

    | Equals (p1, p2, p3) ->
        let result = executeComparison computer p1.value p2.value (=) p3.value
        executeInstruction result

let executeProgram (memory : int[]) inputs =
    let computer = executeInstruction { memory = memory; instructionPointer = 0; inputs = inputs; outputs = [] }

    computer

let day5Part1 memory =
    executeProgram memory [1]

let day5Part2 memory =
    executeProgram memory [5]