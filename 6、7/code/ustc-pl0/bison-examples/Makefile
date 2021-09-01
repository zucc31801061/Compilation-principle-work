CINC	 = -Iinclude
CONF	 = config
SRC	 = src
BIN	 = bin
UTILSRC  = $(SRC)/list.c $(SRC)/error.c $(SRC)/ast.c

CC       = gcc -g $(CINC)
LEX      = flex -i -I 
YACC     = bison -d -y

all: expr expr1 exprL exprL1 asgn asgn1 asgn_err asgn-in asgn2ast midrule

# Generate a compiler for a simple expression language,
# expr: the grammar is ambiguous, and uses precedence declarations
# expr1: the grammar is disambiguous.
# exprL: there is embedded code in the rules.
# exprL1: there is embedded code in the rules, which sets its semantic value.
exp%:
	$(YACC) -b $@ -o $(SRC)/$@.tab.c $(CONF)/$@.y
	$(LEX) -o$(SRC)/$@.lex.c $(CONF)/expr.lex
	$(CC) -o $(BIN)/$@ $(SRC)/$@.lex.c $(SRC)/$@.tab.c -ll -lm

# An example using middle values.
midrule:	
	$(YACC) -b $@ -o $(SRC)/$@.tab.c $(CONF)/$@.y
	$(CC) -o $(BIN)/$@ $(SRC)/$@.tab.c -ll -lm

# Generates a compiler for an assignment statement sequence language,
# which evaluates each assignment statement in an input program.
asgn:   $(CONF)/asgn.y $(CONF)/asgn.lex
	$(YACC) -b asgn -o $(SRC)/asgn.tab.c $(CONF)/asgn.y
	$(LEX) -o$(SRC)/asgn.lex.c $(CONF)/asgn.lex
	$(CC) -o $(BIN)/asgn $(SRC)/asgn.lex.c $(SRC)/symtab.c $(SRC)/asgn.tab.c -ll -lm

# Similar to asgn, 
# but nonterminal 'op' is introduced to represent all infix operators.
asgn1:   $(CONF)/asgn1.y $(CONF)/asgn.lex
	$(YACC) -b asgn -o $(SRC)/asgn1.tab.c $(CONF)/asgn1.y
	mv  $(SRC)/asgn1.tab.h $(SRC)/asgn.tab.h
	$(LEX) -o$(SRC)/asgn.lex.c $(CONF)/asgn.lex
	$(CC) -o $(BIN)/asgn1 $(SRC)/asgn.lex.c $(SRC)/symtab.c $(SRC)/asgn1.tab.c -ll -lm

# Similar to asgn, just to test inherited attributes.
asgn-in:   $(CONF)/asgn-in.y $(CONF)/asgn.lex
	$(YACC) -b asgn -o $(SRC)/$@.tab.c $(CONF)/$@.y
	mv  $(SRC)/$@.tab.h $(SRC)/asgn.tab.h
	$(LEX) -o$(SRC)/asgn.lex.c $(CONF)/asgn.lex
	$(CC) -o $(BIN)/$@ $(SRC)/asgn.lex.c $(SRC)/symtab.c $(SRC)/$@.tab.c -ll -lm

# Generates a compiler for an assignment statement sequence language.
# The compiler provides error recovery.
asgn_err:   $(CONF)/asgn_err.y $(CONF)/asgn.lex
	$(YACC) -b asgn_err -o $(SRC)/asgn_err.tab.c $(CONF)/asgn_err.y
	$(LEX) -o$(SRC)/asgn_err.lex.c $(CONF)/asgn_err.lex
	$(CC) -o $(BIN)/asgn_err $(SRC)/asgn_err.lex.c $(SRC)/symtab.c $(SRC)/asgn_err.tab.c $(UTILSRC) -ll -lm

# Generates a compiler for an assignment statement sequence language,
# which constructs an abstract syntax tree for an input program.
asgn2ast:   $(CONF)/asgn2ast.y $(CONF)/asgn.lex
	$(YACC) -b asgn2ast -o $(SRC)/asgn2ast.tab.c $(CONF)/asgn2ast.y
	$(LEX) -o$(SRC)/asgn.lex.c $(CONF)/asgn.lex
	$(CC) -DDEBUG -o $(BIN)/asgn2ast $(SRC)/asgn.lex.c $(SRC)/symtab.c $(SRC)/asgn2ast.tab.c $(UTILSRC) -ll -lm

clean:
	rm -f *.BAK *.o core *~* *.a 
#	rm -f $(SRC)/*.tab.h $(SRC)/*.tab.c
#	rm -f $(SRC)/*.lex.c *.out
	rm -f $(BIN)/*
