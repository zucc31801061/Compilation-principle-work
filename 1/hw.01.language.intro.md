# **2020-21学年第2学期**

## **实 验 报 告**

![zucc](.\img\zucc.png)

- 课程名称:  <u>编程语言原理与编译</u>
- 实验项目: <u>语言分析与简介</u>
- 专业班级:<u>计算机1801</u>
- 学生学号:<u>31801061</u>
- 学生姓名:<u>王灵霜</u>
- 实验指导教师:<u>郭鸣</u>

### 实验目的

- 掌握编程语言的发展历史
- 编程语言的类型系统、控制结构、模块机制
- 理解编程范式的概念

### Ruby语言分析报告

- 语言范式

  面向对象编程

  ![5](.\img\5.png)

- 语言支持的类型

  - 强/弱类型

    强类型

    ![2](.\img\2.png)

    ![4](.\img\4.png)

  - 静态/动态类型

    动态类型

    ![1](.\img\1.png)

  - 基本类型

    Ruby支持的数据类型包括基本的Number、String、Ranges、Symbols，以及true、false和nil这个几个特殊值，同时还有两种重要的数据结构Array和Hash。

    - 书写整数时，可以根据需要在整数之间任意加入下划线而不会影响数字的值

      ![7](.\img\7.png)

    - 内置Math模块 Math::PI ，Math::exp(10) 计算e的10次幂

      ![8](.\img\8.png)

    - `2**2`表示2的2次方 , `a**=2`等价`a=a**2`

      ![9](.\img\9.png)

    - 输出长字符串非常自由,%Q会解析转义

      ![14](.\img\14.png)

    - `str = "abcde"` `str.delete("d")`与`str.delete!("d")`区别，后者作用于本身

    - 字符串和符号对象可以通过 `tos` 和 `tosym` 来进行相互转化

    - a = 1..5 与 b = 1...5 区别，a包含5，b不包含5

      ![10](.\img\10.png)

    - 区间转数组

      ![11](.\img\11.png)

    - 区间的迭代

      ![12](.\img\12.png)

    - 专门创建数组的方式 "%w"，省去了加很多引号的时间 

      ![13](.\img\13.png)

    - 含有5个1的数组 `a=Array.new(5,1)`

      ![15](.\img\15.png)

    - 删除指定索引位置的元素 `arr.delete_at(2)`，删除指定的元素`arr.delete(3)`

      ![16](.\img\16.png)

    - `arr.each`对每个元素进行操作，原数组不变

      ![17](.\img\17.png)

    - map 产生新的数组，如果map!直接作用于原数组 `puts arr.map{ |a| a**2 } puts arr`

      ![18](.\img\18.png)

    - `s = Hash[1=>'a',2=>'b']` 等价于 `s = {1=>"a", 2=>"b"}`

    - 遍历哈希

      ```ruby
      ar = {"name"=>"jack","age"=>18,"color"=>"red"}
      for key,value in ar do
          puts key + '--' + value.to_s
      end
      ```

    - 使用迭代器

      ```ruby
      ar.each do |name|
          puts name[0].to_s + '--' + name[1].to_s
      end
      #另一种写法
      ar.each do |v,k|
          puts v.to_s + '--' + k.to_s
      end
      ```

    - `puts 5<=>8` 左边对象小返回-1，相等0，否则1

    - `===` 当普通对象处于运算符的左边时，该运算符与“==”功能相同； 但左边的对象是一个 Range 对象，且右边对象包含在该 Range 内时，返回 true，否则返回 false (1..12)===8 返回 true

    - 并行赋值

      ![19](.\img\19.png)

    - 表达式后跟if后while条件 `puts "good" if s >= 10`

    - unless 与if相反，条件为true执行else后面的语句

      ```ruby
      unless (条件) then
          代码块1
      else
          代码块2
      end
      ```

    - case，每个条件后面都有break效果，即score=1只会输出"悲催了"

      ![20](.\img\20.png)

    - break 跳出整个循环

      ```ruby
      # loop会无限次循环，必须使用break跳出循环
          i = 0;
          loop{
              i += 1
              if (i>=3)    break
          }
      ```

    - next 进入下一个循环，redo不会检查循环条件是否成立，就执行下一个循环

      ```ruby
      i = 1
      while(i&lt;=5) do 
          if (i == 5)
              i+=1
              #redo  #多输出6
              next
          end
          puts "当前i：" + i.to_s
          i+=1    
      end
      ```

  - 复合类型

    用class关键字自定义扩展的复合类型（class的attribute 是由基本类型组成）。

- 语言支持的执行方式

  解释执行

  ![3](.\img\3.png)

