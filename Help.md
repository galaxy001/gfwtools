Introduction
============
Shadowsocks is a cross-platform tunnel proxy which can help you get through firewalls.

This iOS version is for **non-jailbroken devices**. It has two features.

1. A web browser with all the traffic going through a Shadowsocks proxy
2. A background global proxy, with some restrictions

Requirements
-------------

- An iOS device, with >= iOS 5
- A Shadowsocks server (See [server setup](#server-side))

As a web browser
----------------
Shadowsocks works as a multi-tab web browser. It's really easy to use.

- Tap the + button to open menu.
- Tap Settings to configure Shadowsocks proxy settings.
- Tap New Tab to open a new Tab. Tap URL field on the top to input URL.
- Swipe a tab to scroll the tabs. Hold and press a tab to swap tabs.
- If you've changed Proxy Mode, **a restart is needed to take effect**.
(Kill the app, then open the app again).

As a global proxy
---------------------

Shadowsocks works as a background global PAC proxy, with some restrictions.

- Only works with Wi-Fi network. But we are working on the cellular network.
- Only works for a few minutes. Due to iOS restrictions, Shadowsocks can't
keep running in the background. It's killed after you left it for a while.
**To keep it running long, you have to come back to Shadowsocks app every
few minutes.**
- If you are an iOS developer, you can find some commented code in the source
code, which tries to **play a piece of music**. Thus the app can keep running all
the time. But this **prevents us from getting approved on the App Store**. You have
to build the app for yourself, and use the app yourself.

So it's a little tricky to use global proxy.

- Set up proxy settings in shadowsocks.
- Copy this link http://127.0.0.1:8090/proxy.pac
- Open iOS Settings -> Wi-Fi -> `i` icon on the right of your connected Wi-Fi -> 
HTTP Proxy. Choose Auto, paste the link in the URL field. Tap back.
- Other apps now go through the proxy. If they don't, kill and restart them.
- Come back every few minutes to keep Shadowsocks running in the background.

<a id="server-side"></a>
How to setup server side
------------------------

Set up a Shadowsocks server on your VPS.

You can choose one of the following servers:

- [Python version](https://github.com/clowwindy/shadowsocks)
- [Go version](https://github.com/shadowsocks/shadowsocks-go) ([中文](http://shadowsocks.github.io/shadowsocks-go/))
- [Node.js version](https://github.com/clowwindy/shadowsocks-nodejs)
- [C version](https://github.com/madeye/shadowsocks-libev)

You can find all available servers [here](https://github.com/clowwindy/shadowsocks/wiki/Ports-and-Clients#server-side).

Some FAQ is in the [Trobleshooting](https://github.com/clowwindy/shadowsocks/wiki/Troubleshooting).

Build (For developers)
----------------------

First, you have to update submodules:

    git submodule update --recursive --init
    open shadowsocks.xcodeproj

Then build with XCode.

Still need help?
----------------

Mailing list: http://groups.google.com/group/shadowsocks

Issue tracker: https://github.com/shadowsocks/shadowsocks-iOS/issues?state=open