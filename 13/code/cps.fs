// 1
let rec sumList list res=
    match list with
    | [] -> res
    | head::tail -> sumList tail res + head
    
sumList [1;3;5] 0;;

// 2
let rec mapList func (list:int list) id =
    match list with
    | [] -> id []
    | head::tail -> 
        mapList func tail (fun v->id (func head::v))

let addone a = a+1;;
let id = fun v -> v;;
mapList addone [1;2;3] id;;

// 3
let rec mapList1 func (list:int list) id1 =
    match list with
    | [] -> id1 []
    | head::tail -> 
        mapList1 func tail (fun v->(func (head::v)id1))

let addone a id1=id1 a;;
let id = fun v -> v;;
mapList1 addone [1;2;3] id;;

// 4
let min a b k =
    if a > b then k b
    else k a

let min4 a b c d k =
    min a b (fun v -> (min c d (fun v2 -> min v v2 k)))

min4 1 2 3 4 id;;

// 5
let myif a b c k=
    match a with
    | true -> k b
    | false -> k c

myif (1>2) 4 5 id;;

// 6
type AUX = 
   | Add of int 
   | Sub of int

let rec aux_cps first list k = 
    match list with
    | [] -> first
    | head::tail -> 
        match head with
        | Add a -> aux_cps  (first+a)  tail ( fun v -> (k v) )
        | Sub a -> aux_cps  (first-a)  tail ( fun v -> (k v) )

aux_cps 0 [Add 2;Add 2;Add 6;Sub 7] id;;

let rec prodc xs k =
    match xs with
    | [] -> k 1
    | head::tail -> prodc tail (fun v -> k (head*v))

prodc [2;3;1;4] id ;;