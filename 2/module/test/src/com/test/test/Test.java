package com.test.test;

import com.test.api.UserServiceImpl;
import com.test.base.User;

public class Test {
    public static void main(String[] args) {
        UserServiceImpl service = new UserServiceImpl();
        service.login();
        User user = new User(1, "wls");
        service.save(user);
    }
}
