/* 
 * Common definitions and declarations for compilers 
 * Author: Yu Zhang (yuzhang@ustc.edu.cn)
 */
#ifndef _COMMON_H_
#define _COMMON_H_
#include "util.h"

// operator kinds
// You could add more kinds of error messages into op.h 
enum {
#define opxx(a, b) OP_##a,
#include "op.h"
	OPLAST
};

//extern char **opname;

// symbolic table
typedef struct symbol {
	char	*name;	// name of the symbol
	bool	isInitial;	// whether it is initialized	
	short	type;	// type of the symbol
	float	val;	// value of the symbol
} *Symbol;

typedef struct entry {
	struct symbol sym;
	struct entry *next;
} *Entry;

typedef struct table {
	// a hashtable to store symbols
	struct entry *buckets[256];
} *Table;
#define HASHSIZE 256

// Function declarations corresponding to symbolic table
Table 	newTable();
Symbol 	lookup(Table ptab, const char *name);
Symbol 	getSym(Table ptab, const char *name);
float 	getVal(Table ptab, const char *name);
Symbol 	setVal(Table ptab, const char *name, float val);
void 	destroyTable();

// Error/warning message
// You could add more kinds of error messages into errcfg.h 
enum {
#define errxx(a, b) a,
#include "errcfg.h"
	LASTERR
};

// An error/warning message
typedef struct errmsg {
	bool	isWarn;
	int 	type;
	char 	*msg;
	int	line;
	int	column;
} *Errmsg;

// Error factory
typedef struct errfactory { 
	List	errors;
	List	warnings;
} *ErrFactory;

// Function declarations on error message management
Errmsg	newError	(ErrFactory errfactory, int type, int line, int col);
Errmsg	newWarning	(ErrFactory errfactory, int type, int line, int col);
void	dumpErrmsg	(Errmsg error);
ErrFactory newErrFactory();
void	dumpErrors	(ErrFactory errfactory);
void	dumpWarnings	(ErrFactory errfactory);
void	destroyErrFactory(ErrFactory *errfact);

// abstract syntax tree
// Structure for tracking locations, same as YYLTYPE in y.tab.h
typedef struct location {
	int first_line;
	int first_column;
	int last_line;
	int last_column;
} *Loc;

typedef struct {
	int 	op;
	//int type;
	float 	val;
	struct astnode	*kids[2];// kids of the AST node
} *Exp;

typedef struct {
	struct astnode *exp;
} *ExpStmt;

typedef struct {
	List  stmts;
} *Block;

typedef struct astnode{
	enum {
		KValue = 0x200,		// numerial value:
		KName,			// name, such as variable name
		KPrefixExp,		// prefix expression
		KInfixExp,		// infix expression
		KAssignExp,		// assignment expression
		KParenExp,		// parentheses expression
		KExpStmt,		// expression statement
		KBlock,			// block
	} kind;	// kind of the AST node
	union {		// information of various kinds of AST node 
		float  val;		// KValue: numerial value
		Symbol sym;		// KName: symbols 
		Exp   exp;		// KPrefixExp,
					// KInfixExp,
					// KAssignExp,
					// KParenExp
		ExpStmt  estmt;		// KExpStmt
		Block  block;		// KBlock
	};
	Loc 	loc;			// locations
} *ASTNode;

typedef struct ASTtree {
	ASTNode root;
} *ASTTree;

// functions for creating various kinds of ASTnodes
ASTNode newNumber(float value);
ASTNode newName(Table ptab, char *name);
ASTNode newPrefixExp(int op, ASTNode exp);
ASTNode newParenExp(ASTNode exp);
ASTNode newInfixExp(int op, ASTNode left, ASTNode right);
ASTNode newAssignment(int op, ASTNode left, ASTNode right);
void	destroyExp(Exp *pexp);
ASTNode newExpStmt(ASTNode exp);
void	destroyExpStmt(ExpStmt *pexpstmt);
ASTNode newBlock();
void	destroyBlock(Block *pblock);
ASTTree newAST();
void	destroyAST(ASTNode *pnode);
void 	dumpAST(ASTNode node);
Loc	setLoc(ASTNode node, Loc loc);

#endif // !def(_COMMON_H_)
