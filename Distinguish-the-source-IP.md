**注意:** 此功能在 [0c50b35][1] 之后可用, 在[此处][3]查看 LuCI 版本。

通过修改 `/usr/bin/ss-rules` 中的 `EXT_ARGS` 变量可以:

  1. 指定某一个设备不走 ss, 其他设备走 ss  

   >如指定 IP 为 192.168.1.32 的设备不走 ss
   ```sh
   EXT_ARGS="! -s 192.168.1.32"
   ```

  2. 指定一个或多个设备走 ss, 其他设备不走 ss 

   >如指定 IP 为 192.168.1.32 的设备走 ss
   ```sh
   EXT_ARGS="-s 192.168.1.32"
   ```

   >多个 IP 使用 <kbd>,</kbd> 隔开, 如指定 IP 为 192.168.1.32 和 192.168.1.36 的设备走 ss
   ```sh
   EXT_ARGS="-s 192.168.1.32,192.168.1.36"
   ```

限制: iptables 使用`-s` 时可以指定多个由 <kbd>,</kbd> 隔开的 IP, 而使用 <kbd>!</kbd> 反转规则时只能指定一个 IP.  
解决: 可以使用 [CIDR][C] 表示多个连续的 IP, 如 `192.168.1.0/25` 可以表示 `192.168.1.1 - 192.168.1.127` .

 [1]: https://github.com/aa65535/openwrt-shadowsocks/commit/0c50b35
 [2]: https://github.com/aa65535/openwrt-shadowsocks/commit/709f931
 [3]: https://github.com/aa65535/openwrt-shadowsocks/wiki/LuCI:-Access-Control
 [C]: http://en.wikipedia.org/wiki/Classless_Inter-Domain_Routing