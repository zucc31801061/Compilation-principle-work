/*
 * asgn.lex : Scanner for a simple
 *            assignment statement parser.
 */
/* See Flex Manual: A.2 C Scanners with Bison Parsers */

%{
#include "asgn_err.tab.h"
/* handle locations */
int yycolumn = 1;
int lparen_num = 0;

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
             strncpy(yylval.name, yytext, yyleng);
             return(IDENTIFIER);
           }
"="        return(ASGN);
"+"        { yylval.ival = PLUS; 	return(PLUS); 	}
"-"        { yylval.ival = MINUS; 	return(MINUS);	}
"*"        { yylval.ival = MULT; 	return(MULT); 	}
"/"        { yylval.ival = DIV; 	return(DIV);  	}
"^"        { yylval.ival = EXPON; 	return(EXPON);	}
"("        { lparen_num++; 		return(LB);	}
")"        { lparen_num--;		return(RB);	}
";"        return(';');
\n         { yycolumn = 1; 		return(EOL);}
[\t ]*     /* throw away whitespace */
.          { yyerror("Illegal character");
             return(EOL);
           }
%%
