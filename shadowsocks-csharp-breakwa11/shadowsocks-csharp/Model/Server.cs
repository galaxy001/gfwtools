using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using SimpleJson;
using Shadowsocks.Controller;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;

namespace Shadowsocks.Model
{
    public class TransLog
    {
        public int size;
        public DateTime recvTime;
        public TransLog(int s, DateTime t)
        {
            size = s;
            recvTime = t;
        }
    }
    public class ServerSpeedLogShow
    {
        public long totalConnectTimes;
        public long totalDisconnectTimes;
        public long errorConnectTimes;
        public long errorTimeoutTimes;
        public long errorNoDataTimes;
        public long errorContinurousTimes;
        public long totalUploadBytes;
        public long totalDownloadBytes;
        public int sumConnectTime;
        public long avgConnectTime;
        public long avgDownloadBytes;
        public long maxDownloadBytes;
    }
    public class ServerSpeedLog
    {
        private long totalConnectTimes = 0;
        private long totalDisconnectTimes = 0;
        private long errorConnectTimes = 0;
        private long errorTimeoutTimes = 0;
        private long errorNoDataTimes = 0;
        private long errorContinurousTimes = 0;
        private long transUpload = 0;
        private long transDownload = 0;
        private List<TransLog> transLog = null;
        private long maxTransDownload = 0;
        private List<int> connectTime = null;
        private int sumConnectTime = 0;
        private List<TransLog> speedLog = null;

