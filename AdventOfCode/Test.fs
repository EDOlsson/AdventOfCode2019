module Test

let shouldEqual expected actual =
    if expected <> actual then printfn "X Test case failure: Expected %A%s                       Actual %A" expected System.Environment.NewLine actual
    else printfn ". Passed"