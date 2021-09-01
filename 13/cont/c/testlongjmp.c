/* Demonstrating setjmp/longjmp non-local jumps in C.
   See Kernighan and Ritchie (1988) p 254.
   sestoft@itu.dk * 2003-03-24, 2009-10-24
 */

#include <setjmp.h>
#include <stdio.h>
#include <stdlib.h>

/* When f is called with a positive even argument such as 10, it
   calls itself recursively

        f(10) --> f(8) --> f(6) --> f(4) --> f(2) --> f(0)

   and then returns along the call chain, printing messages on the
   way as required by the normal continuation.

   When f is called with a positive odd argument such as 11, it calls
   itself recursively

        f(11) --> f(9) --> f(7) --> f(5) --> f(3) --> f(1)

   and then calls longjmp(env, 1) to return straight to where setjmp
   was called in main, ignoring the normal continuation and hence the
   printf statements.
*/

jmp_buf env;

void f(int i) {
  printf("entering to f, i = %d\n", i);
  if (i == 0) {
    printf("reached 0\n");
  } else if (i == 1) {
    printf("reached 1\n");
    longjmp(env, 2);
  } else {
    f(i - 2);
    printf("returning from f, i = %d\n", i);
  }
}

int main(int argc, char *argv[]) {
  int i = atoi(argv[1]);
  int res = -1;

  // setjmp 返回两次
  // res第1次返回0
  //从 longjmp(env,int n)返回 第2个参数: n=2
  if ((res = setjmp(env)) == 0) {
    /* Just after the original call to setjmp(env) we reach this point */
    printf("if true:res is %d\n", res);
    f(i);
  } else {
    /* When and if longjmp is called, we get to this point */

    printf("longjmp was called\n");
    printf("if else:res is %d\n", res);
  }
  return 0;
}
