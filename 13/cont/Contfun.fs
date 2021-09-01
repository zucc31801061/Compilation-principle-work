(* File Cont/Contfun.fs

   A strict functional language with integers, first-order functions,
   and exceptions * sestoft@itu.dk 2009-09-24

   Exceptions in the functional object language are modelled without
   using exceptions in the meta-language.  Instead the interpreter
   uses continuations to represent the rest of the computation.
   The interpreters are written in continuation-passing style, and
   implement tail calls in constant space.

   This file contains two interpreters:

     * coEval1 allows exceptions to be thrown, but not caught; this
       is implemented using a single continuation (which is used by
       ordinary computations but ignored when an exception is thrown)

     * coEval2 allows exceptions to be thrown and caught; this is
       implemented using two continuations: the success continuation
       is used by ordinary computations, and the error continuation
       is used when an exception is thrown.
*)

module Contfun =

    (* Simple environment operations *)

    type 'data env = (string * 'data) list

    let rec lookup env x =
        match env with
        | [] -> failwith (x + " not found")
        | (y, v) :: yr -> if x = y then v else lookup yr x

    (* Abstract syntax of functional language with exceptions *)

    //异常类型
    type exn = Exn of string

    type expr =
        | CstI of int
        | CstB of bool
        | Var of string
        | Let of string * expr * expr
        | Prim of string * expr * expr
        | If of expr * expr * expr
        | Letfun of string * string * expr * expr (* (f, x, fbody, ebody) *)
        | Call of string * expr
        | Raise of exn (* 抛出异常 *)
        | TryWith of expr * exn * expr (* 捕获异常 try e1 with exn -> e2 *)

    // 函数数语言，函数可以是值
// value 类型封装 两种值，分别是
// Int 与 闭包
    type value =
        | Int of int
        | Closure of string * string * expr * value env (* (f, x, fBody, fDeclEnv) *)

    (*
  返回结果类型,可以是一个值或者是异常
  由于程序 可以返回异常，也可以返回正常值
  构造类型 answer 封装返回值，便于处理
*)
    type answer =
        | Result of int
        | Abort of string

    (* This interpreter coEval1 takes the following arguments:

    * An expression e to evaluate.
    * An environment env in which to evalute it.
    * A success continuation cont which accepts as argument the value
      of the expression.

   It returns an answer: Result i or Abort s.  When the evaluation
   of e succeeds, it applies the success continuation to its result,
   and when e raises an exception (Exn s), it returns Abort s.
   Since there is no error continuation, there is no provision for
   handling raised exceptions.
*)
    // coEval1 只允许抛出异常
// 返回类型为 answer
// cont 是 int-> answer 的函数，续算是一个单参数函数，接受当前运算的结果，进行后面的计算。
// cont 的参数是当前运算的正常值 类型 为 int
// coEval1 所有的出口都是尾递归调用
// 没有 Return，通过续算函数 cont 返回值
    let rec coEval1 (e: expr) (env: value env) (cont: int -> answer) : answer =
        match e with
        | CstI i -> cont i // 将i的值交给续算函数cont
        | CstB b -> cont (if b then 1 else 0) // 将 1/0 交给续算函数cont
        | Var x ->
            match lookup env x with
            | Int i -> cont i
            | _ -> Abort "coEval1 Var" //抛出异常值 Abort msg
        | Prim (ope, e1, e2) ->
            coEval1
                e1 // 在环境 env 中对 e1 求值
                env // 算出 e1的值 交给 i1 -- 续算函数的参数 i1
                (fun i1 -> // 该匿名函数是  e1 表达式 求值完毕后的续算 ⭐⭐⭐
                    coEval1
                        e2
                        env //算出 e2 的值 交给 i2 -- 续算函数的参数 i2
                        (fun i2 -> // 该匿名函数是 e1和 e2 表达式 求值完毕后的续算 ⭐⭐⭐
                            match ope with
                            | "*" -> cont (i1 * i2) //向续算 cont 传递 i1 * i2 结果
                            | "+" -> cont (i1 + i2) //同上
                            | "-" -> cont (i1 - i2) //同上
                            | "=" -> cont (if i1 = i2 then 1 else 0)
                            | "<" -> cont (if i1 < i2 then 1 else 0)
                            | _ -> Abort "unknown primitive")) //抛出异常
        | Let (x, eRhs, letBody) ->
            coEval1
                eRhs
                env
                (fun xVal -> // eRHs 求值的结果为 xVal
                    let bodyEnv = (x, Int xVal) :: env // 构造 letBody 新的求值环境
                    coEval1 letBody bodyEnv cont)
        | If (e1, e2, e3) ->
            coEval1
                e1
                env
                (fun b -> // e1 求值后的续算
                    if b <> 0 then
                        coEval1 e2 env cont // 尾递归调用
                    else
                        coEval1 e3 env cont) // 尾递归调用
        | Letfun (f, x, fBody, letBody) ->
            let bodyEnv = (f, Closure(f, x, fBody, env)) :: env
            coEval1 letBody bodyEnv cont
        | Call (f, eArg) ->
            let fClosure = lookup env f

            match fClosure with
            | Closure (f, x, fBody, fDeclEnv) ->
                coEval1
                    eArg
                    env
                    (fun xVal ->
                        let fBodyEnv =
                            // 环境中添加 f 支持递归调用
                            (x, Int xVal) :: (f, fClosure) :: fDeclEnv

                        coEval1 fBody fBodyEnv cont)
            | _ -> Abort "eval Call: not a function"
        | Raise (Exn s) -> Abort s //抛出异常
        | TryWith (e1, exn, e2) -> Abort "Not implemented"
    //coEval1没有实现捕获异常


    let eval1 e env = coEval1 e env (fun v -> Result v)
    //匹配coEval1的参数类型
