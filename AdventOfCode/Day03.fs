module Day3

open Test

type Point = {
    x : int
    y : int
}

type Direction =
    | Up
    | Down
    | Left
    | Right

type WireSegment = {
    direction : Direction
    count : int
}

let generatePointsFromWireSegment start segment =
    match segment.direction with
    | Up -> [| for dy in 1..segment.count do yield { x = start.x; y = start.y + dy } |]
    | Down -> [| for dy in 1..segment.count do yield { x = start.x; y = start.y - dy } |]
    | Left -> [| for dx in 1..segment.count do yield { x = start.x - dx; y = start.y } |]
    | Right -> [| for dx in 1..segment.count do yield { x = start.x + dx; y = start.y } |]

let generatePointsForWirePath wirePath =
    let folder (points, start) segment =
        let pointsForSegment = generatePointsFromWireSegment start segment

        (Array.append points pointsForSegment, Array.last pointsForSegment)

    fst (Array.fold folder (Array.empty, { x = 0; y = 0 }) wirePath)

let parseWirePath (input : string) =
    let parseSegment (instruction : string) =
        match instruction.ToUpperInvariant().[0] with
        | 'U' -> { direction = Up; count = System.Convert.ToInt32(instruction.Substring(1)) }
        | 'D' -> { direction = Down; count = System.Convert.ToInt32(instruction.Substring(1)) }
        | 'L' -> { direction = Left; count = System.Convert.ToInt32(instruction.Substring(1)) }
        | 'R' -> { direction = Right; count = System.Convert.ToInt32(instruction.Substring(1)) }
        | _ -> failwith (sprintf "Invalid input : %s" instruction)

    input.Split([|','|])
    |> Array.map parseSegment

let computeManhattanDistance point =
    abs point.x + abs point.y

let computeOverallSteps point pointsForWirePath1 pointsForWirePath2 =
    let wire1Steps = pointsForWirePath1
                     |> Array.findIndex (fun p -> p = point)

    let wire2Steps = pointsForWirePath2
                     |> Array.findIndex (fun p -> p = point)

    (wire1Steps + 1) + (wire2Steps + 1)


let findNearestIntersection (wirePaths : string[]) =
    let wire1 = parseWirePath wirePaths.[0] |> generatePointsForWirePath
    let wire2 = parseWirePath wirePaths.[1] |> generatePointsForWirePath

    wire1
    |> Array.filter (fun w -> Array.contains w wire2)
    |> Array.minBy computeManhattanDistance

let findShortestPaths (wirePaths : string[]) =
    let wire1 = parseWirePath wirePaths.[0] |> generatePointsForWirePath
    let wire2 = parseWirePath wirePaths.[1] |> generatePointsForWirePath

    wire1
    |> Array.filter (fun w -> Array.contains w wire2)
    |> Array.map (fun p -> computeOverallSteps p wire1 wire2)
    |> Array.min

let testDay3Part1 =
    let result1 = computeManhattanDistance <| findNearestIntersection [|"R8,U5,L5,D3"; "U7,R6,D4,L4"|]
    result1 |> shouldEqual 6

    let result2 = computeManhattanDistance <| findNearestIntersection [|"R75,D30,R83,U83,L12,D49,R71,U7,L72"; "U62,R66,U55,R34,D71,R55,D58,R83"|]
    result2 |> shouldEqual 159

    let result3 = computeManhattanDistance <| findNearestIntersection [|"R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51"; "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"|]
    result3 |> shouldEqual 135

let testDay3Part2 =
    let result1 = findShortestPaths [|"R8,U5,L5,D3"; "U7,R6,D4,L4"|]
    result1 |> shouldEqual 30

    let result2 = findShortestPaths [|"R75,D30,R83,U83,L12,D49,R71,U7,L72"; "U62,R66,U55,R34,D71,R55,D58,R83"|]
    result2 |> shouldEqual 610

    let result3 = findShortestPaths [|"R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51"; "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"|]
    result3 |> shouldEqual 410

// let day3Part1 =
//     let wirePaths = System.IO.File.ReadAllLines "day-3-input"

//     wirePaths |> findNearestIntersection |> computeManhattanDistance

let day3Part2 =
    let wirePaths = System.IO.File.ReadAllLines "day-3-input"

    wirePaths |> findShortestPaths
