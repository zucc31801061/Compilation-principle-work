# **2020-21学年第2学期**

## **实 验 报 告**

![zucc](.\img\zucc.png)

-   课程名称: <u>编程语言原理与编译</u>

-   实验项目: <u>语法分析 LR分析</u>

-   专业班级: <u>计算1801</u>

-   学生学号: <u>31801061</u>

-   学生姓名: <u>王灵霜</u>

-   实验指导教师: <u>郭鸣</u>

## 实验内容

1. 阅读ppt,阅读教材第3章

- 理解LR（0） DFA的构建过程

- 理解如何从DFA状态图，进行LR分析表的构建

- 教材 p50 3.4.3 理解冲突产生原因
  
  移进—归约冲突指的是在移进和归约之间进行选择
  
  归约—归约冲突指的是在两条不同的规则之间进行归约的一种选择
  
  - p51 理解`图3-13` `LR 状态表`，找到其中的冲突项
  
    从state13转为state 17时会遇到由悬挂else导致的移进—归约冲突
  
  - 找到其中的 `Action Table（动作表）` ，`Goto Table（状态转换表）`的定义
  
    action[sm, ai]: 表示当状态sm面临输入ai时的动作
    动作主要包括四种:
  
    - Shift s(移进): 将ai和s=action[sm, ai]压入栈中
    - Reduce A→β(归约): 用产生式A→β归约
    - Accept(接受): 停机，接受，成功
    - Error(报错): 调用出错处理
  
