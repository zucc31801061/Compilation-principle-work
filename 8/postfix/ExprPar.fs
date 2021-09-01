// Implementation file for parser generated by fsyacc
module ExprPar
#nowarn "64";; // turn off warnings that type variables used in production annotations are instantiated to concrete type
open FSharp.Text.Lexing
open FSharp.Text.Parsing.ParseHelpers
# 1 "ExprPar.fsy"

  (* File Expr/ExprPar.fsy
     Parser specification for the simple postfix expression language.
   *)
  open Absyn

# 13 "ExprPar.fs"
// This type is the type of tokens accepted by the parser
type token = 
  | EOF
  | PLUS
  | MINUS
  | TIMES
  | DIVIDE
  | NAME of (string)
  | CSTINT of (int)
// This type is used to give symbolic names to token indexes, useful for error messages
type tokenId = 
    | TOKEN_EOF
    | TOKEN_PLUS
    | TOKEN_MINUS
    | TOKEN_TIMES
    | TOKEN_DIVIDE
    | TOKEN_NAME
    | TOKEN_CSTINT
    | TOKEN_end_of_input
    | TOKEN_error
// This type is used to give symbolic names to token indexes, useful for error messages
type nonTerminalId = 
    | NONTERM__startMain
    | NONTERM_Main
    | NONTERM_Expr
    | NONTERM_OP

// This function maps tokens to integer indexes
let tagOfToken (t:token) = 
  match t with
  | EOF  -> 0 
  | PLUS  -> 1 
  | MINUS  -> 2 
  | TIMES  -> 3 
  | DIVIDE  -> 4 
  | NAME _ -> 5 
  | CSTINT _ -> 6 

// This function maps integer indexes to symbolic token ids
let tokenTagToTokenId (tokenIdx:int) = 
  match tokenIdx with
  | 0 -> TOKEN_EOF 
  | 1 -> TOKEN_PLUS 
  | 2 -> TOKEN_MINUS 
  | 3 -> TOKEN_TIMES 
  | 4 -> TOKEN_DIVIDE 
  | 5 -> TOKEN_NAME 
  | 6 -> TOKEN_CSTINT 
  | 9 -> TOKEN_end_of_input
  | 7 -> TOKEN_error
  | _ -> failwith "tokenTagToTokenId: bad token"

/// This function maps production indexes returned in syntax errors to strings representing the non terminal that would be produced by that production
let prodIdxToNonTerminal (prodIdx:int) = 
  match prodIdx with
    | 0 -> NONTERM__startMain 
    | 1 -> NONTERM_Main 
    | 2 -> NONTERM_Expr 
    | 3 -> NONTERM_Expr 
    | 4 -> NONTERM_Expr 
    | 5 -> NONTERM_OP 
    | 6 -> NONTERM_OP 
    | 7 -> NONTERM_OP 
    | 8 -> NONTERM_OP 
    | _ -> failwith "prodIdxToNonTerminal: bad production index"

let _fsyacc_endOfInputTag = 9 
let _fsyacc_tagOfErrorTerminal = 7

// This function gets the name of a token as a string
let token_to_string (t:token) = 
  match t with 
  | EOF  -> "EOF" 
  | PLUS  -> "PLUS" 
  | MINUS  -> "MINUS" 
  | TIMES  -> "TIMES" 
  | DIVIDE  -> "DIVIDE" 
  | NAME _ -> "NAME" 
  | CSTINT _ -> "CSTINT" 

// This function gets the data carried by a token as an object
let _fsyacc_dataOfToken (t:token) = 
  match t with 
  | EOF  -> (null : System.Object) 
  | PLUS  -> (null : System.Object) 
  | MINUS  -> (null : System.Object) 
  | TIMES  -> (null : System.Object) 
  | DIVIDE  -> (null : System.Object) 
  | NAME _fsyacc_x -> Microsoft.FSharp.Core.Operators.box _fsyacc_x 
  | CSTINT _fsyacc_x -> Microsoft.FSharp.Core.Operators.box _fsyacc_x 
