项目从 [709f931][0] 开始改为使用 UCI 配置各项功能，下面介绍一下 UCI 选项以及命令行下的设置方法。

**以下为常用的命令选项, 完整的选项可以参考 [官方Wiki][1]**  
 - 使用 `uci export shadowsocks` 查看当前的 UCI 配置文件  
 - 使用 `uci set shadowsocks.@shadowsocks[-1].option='value'` 修改/增加 `option`  
 - 使用 `uci delete shadowsocks.@shadowsocks[-1].option` 删除 `option`  
 - 使用 `uci commit shadowsocks` 提交对 UCI 配置文件的修改, 这样 UCI 配置才能生效  

**以下为有效的 option (可以使用 set 或者 delete 增删改)**  

 Option           | Description
 -----------------|-----------------------------------
 enable           | 总开关 `[0.关闭 1.开启]`
 use_conf_file    | 是否使用配置文件 `[0.不使用 1.使用]`
 config_file      | 配置文件路径, `use_conf_file` 值为 `1` 时有效
 server           | 服务器地址, `use_conf_file` 值为 `0` 时有效
 server_port      | 服务器端口, `use_conf_file` 值为 `0` 时有效
 local            | 本地IP地址 默认值为 `0.0.0.0`
 local_port       | 本地端口, `use_conf_file` 值为 `0` 时有效
 password         | 密码, `use_conf_file` 值为 `0` 时有效
 timeout          | 连接超时, `use_conf_file` 值为 `0` 时有效
 encrypt_method   | 加密方式, `use_conf_file` 值为 `0` 时有效
 ignore_list      | 忽略IP列表文件路径, 留空或者设置为 `/dev/null` 则全局代理
 udp_relay        | 是否打开 UDP 支持 `[0.关闭 1.开启]`
 tunnel_enable    | 是否启用UDP转发 `[0.关闭 1.开启]`
 tunnel_port      | 本地UDP端口, `tunnel_enable` 值为 `1` 时有效
 tunnel_forward   | UDP转发地址, `tunnel_enable` 值为 `1` 时有效
 lan_ac_mode      | 内网访问控制模式 `[0.已禁用 1.仅允许列表内 2.仅允许列表外]`
 lan_ac_ip        | 内网访问控制IP, 设置多个时使用 `add_list` 选项而不是 `set`
 wan_bp_ip        | 外网被忽略的IP, 设置多个时使用 `add_list` 选项而不是 `set`
 wan_fw_ip        | 外网走代理的IP, 设置多个时使用 `add_list` 选项而不是 `set`
 ipt_ext          | iptables 的附加参数, 请小心设置

**命令示例**

 - 修改内网访问控制模式  
  >`uci set shadowsocks.@shadowsocks[-1].ac_mode='1'`  

 - 设置内网访问控制IP  
  >```
  uci add_list shadowsocks.@shadowsocks[-1].lan_ac_ip='192.168.1.32'
  uci add_list shadowsocks.@shadowsocks[-1].lan_ac_ip='192.168.1.36'
  >```

 - 提交修改  
  >`uci commit shadowsocks`  


  [0]: https://github.com/aa65535/openwrt-shadowsocks/commit/709f931d9cd69605e176d7dafe8ab1e87e5b07e3
  [1]: http://wiki.openwrt.org/doc/uci
