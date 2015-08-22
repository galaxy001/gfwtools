简介
---

`ss-rules` 被用于为 `ss-redir` 生成代理规则，
它是高效且方便的, 而且适用于其他 Linux 系统。

参数
---

```
-s
    SS 服务端的 IP地址或者域名, 不建议使用域名, 因为需要提前解析

-l
    本地端口号, 即 ss-redir 的端口, 流量将被重定向到这个端口

-c
    默认是 SS 的配置文件, 优先级低于命令行参数

-i
    不被 ss-redir 代理的 IP列表文件, 默认是 CHNRoute

-a
    内网访问控制IP, 使用空格隔开, 使用(w/b)作为前缀定义访问控制模式
    w: 仅允许列表内, b: 仅允许列表外

-b
    不被 ss-redir 代理的 IP, 多个 IP 使用空格隔开

-w
    被 ss-redir 代理的 IP, 多个 IP 使用空格隔开, 优先级高于 -b 参数

-e
    为 iptables 添加额外的选项和参数

-o
    同时将代理规则应用到 OUTPUT 链, 让路由自身流量也被代理

-f
    清空代理规则并退出, 指定此参数后, 其他参数皆无效
```

示例
---

 1. 使用 SS 的配置文件启动  
    `ss-rules -c /etc/shadowsocks/config.json`

 2. 使用命令行参数覆盖配置文件中的值  
    `ss-rules -c /etc/shadowsocks/config.json -l 1081`

 3. 使用 `-i` 参数设置不被代理的 IP  
    `ss-rules -c /etc/shadowsocks/config.json -i /etc/shadowsocks/ignore.list`

 4. 使用内网访问控制仅允许指定的 IP 可以使用 SS  
    `ss-rules -c /etc/shadowsocks/config.json -i /etc/shadowsocks/ignore.list -a "w192.168.1.32 192.168.1.36"`

 5. 使指定的 IP 不被 SS 代理  
    `ss-rules -c /etc/shadowsocks/config.json -i /etc/shadowsocks/ignore.list -b "208.67.220.220 208.67.222.222"`
