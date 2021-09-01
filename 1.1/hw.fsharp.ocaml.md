# 2020-21学年第2学期

## 实 验 报 告

![zucc](.\img\zucc.png)

- 课程名称: <u>编程语言原理与编译</u>
- 实验项目: <u>FSharp OCaml语言</u>
- 专业班级: <u>计算机1801</u>
- 学生学号: <u>31801061</u>
- 学生姓名: <u>王灵霜</u>
- 实验指导教师: <u>郭鸣</u>

### 实验目的

- 掌握FSharp、OCaml语言的发展历史
- 编程ML系列语言的类型系统、控制结构


### 实验内容

1. FSharp
    - 阅读 
      - fsharp/fsharp.crash.course.pdf文件
      - [F#/OCaml简介](http://localhost:5001/plc2021/#/02/ocaml)
      
    - 运行 fsharp/code/Appendix.fs 里面的代码
      
      ![1](.\img\1.png)
      
      - `#q;;` 或 `Ctrl+D` 退出 `dotnet fsi` 命令行求值器
      
    - 请写出一个快速排序算法，注意 let rec 的使用。
    
      ```F#
      let rec quickSort (list : int list) =
          match list with
          | []    ->  []
          | [single] -> [single]
          | head :: tail ->
              let leftList = 
                      tail
                          |> List.choose(fun item -> if item <= head then Some(item) else None)
      
              let rightList = 
                      tail
                          |> List.choose(fun item -> if item > head then Some(item) else None)
      
              quickSort(leftList) @ [head] @ quickSort(rightList)
      ```
    
      ![2](.\img\2.png)
    
    - 阅读 [安装F#](http://sigcc.gitee.io/plc2021/#/install/fsharp)
      - Calc Demo 的代码 code/calcfs.zip 
      
      - 参考页面运行
      
        ![3](.\img\3.png)
- 参考[深度优先搜索](http://sigcc.gitee.io/plc2021/#/02/ocaml?id=深度优先搜索-4)，写移植到F#（自选）
  
1. OCaml（自选）
   - 阅读 [安装OCaml](http://sigcc.gitee.io/plc2021/#/install/ocaml)
   
   - ocaml目录下的 lecture.ml 中代码
   
   - [深度优先搜索](http://sigcc.gitee.io/plc2021/#/02/ocaml?id=深度优先搜索-4)
   
   - 在linux 里面 安装 ocaml 配置utop 求值环境
- 同上写出快速排序算法
  
  ……重装了系统虚拟机被我卸了，于是先不做吧。
1. VS Code 快捷方式
    - `Alt+Enter` 运行选中代码块
    - `Alt+/` 运行当前行

电子版教材是Standard ML 与OCaml的区别如下
[http://www.mpi-sws.org/~rossberg/sml-vs-ocaml.html](http://www.mpi-sws.org/~rossberg/sml-vs-ocaml.html)

### 提交方式

- 打包zip上传到bb

- 实验报告采用Markdown格式

- zip内容包括Markdown文本、代码、部分体现实验过程的典型截屏(.png格式)

  

