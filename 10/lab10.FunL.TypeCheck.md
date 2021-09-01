# 2020-2021学年第2学期

##  实 验 报 告

![zucc](.\img\zucc.png)

-   课程名称: <u>编程语言原理与编译</u>

-   实验项目: <u>函数式语言，类型检查</u>

-   专业班级: <u>计算机1801</u>

-   学生学号: <u>31801061</u>

-   学生姓名: <u>王灵霜</u>

-   实验指导教师:<u>郭鸣</u>



## 实验内容

1. 阅读 课件，阅读课本第5章、15 章、16 章

2. 阅读`3.monotype.p18-43.pdf` `type-rule-tutorial.pdf`

	- 理解各个语言语法构造类型检查的过程.
	- 掌握单态类型推理规则

3. 阅读 readings 下 `1.typecheck.ustc.pdf` 

    - 运行文件中 p39-45 例子程序,说明结果

      例1 编译时的控制流检查的例子

      continue需要在循环中才能起作用

      ![1](.\img\1.png)

      例2 编译时的唯一性检查的例子

      switch拥有两个相同的case

      ![2](.\img\2.png)

      例3

      ①指针类型不兼容![3](.\img\3.png)

      ②无错误

      ③无错误 

      ④运行结果如下

      ![4](.\img\4.png)

    - 在`gcc` 里面 编译 p58 的程序,根据运行结果回答问题

      ![5](.\img\5.png)

      c语言对结构体使用名字等价，所以第二个报错了，但是对其他常规类型使用结构等价，所以能赋值成功。

    - 写出下面 p56 对应的 `fsharp` 代码 ,构造该类型的值,查看其类型.

    ```c
    
    typedef struct cell {
        int info;
        struct cell  next;
    } cell;
    
    ```

    ```fsharp
    type cell = 
        | Next of int * cell
        | NULL
    ```

    - 学习p60多态函数部分 请给出一个多态的`Tree` 的定义,写出计算`Tree` 深度的多态函数 `depth  : 'a Tree -> int`

      ```fsharp
      type Tree<'T> =
          { left: Tree<'T> option
            data: 'T
            right: Tree<'T> option
            height: int }
      
      let depth (tree: Tree<'T> option) : int =
          match tree with
          | None -> 0
          | Some t -> t.height
      ```

    - 理解`类型协变`与`类型逆变`（自选）

