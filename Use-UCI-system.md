项目从 [709f931][0] 开始改为使用 UCI 配置各项功能，现将各个 UCI 选项做一下说明，方便在命令行下的配置。

以下为常用的命令选项, 完整的选项可以参考 [官方Wiki][1]  
 - 使用 `uci export shadowsocks` 查看当前的 UCI 配置文件  
 - 使用 `uci set shadowsocks.@shadowsocks[-1].option='value'` 修改/增加 `option`  
 - 使用 `uci delete shadowsocks.@shadowsocks[-1].option` 删除 `option`  
 - 使用 `uci commit shadowsocks` 提交对 UCI 配置文件的修改, 这样 UCI 配置才能生效  

以下为有效的 `option`(可以使用 set 或者 delete 增删改)  

 Option             | Description
 -------------------|-----------------------------------
 `enable`           | 总开关 `[0.关闭 1.开启]`
 `use_conf_file`    | 是否使用配置文件 `[0.不使用 1.使用]`
 `config_file`      | 配置文件路径, `use_conf_file` 值为 `1` 时有效
 `server`           | 服务器地址, `use_conf_file` 值为 `0` 时有效
 `server_port`      | 服务器端口, `use_conf_file` 值为 `0` 时有效
 `local_port`       | 本地端口, `use_conf_file` 值为 `0` 时有效
 `password`         | 密码, `use_conf_file` 值为 `0` 时有效
 `encrypt_method`   | 加密方式, `use_conf_file` 值为 `0` 时有效
 `ignore_list`      | 忽略 IP 列表文件路径
 `tunnel_enable`    | 使用启用 UDP 转发 `[0.关闭 1.开启]`
 `tunnel_port`      | 本地 UDP 端口, `tunnel_enable` 值为 `1` 时有效
 `tunnel_forward`   | UDP 转发地址, `tunnel_enable` 值为 `1` 时有效
 `ac_mode`          | 内网访问控制模式 `[0.关闭 1.白名单 2.黑名单]`
 `accept_ip`        | 白名单 IP, 设置多个时使用 `add_list` 参数而不是 `set`
 `reject_ip`        | 黑名单 IP, 只能设置一个


  [0]: https://github.com/aa65535/openwrt-shadowsocks/commit/709f931d9cd69605e176d7dafe8ab1e87e5b07e3
  [1]: http://wiki.openwrt.org/doc/uci