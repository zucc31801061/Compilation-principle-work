

## 使用说明

fslex,fsyacc 生成 后缀式四则运算表达式语言的 
 - 词法分析器 
 - 语法分析器

```sh

# 注意修改路径
# 生成扫描器
dotnet "C:\Users\gm\.nuget\packages\fslexyacc\10.2.0\build\/fslex/netcoreapp3.1\fslex.dll"  --unicode ExprLex.fsl

# 生成分析器
dotnet "C:\Users\gm\.nuget\packages\fslexyacc\10.2.0\build\/fsyacc/netcoreapp3.1\fsyacc.dll"  -v --module ExprPar ExprPar.fsy

# ExprPar.fsyacc.output 有DFA状态表可以查看

//命令行运行程序
dotnet fsi 

#r "nuget: FsLexYacc";;  //命令行添加包引用

#load "Absyn.fs" "ExprPar.fs" "ExprLex.fs" "Parse.fs"

open Parse;;
fromString "2  3 + 4 *";;
fromFile "expr.in.txt";;        

#q;; 
```
