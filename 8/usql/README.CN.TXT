Generating and compiling the micro-SQL lexer and parser (Usql/README.TXT)
-------------------------------------------------------------------------

运行命令生成 micro-SQL 词法分析器和语法分析器

   fslex --unicode UsqlLex.fsl
   fsyacc --module UsqlPar UsqlPar.fsy
   fsi   Absyn.fs UsqlPar.fs UsqlLex.fs Parse.fs

在 F# session F#会话中练习:

   open Parse;;
   fromString "SELECT Employee.name, salary * (1 - taxrate) FROM Employee";;

   #q;;
