module Absyn

type expr = 
    | CstI of int
    | Var of  string
    | Prim of string * expr * expr