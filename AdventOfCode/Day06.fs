module Day6

open Test
open System

type OrbitingBody = {
    name : string
    satellites : OrbitingBody list
}

type Orbit = {
    orbited : string
    orbiter : string
}

type OrbitMap = {
    bodies : string list
    orbits : Orbit list
}

let private parseOrbitMap mapEntries =
    let parseEntry entry =
        let regexMatch = System.Text.RegularExpressions.Regex.Match(entry, "(?<orbited>\w+)\)(?<orbiter>\w+)")
        { orbited = regexMatch.Groups.["orbited"].Value; orbiter = regexMatch.Groups.["orbiter"].Value }

    let addBody body bodies =
        if (List.contains body bodies) then bodies else body :: bodies

    mapEntries
    |> Array.map parseEntry
    |> Array.fold (fun map orbit -> { map with bodies = addBody orbit.orbiter map.bodies; orbits = orbit :: map.orbits }) { bodies = ["COM"]; orbits = [] }

let countOrbits orbitMap =
    let rec countSingleBodyOrbits name runningTotal orbits =
        let orbit = List.find (fun o -> o.orbiter = name) orbits
        if (orbit.orbited = "COM") then runningTotal + 1 else countSingleBodyOrbits orbit.orbited (runningTotal + 1) orbits

    orbitMap.bodies
    |> List.filter (fun b -> b <> "COM")
    |> List.sumBy (fun b -> countSingleBodyOrbits b 0 orbitMap.orbits)


let runDay6Part1 map =
    let orbitMap = parseOrbitMap map
    countOrbits orbitMap

let findPathToCom name orbitMap =
    // orbitMap.orbits
    // |> List.fold (fun path body -> body.orbited :: path) []

    let rec findPath' name pathSoFar orbits =
        let orbit = List.find (fun o -> o.orbiter = name) orbits
        if (orbit.orbited = "COM") then "COM" :: pathSoFar else findPath' orbit.orbited (orbit.orbiter :: pathSoFar) orbits

    findPath' name [] orbitMap.orbits