/*
 * asgn.lex : Scanner for a simple
 *            assignment statement parser.
 */
/* See Flex Manual: A.2 C Scanners with Bison Parsers */

%{
#include "common.h"
#include "asgn.tab.h"
/* handle locations */
int yycolumn = 1;

#define YY_USER_ACTION yylloc.first_line = yylloc.last_line = yylineno; \
    yylloc.first_column = yycolumn; yylloc.last_column = yycolumn+yyleng-1; \
    yycolumn += yyleng;
%}

%option yylineno

D	   [0-9]
L	   [a-zA-Z_]
H	   [a-fA-F0-9]

%%

{D}+       { yylval.fval = atol(yytext);
             return(NUMBER);
           }
{D}+\.{D}+ { 
             sscanf(yytext,"%f",&yylval.fval);
             return(NUMBER);
           }
{L}({L}|{D})*	   { yylval.name = malloc(yyleng+1);
	     yylval.name[yyleng] = '\0';
             strncpy(yylval.name, yytext, yyleng);
             return(IDENTIFIER);
           }
"="        { yylval.ival = OP_ASGN;	return(ASGN); }
"+"        { yylval.ival = OP_PLUS; 	return(PLUS); }
"-"        { yylval.ival = OP_MINUS; 	return(MINUS);}
"*"        { yylval.ival = OP_MULT; 	return(MULT); }
"/"        { yylval.ival = OP_DIV; 	return(DIV);  }
"^"        { yylval.ival = OP_EXPON; 	return(EXPON);}
"("        return(LB);
")"        return(RB);
";"        return(';');
\n         { yycolumn = 1; 		return(EOL);}
[\t ]*     /* throw away whitespace */
.          { yyerror("Illegal character");
             return(EOL);
           }
%%
