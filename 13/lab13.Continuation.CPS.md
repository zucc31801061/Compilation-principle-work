#  x 2020-2021学年第2学期

##  实 验 报 告

![zucc](.\img\zucc.png)

-   课程名称: <u>编程语言原理与编译</u>

-   实验项目: <u>续算[Continuation](http://10.66.27.234:8090/#/06.cont/cont?id=callcc-call-with-current-continuation), CPS风格</u>

-   专业班级: <u>计算机1801</u>

-   学生学号: <u>31801061</u>

-   学生姓名: <u>王灵霜</u>

-   实验指导教师:<u>郭鸣</u>



## 实验内容

1. 阅读课件 

    - [续算, CPS风格](http://sigcc.gitee.io/plc2021/#/cont)
    - 2.07-08-rec-op-CPS.pdf
    - [MicroC优化编译器](http://sigcc.gitee.io/plc2021/#/optmc/optmc) 

2. 运行 cont目录中 [README.md](cont/README.md) 中的实验 A ，实验 B

    

    ![2](.\img\2.png)

    ![3](.\img\3.png)

    - 理解解释器

      - 抛出异常`Raise of  exn `，续算的实现

      - 捕获异常`TryWith of  expr  *  exn * expr`，续算的实现

      - 请写出 函数式语言 和 命令式语言 解释器参数

        函数式语言：

        ```fsharp
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
        ```

        命令式语言：

        ```fsharp
        type stmt = 
          | Asgn of string * expr
          | If of expr * stmt * stmt
          | Block of stmt list
          | For of string * expr * expr * stmt
          | While of expr * stmt
          | Print of expr
          | Throw of exn
          | TryCatch of stmt * exn * stmt
        ```

      - 比较他们有哪些差异   

        1.函数式中使用env，用cont: int -> answer来记录当前结果；命令式中使用store并且需要（exn * naivestore -> answer来记录当前状况）

        2.执行流程不同


3. 实现 cps目录中 cps-activity.pdf 中的 CPS 转换

     - pdf文件里面的代码是`haskell` 请在这里试试原来的代码https://www.tryhaskell.org/

     - 将代码改写为 F#

       ```fsharp
       // 1
       let rec sumList list res=
           match list with
           | [] -> res
           | head::tail -> sumList tail res + head
           
       sumList [1;3;5] 0;;
       ```
       ![4](.\img\4.png)
       ```fsharp
       // 2
       let rec mapList func (list:int list) id =
           match list with
           | [] -> id []
           | head::tail -> 
               mapList func tail (fun v->id (func head::v))
       
       let addone a = a+1;;
       let id = fun v -> v;;
       mapList addone [1;2;3] id;;
       ```
       ![5](.\img\5.png)
       ```fsharp
       // 3
       let rec mapList1 func (list:int list) id =
           match list with
           | [] -> id []
           | head::tail -> 
               mapList1 func tail (fun v->(func (head::v)id))
       
       let addone a id=id a;;
       let id = fun v -> v;;
       mapList addone [1;2;3] id;;
       ```
       ![6](.\img\6.png)
       ```fsharp
       // 4
       let min a b k =
           if a > b then k b
           else k a
       
       let min4 a b c d k =
           min a b (fun v -> (min c d (fun v2 -> min v v2 k)))
       
       min4 1 2 3 4 id;;
       ```
       ![7](.\img\7.png)
       ```fsharp
       // 5
       let myif a b c k=
           match a with
           | true -> k b
           | false -> k c
       
       myif (1>2) 4 5 id;;
       ```
       ![8](.\img\8.png)
       ```fsharp
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
       ```
       ![9](.\img\9.png)
       
     - 阅读课件  [CPS 转换](http://sigcc.gitee.io/plc2021/#/08/cont?id=cps-转换)，理解CPS 变换的四个步骤

       1.给函数定义加上续算参数k

       2.应用续算参数k到尾位置的简单表达式

       3.续算参数k直接作为参数传递给尾调用函数

       4.不在位置的函数调用需要构造新的续算函数，需要构造新的续算函数，其中包含了原来的续算k

     - 阅读课件 [利用 id 函数](http://sigcc.gitee.io/plc2021/#/08/cont?id=利用-id-函数🎈-evaluating-facc-n-id) 

       - CPS 程序取值 id 函数的作业

         ```fsharp
         let rec prodc xs k =
             match xs with
             | [] -> k 1
             | head::tail -> prodc tail (fun v -> k (head*v))
         
         prodc [2;3;1;4] id ;;
         ```

         ![10](.\img\10.png)


4. 完成 MicroC实验中[ReadME.md](https://gitee.com/sigcc/plzoofs/blob/master/microc/ReadME.md) 的 C D 部分 基于续算的优化编译器

    ![11](.\img\11.png)

    ![12](.\img\12.png)

    ![13](.\img\13.png)

    ![14](.\img\14.png)

    ![15](.\img\15.png)

    - 生成microcc.exe ,用优化编译器编译microc案例代码 ex*.c

      ![16](.\img\16.png)

    - 在优化编译器中编译`ex11.c`比较生成的代码,并测试性能 

      ![17](.\img\17.png)

5. 请说明MicroC 虚拟机尾递归调用`TCALL`指令的对栈帧的操作，参数`m ，n`分别是什么含义

    当编译器检测到一个函数调用是尾递归的时候，它就覆盖当前的活动记录而不是在栈中去创建一个新的。编译器可以做到这点，因为递归调用是当前活跃期内最后一条待执行的语句，于是当这个调用返回时栈帧中并没有其他事情可做，因此也就没有保存栈帧的必要了。通过覆盖当前的栈帧而不是在其之上重新添加一个，这样所使用的栈空间就大大缩减了，这使得实际的运行效率会变得更高。

    - `m` 尾调用函数的参数
    - `n` 原函数的参数个数
    - 递归调用时 `m = n`

6. 请写出案例程序，用`microcc`编译，用运行结果说明该编译器生成代码是`尾递归优化`的。

    ```c
    void main(int i){
        fun(i);
    }
    void fun(int n){
        if(n>0){
            fun(n-1);
        }
        else{
            print n;
        }
    }
    ```

    ```sh
    PS D:\计算机科学\study2021上\编程语言原理与编译\课程资料\sigcc-plzoofs-master\plzoofs\microc> ./bin/Debug/net5.0/machine.exe -t myex.out 5
    [ ]{0: LDARGS}
    [ 5 ]{1: CALL 1 5}
    [ 4 -999 5 ]{5: GETBP}
    [ 4 -999 5 2 ]{6: LDI}
    [ 4 -999 5 5 ]{7: TCALL 1 1 11}
    [ 4 -999 5 ]{11: GETBP}        
    [ 4 -999 5 2 ]{12: LDI}        
    [ 4 -999 5 5 ]{13: CSTI 0}     
    [ 4 -999 5 5 0 ]{15: SWAP}     
    [ 4 -999 5 0 5 ]{16: LT}       
    [ 4 -999 5 1 ]{17: IFZERO 28}  
    [ 4 -999 5 ]{19: GETBP}        
    [ 4 -999 5 2 ]{20: LDI}        
    [ 4 -999 5 5 ]{21: CSTI 1}
    [ 4 -999 5 5 1 ]{23: SUB}
    [ 4 -999 5 4 ]{24: TCALL 1 1 11}
    [ 4 -999 4 ]{11: GETBP}
    [ 4 -999 4 2 ]{12: LDI}
    [ 4 -999 4 4 ]{13: CSTI 0}
    [ 4 -999 4 4 0 ]{15: SWAP}
    [ 4 -999 4 0 4 ]{16: LT}
    [ 4 -999 4 1 ]{17: IFZERO 28}
    [ 4 -999 4 ]{19: GETBP}
    [ 4 -999 4 2 ]{20: LDI}
    [ 4 -999 4 4 ]{21: CSTI 1}
    [ 4 -999 4 4 1 ]{23: SUB}
    [ 4 -999 4 3 ]{24: TCALL 1 1 11}
    [ 4 -999 3 ]{11: GETBP}
    [ 4 -999 3 2 ]{12: LDI}
    [ 4 -999 3 3 0 ]{15: SWAP}
    [ 4 -999 3 0 3 ]{16: LT}
    [ 4 -999 3 1 ]{17: IFZERO 28}
    [ 4 -999 3 ]{19: GETBP}
    [ 4 -999 3 2 ]{20: LDI}
    [ 4 -999 3 3 ]{21: CSTI 1}
    [ 4 -999 3 3 1 ]{23: SUB}
    [ 4 -999 3 2 ]{24: TCALL 1 1 11}
    [ 4 -999 2 ]{11: GETBP}
    [ 4 -999 2 2 ]{12: LDI}
    [ 4 -999 2 2 ]{13: CSTI 0}
    [ 4 -999 2 2 0 ]{15: SWAP}
    [ 4 -999 2 0 2 ]{16: LT}
    [ 4 -999 2 1 ]{17: IFZERO 28}
    [ 4 -999 2 ]{19: GETBP}
    [ 4 -999 2 2 ]{20: LDI}
    [ 4 -999 2 2 ]{21: CSTI 1}
    [ 4 -999 2 2 1 ]{23: SUB}
    [ 4 -999 2 1 ]{24: TCALL 1 1 11}
    [ 4 -999 1 ]{11: GETBP}
    [ 4 -999 1 2 ]{12: LDI}
    [ 4 -999 1 1 ]{13: CSTI 0}
    [ 4 -999 1 1 0 ]{15: SWAP}
    [ 4 -999 1 0 1 ]{16: LT}
    [ 4 -999 1 1 ]{17: IFZERO 28}
    [ 4 -999 1 ]{19: GETBP}
    [ 4 -999 1 2 ]{20: LDI}
    [ 4 -999 1 1 ]{21: CSTI 1}
    [ 4 -999 1 1 1 ]{23: SUB}
    [ 4 -999 1 0 ]{24: TCALL 1 1 11}
    [ 4 -999 0 ]{11: GETBP}
    [ 4 -999 0 2 ]{12: LDI}
    [ 4 -999 0 0 ]{13: CSTI 0}
    [ 4 -999 0 0 0 ]{15: SWAP}
    [ 4 -999 0 0 0 ]{16: LT}
    [ 4 -999 0 0 ]{17: IFZERO 28}
    [ 4 -999 0 ]{28: GETBP}
    [ 4 -999 0 2 ]{29: LDI}
    [ 4 -999 0 0 ]{30: PRINTI}
    0 [ 4 -999 0 0 ]{31: RET 1}
    [ 0 ]{4: STOP}
    
    Ran 0.065 seconds
    PS D:\计算机科学\study2021上\编程语言原理与编译\课程资料\sigcc-plzoofs-master\plzoofs\microc> ./bin/Debug/net5.0/machine.exe -t myex.out 2
    [ ]{0: LDARGS}
    [ 2 ]{1: CALL 1 5}
    [ 4 -999 2 ]{5: GETBP}
    [ 4 -999 2 2 ]{6: LDI}
    [ 4 -999 2 2 ]{7: TCALL 1 1 11}
    [ 4 -999 2 ]{11: GETBP}
    [ 4 -999 2 2 ]{12: LDI}
    [ 4 -999 2 2 ]{13: CSTI 0}
    [ 4 -999 2 2 0 ]{15: SWAP}
    [ 4 -999 2 0 2 ]{16: LT}
    [ 4 -999 2 1 ]{17: IFZERO 28}
    [ 4 -999 2 ]{19: GETBP}
    [ 4 -999 2 2 ]{20: LDI}
    [ 4 -999 2 2 ]{21: CSTI 1}
    [ 4 -999 2 2 1 ]{23: SUB}
    [ 4 -999 2 1 ]{24: TCALL 1 1 11}
    [ 4 -999 1 ]{11: GETBP}
    [ 4 -999 1 2 ]{12: LDI}
    [ 4 -999 1 1 ]{13: CSTI 0}
    [ 4 -999 1 1 0 ]{15: SWAP}
    [ 4 -999 1 0 1 ]{16: LT}
    [ 4 -999 1 1 ]{17: IFZERO 28}
    [ 4 -999 1 ]{19: GETBP}
    [ 4 -999 1 2 ]{20: LDI}
    [ 4 -999 1 1 ]{21: CSTI 1}
    [ 4 -999 1 1 1 ]{23: SUB}
    [ 4 -999 1 0 ]{24: TCALL 1 1 11}
    [ 4 -999 0 ]{11: GETBP}
    [ 4 -999 0 2 ]{12: LDI}
    [ 4 -999 0 0 ]{13: CSTI 0}
    [ 4 -999 0 0 0 ]{15: SWAP}
    [ 4 -999 0 0 0 ]{16: LT}
    [ 4 -999 0 0 ]{17: IFZERO 28}
    [ 4 -999 0 ]{28: GETBP}
    [ 4 -999 0 2 ]{29: LDI}
    [ 4 -999 0 0 ]{30: PRINTI}
    0 [ 4 -999 0 0 ]{31: RET 1}
    [ 0 ]{4: STOP}
    
    Ran 0.034 seconds
    ```
调用返回时栈帧中并没有其他事情可做，没有保存栈帧的必要了,通过覆盖当前的栈帧而不是在其之上重新添加一个实现递归的调用
7. 查看 c 编译器的优化效果， 访问 https://godbolt.org/

    - 编译如下的  S16-lecture-01.pdf  p44 程序
    
      - 阅读上述pdf，理解常规优化方法
    
        ![18](.\img\18.png)

    - 选择   参数 -O0  或 -O3 查看生成的汇编代码
      
      ![19](.\img\19.png)
      
      ![20](.\img\20.png)
      
      - 注意左边源代码 与右边汇编指令的对应关系
      - 优化级别高，访问内存少 	
      
    - 选择不同的编译器  clang  gcc 查看生成的汇编
    
      ![21](.\img\21.png)
    
    ```c
    int sumcalc(int a, int b, int N)
    {
        int i, x, t, u, v;
        x = 0;
        u = ((a<<2)/b);
        v = 0;
        for(i = 0; i <= N; i++) {
            t = i+1;
            x = x + v + t*t;
            v = v + u;
        }
        return x;
    }
    ```


8. 自选，完成  Cont 目录里面 c 语言的 longjmp and setjmp 

9. 自选, 实现支持异常指令的 Stack Machine 

10. 自选,在大作业实现的语言中支持异常处理，尾递归优化



## 实验要求

1. 使用Markdown文件完成，推荐Typora
1. 使用[Git](https://learngitbranching.js.org/)工具管理作业代码、文本文件

