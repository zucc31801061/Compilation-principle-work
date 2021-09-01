
class ExceptionDemo {
    class Ex1 extends Exception {

    }

    class Ex2 extends Ex1 {

    }

    void foo() throws Exception {
        try {
            bar();
        } catch (Ex1 e) {
            System.out.println("catch foo");
        }
    }

    void bar() throws Exception {
        baz();
    }

    void baz() throws Exception {
        try {
            throw new Ex1();
        } catch (Ex2 e) {
            System.out.println("catch barz");
        }
    }

}
