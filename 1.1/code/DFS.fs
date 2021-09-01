let top_sort_map edges =
(* Create an empty successor list for each node *)
let succs = List.fold_left
(fun map (s,d) ->
StringMap.add d [] (StringMap.add s [] map)
) StringMap.empty edges
in
(* Build the successor list for each source node *)
let succs = List.fold_left
(fun succs (s, d) ->
let ss = StringMap.find s succs
in StringMap.add s (d::ss) succs) succs edges
in
(* Visit recursively, storing each node after visiting successors *)
let rec visit (order, visited) n =
    if StringSet.mem n visited then
    (order, visited)
    else
    let (order, visited) = List.fold_left
    visit (order, StringSet.add n visited)
    (StringMap.find n succs)
    in (n::order, visited)
in
(* Visit the source of each edge *)
fst (List.fold_left visit ([], StringSet.empty) (List.map fst edges