- 教材 p50 3.4.2.优先级指导 
  - 理解在语法说明文件中，优先级的指定方式 
    - 什么时左/右结合/非结合 ，如何在语法说明文件里面声明 p53

      左结合：从左向右运算

      右结合：从右向左运算

      非结合：不进行结合的操作

      声明：

      %left：左结合

      %right：右结合

      %nonassoc： 非结合

    - 如何用 `%prec` 指示，自定义某规则的优先级 p53

      使用%prec指导命令把终结符（terminal）所具有的优先级赋予当前的规则

      这个%prec指示本条规则“| '-' exp”具有与NEG相同的优先级
  - [http://mdaines.github.io/grammophone/#](http://mdaines.github.io/grammophone/) 核对你的作业
  - 请勿抄袭


2. 设有如下文法 

```sh
S -> S A b .
S -> a c b .
A -> b B c .
A -> b c .
B -> b a .
B -> A c .
```
分析栈上的内容如下,请分别写出可归约串是什么（▽ 表示栈底）：
```sh
(a)▽SSAb
(b)▽SSbbc
(c)▽SbBc
(d)▽Sbbc
```

(a) SAb

(b) bc

(c) bBC

(d) bc

3. 设有如下输入串，请用2中的文法，采用 shift/reduce分析下面的串。

   请按ppt 中 构造表格，列出 

   分析栈，输入流， shift/reduce操作 的内容

```sh
(a) acb
(b) acbbcb
(c) acbbbacb
(d) acbbbcccb
(e) acbbcbbcb
```

| 符号栈 | 输入串 | 操作              |
| ------ | ------ | ----------------- |
| $      | acb$   | 移进              |
| $a     | cb$    | 移进              |
| $ac    | b$     | 移进              |
| $acb   | $      | 归约 S -> a c b . |
| $S     | $      | 接受              |

| 符号栈 | 输入串  | 动作              |
| ------ | ------- | ----------------- |
| $      | acbbcb$ | 移进              |
| $a     | cbbcb$  | 移进              |
| $ac    | bbcb$   | 移进              |
| $acb   | bcb$    | 归约 S -> a c b . |
| $S     | bcb$    | 移进              |
| $Sb    | cb$     | 移进              |
| $Sbc   | b$      | 归约 A -> b c .   |
| $SA    | b$      | 移进              |
| $SAb   | $       | 归约 S -> S A b . |
| $S     | $       | 接受              |

| 符号栈 | 输入串    | 动作              |
| ------ | --------- | ----------------- |
| $      | acbbbacb$ | 移进              |
| $a     | cbbbacb$  | 移进              |
| $ac    | bbbacb$   | 移进              |
| $acb   | bbacb$    | 归约 S -> a c b . |
| $S     | bbacb$    | 移进              |
| $Sb    | bacb$     | 移进              |
| $Sbb   | acb$      | 移进              |
| $Sbba  | cb$       | 归约 B -> b a .   |
| $SbB   | cb$       | 移进              |
| $SbBc  | b$        | 归约 A -> b B c . |
| $SA    | b$        | 移进              |
| $SAb   | $         | 归约 S -> S A b . |
| $S     | $         | 接受              |

| 符号栈 | 输入串     | 动作              |
| ------ | ---------- | ----------------- |
| $      | acbbbcccb$ | 移进              |
| $a     | cbbbcccb$  | 移进              |
| $ac    | bbbcccb$   | 移进              |
| $acb   | bbcccb$    | 归约 S -> a c b . |
| $S     | bbcccb$    | 移进              |
| $Sb    | bcccb$     | 移进              |
| $Sbb   | cccb$      | 移进              |
| $Sbbc  | ccb$       | 归约 A -> b c .   |
| $SbA   | ccb$       | 移进              |
| $SbAc  | cb$        | 归约 B -> A c .   |
| $SbB   | cb$        | 移进              |
| $SbBc  | b$         | 归约 A -> b B c . |
| $SA    | b$         | 移进              |
| $SAb   | $          | 归约 S -> S A b . |
| $S     | $          | 接受              |

| 符号栈 | 输入串     | 动作              |
| ------ | ---------- | ----------------- |
| $      | acbbcbbcb$ | 移进              |
| $a     | cbbcbbcb$  | 移进              |
| $ac    | bbcbbcb$   | 移进              |
| $acb   | bcbbcb$    | 归约 S -> a c b . |
| $S     | bcbbcb$    | 移进              |
| $Sb    | cbbcb$     | 移进              |
| $Sbc   | bbcb$      | 归约 A -> b c     |
| $SA    | bbcb$      | 移进              |
| $SAb   | bcb$       | 归约 S -> S A b   |
| $S     | bcb$       | 移进              |
| $Sb    | cb$        | 移进              |
| $Sbc   | b$         | 归约 A -> b c     |
| $SA    | b$         | 移进              |
| $SAb   | $          | 归约 S -> S A b . |
| $S     | $          | 接受              |

4. 设有如下文法和输入串，请说明是否有shift/reduce冲突 或者 reduce/reduce 冲突

```sh
S -> S a b .
S -> b A .
A -> b b .
A -> b A .
A -> b b c .
A -> c .
```

输入串
```sh
(a) b c
(b) b b c a b
(c) b a c b
```

| 符号栈 | 输入串 | 动作                                              |
| ------ | ------ | ------------------------------------------------- |
| $      | bc$    | 移进                                              |
| $b     | c$     | 移进                                              |
| $bc    | $      | 归约  A -> c .                                    |
| $bA    | $      | 产生reduce/reduce冲突，对应 S -> b A . A -> b A . |

| 符号栈 | 输入串 | 动作                                          |
| ------ | ------ | --------------------------------------------- |
| $      | bbcab$ | 移进                                          |
| $b     | bcab$  | 移进                                          |
| $bb    | cab$   | 产生shift/reduce冲突，移进c或者归约A -> b b . |

| 符号栈 | 输入串 | 动作         |
| ------ | ------ | ------------ |
| $      | bacb$  | 移进         |
| $b     | acb$   | 移进         |
| $ba    | cb$    | 移进         |
| $bac   | b$     | 归约A -> c . |
| $baA   | b$     | 移进         |
| $baAb  | $      | 出错         |

5. 阅读lecture03.p31.fsyacc.pdf p31页 掌握fslex,fsyacc使用

   ![1](.\img\1.png)

   ![2](.\img\2.png)

   阅读 calcvar 中

- 词法说明 [lexer.fsl](https://gitee.com/sigcc/plzoofs/blob/master/calcvar/lexer.fsl)
- 语法说明 [parser.fsy](https://gitee.com/sigcc/plzoofs/blob/master/calcvar/parser.fsy)
- 调试运行代码
- 理解优先级指导的写法
  - 阅读 [ReadME]( https://gitee.com/sigcc/plzoofs/blob/master/calcvar/README.markdown)

6. plzoofs [calcvar](https://gitee.com/sigcc/plzoofs/tree/master/calcvar)项目，给`fsyacc` 工具添加 `-v` 参数，查看生成语法分析器的 LR 状态表
```html
// calcvar.fsproj 
<FsYacc *Include*="parser.fsy">
    <OtherFlags> -v --module Parser</OtherFlags>
</FsYacc>
```
注意下特定状态的
- action table

- goto table

  ![3](.\img\3.png)

  ![4](.\img\4.png)


7. 阅读Fun语言中

- 词法说明 [FunLex.fsl](https://gitee.com/sigcc/plzoofs/blob/master/Fun/FunLex.fsl)

- 语法说明 [FunPar.fsy](https://gitee.com/sigcc/plzoofs/blob/master/Fun/FunPar.fsyfsl)

- 调试运行代码

  ![6](.\img\6.png)

- 同上`fsyacc`工具添加 `-v`查看 `LR` 分析状态表

  ![5](.\img\5.png)

8. 参考p49,用fslex fsyacc 实现SPL的词法分析，语法分析，构造语法树

   - 参考 教材 p67，教材是C程序、yacc工具
   - 英文版教材是ML程序，请自行参考
   - 参考 [FunPar.fsy](https://gitee.com/sigcc/plzoofs/blob/master/Fun/FunPar.fsyfsl) 抽象语法树的构造
   
   SPL语法:

```sh

Stm -> Stm ; Stm
Stm -> id := Exp
Stm -> print ( ExpList )
Exp -> id
Exp -> num
Exp -> Exp Binop Exp
Exp -> ( Stm , Exp )
ExpList -> Exp , ExpList
ExpList -> Exp
Binop -> +
Binop -> -
Binop -> /
Binop -> *

```

SPL例子代码:

```pascal
a := 5 + 3; b := (print(a,a-1),10*a); print(b)
```


9. 阅读MicroC 语法分析器

   - https://gitee.com/sigcc/plzoofs/blob/master/microc/CPar.fsy
   - 由于 `C` 语言的指针，数组语法分析比较复杂，构造语法树时用到了比较高级的函数式编程技巧
   - 大家慢慢理解
10. Fsharp参考案例（自选）

   -   Postfix/  后缀式 运算 1 2 + 3 *
   -   Usql/   sql 语言语法解析

​    

​    

## 参考资料

​    

   http://bb.zucc.edu.cn/webapps/cmsmain/webui/users/j04014/PLC/book

   flex与bison 中文版 第二版 高清.pdf

### 提交方式

- 打包`zip`上传到`bb`

- 实验报告采用`Markdown`格式

- `zip`内容包括`Markdown`文本、代码、部分体现实验过程的典型截屏(`.png`格式)

  