- 语言的特殊特性

  - coroutine

    Ruby1.9分支中有一个新特性——纤程(Fiber)，它使用了一个叫做协程(Coroutine)的概念。基本上，调用` Fiber.yield`方法会停止（“挂起”）这段代码的执行。fib是这个纤程的句柄，你可以用它对纤程进行操作。`fib.resume`所完成的正是该代码所表达的意思：它恢复了`Fiber.yield`调用之后语句的执行，使该纤程恢复执行状态。`Fiber.yield`带着一个参数`y`被调用。在某种程度上，这可以被认为是与`return y`相类似。`return`和协程的`Fiber.yield`之间的区别在于代码调用后其上下文发生的变化上。这就是说，`return`的意思是一个函数调用的激活格局（或者叫做栈格局）被取消分配了（deallocate），即所有的本地变量都消失了。在一个协程里，`yield`保存着这个激活格局并且其内部的所有数据还是存在的，因此在调用了`resume`方法之后，代码还能继续使用这些环境。

    ![6](.\img\6.png)

  - yield

    yield关键字在ruby中表示调用块。yield的出现应该满足两个条件：一个方法的定义，在方法内部有yield的出现；一个是方法的调用处，在方法的调用处会有程序块的出现。

    ![24](.\img\24.png)

  - actor

    coroutine可以实现actor风格，actor跟coroutine并没有必然的联系。调度器顾名思义就是协调actor的运行，每次挑选适当的actor并执行，可以想象调度器内部应该维护一个等待调度的actor队列，Scheduler每次从队列里取出一个actor并执行，执行完之后取下一个actor执行，不断循环持续这个过程；在没有actor可以调度的时候，调度器应该让出执行权。因此调度器本身也是一个Fiber，它内部有个queue，用于维护等待调度的actor。

    ```ruby
    class Thread
       # 得到当前线程的调度器
       def  __scheduler__
        @internal_scheduler||=FiberActor::Scheduler.new
      end
    end
    
    class Fiber
       # 得到当前Fiber的actor
       def  __actor__
        @internal_actor
      end
    end
    module FiberActor
       class Scheduler
         def initialize
          @queue=[]
          @running=false
        end
    
         def run
           return  if @running
          @running=true
           while true
             # 取出队列中的actor并执行
             while actor=@queue.shift
              begin
                actor.fiber.resume
              rescue => ex
                puts  " actor resume error,#{ex} "
              end
            end
             # 没有任务，让出执行权
            Fiber. yield
          end
        end
    
         def reschedule
           if @running
             # 已经启动，只是被挂起，那么再次执行
            @fiber.resume
           else
             # 将当前actor加入队列
            self << Actor.current
          end
        end
    
         def running?
          @running
        end
    
         def <<(actor)
           # 将actor加入等待队列
          @queue << actor unless @queue.last == actor
           # 启动调度器
          unless @running
             @queue << Actor.current
             @fiber=Fiber.new { run }
             @fiber.resume
          end
        end
      end
    end
    module FiberActor  
       class Actor
        attr_accessor :fiber
         # 定义类方法
         class << self
           def scheduler
            Thread.current. __scheduler__
          end
    
           def current
            Fiber.current. __actor__
          end
    
           # 启动一个actor
           def spawn(*args,&block)
            fiber=Fiber.new do
               block.call(args)
            end
            actor=new(fiber)
            fiber.instance_variable_set :@internal_actor,actor
            scheduler << actor
            actor
          end
    
           def receive(&block)
            current.receive(&block)
          end
        end
    
         def initialize(fiber)
           @mailbox=[]
           @fiber=fiber
        end
    
         # 给actor发送消息
         def << (msg)
          @mailbox << msg
           # 加入调度队列
          Actor.scheduler << self
        end
    
         def receive(&block)
           # 没有消息的时候，让出执行权
          Fiber. yield  while @mailbox.empty?
          msg=@mailbox.shift
          block.call(msg)
        end
    
         def alive?
          @fiber.alive?
        end
      end
    
    end
    ```

  - mixin

    ![21](.\img\21.png)

    ![22](.\img\22.png)

    ![23](.\img\23.png)

    在Ruby中，我们可以把一个模块混入（Mixin）到对象中，从而达到类似多重继承的效果。

- 鸭子类型

  ruby高效的优势之一就是其类系统中，多个类不必继承自相同的父亲就可以用同样的方式来使用。以下示例中含有int、float、string虽然都是不同类型但是都用同一种方法to_i来实现。

  ![25](.\img\25.png)

- 特点

  简单来说，特点就是容易写，容易读，语法功能强大，软件包很多。

  - Ruby 是面向对象语言。Ruby 提供了机制，将数据和方法封装到对象里，实现了一个类到另一个类的继承机制，还提供对象多态机制。

  - Ruby 是真正的OOP语言。Ruby所有的一切——包括字符串或整型之类的基本数据类型——都是以对象的形态来表达的。

  - Ruby 是支持多种平台的语言。Ruby 可以运行在Linux及其他UNIX变体、各种版本Windows平台、BeOS，MS-DOS 等。

  - Ruby 是开源的。

  - Ruby 具有异常（exception）机制。

  - Ruby 是可扩展的。

  - Ruby 具有安全性特性。

  - Ruby 具有灵活的语法特性。

  - Ruby 有丰富的程序库可供使用。

- 应用领域

  适合于快速开发，互联网应用方面使用比较广泛，其实一般的程序也可以使用。

- 历史

  Ruby，一种简单快捷的面向对象（面向对象程序设计）脚本语言，在20世纪90年代由日本人松本行弘开发，遵守GPL协议和Ruby License。它的灵感与特性来自于Perl、Smalltalk、Eiffel、Ada以及Lisp语言。但其归根结底源于Perl和Lisp两类语言，与C，C++，C#，Java是不同大类。