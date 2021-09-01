let rec quickSort (list : int list) =
    match list with
    | []    ->  []
    | [single] -> [single]
    | head :: tail ->
        let leftList = 
                tail
                    |> List.choose(fun item -> if item <= head then Some(item) else None)

        let rightList = 
                tail
                    |> List.choose(fun item -> if item > head then Some(item) else None)

        quickSort(leftList) @ [head] @ quickSort(rightList)

let data =[3;2;1;4;5;7;6]
let result= quickSort data