package com.test.api;

import com.test.base.User;
import com.test.service.UserService;
import com.test.util.MyStringUtil;

public class UserServiceImpl implements UserService {
    @Override
    public void save(User user) {
        /**
         * 成功引用util的包内容
         */
        MyStringUtil.size("123");

        /**
         * ListUtil此类  这个导包就导入不进来
         */
        //ListUtil.size();

        System.out.println(user);
    }

    @Override
    public void login() {
        System.out.println("UserServiceImpl login method");
    }
}
