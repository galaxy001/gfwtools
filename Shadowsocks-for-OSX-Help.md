Shadowsocks GUI designed for Mac OSX 10.7+, automatically configuring system proxy settings, with a GFWList PAC.

[中文帮助](https://github.com/shadowsocks/shadowsocks-iOS/wiki/Shadowsocks-for-OSX-%E5%B8%AE%E5%8A%A9)
Download
--------
https://sourceforge.net/projects/shadowsocksgui/

Basic Usage
-----------
1. Unzip the file, move it to a proper place, open it.
2. If any security prompt come out, please select *Allow*.
3. Shadowsocks will automatically configure the global system proxy settings.
4. Close any other proxy or VPN program. If you are using SwitchySharp, please set its mode to *Use System Proxy Settings*.

Advanced
--------
1. If you'd rather use SwitchySharp than a global PAC proxy, you can turn global proxy off in the menu bar icon. Shadowsocks will still be running at `127.0.0.1:1080`.
2. You can use your own server instead of the public server. The server preferences are also in the menu bar icon.
3. You may need to restart Chrome after you choose another server.
4. You can check the logs in the Console.app. Logs beginning with `ShadowsocksX:` is logs of Shadowsocks.

