type tree = Leaf | Node of tree * int * tree;;

let rec insert key t =
    match t with
    | Leaf -> Node(Leaf, key, Leaf)
    | Node(left ,k ,right) -> 
        if key<k then
            Node(insert key left, key, right)
        else
            Node(left, key, insert key right);;
    

let rec search key t =
    match t with
    | Leaf -> false
    | Node(left ,k ,right) -> 
        if key = k then
            true
        else if key < k then
            search key left
        else
            search key right;;

let rec add key bind t =
    match t with
    | Leaf -> bind := Node(Leaf, key, Leaf)
              Node(Leaf, key, Leaf)
    | Node(left ,k ,right) -> 
        if key<k then
            Node(add key bind left, k, right)            
        else if key>k then
            Node(left, k, add key bind right)
        else
            bind := t
            Node(left ,k ,right);;

let rec lookup key t =
    match t with
    | Leaf -> Leaf
    | Node(left ,k ,right) ->
        if key = k then
            t
        else if key < k then
            lookup key left
        else
            lookup key right;;

(* let mytree = insert 5 Leaf;;
search 5 mytree;;
search 7 mytree;; *)

let mytree = Leaf;;
let bindTree = ref mytree;;
let myta = add 5 bindTree mytree;;
bindTree;;
let mytb = add 7 bindTree myta;;
bindTree;;
let mytc = add 9 bindTree mytb;;
bindTree;;
lookup 5 mytc;;
lookup 7 mytc;;