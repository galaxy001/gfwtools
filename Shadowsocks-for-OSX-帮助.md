为 OS X 10.8+ 设计的 Shadowsocks 图形界面，启动后可自动实现全局翻墙，并根据 GFWList 区分墙内外流量。

下载
----
https://sourceforge.net/projects/shadowsocksgui/

基本使用
-------
1. 解压后移动到合适目录下，然后启动。
2. 如果弹出系统安全提示，请选「允许」。
3. Shadowsocks 会自动设置为全局 PAC 代理，Chrome、Safari、Twitter 都可以正常使用了。
4. 如果你开启了其它翻墙工具，请先将它们关闭。如果你使用了 Chrome 扩展程序 SwitchySharp，请把它的模式设置为「使用系统代理设置」。
5. 启动后可以在菜单栏右边找到 Shadowsocks 图标。

高级使用
-------
1. 如果你不想用全局 PAC 代理，想配合 SwitchySharp 等插件使用，可在菜单栏图标里点`关闭 Shadowsocks`。关闭后代理仍会运行在 `127.0.0.1:1080` 上，代理类型为 SOCKS v5。之所以不叫`关闭 PAC`，因为很多人不懂什么是 PAC。写`关闭 Shadowsocks` 更容易理解。
2. 默认使用公共服务器，可以在菜单栏图标里配置自定义服务器。
3. 切换服务器后，因为 Chrome 保持长连接，可能需要重启浏览器才能生效。也可以重启 ShadowsocksX 来强制 Chrome 重新连接。
4. 可以在菜单里点 `编辑 PAC` 来修改 PAC 文件，文件保存后会自动通知浏览器重新加载。推荐用 Xcode 等代码编辑器来编辑。如果用系统自带的文本编辑器，引号可能自动半角变全角，需要撤销一下回到半角。
5. 可以在菜单栏图标里打开控制台查看日志，其中 `ShadowsocksX:` 开头的是 Shadowsocks 的日志。

