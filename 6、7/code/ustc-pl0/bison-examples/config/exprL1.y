/*
 * expr1.y : A simple yacc expression parser
 *          Based on the Bison manual example. 
 * The parser eliminates conflicts by introducing more nondeterminals.
 */

%{
#include <stdio.h>
#include <math.h>
#define TAG
%}

%union {
   float val;
}

%token NUMBER
%token PLUS MINUS MULT DIV EXPON
%token EOL
%token LB RB

%left  MINUS PLUS
%left  MULT DIV
%right EXPON

%type  <val> exp term fact NUMBER


%%
input   :
        | input line
        ;

line    : EOL
        | NUMBER {
		#ifndef TAG
		printf("Line %d: ", (int)$1);
		#else
		$<val>lineno = $1;
		//$<val>$ = $1;
		#endif
		}[lineno]
	  exp EOL { 
		#ifndef  TAG
		printf("%g\n",$3);
		#else
		printf("Line %d: %g\n", (int)$<val>lineno, $3);
		#endif
		} 

exp     : exp PLUS  term         { $$ = $1 + $3;   }
        | exp MINUS term         { $$ = $1 - $3;   }
        | term                   { $$ = $1;        }
        ;
term    : term MULT fact         { $$ = $1 * $3;   }
        | term DIV  fact         { $$ = $1 / $3;   }
        | fact                   { $$ = $1;        }
        ;
fact    : NUMBER                 { $$ = $1;        }
        | MINUS fact             { $$ = -$2;       }
        | fact EXPON fact        { $$ = pow($1,$3);}
        | LB exp RB              { $$ = $2;        }
        ;

%%

yyerror(char *message)
{
  printf("%s\n",message);
}

int main(int argc, char *argv[])
{
  yyparse();
  return(0);
}








