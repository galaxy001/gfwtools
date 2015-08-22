**注意:** 此功能在 [709f931][1] 之后可用，且需要安装 [LuCI][2] 。

关于 LuCI 中的 **访问控制**:

 - LAN

  - 访问控制  
     >默认是禁用的，也就是全部内网设备都会经过 SS 代理  
     **仅允许列表内** 和 **仅允许列表外** 仅当 **内网IP列表** 不为空时有效  

  - 内网IP列表  
     >需要被控制的内网设备IP, 可以填写多个, 但每个输入框只能填写一个  
     点击输入框后面的 **添加按钮** 增加新的输入框  

 - WAN

  - 被忽略的IP  
     >当你希望某些国外的IP可以直连时, 可以将其填写在这里  

  - 走代理的IP  
     >当你希望某些国内的IP可以走SS时, 可以将其填写在这里  


 [1]: https://github.com/aa65535/openwrt-shadowsocks/commit/709f931
 [2]: https://github.com/aa65535/openwrt-dist-luci
 [C]: http://en.wikipedia.org/wiki/Classless_Inter-Domain_Routing
