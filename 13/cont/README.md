## 基于续算的求值器
---------------------------------------------------------------------

 A.  基于续算函数式语言，支持异常处理

```sh

>dotnet fsi 

# load "Contfun.fs"

open Contfun;;
eval1 ex1 [];; 
eval1 ex2 [("n", Int 10)];

```


B. 基于续算的命令式语言，支持异常处理

```sh
>dotnet fsi 

# load "Contimp.fs"

open Contimp;;
run1 ex1;;
run1 ex2;;
run2 ex3;;
```

C. micro-Icon 语言，表达式可以返回多个值:

```sh   

dotnet fsi 

   Icon.fs

   open Icon;;
   run ex1;;
   run ex2;;
   run ex3and;;
   run ex3or;;

```

D.  Java 的续算实现，代码见 /java  目录

```sh

javac Factorial.java
java Factorial 10

javac ExceptionDemo.java
javap -c ExceptionDemo.class 查看异常表
```

E. C语言的 `longjmp` 和`setjmp`  调用，代码见 /c  目录

```sh
gcc testlongjmp.c -o testlongjmp
./testlongjmp 10
./testlongjmp 11
```