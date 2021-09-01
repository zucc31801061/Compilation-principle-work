/*
 * asgn.y : A simple yacc assignment statement parser
 *          Based on the Bison manual example. 
 * The parser computes each assignment and output its value.
 * Author: Yu Zhang (yuzhang@ustc.edu.cn)
 */

%{
#include <stdio.h>
#include <math.h>
#include <common.h>
Table symtab;
ErrFactory errfactory;
extern int lparen_num;
%}

%union {
	float fval;
	char *name;
	int  ival;
}
%locations
%debug

%token NUMBER IDENTIFIER
%token PLUS MINUS MULT DIV EXPON ASGN
%token EOL
%token LB RB

%left  MINUS PLUS
%left  MULT DIV
%right EXPON

%type  <fval> asgnexp exp NUMBER
%type  <ival> PLUS MINUS MULT DIV EXPON ASGN
%type  <name> IDENTIFIER

%%
input   :
	| input line
	;

line    : EOL
	| asgnexp ';' EOL 
	  { 
	    printf("\n");
	  }
	| asgnexp EOL
	  { 
	    /* see Bison manual 3.6 Tracking Locations */ 
	    newWarning(errfactory, NoSemicolon, 
		@2.first_line, @2.first_column);
	    printf("\n");
	  }
	;

asgnexp : IDENTIFIER ASGN exp    
	  {
	    setVal(symtab, $1, $3);  
	    /* see Bison manual 3.6 Tracking Locations */ 
	    printf("%4d(%2d): %s = %g; ", @$.first_line, @$.last_column, $1, $3);
	    $$ = $3;
	    //printf("lookahead %d", yychar);
	  }
	| exp ASGN exp
	  {
	    printf("%4d(%2d): %s = %g; ", @$.first_line, @$.last_column, "??", $3);
	    $$ = $3;
	    newError(errfactory, IllegalLValue, 
		@1.first_line, @1.first_column);
	  }
	| error ASGN exp
	  {
	    printf("%4d(%2d): %s = %g; ", @$.first_line, @$.last_column, "??", $3);
	    $$ = $3;
	    newError(errfactory, IllegalLValue, 
		@1.first_line, @1.first_column);
	  }
	| IDENTIFIER ASGN error ';'
	  {
	    printf("%4d(%2d): %s = %s; ", @$.first_line, @$.last_column, $1, "??");
	    if (lparen_num > 0)
	    	newError(errfactory, UnmatchedLParen, 
			@3.first_line, @3.last_column);
	    else if (lparen_num < 0)
	    	newError(errfactory, UnmatchedRParen, 
			@3.first_line, @3.last_column);
	    else
	    	newError(errfactory, IllegalExpr, 
			@3.first_line, @3.first_column);
	    $$ = 0;
	  }
        ;

	    
exp     : NUMBER
	  {
	    $$ = $1;
	  }
	| IDENTIFIER
	  {
	    $$ = getVal(symtab, $1); 
	  }
	| exp PLUS exp
	  {
	    $$ = $1 + $3;
	  }
	| exp MINUS exp
	  {
	    $$ = $1 - $3;
	  }
	| exp MULT exp
	  {
	    $$ = $1 * $3;
	  }
	| exp DIV exp
	  {
	    $$ = $1 / $3;
	  }
	| exp EXPON exp
	  {
	    $$ = pow($1, $3); 
	  }
        | MINUS  exp %prec MINUS
	  {
	    $$ = -$2;
	  }
        | LB exp RB
	  {
	    $$ = $2;
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
	errfactory = newErrFactory();
	yyparse();
	dumpErrors(errfactory);
	dumpWarnings(errfactory);
	destroyTable(&symtab);
	destroyErrFactory(&errfactory);
	return(0);
}
