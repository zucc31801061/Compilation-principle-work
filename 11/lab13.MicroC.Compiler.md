# 2020-2021学年第2学期

##  实 验 报 告

![zucc](.\img\zucc.png)

-   课程名称: <u>编程语言原理与编译</u>

-   实验项目: <u>MicroC编译器</u>

-   专业班级: <u>计算机1801</u>

-   学生学号: <u>31801061</u>

-   学生姓名: <u>王灵霜</u>

-   实验指导教师:<u>郭鸣</u>

## 实验内容

### 1.  阅读课件 [MicroC实现,编译器 - 编程语言与编译](http://sigcc.gitee.io/plc2021/#/07/microc.compiler)

### 2.  阅读`MicroC` 解释虚拟机指令集

- LDI
  - 将栈帧上某位置的值入栈 `s,i --> s,v ; v = s(i)`
  - `i`值为相对栈底的偏移量，从 `0` 开始索引, 如 `s(0)` 表示栈底的第一个值。
  - 访问局部变量 `x` 的值: `x + 3`
- STI
  - 将值写入栈上某个位置 `s,i,v ---> s,v ; s(i) <= v`
  - 该指令用于赋值语句`x`: `x = y + 3`
- GETBP
  - GETBP 得到当前栈帧基地址 bp
  - bp+0 对应函数第1个参数/局部变量 v1
  - bp+1 .......第2个............ v2
- GETSP
  - 获得当前栈的地址sp
- CALL
  - `m`是函数参数个数, `a` 是函数跳转目标地址
  - call 执行后，将 返回地址`r`，上个栈帧原来`bp`值保存到栈上,参数v1---vm拷贝到栈上。
  - 新 `bp` 寄存器的值 指向当前栈帧基地址，即从函数参数开始的地址
- RET
  - `RET m` 与 `CALL m` 对应，从`bp` 开始算，第`m`个值，就是`v`
  - `bp+0` --> v1

请说明上面指令的作用

### 3.  完成  [MicroC](https://gitee.com/sigcc/plzoofs/tree/master/microc) /ReadME.md 任务`B`

![5](.\img\5.png)

![6](.\img\6.png)

![7](.\img\7.png)

![8](.\img\8.png)

```sh
# 编译microc编译器,并用microc编译器 编译 ex3.c 
~ microc>dotnet clean  microc.fsproj  # optional
~ microc>dotnet build  microc.fsproj  # optional
~ microc>dotnet run -p microc.fsproj ex3.c # 编译
Micro-C Stack VM compiler v 1.2.0 of 2021-5-12
Compiling ex3.c ......

VM instructions saved in file:
        ex3.ins
VM instructions saved in file:
        ex3.insx86
x86 assembly saved in file:
        ex3.asm
Numeric code saved in file:
        ex3.out
Please run with VM.
~ microc>cat ex3.ins  # Stack VM 指令
LDARGS 1
CALL (1, "L1_main")
STOP
FLabel (1, "L1_main")
INCSP 1
GETBP
OFFSET 1
ADD
CSTI 0
...
~ microc>cat ex3.out           # ex3.out机器码
24 19 1 5 25 15 1 13 0 1 1 0 0 12 15 -1 16 43 13 0 1 1 11 22 15 -1 13 0 1 1 13 0 1 1 11 0 1 1 12 15 -1 15 0 13 0 1 1 11 13 0 0 1 11 7 18 18 15 -1 21 0
~ microc> gcc machine.c -o machine # gcc编译器生成虚拟机
~ microc>./machine.exe ex3.out 10  # 执行ex3.out机器码
0 1 2 3 4 5 6 7 8 9 Used   0.000 cpu seconds
```

![9](.\img\9.png)

### 4.  请阅读 `ex9.trace.0.txt` `ex9.trace.3.txt`理解 源代码 和 指令的对应关系

```sh
 ./machine.exe ex9.out 0
 ./machine.exe -trace ex9.out 3
```
[运行示例参见](http://sigcc.gitee.io/plc2021/#/05/microc.compiler?id=the-code-generated-for-ex9c)

![10](.\img\10.png)

### 5.  请用 运行下面的命令,仿照4 写出 虚拟机代码的注释

- ./machine.exe ex5.out 5

- ./machine -trace ex5.out 5

  ![11](.\img\11.png)

### 6. 在ex1.c 中

- 加上代码 h[4] = 5; 

  见lab12

- 结合MicroC堆栈的布局，请解释一下程序运行的结果

  见lab12

- 试试分别加上 h[5]---h[12]，程序发生了什么？
  - 网上搜索缓冲区溢出漏洞的基本原理
  
    进程分控制层面和数据层面两个部分，每个部分各占一部分内存。
  
    当程序没有对数据层面内存大小做限制时，输入一个超过数据内存大小的数据就会发生数据层面的数据把控制层面内存覆盖的情况，此时如果在数据尾部加上一些操作系统指令就会把该指令加载到控制层面
  
    内存（即寄存器）当中去，当CPU执行下一个控制层面内存里的内容时就会加载该恶意指令。

### 7. 在大作业编译器中，尝试完成下面的任务

#### Exercise 8.3

This abstract syntax for preincrement `++e` and predecrement `--e` was
introduced in Exercise 7.4:

```fsharp
type expr =
...
| PreInc of access (* C/C++/Java/C ++i or ++a[e] *)
| PreDec of access (* C/C++/Java/C --i or --a[e] *)
```

Modify the compiler (`function cExpr`) to generate code for `PreInc(acc)` and
`PreDec(acc)`. To parse micro-C source programs containing these expressions,
you also need to modify the lexer and parser.
It is tempting to expand `++e` to the assignment expression `e = e+1`, but that
would evaluate e twice, which is wrong. Namely, e may itself have a side effect, as
in `++arr[++i]`.
Hence e should be computed only once. For instance, `++i` should compile to
something like this: 

`<code to compute address of i>;, DUP, LDI, CSTI 1, ADD, STI`, 

where the address of `i` is computed once and then duplicated.
Write a program to check that this works. If you are brave, try it on expressions of
the form `++arr[++i]` and check that i and the elements of arr have the correct
values afterwards.

#### Exercise 8.6

Extend the lexer, parser, abstract syntax and compiler to implement\
switch statements such as this one:

```fsharp

switch (month) {
    case 1:
        { days = 31; }
    case 2:
        { days = 28; if (y%4==0) days = 29; }
    case 3:
        { days = 31; }
}
```

Unlike in C, there should be no fall-through from one case to the next: after the

last statement of a case, the code should jump to the end of the switch statement.

The parenthesis after switch must contain an expression. The value after a case

must be an integer constant, and a case must be followed by a statement block.

A switch with n cases can be compiled using n labels, the last of which is at the

very end of the switch. For simplicity, do not implement the break statement or

the default branch.

#### Exercise 8.7

(Would be convenient) Write a disassembler that can display a machine code program in a more readable way. You can write it in Java, using a variant

of the method insname from Machine.java.

#### Exercise 8.9

Extend the language and compiler to accept initialized declarations\
such as

```fsharp
int i = j + 32;
```

Doing this for local variables (inside functions) should not be too hard. For global
ones it requires more changes.

## 实验要求

1. 完成下面各题目
2. 使用Markdown文件完成，推荐Typora
3. 使用[Git](https://learngitbranching.js.org/)工具管理作业代码、文本文件