4.  阅读`MicroML`[源代码](https://gitee.com/sigcc/plzoofs/blob/master/Fun/ReadME.md)

    - 函数定义/函数调用的实现
    
      函数定义：程序中一段具有特定功能的，可重用的代码
    
      函数调用：定义一个递归的函数作为求值的函数，对于传入的表达式，先判断合法性，之后进行求值
    
    - 阅读 结合教材第5章、课件里面的 环境与闭包 理解闭包概念
    
      - 结合代码，理解如何在环境中表示闭包
    
        环境记录了一系列名称–值的绑定关系，闭包是 <函数，函数声明环境> 的元组，
        其中函数表示为：参数 v1…vn -> 函数体exp
    
        ```fsharp
        plus_x = {…, x ->12, …}
        
        {plus_x -> <y -> y + x, plus_x >} + plus_x
        ```
    
        其中plus_x表示当前环境，元祖<,>则表示闭包，闭包中会记录当前环境plus_x
    
    - 静态作用域,动态作用域的实现
    
      - 类型检查的实现
    
    - 高阶函数的实现方法（自选）
    
      - [http://sigcc.gitee.io/plc2021/#/05/highOrder.typeInfer](http://sigcc.gitee.io/plc2021/#/05/highOrder.typeInfer)
    
    - 多态函数的类型归并unify 的实现（自选）
    
5. 理解程序设计语言的类型系统

    - 查看并运行`ExprType.fs` 查看 `a b c e1 e2 e3 eval` 的类型并加以说明

      ![6](.\img\6.png)

    - 请定义一个积类型 `string * string * int`，表达学生信息(姓名，学号，成绩) 请构造几个合法的值

      ```fsharp
      type stu = Student(string*string*int)
      let e1 = Student('王灵霜','31801061',1);
      ```

    - 请定义一个函数，使得函数类型为： `'a ->'a->'a`

      ```fsharp
      let a (p:'T): 'T -> 'T = 
          let b p:'T = p
          b
      ```

    - 查看并运行`ExprEnv.fs` 理解什么是求值环境 `env`

      ![7](.\img\7.png)

6. 阅读`4.unification.pdf`(自选)

    - 理解 多态类型推理的类型归并unify方法

7. 设计你大作业的类型系统,在大作业最终报告里面说明

    - 提供哪些基本类型 int/double/char/string/bool

    - 提供哪些类型复合机制  pair list array tuple

    - 完成哪些类型检查/类型推理机制

    - 强类型还是弱类型/静态类型还是动态类型 等.

8. 修改`MicroML` 的函数定义,支持多参数的函数

#### Exercise 4.3

For simplicity, the current implementation of the functional language
requires all functions to take **exactly one** argument. This seriously limits the programs that can be written in the language (at least it limits what that can be written
without excessive cleverness and complications).

Modify the language to allow functions to **take one or more** arguments. Start by
modifying the abstract syntax in` Absyn.fs` to permit a list of parameter names in
`Letfun` and a list of argument expressions in `Call`.
Then modify the eval interpreter in file `Fun.fs` to work for the new abstract
syntax. You must modify the closure representation to accommodate a list of parameters. Also, modify the Letfun and `Call` clauses of the interpreter. You will
need a way to zip together a list of variable names and a list of variable values, to
get an environment in the form of an association list; so function `List.zip` might
be useful.

#### Exercise 4.4

In continuation of Exercise 4.3, modify the parser specification to
accept a language where functions may take any (non-zero) number of arguments.
The resulting parser should permit function declarations such as these:

```fsharp
let pow x n = if n=0 then 1 else x * pow x (n-1) in pow 3 8 end
let max2 a b = if a<b then b else a
in let max3 a b c = max2 a (max2 b c)
in max3 25 6 62 end
end
```

### 附加题 (完成数量不限,需要单独提交)

#### Exercise 4.6

Extend the abstract syntax, the concrete syntax, and the interpreter for
the untyped functional language to handle tuple constructors `(...)` and component
selectors `#i` (where the first component is` #1`):

```fsharp

type expr =
| ...
| Tup of expr list
| Sel of int * expr
| ...

```

If we use the concrete syntax `#2(e)` for `Sel(2, e)` and the concrete syntax
`(e1, e2)` for `Tup[e1; e2]` then you should be able to write programs such as
these:

```fsharp

let t = (1+2, false, 5>8)
in if #3(t) then #1(t) else 14 end
and
let max xy = if #1(xy) > #2(xy) then #1(xy) else #2(xy)
in max (3, 88) end

```

This permits functions to take multiple arguments and return multiple results.
To extend the interpreter correspondingly, you need to introduce a new kind of
value, namely a tuple value `TupV(vs)`, and to allow eval to return a result of
type value (not just an integer):

```fsharp

type value =
| Int of int
| TupV of value list
| Closure of string * string list * expr * value env
let rec eval (e : expr) (env : value env) : value = ...
```

Note that this requires some changes elsewhere in the eval interpreter. For instance, the primitive operations currently work because eval always returns an
`int`, but with the suggested change, they will have to check (by pattern matching)
that eval returns an `Int i`.

#### Exercise 4.7

Modify the abstract syntax tyexpr and the type checker functions
typ and typeCheck in `TypedFun/TypedFun.fs `to allow functions to take
any number of typed parameters.
This exercise is similar to Exercise 4.3, but concerns the typed language. The
changes to the interpreter function eval are very similar to those in Exercise 4.3
and can be omitted; just delete the eval function.

#### Exercise 4.8

Add lists (`CstN` is the empty list `[]`, `ConC(e1,e2)` is `e1::e2`),
and list pattern matching expressions to the untyped functional language, where
`Match(e0, e1, (h,t, e2))` is match `e0` with
` [] -> e1 | h::t-> e2.`

```fsharp
type expr =
| ...
| CstN
| ConC of expr * expr
| Match of expr * expr * (string * string * expr)
| ..

```
## 实验要求

1. 完成各题目
2. 使用Markdown文件完成，推荐Typora
3. 使用[Git](https://learngitbranching.js.org/)工具管理作业代码、文本文件