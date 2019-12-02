module Test

let shouldEqual expected actual =
    if expected <> actual then failwith (sprintf "Test case failure: Expected %A%s                     Actual %A" expected System.Environment.NewLine actual)