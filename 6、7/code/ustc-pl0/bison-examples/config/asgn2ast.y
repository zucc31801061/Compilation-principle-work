/*
 * asgn.y : A simple yacc assignment statement parser
 *          Based on the Bison manual example. 
 * The parser constructs abstract syntax tree for the input program.
 * Author: Yu Zhang (yuzhang@ustc.edu.cn)
 */

%{
#include <stdio.h>
#include <math.h>
#include <common.h>
Table symtab;
ASTTree ast;
%}

%union {
	float fval;
	char *name;
	int  ival;
	ASTNode node;
}
%locations

%token NUMBER IDENTIFIER
%token PLUS MINUS MULT DIV EXPON ASGN
%token EOL
%token LB RB

%left  MINUS PLUS
%left  MULT DIV
%right EXPON

%type  <fval> NUMBER
%type  <ival> PLUS MINUS MULT DIV EXPON ASGN
%type  <name> IDENTIFIER
%type  <node> input line asgnexp exp 
%%
goal	: input
	  {
	    debug("goal ::= input\n");
	    ast->root = $1;
	  }

input   :
	  {
	    debug("input ::= \n");

	    $$ = newBlock();
	  }
	| input line
	  {
	    debug("input ::= input line\n");
	    addLast($1->block->stmts, $2);
	    $$ = $1;
	    setLoc($$, (Loc)&(@$));
	  }
	;

line    : EOL
	  {
	    debug("line ::= EOL\n");
	    $$ = NULL;
	  }
	| asgnexp ';' EOL 
	  { 
	    debug("line ::= asgnexp ';' EOL\n");
	    $$ = newExpStmt($1);
	    setLoc($$, (Loc)&(@$));
	  }
	;

asgnexp : IDENTIFIER ASGN exp    
	  {
	    debug("asgnexp ::= IDENTIFIER ASGN exp\n");
	    $$ = newAssignment($2, newName(symtab, $1), $3);
	    setLoc($$, (Loc)&(@$));
	  }
        ;

exp     : NUMBER
	  {
	    debug("exp ::= NUMBER\n");
	    $$ = newNumber($1);
	    setLoc($$, (Loc)&(@$));
	  }
	| IDENTIFIER
	  {
	    debug("exp ::= IDENTIFIER\n");
	    $$ = newName(symtab, $1); 
	    setLoc($$, (Loc)&(@$));
	  }
	| exp PLUS exp
	  {
	    debug("exp ::= exp PLUS exp\n");
	    $$ = newInfixExp($2, $1, $3); 
	    setLoc($$, (Loc)&(@$));
	  }
	| exp MINUS exp
	  {
	    debug("exp ::= exp MINUS exp\n");
	    $$ = newInfixExp($2, $1, $3); 
	    setLoc($$, (Loc)&(@$));
	  }
	| exp MULT exp
	  {
	    debug("exp ::= exp MULT exp\n");
	    $$ = newInfixExp($2, $1, $3); 
	    setLoc($$, (Loc)&(@$));
	  }
	| exp DIV exp
	  {
	    debug("exp ::= exp DIV exp\n");
	    $$ = newInfixExp($2, $1, $3); 
	    setLoc($$, (Loc)&(@$));
	  }
	| exp EXPON exp
	  {
	    debug("exp ::= exp EXPON exp\n");
	    $$ = newInfixExp($2, $1, $3); 
	    setLoc($$, (Loc)&(@$));
	  }
        | MINUS  exp %prec MINUS
	  {
	    debug("exp ::= MINUS exp\n");
	    $$ = newPrefixExp($1, $2); 
	    setLoc($$, (Loc)&(@$));
	  }
        | LB exp RB
	  {
	    debug("exp ::= LB exp RB\n");
	    $$ = newParenExp($2);
	    setLoc($$, (Loc)&(@$));
	  }
        ;

%%

yyerror(char *message)
{
	printf("%s\n",message);
}

int main(int argc, char *argv[])
{
	symtab = newTable();
	ast = newAST();
	printf("Parsing ...\n");
	yyparse();
	printf("\n\nDump the program from the generated AST:\n");
	dumpAST(ast->root);
	destroyAST(&ast->root);
	printf("\n\nFinished destroying AST.\n");
	destroyTable(&symtab);
	printf("\n\nFinished destroying symbolic table.\n");
	return(0);
}
