**注意:** 此功能在 [709f931][1] 之后可用，需要安装 [LuCI][2] 。

通过设置 LuCI 中的 **内网访问控制** 可以:

 - 指定某些设备不走 ss, 其他设备走 ss  
   >如指定 IP 为 192.168.1.32 的设备不走 ss  
   >**访问控制模式** 修改为 **黑名单**  
   >**内网 IP 地址** 填写 `192.168.1.32`  

 - 指定某些设备走 ss, 其他设备不走 ss  
   >如指定 IP 为 192.168.1.32 的设备走 ss  
   >**访问控制模式** 修改为 **白名单**  
   >**内网 IP 地址** 填写 `192.168.1.32`  

 **内网 IP 地址** 可以填写多个, 但每行只能填写一个  
 点击输入框后面的**添加按钮**增加新的输入框  


 [1]: https://github.com/aa65535/openwrt-shadowsocks/commit/709f931
 [2]: https://github.com/aa65535/openwrt-dist-luci
 [C]: http://en.wikipedia.org/wiki/Classless_Inter-Domain_Routing
