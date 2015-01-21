Intro
---

 [ss-rules][0] 被用于为 `ss-redir` 生成指定的代理规则，  
 它是高效且方便的，您无需知道它是怎么工作的，只需传递给它正确的参数，  
 在 OpenWrt 上，默认被安装在 `/usr/bin`, 它同样适用于其他 Linux 系统。

Usage
---

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
    　　不被 ss-redir 代理的 IP, 使用空格隔开

     -w
    　　被 ss-redir 代理的 IP, 使用空格隔开, 优先级高于 `-b`

     -e
    　　为 iptables 添加额外的选项和参数

     -o
    　　同时将代理规则应用到 OUTPUT 链, 同时让路由自身流量也被代理

     -f
    　　清空代理规则并退出, 指定此参数后, 其他参数皆无效


  [0]: https://github.com/shadowsocks/openwrt-shadowsocks/blob/master/files/shadowsocks.rule
