

使用说明

linux平台 

fslex --> mono /usr/lib/cli/fsharp/fslex.exe
fsyacc --> mono /usr/lib/cli/fsharp/fsyacc.exe
fsi --> fsharpi

A. fslex,fsyacc 生成 四则运算表达式语言 的 词法分析器 语法分析器

   // 生成词法分析器
   fslex --unicode ExprLex.fsl
   //生成语法分析器
   fsyacc --module ExprPar ExprPar.fsy
   //生成action table goto table
   fsyacc -v --module ExprPar ExprPar.fsy
   // fsi Repl
   fsi -r FsLexYacc.Runtime.dll  Absyn.fs ExprPar.fs ExprLex.fs Parse.fs
    
   #r  "FsLexYacc.Runtime.dll" ;;
   #load "Absyn.fs" "ExprPar.fs" "ExprLex.fs" "Parse.fs" ;;
 
   open Parse;;

   fromString "2 + 3 * 4";;

   fromFile "expr.in.txt";;        

   #q;; 


B. 生成词法分析器和语法分析器，并用解释器eval求值。编译器scomp编译到 stack Machine code , seval 执行 stack machine code 。

   fslex --unicode ExprLex.fsl
   fsyacc --module ExprPar ExprPar.fsy
   fsi -r  FsLexYacc.Runtime.dll  Absyn.fs ExprPar.fs ExprLex.fs Parse.fs Expr.fs

   open Parse;;
   open Expr;;
   run (fromString "2 + 3 * 4");;
   eval (fromString "2 + x * 4") [("x", 3)];;
   eval (fromString "let x = 1+2 in 2 + x * 4 end") [];;

   let code1 = scomp (fromString "2 + 3 * 4") [];;
   seval code1 [];;

   let code2 = scomp (fromString "2 + x * 4") [Bound "x"];;
   seval code2 [3];;

   let code3 = scomp (fromString "let x = 1+2 in 2 + x * 4 end") [];;
   seval code3 [];;

   #q;;