let _fsyacc_gotos = [| 0us; 65535us; 1us; 65535us; 0us; 1us; 3us; 65535us; 0us; 2us; 2us; 6us; 6us; 6us; 1us; 65535us; 6us; 7us; |]
let _fsyacc_sparseGotoTableRowOffsets = [|0us; 1us; 3us; 7us; |]
let _fsyacc_stateToProdIdxsTableElements = [| 1us; 0us; 1us; 0us; 2us; 1us; 4us; 1us; 1us; 1us; 2us; 1us; 3us; 2us; 4us; 4us; 1us; 4us; 1us; 5us; 1us; 6us; 1us; 7us; 1us; 8us; |]
let _fsyacc_stateToProdIdxsTableRowOffsets = [|0us; 2us; 4us; 7us; 9us; 11us; 13us; 16us; 18us; 20us; 22us; 24us; |]
let _fsyacc_action_rows = 12
let _fsyacc_actionTableElements = [|2us; 32768us; 5us; 4us; 6us; 5us; 0us; 49152us; 3us; 32768us; 0us; 3us; 5us; 4us; 6us; 5us; 0us; 16385us; 0us; 16386us; 0us; 16387us; 6us; 32768us; 1us; 8us; 2us; 9us; 3us; 10us; 4us; 11us; 5us; 4us; 6us; 5us; 0us; 16388us; 0us; 16389us; 0us; 16390us; 0us; 16391us; 0us; 16392us; |]
let _fsyacc_actionTableRowOffsets = [|0us; 3us; 4us; 8us; 9us; 10us; 11us; 18us; 19us; 20us; 21us; 22us; |]
let _fsyacc_reductionSymbolCounts = [|1us; 2us; 1us; 1us; 3us; 1us; 1us; 1us; 1us; |]
let _fsyacc_productionToNonTerminalTable = [|0us; 1us; 2us; 2us; 2us; 3us; 3us; 3us; 3us; |]
let _fsyacc_immediateActions = [|65535us; 49152us; 65535us; 16385us; 16386us; 16387us; 65535us; 16388us; 16389us; 16390us; 16391us; 16392us; |]
let _fsyacc_reductions ()  =    [| 
# 115 "ExprPar.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _1 = parseState.GetInput(1) :?> Absyn.expr in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
                      raise (FSharp.Text.Parsing.Accept(Microsoft.FSharp.Core.Operators.box _1))
                   )
                 : 'gentype__startMain));
# 124 "ExprPar.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _1 = parseState.GetInput(1) :?> 'gentype_Expr in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 19 "ExprPar.fsy"
                                                               _1                
                   )
# 19 "ExprPar.fsy"
                 : Absyn.expr));
# 135 "ExprPar.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _1 = parseState.GetInput(1) :?> string in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 23 "ExprPar.fsy"
                                                               Var _1            
                   )
# 23 "ExprPar.fsy"
                 : 'gentype_Expr));
# 146 "ExprPar.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _1 = parseState.GetInput(1) :?> int in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 24 "ExprPar.fsy"
                                                               CstI _1           
                   )
# 24 "ExprPar.fsy"
                 : 'gentype_Expr));
# 157 "ExprPar.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _1 = parseState.GetInput(1) :?> 'gentype_Expr in
            let _2 = parseState.GetInput(2) :?> 'gentype_Expr in
            let _3 = parseState.GetInput(3) :?> 'gentype_OP in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 25 "ExprPar.fsy"
                                                              Prim(_3, _1, _2) 
                   )
# 25 "ExprPar.fsy"
                 : 'gentype_Expr));
# 170 "ExprPar.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 30 "ExprPar.fsy"
                               "+"
                   )
# 30 "ExprPar.fsy"
                 : 'gentype_OP));
# 180 "ExprPar.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 30 "ExprPar.fsy"
                                             "-"
                   )
# 30 "ExprPar.fsy"
                 : 'gentype_OP));
# 190 "ExprPar.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 30 "ExprPar.fsy"
                                                            "*"
                   )
# 30 "ExprPar.fsy"
                 : 'gentype_OP));
# 200 "ExprPar.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 30 "ExprPar.fsy"
                                                                          "/"
                   )
# 30 "ExprPar.fsy"
                 : 'gentype_OP));
|]
# 211 "ExprPar.fs"
let tables : FSharp.Text.Parsing.Tables<_> = 
  { reductions= _fsyacc_reductions ();
    endOfInputTag = _fsyacc_endOfInputTag;
    tagOfToken = tagOfToken;
    dataOfToken = _fsyacc_dataOfToken; 
    actionTableElements = _fsyacc_actionTableElements;
    actionTableRowOffsets = _fsyacc_actionTableRowOffsets;
    stateToProdIdxsTableElements = _fsyacc_stateToProdIdxsTableElements;
    stateToProdIdxsTableRowOffsets = _fsyacc_stateToProdIdxsTableRowOffsets;
    reductionSymbolCounts = _fsyacc_reductionSymbolCounts;
    immediateActions = _fsyacc_immediateActions;
    gotos = _fsyacc_gotos;
    sparseGotoTableRowOffsets = _fsyacc_sparseGotoTableRowOffsets;
    tagOfErrorTerminal = _fsyacc_tagOfErrorTerminal;
    parseError = (fun (ctxt:FSharp.Text.Parsing.ParseErrorContext<_>) -> 
                              match parse_error_rich with 
                              | Some f -> f ctxt
                              | None -> parse_error ctxt.Message);
    numTerminals = 10;
    productionToNonTerminalTable = _fsyacc_productionToNonTerminalTable  }
let engine lexer lexbuf startState = tables.Interpret(lexer, lexbuf, startState)
let Main lexer lexbuf : Absyn.expr =
    engine lexer lexbuf 0 :?> _