%error-verbose
%debug
%{
# include <stdio.h>
# include <stdlib.h>
  static void yyerror (const char *msg);
  static int yylex (void);
# define USE(Var)
%}

%union
{
  int val;
};

%type <val> a_1 a_2 a_5
            sum_of_the_five_previous_values

%%
exp: a_1 a_2 { $<val>$ = 3; } { $<val>$ = $<val>3 + 1; } a_5
     sum_of_the_five_previous_values
    {
       USE (($1, $2, $<foo>3, $<foo>4, $5));
       printf ("%d\n", $6);
    }
;
a_1: { $$ = 1; };
a_2: { $$ = 2; };
a_5: { $$ = 5; };

sum_of_the_five_previous_values:
    {
       $$ = $<val>0 + $<val>-1 + $<val>-2 + $<val>-3 + $<val>-4;
    }
;

%%

static int
yylex (void)
{
  static int called;
  if (called++)
    abort ();
  return EOF;
}

static void
yyerror (const char *msg)
{
  fprintf (stderr, "%s\n", msg);
}

int
main (void)
{
  return yyparse ();
}

