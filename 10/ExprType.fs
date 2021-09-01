(* Programming language concepts for software developers, 2010-08-28 *)

(* Representing object language expressions using recursive datatypes *)

module Expr

//定义类型 expr
type expr = 
  | CstI of int
  | Prim of string * expr * expr;;

//调用类型 构造子 CstI
//返回 e1 值，其类型为 expr
let e1 = CstI 17;;

let e2 = Prim("-", CstI 3, CstI 4);;

let e3 = Prim("+", Prim("*", CstI 7, CstI 9), CstI 10);;

//查看e1的类型
e1;;


(* Evaluating expressions using recursive functions *)

let rec eval (e : expr) : int =
    match e with
    | CstI i -> i
    | Prim("+", e1, e2) -> eval e1 + eval e2
    | Prim("*", e1, e2) -> eval e1 * eval e2
    | Prim("-", e1, e2) -> eval e1 - eval e2
    | Prim _            -> failwith "unknown primitive";;
//查看表达式求值函数  eval 的类型
eval;;

let e1v = eval e1;;
let e2v = eval e2;;
let e3v = eval e3;;


(* Changing the meaning of subtraction *)
//修改表达式求值函数的语义

let rec evalm (e : expr) : int =
    match e with
    | CstI i -> i
    | Prim("+", e1, e2) -> evalm e1 + evalm e2
    | Prim("*", e1, e2) -> evalm e1 * evalm e2
    | Prim("-", e1, e2) -> 
      let res = evalm e1 - evalm e2
      if res < 0 then 0 else res 
    | Prim _            -> failwith "unknown primitive";;


let e4v = evalm (Prim("-", CstI 10, CstI 27));;

let a = [ ] // 'a list
let b = [||]  // 'a array
let c = [1;2] // int list

a;;
b;;

printfn "%A" e4v