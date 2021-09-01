/*
 * TODO Functions of Abstract Syntax Tree
 * Author: Yu Zhang (yuzhang@ustc.edu.cn)
 */
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include "common.h"

char *opname[]={
#undef opxx
#define opxx(a, b) b,
#include "op.h"
	"Undefined Op"
};

ASTNode
newNumber(float value)
{
	ASTNode new;
	NEW0(new);
	new->kind = KValue;
	new->val = value;
	return new;
}

ASTNode
newName(Table ptab, char *name)
{
	ASTNode new;
	NEW0(new);
	new->kind = KName;
	new->sym = getSym(ptab, name);
	return new;
}

ASTNode
newPrefixExp(int op, ASTNode exp)
{
	ASTNode new;
	NEW0(new);
	new->kind = KPrefixExp;
	Exp newexp;
	NEW0(newexp);
	newexp->op = op;
	newexp->kids[0] = exp;
	new->exp = newexp;
	return new;
}

ASTNode
newParenExp(ASTNode exp)
{
	ASTNode new;
	NEW0(new);
	new->kind = KParenExp;
	Exp newexp;
	NEW0(newexp);
	newexp->op = -1;
	newexp->kids[0] = exp;
	new->exp = newexp;
	return new;
}

ASTNode
newInfixExp(int op, ASTNode left, ASTNode right)
{
	ASTNode new;
	NEW0(new);
	new->kind = KInfixExp;
	Exp newexp;
	NEW0(newexp);
	newexp->op = op;
	newexp->kids[0] = left;
	newexp->kids[1] = right;
	new->exp = newexp;
	return new;
}

ASTNode
newAssignment(int op, ASTNode left, ASTNode right)
{
	ASTNode new;
	NEW0(new);
	new->kind = KAssignExp;
	Exp newexp;
	NEW0(newexp);
	newexp->op = op;
	newexp->kids[0] = left;
	newexp->kids[1] = right;
	new->exp = newexp;
	return new;
}

void
destroyExp(Exp *pnode)
{
	if (*pnode == NULL) return;
	Exp node = *pnode;
	destroyAST(&node->kids[0]);
	destroyAST(&node->kids[1]);
	free(node);
	*pnode = NULL;
}

ASTNode
newExpStmt(ASTNode exp)
{
	ASTNode new;
	NEW0(new);
	new->kind = KExpStmt;
	ExpStmt newstmt;
	NEW0(newstmt);
	new->estmt = newstmt;
	newstmt->exp = exp;
	return new;
}

void
destroyExpStmt(ExpStmt *pnode)
{
	if (*pnode == NULL) return;
	ExpStmt node = *pnode;
	destroyAST(&node->exp);
	free(node);
	*pnode = NULL;
}

ASTNode
newBlock()
{
	ASTNode new;
	NEW0(new);
	new->kind = KBlock;
	Block newb;
	NEW0(newb);
	new->block = newb;
	newb->stmts = newList();
	return new;
}

void
destroyBlock(Block *pnode)
{
	if (*pnode == NULL) return;
	Block node = *pnode;
	destroyList(&node->stmts, destroyAST);
	free(node);
	*pnode = NULL;
}

ASTTree
newAST()
{
	ASTTree new;
	NEW0(new);
	return new;
}

void
destroyAST(ASTNode *pnode)
{
	if (*pnode == NULL) return;
	ASTNode node = *pnode;
	int kind = node->kind;
	
	switch (kind) {
	case KValue:
	case KName:
		break;
	case KPrefixExp:
	case KParenExp:
	case KInfixExp:
	case KAssignExp:
		destroyExp(&node->exp);
		break;
	case KExpStmt:
		destroyExpStmt(&node->estmt);
		break;
	case KBlock:
		destroyBlock(&node->block);
		break;
	default:
		printf("Unhandled ASTNode kind!\n");
	}
	free(node);
	*pnode = NULL;
}

Loc
setLoc(ASTNode node, Loc loc)//fline, int fcol, int lline, int lcol)
{
	if (node->loc == NULL )
		NEW0(node->loc);
	node->loc->first_line = loc->first_line;
	node->loc->first_column = loc->first_column;
	node->loc->last_line = loc->last_line;
	node->loc->last_column = loc->last_column;
	return node->loc;
}

void
dumpAST(ASTNode node)
{
	if (node == NULL) return;
	int kind = node->kind;
	
	switch (kind) {
	case KValue:
		printf("%g", node->val);
		break;
	case KName:
		printf("%s", node->sym->name);
		break;
	case KPrefixExp:
		printf("%s", opname[node->exp->op]);
		dumpAST(node->exp->kids[0]);
		break;
	case KParenExp:
		printf("(");
		dumpAST(node->exp->kids[0]);
		printf(")");
		break;
	case KInfixExp:
		dumpAST(node->exp->kids[0]);
		printf("%s", opname[node->exp->op]);
		dumpAST(node->exp->kids[1]);
		break;
	case KAssignExp:
		dumpAST(node->exp->kids[0]);
		printf("%s", opname[node->exp->op]);
		dumpAST(node->exp->kids[1]);
		break;
	case KExpStmt:
		dumpAST(node->estmt->exp);
		printf(";");
		break;
	case KBlock:
	{
		List stmts = node->block->stmts;
		ListItr itr = getGListItr(stmts, 0);
		while ( hasNext(itr) )  {
			dumpAST((ASTNode)nextItem(itr));
			printf("\n");
		}
		break;
	}
	default:
		printf("Unhandled ASTNode kind!\n");
	}
}