        public ServerSpeedLogShow Translate()
        {
            ServerSpeedLogShow ret = new ServerSpeedLogShow();
            lock (this)
            {
                ret.avgDownloadBytes = AvgDownloadBytes;
                ret.avgConnectTime = AvgConnectTime;
                ret.maxDownloadBytes = maxTransDownload;
                ret.totalConnectTimes = totalConnectTimes;
                ret.totalDisconnectTimes = totalDisconnectTimes;
                ret.errorConnectTimes = errorConnectTimes;
                ret.errorTimeoutTimes = errorTimeoutTimes;
                ret.errorNoDataTimes = errorNoDataTimes;
                ret.errorContinurousTimes = errorContinurousTimes;
                ret.totalUploadBytes = transUpload;
                ret.totalDownloadBytes = transDownload;
                ret.sumConnectTime = sumConnectTime;
            }
            return ret;
        }
        public long TotalConnectTimes
        {
            get
            {
                lock (this)
                {
                    return totalConnectTimes;
                }
            }
        }
        public long TotalDisconnectTimes
        {
            get
            {
                lock (this)
                {
                    return totalDisconnectTimes;
                }
            }
        }
        public long ErrorConnectTimes
        {
            get
            {
                lock (this)
                {
                    return errorConnectTimes;
                }
            }
        }
        public long ErrorTimeoutTimes
        {
            get
            {
                lock (this)
                {
                    return errorTimeoutTimes;
                }
            }
        }
        public long ErrorNoDataTimes
        {
            get
            {
                lock (this)
                {
                    return errorNoDataTimes;
                }
            }
        }
        public long ErrorContinurousTimes
        {
            get
            {
                lock (this)
                {
                    return errorContinurousTimes;
                }
            }
        }
        public long TotalUploadBytes
        {
            get
            {
                lock (this)
                {
                    return transUpload;
                }
            }
        }
        public long TotalDownloadBytes
        {
            get
            {
                lock (this)
                {
                    return transDownload;
                }
            }
        }
        public long AvgDownloadBytes
        {
            get
            {
                List<TransLog> transLog;
                lock (this)
                {
                    if (this.transLog == null)
                        return 0;
                    transLog = new List<TransLog>();
                    for (int i = 1; i < this.transLog.Count; ++i)
                    {
                        transLog.Add(this.transLog[i]);
                    }
                }
                {
                    long totalBytes = 0;
                    double totalTime = 0;
                    if (transLog.Count > 0 && DateTime.Now > transLog[transLog.Count - 1].recvTime.AddSeconds(10))
                    {
                        transLog.Clear();
                        return 0;
                    }
                    for (int i = 1; i < transLog.Count; ++i)
                    {
                        totalBytes += transLog[i].size;
                    }

                    if (false)
                    {
                        for (int i = 1; i < transLog.Count; ++i)
                        {
                            long speed = (long)(transLog[i].size / (transLog[i].recvTime - transLog[i - 1].recvTime).TotalSeconds);
                            if (speed > maxTransDownload)
                                maxTransDownload = speed;
                        }
                    }
                    else if (false)
                    {
                        int maxSpeed = 0;
                        if (speedLog != null)
                        {
                            for (int i = 0; i < speedLog.Count; ++i)
                            {
                                maxSpeed = Math.Max(maxSpeed, speedLog[i].size);
                            }
                        }
                        maxTransDownload = maxSpeed;
                    }
                    else //if (false)
                    {
                        long sumBytes = 0;
                        int iBeg = 0;
                        int iEnd = 0;
                        for (iEnd = 0; iEnd < transLog.Count; ++iEnd)
                        {
                            sumBytes += transLog[iEnd].size;
                            while (iBeg + 10 <= iEnd // 10 packet
                                && (transLog[iEnd].recvTime - transLog[iBeg].recvTime).TotalSeconds > 5)
                            {
                                //if ((transLog[iBeg + 1].recvTime - transLog[iBeg].recvTime).TotalMilliseconds > 20)
                                {
                                    long speed = (long)((sumBytes - transLog[iBeg].size) / (transLog[iEnd].recvTime - transLog[iBeg].recvTime).TotalSeconds);
                                    if (speed > maxTransDownload)
                                        maxTransDownload = speed;
                                }
                                sumBytes -= transLog[iBeg].size;
                                iBeg++;
                            }
                        }
                    }
                    if (transLog.Count > 1)
                        totalTime = (transLog[transLog.Count - 1].recvTime - transLog[0].recvTime).TotalSeconds;
                    if (totalTime > 1)
                    {
                        long ret = (long)(totalBytes / totalTime);
                        if (ret > maxTransDownload)
                            maxTransDownload = ret;
                        return ret;
                    }
                    else
                        return 0;
                }
            }
        }
        public long MaxDownloadBytes
        {
            get
            {
                lock (this)
                {
                    return maxTransDownload;
                }
            }
        }
        public long AvgConnectTime
        {
            get
            {
                lock (this)
                {
                    if (connectTime != null)
                    {
                        if (connectTime.Count > 4)
                        {
                            List<int> sTime = new List<int>();
                            foreach (int t in connectTime)
                            {
                                sTime.Add(t);
                            }
                            sTime.Sort();
                            int sum = 0;
                            for (int i = 0; i < connectTime.Count / 2; ++i)
                            {
                                sum += sTime[i];
                            }
                            return sum / (connectTime.Count / 2);
                        }
                        if (connectTime.Count > 0)
                            return sumConnectTime / connectTime.Count;
                    }
                    return -1;
                }
            }
        }
        public void ClearError()
        {
            lock (this)
            {
                if (totalConnectTimes > totalDisconnectTimes)
                    totalConnectTimes -= totalDisconnectTimes;
                else
                    totalConnectTimes = 0;
                totalDisconnectTimes = 0;
                errorConnectTimes = 0;
                errorTimeoutTimes = 0;
                errorNoDataTimes = 0;
                errorContinurousTimes = 0;
            }
        }
        public void Clear()
        {
            lock (this)
            {
                if (totalConnectTimes > totalDisconnectTimes)
                    totalConnectTimes -= totalDisconnectTimes;
                else
                    totalConnectTimes = 0;
                totalDisconnectTimes = 0;
                errorConnectTimes = 0;
                errorTimeoutTimes = 0;
                errorNoDataTimes = 0;
                errorContinurousTimes = 0;
                transUpload = 0;
                transDownload = 0;
                maxTransDownload = 0;
            }
        }
        public void AddConnectTimes()
        {
            lock (this)
            {
                totalConnectTimes += 1;
            }
        }
        public void AddDisconnectTimes()
        {
            lock (this)
            {
                totalDisconnectTimes += 1;
            }
        }
        public void AddErrorTimes()
        {
            lock (this)
            {
                errorConnectTimes += 1;
                errorContinurousTimes += 1;
            }
        }
        public void AddNoDataTimes()
        {
            lock (this)
            {
                errorNoDataTimes += 1;
                errorContinurousTimes += 1;
            }
        }
        public void HasData()
        {
            lock (this)
            {
                errorNoDataTimes = 0;
                errorContinurousTimes = 0;
            }
        }
        public void AddTimeoutTimes()
        {
            lock (this)
            {
                errorTimeoutTimes += 1;
                errorContinurousTimes += 1;
            }
        }
        public void AddUploadBytes(long bytes)
        {
            lock (this)
            {
                transUpload += bytes;
            }
        }
        public void AddDownloadBytes(long bytes)
        {
            lock (this)
            {
                transDownload += bytes;
                if (transLog == null)
                    transLog = new List<TransLog>();
                if (transLog.Count > 0 && (DateTime.Now - transLog[transLog.Count - 1].recvTime).TotalMilliseconds < 100)
                {
                    transLog[transLog.Count - 1].size += (int)bytes;
                }
                else
                {
                    transLog.Add(new TransLog((int)bytes, DateTime.Now));
                    while (transLog.Count > 0 && DateTime.Now > transLog[0].recvTime.AddSeconds(10))
                    {
                        transLog.RemoveAt(0);
                    }
                }
            }
        }
        public void AddConnectTime(int millisecond)
        {
            lock (this)
            {
                if (connectTime == null)
                    connectTime = new List<int>();
                connectTime.Add(millisecond);
                sumConnectTime += millisecond;
                while (connectTime.Count > 20)
                {
                    sumConnectTime -= connectTime[0];
                    connectTime.RemoveAt(0);
                }
            }
        }
        public void AddSpeedLog(TransLog speed)
        {
            lock (this)
            {
                if (speedLog == null)
                    speedLog = new List<TransLog>();
                if (speed.size > 0)
                    speedLog.Add(speed);
                while (speedLog.Count > 20)
                {
                    speedLog.RemoveAt(0);
                }
            }
        }
    }
    public class DnsBuffer
    {
        public IPAddress ip;
        public DateTime updateTime;
        public string host;
        public bool isExpired(string host)
        {
            if (updateTime == null) return true;
            if (this.host != host) return true;
            return (DateTime.Now - updateTime).TotalMinutes > 10;
        }
        public void UpdateDns(string host, IPAddress ip)
        {
            updateTime = DateTime.Now;
            this.ip = ip;
            this.host = host;
        }
    }
    public class Connections
    {
        private System.Collections.Generic.Dictionary<Socket, Int32> sockets = new Dictionary<Socket, int>();
        public bool AddRef(Socket socket)
        {
            lock (this)
            {
                if (sockets.ContainsKey(socket))
                {
                    sockets[socket] += 1;
                }
                else
                {
                    sockets[socket] = 1;
                }
                return true;
            }
        }
        public bool DecRef(Socket socket)
        {
            lock (this)
            {
                if (sockets.ContainsKey(socket))
                {
                    sockets[socket] -= 1;
                    if (sockets[socket] == 0)
                    {
                        sockets.Remove(socket);
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
        }
        public void CloseAll()
        {
            Socket[] s;
            lock (this)
            {
                s = new Socket[sockets.Count];
                sockets.Keys.CopyTo(s, 0);
            }
            foreach (Socket socket in s)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Send);
                    //socket.Shutdown(SocketShutdown.Both);
                    //socket.Close();
                }
                catch
                {

                }
            }
        }
        public int Count
        {
            get
            {
                return sockets.Count;
            }
        }
    }

