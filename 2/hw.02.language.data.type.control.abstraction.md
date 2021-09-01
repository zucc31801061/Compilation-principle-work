# 2020-21学年第2学期

## 实 验 报 告

![zucc](.\img\zucc.png)

- 课程名称: <u>编程语言原理与编译</u>
- 实验项目: <u>类型模块与控制流</u>
- 专业班级: <u>计算1801</u>
- 学生学号: <u>31801061</u>
- 学生姓名: <u>王灵霜</u>
- 实验指导教师: <u>郭鸣</u>

### 实验目的

- 掌握FSharp、OCaml语言的发展历史
- 编程ML系列语言的类型系统、控制结构


### 实验内容

1. 阅读

      - plan04-1.pdf 类型，理解下面的概念
        -   类型推理
        -   类型检查
        -   类型相容

      - plan07.pdf 模块
        -   模块组织
        -   模块接口定义

1. 学习 F#/OCaml 语言 
   
    - 参考[lec.fsharp.ocaml.zip](https://bb.zucc.edu.cn/bbcswebdav/users/j04014/PLC/backup/lec.fsharp.ocaml.zip)
    - 参考 [F#入门](http://sigcc.gitee.io/plc2021/#/02/fsharp)
      - code/lecture.fs 通读理解后，运行代码
      
        ![1](.\img\1.png)
      
      - FSharpKoans-fsharp 项目
        - 进入`FSharpKoans-fsharp/FSharpKoans` directory and run `dotnet watch run`
        
        - 参考程序运行的提示，修改对应代码，完成公案 FSharpKoans
        
          ![2](.\img\2.png)
    - 教材p3 1.3 直线式程序解释器 straight_line_program_interpreter.ml 理解解释器（自选）
      - 中文版C语言程序有些晦涩，参考[Modern.compiler.implementation.in.ML.pdf](https://bb.zucc.edu.cn/bbcswebdav/users/j04014/PLC/book/Modern.compiler.implementation.in.ML.pdf) 
      - 将代码迁移到 F#（自选）
    
1. 用F#/OCaml完成 教材习题
      - p9 1.1 a b
      

  先用F#复现书上的代码，插入节点
```F#
type tree = Leaf | Node of tree * int * tree;;

let rec insert key t =
    match t with
    | Leaf -> Node(Leaf, key, Leaf)
    | Node(left ,k ,right) -> 
        if key<k then
            Node(insert key left, key, right)
        else
            Node(left, key, insert key right);;
```

​    a

```F#
let rec search key t =
    match t with
    | Leaf -> false
    | Node(left ,k ,right) -> 
        if key = k then
            true
        else if key < k then
            search key left
        else
            search key right;;
```

​    ![3](.\img\3.png)
​      

​    将当前插入的信息节点赋值给指针bind

```F#
let rec add key bind t =
    match t with
    | Leaf -> bind := Node(Leaf, key, Leaf)
              Node(Leaf, key, Leaf)
    | Node(left ,k ,right) -> 
        if key<k then
            Node(add key bind left, k, right)            
        else if key>k then
            Node(left, k, add key bind right)
        else
            bind := t
            Node(left ,k ,right);;
```

​    b

```F#
let rec lookup key t =
    match t with
    | Leaf -> Leaf
    | Node(left ,k ,right) ->
        if key = k then
            t
        else if key < k then
            lookup key left
        else
            lookup key right;;
```

​    ![4](.\img\4.png)

  - 选做 p9 1.1 c d

1. 在你会的语言中 ，如`C Java Python `里面 是如何实现模块机制的，请列出相关的代码说明，至少1种语言。

   jdk：Java11

   IDE：IDEA

   源码在文件夹module中

   模块化严格限定了不同模块之间不同包的访问。使用Java进行模块化实践具体如下：

   创建项目，并在其中创建多个模块（java即可，不需要maven）

   ![6](.\img\6.png)

   在各个子模块下创建module-info.java

   ![7](.\img\7.png)

   每个模块导入的包exports在这个文件里面，需要用到的包所对应的模块用requires，以api为例

   ```java
   module api {
       exports com.test.api;
       requires base;
       requires service;
       requires util;
   }
   ```

   exports只能对包进行报错, 不能对类进行操作。

   exports 的包不能为空, 也不能不存在。

   如果事先写了requires ,但是在代码中没有引用到该模块的内容,module-info中会报错。

   模块中不包含的包，即使引用了模块，且该包在该模块对应的项目中，也无法使用。

   util中含两个包，但只exports了一个

   ![8](.\img\8.png)

   ```java
   module util {
       exports com.test.util;
   }
   ```

   故在api中

   ```java
   public class UserServiceImpl implements UserService {
       @Override
       public void save(User user) {
           /**
            * 成功引用util的包内容
            */
           MyStringUtil.size("123");
   
           /**
            * ListUtil此类  这个导包就导入不进来
            */
           //ListUtil.size();
   
           System.out.println(user);
       }
   
       @Override
       public void login() {
           System.out.println("UserServiceImpl login method");
       }
   }
   ```

   Test

   ```java
   public class Test {
       public static void main(String[] args) {
           UserServiceImpl service = new UserServiceImpl();
           service.login();
           User user = new User(1, "wls");
           service.save(user);
       }
   }
   ```

   运行结果

   ![5](.\img\5.png)

1. 在语言分析报告中

- 分析某语言的数据类型，用例子说明该语言的以下特性：
  - 基本类型
   - 复合类型（提供了哪些机制）
    - 抽象数据类型 （class）
    - 强类型，弱类型
    - 静态类型，动态类型
  
- 分析某语言的控制流抽象机制，请问有没有新的语言提供如下的控制流机制
    - 协作程序Coroutine
    - 异步Async
    - 并发 Parallel
    - 如 Lua C# Pony Lisp  Javascript  语言里面的新特性
    
### 提交方式

- 打包zip上传到bb

- 实验报告采用Markdown格式

- zip内容包括Markdown文本、代码、部分体现实验过程的典型截屏(.png格式)

  

