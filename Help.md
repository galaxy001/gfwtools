Introduction
============
Shadowsocks is a cross-platform tunnel proxy which can help you get through firewalls.

iOS version has two features.

1. As a web browser
2. As a global PAC proxy, with some limitations

Requirements
-------------

- An iOS device, with > iOS 5
- A Shadowsocks server (See [server setup](#how-to-setup-server-side))

As a web browser
----------------
Shadowsocks works as a multi-tab web browser. It's really easy to use.

- Tap the + button to open menu.
- Tap settings to input proxy settings.
- Tap Help for Help.
- If you've changed Proxy Mode, **a restart is needed to take effect**.
(Double tap home button, kill the app, then open the app again).

As a global PAC proxy
---------------------
Shadowsocks works as a background global PAC proxy, with some restrictions.

- Only works with Wi-Fi network.
- Only works for a few minutes. Due to iOS restrictions, Shadowsocks can't
keep running in the background. It's killed after you left it for a while.
**To keep it running long, you have to come back to Shadowsocks app every
few minutes.**
- If you are an iOS developer, you can find some commented code in the source
code, which plays a piece of music. Thus the app can keep running all the time.
But this **prevents us from getting approved in the App Store**. You have to build
the app for yourself, and use the app yourself.

How to use global proxy:

- Set up proxy settings in shadowsocks.
- Copy this link http://127.0.0.1:8090/proxy.pac
- Open iOS Settings -> Wi-Fi -> `i` icon on the right of your connected Wi-Fi -> HTTP Proxy. Choose Auto, paste the link in the URL field.
- Other apps now go through the proxy.
- Come back every few minutes to keep Shadowsocks running in the background.

How to setup server side
------------------------

You can choose one of the following servers:

- [Python version](https://github.com/clowwindy/shadowsocks)
- [Go version](https://github.com/shadowsocks/shadowsocks-go) ([中文](http://shadowsocks.github.io/shadowsocks-go/))
- [Node.js version](https://github.com/clowwindy/shadowsocks-nodejs)
- [C version](https://github.com/madeye/shadowsocks-libev)

You can find all available servers [here](https://github.com/clowwindy/shadowsocks/wiki/Ports-and-Clients#server-side).

Some FAQ is in the [Trobleshooting](https://github.com/clowwindy/shadowsocks/wiki/Troubleshooting).

Contact
--------

Mailing list: http://groups.google.com/group/shadowsocks