    [Serializable]
    public class Server
    {
        public string server;
        public int server_port;
        public string password;
        public string method;
        public string remarks;
        public bool tcp_over_udp;
        public bool udp_over_tcp;
        public bool obfs_tcp;
        public bool obfs_udp;
        private bool enable;

        private ServerSpeedLog serverSpeedLog = new ServerSpeedLog();
        private DnsBuffer dnsBuffer = new DnsBuffer();
        private DnsBuffer dnsTargetBuffer = new DnsBuffer();
        private Connections Connections = new Connections();

        public void CopyServer(Server Server)
        {
            this.serverSpeedLog = Server.serverSpeedLog;
            this.dnsBuffer = Server.dnsBuffer;
            this.dnsTargetBuffer = Server.dnsTargetBuffer;
            this.Connections = Server.Connections;
            this.enable = Server.enable;
        }
        public void SetConnections(Connections Connections)
        {
            this.Connections = Connections;
        }

        public Connections GetConnections()
        {
            return Connections;
        }

        public DnsBuffer DnsBuffer()
        {
            return dnsBuffer;
        }

        public DnsBuffer DnsTargetBuffer()
        {
            return dnsTargetBuffer;
        }

        public ServerSpeedLog ServerSpeedLog()
        {
            return serverSpeedLog;
        }
        public void SetServerSpeedLog(ServerSpeedLog log)
        {
            serverSpeedLog = log;
        }
        public string FriendlyName()
        {
            if (string.IsNullOrEmpty(server))
            {
                return I18N.GetString("New server");
            }
            if (string.IsNullOrEmpty(remarks))
            {
                if (server.IndexOf(':') >= 0)
                {
                    return "[" + server + "]:" + server_port;
                }
                else
                {
                    return server + ":" + server_port;
                }
            }
            else
            {
                if (server.IndexOf(':') >= 0)
                {
                    return remarks + " ([" + server + "]:" + server_port + ")";
                }
                else
                {
                    return remarks + " (" + server + ":" + server_port + ")";
                }
            }
        }

