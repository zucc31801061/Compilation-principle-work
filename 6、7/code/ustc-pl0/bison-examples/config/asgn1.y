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
%}

%union {
	float fval;
	char *name;
	int  ival;
}
%locations

%token NUMBER IDENTIFIER
%token PLUS MINUS MULT DIV EXPON ASGN
%token EOL
%token LB RB

%left  MINUS PLUS
%left  MULT DIV
%right EXPON

%type  <fval> asgnexp exp NUMBER
%type  <ival> op PLUS MINUS MULT DIV EXPON ASGN
%type  <name> IDENTIFIER

%%
input   :
	| input line
	;

line    : EOL
	| asgnexp ';' EOL 
	  { 
	    /* see Bison manual 3.6 Tracking Locations */ 
	    printf("\n");
	  }

asgnexp : IDENTIFIER ASGN exp    
	  {
	    /* see Bison manual 3.6 Tracking Locations */ 
	    setVal(symtab, $1, $3);  
	    printf("%4d(%2d): %s = %g", @$.first_line, @$.last_column, $1, $3);
	    $$ = $3;        
	  }
        ;
op	: PLUS	  {    $$ = $1;	}
	| MINUS	  {    $$ = $1;	}
	| MULT	  {    $$ = $1;	}
	| DIV	  {    $$ = $1;	}
	| EXPON	  {    $$ = $1;	}
	;
exp     : NUMBER
	  {
	    $$ = $1;
	  }
	| IDENTIFIER
	  {
	    $$ = getVal(symtab, $1); 
	  }
	| exp op exp
	  {
	    switch($2) {
	    case OP_PLUS:
	        $$ = $1 + $3; break;
	    case OP_MINUS:
	        $$ = $1 - $3; break;
	    case OP_MULT:
	        $$ = $1 * $3; break;
	    case OP_DIV:
	        $$ = $1 / $3; break;
	    case OP_EXPON:
	        $$ = pow($1, $3); break;
	    }
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
	yyparse();
	destroyTable(&symtab);
	return(0);
}
