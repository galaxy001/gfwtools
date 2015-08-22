 1. 新建一个文件 `/root/update_ignore_list` 写入如下内容:  

    ```sh
    #!/bin/sh
    
    wget -O- 'http://ftp.apnic.net/apnic/stats/apnic/delegated-apnic-latest' | \
        awk -F\| '/CN\|ipv4/ { printf("%s/%d\n", $4, 32-log($5)/log(2)) }' > \
        /etc/shadowsocks/ignore.list
    
    if pidof ss-redir>/dev/null; then
        /etc/init.d/shadowsocks rules
    fi
    ```

 2. 使用 `chmod +x /root/update_ignore_list` 添加可执行权限

 3. 打开路由器管.理页面 `系统 - 计划任务` 填写如下内容（每天 04:30 执行）:  
    ```
    30    4     *     *     *     /root/update_ignore_list>/dev/null 2>&1
    ```
