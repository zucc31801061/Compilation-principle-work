# **2020-21学年第2学期**

## **实 验 报 告**

![zucc](.\img\zucc.png)

-   课程名称:编程语言原理与编译

-   实验项目: 语法分析`LL(1)` 递归下降分析

-   专业班级:计算机1801

-   学生学号:31801061

-   学生姓名:王灵霜

-   实验指导教师:郭鸣

## 实验内容

1. 阅读
    - `Recursive Descent Parsing.pdf`
    - 阅读课本`3.2`章节

1. 理解BNF  (Backus-Naur Form) 巴科斯范式的写法

    在双引号中的字("word")代表着这些字符本身。而double_quote用来代表双引号。

    在双引号外的字（有可能有下划线）代表着语法部分。

    尖括号( < > )内包含的为必选项。

    方括号( [ ] )内包含的为可选项。

    大括号( { } )内包含的为可重复0至无数次的项。
    
    竖线( | )表示在其左右两边任选一项，相当于"OR"的意思。
    
    ::= 是“被定义为”的意思。

  - `< >` 表示非终结符  
  - `::=`  表示产生式规则 
```sh
<expression> ::= <number>
    | <expression> + <expression>
    | <expression> - <expression>    
    | <expression> * <expression>   
    | <expression> / <expression>   
    | ( <expression> )
<number> ::= <digit> | <number> <digit>
<digit> ::= 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9
```

- 扩展的巴科斯范式  **`EBNF`**  ,引入了类似于正则表达式的机制

- 方便采用递归下降分析实现分析器
   - `( )` 表示分组
   - `*` 表示重复 `0` 个或多次 有时用 `{ }` 表示
   - `+` 表示重复 `1` 个或多次
   - `?` 表示可选项 有的用 `[ ]` 表示
   - `'a'` 表示终结符

```sh
    EXP → TERM (('+' | '-') TERM)*
    TERM → FACTOR (('\*' | '/') FACTOR)*
    FACTOR → NUMBER | '(' EXP ')'
    NUMBER → ('0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9')+
```

阅读 `ustc-pl0.zip` 中 `PL/0`语言的`BNF` 范式并理解

3. 设有文法

```sh
1. S → ABD
2. A → aA
3. A → ε
4. B → bB
5. B → ε
6. D → dD
7. D → ε
```


   参考 https://gitee.com/sigcc/plzoofs/tree/master/expr   rdp目录
   - 阅读 `Recursive Descent Parsing.pdf`

   - 完成文法的递归下降解析器 `Java`语言实现(可选)

     ```java
     public class rdp  {
         static String expression;
         static Character sym;
         static int index;
         static boolean hasErr;
         
         public static void main(String []main){
             Scanner input=new Scanner(System.in);
             expression=input.next();
             while (!expression.equals("end")){
                 expression+="#";
                 index=-1;
                 hasErr=false;
                 advance();
                 S();
                 if (hasErr){
                     error();
                 }
                 else if (sym=='#'){
                     System.out.println(expression.replace("#","")+"是合法的");
                 }else{
                     error();
                 }
                 expression=input.next();
             }
         }
     
         public static void S(){
                 A();
                 B();
                 D();
         }
     
         public static void A(){
             if (sym=='a'){
                 advance();
                 A();
             }
         }
         
         public static void B(){
             if (sym=='b'){
                 advance();
                 B();
             }
         }
         
         public static void D(){
             if (sym=='d'){
                 advance();
                 D();
             }
         }
     
         public static void advance(){
             index++;
             sym=expression.charAt(index);
         }
         
         public static void error(){
             System.out.println(expression.replace("#","")+"是不合法的");
         }
     
     }
     ```

   - 完成文法递归下降解析器 `FSharp`语言实现

     ```fsharp
     exception ParseError
     
     // 1. S → ABD  First(S) = {a, b, d}
     // 2. A → aA
     // 3. A → ε Follow(A) = {b, d}
     // 4. B → bB
     // 5. B → ε Follow(B) = {d}
     // 6. D → dD
     // 7. D → ε
     
     let rec A =  
         function
         | 'a' :: tr -> A tr // rule 2
         | 'b' :: tr -> 'b' :: tr // rule 3
         | 'd' :: tr -> 'd' :: tr // rule 3
         | _ -> raise ParseError
     and B = 
         function
         | 'b' :: tr -> B tr // rule 4
         | 'd' :: tr -> 'd' :: tr // rule 5
         | _ -> raise ParseError 
     and D = 
         function
         | 'd' :: tr -> D tr // rule 6
         | t -> t 
     
     let S ts = 
          A ts |> B |> D // rule 1
     
     let parse ts = 
         match S ts with
         | [] -> printfn "Parse OK!"
         | _ -> raise ParseError
     
     parse ['a'; 'b'; 'd';]  // accpet
     parse ['a'; 'b'; 'd';  'a';] // error
     ```


4. 实现 3.2节 SLP文法的递归下降分析程序

   ```fsharp
   type id = string
   type binop = 
       | Plus
       | Minus
       | Times
       | Div
       | Assign 
       
   type parenthesis = 
       | Left
       | Right
   
   type stm = 
       | CompoundStm of stm * stm
       | AssignStm of id * exp
       | PrintStm of explist // ( )
   and exp = 
       | IdExp of id 
       | NumExp of int 
       | OpExp of exp * binop * exp
       | EseqExp of stm * exp  // ( )
   and explist =
       | PairExpList of exp * explist
       | LastExpList of exp
   
   let rec stm_d tokens = 
       match tokens with
       | id :: after_id -> // assign
           match after_id with
           | Assign :: after_assign -> 
               match exp_d after_assign with
               | (exp_parse, tokens_after_exp) -> (AssignStm(id, exp_parse), tokens_after_exp)
       
   and exp_d tokens =  // (5 Plus 3)
       match tokens with
       | exp e:: after_e ->
           match exp_d after_e with
           | binop b :: after_binop ->
               match exp_d after_binop with
               | exp ex:: after_ex -> (OpExp(e, b, ex), after_ex)
   
   let e = 
   [ id "a"
     Assign
     5
     Plus
     3
   ]
   
   stm_d e
   ```
   
   - 词法分析部分
     - 参考 https://gitee.com/sigcc/plzoofs/blob/master/Fun/ 简单的函数式语言的词法分析器
     -  https://gitee.com/sigcc/plzoofs/blob/master/Fun/main.fsx 简单的函数式语言的例子
   
5. （选做）用表驱动的方法，实现一个下推自动机 PDA ，分析`LL(1)`语法

6. 大作业分组登记, 大作业参考资料

   准备考虑大作业思路参考[任务列表](https://gitee.com/sigcc/plzoofs/blob/master/microc/task.md)

   1.语言用 fsharp /ocaml   

   2.设计自己的语言或改进现有语言

   3.设计新的语言特性

   4.中间代码 Stack IR, 线性IR

   5.目标代码 LLVM WASM MIPS ARM

   6.解释器,编译器


### 提交方式

- 打包zip上传到bb

- 实验报告采用Markdown格式

- zip内容包括Markdown文本、代码、部分体现实验过程的典型截屏(.png格式)

  