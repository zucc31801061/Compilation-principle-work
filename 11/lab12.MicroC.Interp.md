# 2020-2021学年第2学期

##  实 验 报 告

![zucc](.\img\zucc.png)

- 课程名称: <u>编程语言原理与编译</u>

- 实验项目: <u>MicroC解释器</u>

- 专业班级: <u>计算机1801</u>

- 学生学号: <u>31801061</u>

- 学生姓名: <u>王灵霜</u>

- 实验指导教师:<u>郭鸣</u>

## 实验内容

### 1. 阅读课件 [MicroC实现,解释器 - 编程语言与编译](http://sigcc.gitee.io/plc2021/#/06/microc.interp)

### 2. 阅读 [计算的本质](https://bb.zucc.edu.cn/bbcswebdav/users/j04014/books/Understanding.Computation)第1 2 3章

- [计算的本质](https://bb.zucc.edu.cn/bbcswebdav/users/j04014/books/Understanding.Computation)

  计算的本质是依据一定的法则通过对有关符号串的变换来获得信息的一种过程。

- 请说明大步语义，小步语义的区别

  小步语义。就是假想一台机器，用这台机器直接按照这种语言的语法进行操作一小步一小步地对其进行反复规约，从而对一个程序求值。小步语义大部分都带有迭代的味道，它要求抽象机器反复执行规约步骤（Machine#run中的 while 循环），这些步骤以及与它们同样类型的信息可以作为自身的输入和输出，这让它们适合这种反复进行的应用程序。

  大步语义。定义如何从一个表达式或者语句直接得到它的结果。这必然需要把程序的执行当成一个递归的而不是迭代的过程：大步语义说的是，为了对一个更大的表达式求值，我们要对所有比它小的子表达式求值，然后把结果结合起来得到最终答案。大步规约的规则描述了如何只对程序的抽象语法树访问一次就计算出整个程序的结果，因此不需要处理状态和重复。我们将只对表达式和语句类定义一个#evaluate 方法，然后直接调用它。

### 3. 阅读课件 2.call.by.parameters.pdf

- 请说明 Call by reference, Call by value的区别

  call by value：参数在函数调用之前进行求值，函数接受参数的副本（形参），因此在函数中改变不会影响外面的值。

  call by reference：在函数调用发生之前对参数进行求值，参数传递的是它的指针，即地址，因此在函数内部改变其值会导致外部的值也同时改变。

- (选做)请说明什么是Call by need 

  参数被封装在容器中，传递参数时则传递容器，当某个thunk第一次被调用时会把参数的值保存到缓存中，再次调用时则直接从缓存中读取。

### 4. 阅读简单命令式语言代码`imp.zip`(自选)

- 理解命令式语言**存储模型**

- 写出函数`setSto` `getSto` 的类型声明

  ```fsharp
  // 存储就是(变量名，变量值)形式，因此在代码中定义为map从string到int的映射
  type naivestore = Map<string,int>
  // getSto给定某个存储和变量名从中得到该变量的值
  val getSto : store:naivestore -> x:string -> int
  // setSto给定(变量名， 变量值)pair以及存储后更新该存储的内容
  val setSto : store:naivestore -> k:string * v:int -> Map<string,int>
  ```

- 请说明 命令式语言与函数式语言**执行模型**的不同之处

  命令式编程：专注于”如何去做”，不管”做什么”，只是按照命令去做。解决某一问题的具体算法实现。
  函数式编程：把运算过程尽量写成一系列嵌套的函数调用。函数式编程强调没有”副作用”，意味着函数要保持独立，所有功能就是返回一个新的值，没有其他行为，尤其是不得修改外部变量的值。所谓”副作用”（side effect），指的是函数内部与外部互动（最典型的情况，就是修改全局变量的值），产生运算以外的其他结果。

  两者很类似，函数式没有状态，只有函数参数。

### 5. 阅读`MicroC` 解释器代码

- 请说明 抽象语法树中 对**左值和右值**的表示方式

  AccVar ("x") 获取x的地址

  Prim2("+" , Access(AccVar ("x")), CstI 5)  访问x的地址内的实际变量值

- 请说明 表达式`a[i] + x` **左值求值**和**右值求值**的过程,需要调用解释器的哪些方法

  左值：调用access函数求左值，数组通过access求值得到地址a，通过eval求值index得到 i，最终返回(a+i) 

  右值：调用eval函数求右值，通过access函数获取x的地址，通过getSto函数获取x在store中的值

- 请写出 `MicroC` 解释器中以下3个函数的类型声明,说明每个参数的含义

```fsharp
eval
eval : expr -> locEnv -> gloEnv -> store -> int * store
输入: 表达式expr，局部环境locEnv，全局环境gloEnv，当前存储store
输出: 表达式expr的值(int类型)，和被修改的store

exec
exec : stmt -> locEnv -> gloEnv -> store -> store
输入：执行语句stmt，局部环境locEnv，全局环境gloEnv，当前存储store
输出：更改后的存储store

access
access : access -> locEnv -> gloEnv -> store -> address * store
输入: 待求值的access类型（变量x，指针*p，数组a[4]），局部环境locEnv,全局环境gloEnv,当前存储store
输出: access的左值（地址），store
```

- 用解释器 运行 `ex9.c` 给出运行结果. 说明递归调用过程.

  ![3](.\img\3.png)

- gitee.com/sigcc/plzoofs microc目录 完成 `ReadME.md`中的A部分.

  ![1](.\img\1.png)

  ![2](.\img\2.png)

### 6. 在ex1.c 中

- 加上这行 代码 h[4] = 5; 此时会发生数组越界

- 请解释一下程序运行的结果

  ![4](.\img\4.png)

### 7. 修改解释器，提示越界访问错误（自选）

### 预习下章 micro C stack machine 指令系统 重点理解

```bash
LDI
STI
GETBP
GETSP
CALL
RET
等指令
```

请使用编译器 输出 ex9.c的指令代码

## 实验要求

1. 完成下面各题目
2. 使用Markdown文件完成，推荐Typora
3. 使用[Git](https://learngitbranching.js.org/)工具管理作业代码、文本文件
