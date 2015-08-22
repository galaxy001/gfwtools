Shadowsocks for iOS
===================

***Notice: This version is deprecated. Please wait for iOS 9's new VPN API***

Features
--------
Shadowsocks is a cross-platform tunnel proxy which can help you get through firewalls.

This iOS version is for **non-jailbroken devices**. It has two features.

1. A web browser with all the traffic going through a Shadowsocks proxy
2. A background global proxy, with some restrictions

Install
-------
[![Available on the App Store](https://raw.github.com/shadowsocks/shadowsocks-iOS/master/appstore.png)](https://itunes.apple.com/us/app/shadowsocks/id665729974?ls=1&mt=8)

Please visit the [App Store](https://itunes.apple.com/us/app/shadowsocks/id665729974?ls=1&mt=8).

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
keep running in the background. It's killed after you leave it for a while.
**To keep it running for an extended period of time, you have to come back to the Shadowsocks app every
few minutes.**

So it's a little tricky to use global proxy.

- Set up proxy settings in shadowsocks.
- Copy this link http://127.0.0.1:8090/proxy.pac
- Open iOS Settings -> Wi-Fi -> `i` icon on the right of your connected Wi-Fi -> 
HTTP Proxy. Choose Auto, paste the link in the URL field. Tap back.
- Other apps now go through the proxy. If they don't, kill and restart them.
- Come back every few minutes to keep Shadowsocks running in the background.

Still need help?
----------------

Mailing list: http://groups.google.com/group/shadowsocks

Issue tracker: https://github.com/shadowsocks/shadowsocks-iOS/issues?state=open