//id 函数 将结果转换为 answer类型
    let run1 e = eval1 e []

    (* This interpreter coEval2 takes the following arguments:

    * An expression e to evaluate.
    * An environment env in which to evalute it.
    * A success continuation cont which accepts as argument the value
      of the expression.
    * A error continuation econt, which is applied when an exception
      is thrown

   It returns an answer: Result i or Abort s.  When the evaluation
   of e succeeds, it applies the success continuation to its result,
   and when e raises an exception exn, it applies the failure
   continuation to exn.  The failure continuation may choose to catch
   the exception.
*)

    // 续算econt 为抛出异常捕获后的处理函数
    let rec coEval2 (e: expr) (env: value env) (cont: int -> answer) (econt: exn -> answer) : answer =
        match e with
        | CstI i -> cont i
        | CstB b -> cont (if b then 1 else 0)
        | Var x ->
            match lookup env x with
            | Int i -> cont i
            | _ -> Abort "coEval2 Var"
        | Prim (ope, e1, e2) ->
            coEval2
                e1
                env
                (fun i1 ->
                    coEval2
                        e2
                        env
                        (fun i2 ->
                            match ope with
                            | "*" -> cont (i1 * i2)
                            | "+" -> cont (i1 + i2)
                            | "-" -> cont (i1 - i2)
                            | "=" -> cont (if i1 = i2 then 1 else 0)
                            | "<" -> cont (if i1 < i2 then 1 else 0)
                            | _ -> Abort "unknown primitive")
                        econt)
                econt

        | Let (x, eRhs, letBody) ->
            coEval2
                eRhs
                env
                (fun xVal ->
                    let bodyEnv = (x, Int xVal) :: env
                    coEval2 letBody bodyEnv cont econt)
                econt
        | If (e1, e2, e3) ->
            coEval2
                e1
                env
                (fun b ->
                    if b <> 0 then
                        coEval2 e2 env cont econt
                    else
                        coEval2 e3 env cont econt)
                econt
        | Letfun (f, x, fBody, letBody) ->
            let bodyEnv = (f, Closure(f, x, fBody, env)) :: env
            coEval2 letBody bodyEnv cont econt
        | Call (f, eArg) ->
            let fClosure = lookup env f

            match fClosure with
            | Closure (f, x, fBody, fDeclEnv) ->
                coEval2
                    eArg
                    env
                    (fun xVal ->
                        let fBodyEnv =
                            (x, Int xVal) :: (f, fClosure) :: fDeclEnv

                        coEval2 fBody fBodyEnv cont econt)
                    econt
            | _ -> raise (Failure "eval Call: not a function")
        | Raise exn -> econt exn
        | TryWith (e1, exn, e2) ->

            //将异常处理函数定义为续算 econ1
            let econt1 thrown =

                //捕获异常 thrown后 计算e2 ,否则继续抛出异常
                if thrown = exn then
                    coEval2 e2 env cont econt
                else
                    econt thrown // 继续抛出异常

            coEval2 e1 env cont econt1

    (* The top-level error continuation returns the continuation,
       adding the text Uncaught exception *)

    let eval2 e env =
        coEval2
            e
            env
            // 正常续算,返回 该值
            (fun v -> Result v)
            //异常续算, 返回异常信息
            (fun (Exn s) -> Abort("Uncaught exception: " + s))

    let run2 e = eval2 e []


    (* Examples in abstract syntax *)

    let ex1 =
        Letfun("f1", "x", Prim("+", Var "x", CstI 1), Call("f1", CstI 12))

    (* Factorial *)

    let ex2 =
        Letfun(
            "fac",
            "x",
            If(Prim("=", Var "x", CstI 0), CstI 1, Prim("*", Var "x", Call("fac", Prim("-", Var "x", CstI 1)))),
            Call("fac", Var "n")
        )

    let fac10 = eval1 ex2 [ ("n", Int 10) ]

    (* Example: deep recursion to check for constant-space tail recursion *)

    let exdeep =
        Letfun(
            "deep",
            "x",
            If(Prim("=", Var "x", CstI 0), CstI 1, Call("deep", Prim("-", Var "x", CstI 1))),
            Call("deep", Var "n")
        )

    let rundeep n = eval1 exdeep [ ("n", Int n) ]

    (* Example: throw an exception inside expression *)

    let ex3 =
        Prim("*", CstI 11, Raise(Exn "out here"))

    (* Example: throw an exception and catch it *)

    let ex4 =
        TryWith(Prim("*", CstI 11, Raise(Exn "Outahere")), Exn "Outahere", CstI 999)

    (* Example: throw an exception in a called function *)

    let ex5 =
        Letfun(
            "fac",
            "x",
            If(
                Prim("<", Var "x", CstI 0),
                Raise(Exn "negative x in fac"),
                If(Prim("<", Var "x", CstI 0), CstI 1, Prim("*", Var "x", Call("fac", Prim("-", Var "x", CstI 1))))
            ),
            Call("fac", CstI -10)
        )

    (* Example: throw an exception but don't catch it *)

    let ex6 =
        TryWith(Prim("*", CstI 11, Raise(Exn "Outahere")), Exn "Noway", CstI 999)

    let ex7 =
        TryWith(Prim("*", CstI 11, Raise(Exn "Outahere")), Exn "Outahere", CstI 999)


    let all = [ ex1; ex3; ex4; ex5; ex6; ex7 ]

    let show all =
        let showOne elem =
            printf "%A:\n" elem
            run1 elem |> printf "run1:%A\n" //run1 只抛出异常
            run2 elem |> printf "run2:%A\n" //run2可以捕获异常
            printf "\n"
            ()

        List.iter showOne all

    show all
