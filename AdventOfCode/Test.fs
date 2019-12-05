module Test

let shouldEqual expected actual =
    if expected <> actual then printfn "X Test case failure: Expected %A%s                       Actual %A" expected System.Environment.NewLine actual
    else printfn ". Passed"

let shouldEqualWithLabel label expected actual =
    if expected <> actual then printfn "X Test case failure [%s]%s    Expected %A%s      Actual %A" label System.Environment.NewLine expected System.Environment.NewLine actual
    else printfn ". Passed"