        public Server Clone()
        {
            Server ret = new Server();
            ret.server = (string)server.Clone();
            ret.server_port = server_port;
            ret.password = (string)password.Clone();
            ret.method = (string)method.Clone();
            ret.remarks = (string)remarks.Clone();
            ret.enable = enable;
            ret.udp_over_tcp = udp_over_tcp;
            ret.obfs_tcp = obfs_tcp;
            ret.obfs_udp = obfs_udp;
            return ret;
        }

        public Server()
        {
            this.server = "127.0.0.1";
            this.server_port = 8388;
            this.method = "aes-256-cfb";
            this.password = "0";
            this.remarks = "";
            this.udp_over_tcp = false;
            this.obfs_tcp = false;
            this.obfs_udp = false;
            this.enable = true;
        }

        public Server(string ssURL) : this()
        {
            string[] r1 = Regex.Split(ssURL, "ss://", RegexOptions.IgnoreCase);
            string base64 = r1[1].ToString();
            byte[] bytes = null;
            string data = "";
            if (base64.LastIndexOf('@') > 0)
            {
                data = base64;
            }
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    try
                    {
                        bytes = System.Convert.FromBase64String(base64);
                    }
                    catch (FormatException)
                    {
                        base64 += "=";
                    }
                }
                if (bytes != null)
                {
                    data = Encoding.UTF8.GetString(bytes);
                }
            }
            if (data.Length == 0)
            {
                throw new FormatException();
            }
            try
            {
                int indexLastAt = data.LastIndexOf('@');
                int remarkIndexLastAt = data.IndexOf('#', indexLastAt);
                if (remarkIndexLastAt > 0)
                {
                    if (remarkIndexLastAt + 1 < data.Length)
                        this.remarks = data.Substring(remarkIndexLastAt + 1);
                    data = data.Substring(0, remarkIndexLastAt);
                }

                string afterAt = data.Substring(indexLastAt + 1);
                int indexLastColon = afterAt.LastIndexOf(':');
                this.server_port = int.Parse(afterAt.Substring(indexLastColon + 1));
                this.server = afterAt.Substring(0, indexLastColon);

                string beforeAt = data.Substring(0, indexLastAt);
                string[] parts = beforeAt.Split(new[] { ':' });
                this.method = parts[0];
                this.password = parts[1];
            }
            catch (IndexOutOfRangeException)
            {
                throw new FormatException();
            }
        }

        public bool isEnable()
        {
            return enable;
        }

        public void setEnable(bool enable)
        {
            this.enable = enable;
        }
    }
}
