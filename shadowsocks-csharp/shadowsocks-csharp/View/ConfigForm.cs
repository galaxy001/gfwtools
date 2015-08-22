﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using Shadowsocks.Controller;
using Shadowsocks.Model;
using Shadowsocks.Properties;
using ZXing.QrCode.Internal;

namespace Shadowsocks.View
{
    public partial class ConfigForm : Form
    {
        private ShadowsocksController controller;
        private UpdateChecker updateChecker;

        // this is a copy of configuration that we are working on
        private Configuration _modifiedConfiguration;
        private int _oldSelectedIndex = -1;

        public ConfigForm(ShadowsocksController controller, UpdateChecker updateChecker)
        {
            this.Font = System.Drawing.SystemFonts.MessageBoxFont;
            InitializeComponent();

            // a dirty hack
            //this.ServersListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.PerformLayout();

            this.Icon = Icon.FromHandle(Resources.ssw128.GetHicon());
            this.controller = controller;
            this.updateChecker = updateChecker;
            if (updateChecker.LatestVersionURL == null)
                LinkUpdate.Visible = false;

            UpdateTexts();
            controller.ConfigChanged += controller_ConfigChanged;

            LoadCurrentConfiguration();
        }

        private void UpdateTexts()
        {
            this.Text = I18N.GetString("Edit Servers") + "("
                + (controller.GetCurrentConfiguration().shareOverLan ? "any" : "local") + ":" + controller.GetCurrentConfiguration().localPort.ToString()
                + I18N.GetString(" Version") + UpdateChecker.Version
                + ")";

            AddButton.Text = I18N.GetString("&Add");
            DeleteButton.Text = I18N.GetString("&Delete");
            UpButton.Text = I18N.GetString("Up");
            DownButton.Text = I18N.GetString("Down");

            IPLabel.Text = I18N.GetString("Server IP");
            ServerPortLabel.Text = I18N.GetString("Server Port");
            PasswordLabel.Text = I18N.GetString("Password");
            EncryptionLabel.Text = I18N.GetString("Encryption");
            ProxyPortLabel.Text = I18N.GetString("Proxy Port");
            RemarksLabel.Text = I18N.GetString("Remarks");

            LabelExpertSetting.Text = I18N.GetString(LabelExpertSetting.Text);
            UDPoverTCPLabel.Text = I18N.GetString(UDPoverTCPLabel.Text);
            ObfsTCPLabel.Text = I18N.GetString(ObfsTCPLabel.Text);
            ObfsUDPLabel.Text = I18N.GetString(ObfsUDPLabel.Text);
            LabelNote.Text = I18N.GetString(LabelNote.Text);
            CheckUDPoverUDP.Text = I18N.GetString(CheckUDPoverUDP.Text);
            CheckObfsTCP.Text = I18N.GetString(CheckObfsTCP.Text);
            CheckObfsUDP.Text = I18N.GetString(CheckObfsUDP.Text);
            LabelLink.Text = I18N.GetString(LabelLink.Text);

            ServerGroupBox.Text = I18N.GetString("Server");

            ReconnectLabel.Text = I18N.GetString("Reconnect Times");
            TTLLabel.Text = I18N.GetString("TTL");
            CheckAutoBan.Text = I18N.GetString("AutoBan");

            Socks5ProxyGroup.Text = I18N.GetString("Socks5 Proxy");
            CheckSocks5Proxy.Text = I18N.GetString("Socks5 Proxy On");
            LabelS5Server.Text = I18N.GetString("Server IP");
            LabelS5Port.Text = I18N.GetString("Server Port");
            LabelS5Server.Text = I18N.GetString("Server IP");
            LabelS5Port.Text = I18N.GetString("Server Port");
            LabelS5Username.Text = I18N.GetString("Username");
            LabelS5Password.Text = I18N.GetString("Password");

            LabelRandom.Text = I18N.GetString("Balance");
            for (int i = 0; i < RandomComboBox.Items.Count; ++i)
            {
                RandomComboBox.Items[i] = I18N.GetString(RandomComboBox.Items[i].ToString());
            }

            OKButton.Text = I18N.GetString("OK");
            MyCancelButton.Text = I18N.GetString("Cancel");
        }

        private void controller_ConfigChanged(object sender, EventArgs e)
        {
            LoadCurrentConfiguration();
        }
        
        private void ShowWindow()
        {
            this.Opacity = 1;
            this.Show();
            IPTextBox.Focus();
        }

        private int SaveOldSelectedServer()
        {
            try
            {
                if (_oldSelectedIndex == -1 || _oldSelectedIndex >= _modifiedConfiguration.configs.Count)
                {
                    return 0; // no changes
                }
                Server server = new Server
                {
                    server = IPTextBox.Text,
                    server_port = int.Parse(ServerPortTextBox.Text),
                    password = PasswordTextBox.Text,
                    method = EncryptionSelect.Text,
                    remarks = RemarksTextBox.Text,
                    udp_over_tcp = CheckUDPoverUDP.Checked,
                    obfs_tcp = CheckObfsTCP.Checked,
                    obfs_udp = CheckObfsUDP.Checked
                };
                int localPort = int.Parse(ProxyPortTextBox.Text);
                Configuration.CheckServer(server);
                Configuration.CheckPort(localPort);
                int ret = 0;
                if (_modifiedConfiguration.configs[_oldSelectedIndex].server != server.server
                    || _modifiedConfiguration.configs[_oldSelectedIndex].server_port != server.server_port
                    || _modifiedConfiguration.configs[_oldSelectedIndex].remarks != server.remarks
                    )
                {
                    ret = 1; // display changed
                }
                _modifiedConfiguration.configs[_oldSelectedIndex] = server;
                _modifiedConfiguration.localPort = localPort;
                _modifiedConfiguration.reconnectTimes = int.Parse(ReconnectText.Text);
                _modifiedConfiguration.randomAlgorithm = RandomComboBox.SelectedIndex;
                _modifiedConfiguration.TTL = int.Parse(TTLText.Text);
                _modifiedConfiguration.socks5enable = CheckSocks5Proxy.Checked;
                _modifiedConfiguration.socks5Host = TextS5Server.Text;
                _modifiedConfiguration.socks5Port = int.Parse(TextS5Port.Text);
                _modifiedConfiguration.socks5User = TextS5User.Text;
                _modifiedConfiguration.socks5Pass = TextS5Pass.Text;

                _modifiedConfiguration.autoban = CheckAutoBan.Checked;

                return ret;
            }
            catch (FormatException)
            {
                MessageBox.Show(I18N.GetString("Illegal port number format"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return -1; // ERROR
        }

        private void GenQR(string ssconfig)
        {
            string qrText = ssconfig;
            QRCode code = ZXing.QrCode.Internal.Encoder.encode(qrText, ErrorCorrectionLevel.M);
            ByteMatrix m = code.Matrix;
            int blockSize = Math.Max(PictureQRcode.Height / m.Height, 1);
            Bitmap drawArea = new Bitmap((m.Width * blockSize), (m.Height * blockSize));
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                g.Clear(Color.White);
                using (Brush b = new SolidBrush(Color.Black))
                {
                    for (int row = 0; row < m.Width; row++)
                    {
                        for (int col = 0; col < m.Height; col++)
                        {
                            if (m[row, col] != 0)
                            {
                                g.FillRectangle(b, blockSize * row, blockSize * col, blockSize, blockSize);
                            }
                        }
                    }
                }
            }
            PictureQRcode.Image = drawArea;
        }

        private void LoadSelectedServer()
        {
            if (ServersListBox.SelectedIndex >= 0 && ServersListBox.SelectedIndex < _modifiedConfiguration.configs.Count)
            {
                Server server = _modifiedConfiguration.configs[ServersListBox.SelectedIndex];

                IPTextBox.Text = server.server;
                ServerPortTextBox.Text = server.server_port.ToString();
                PasswordTextBox.Text = server.password;
                ProxyPortTextBox.Text = _modifiedConfiguration.localPort.ToString();
                EncryptionSelect.Text = server.method ?? "aes-256-cfb";
                RemarksTextBox.Text = server.remarks;
                CheckUDPoverUDP.Checked = server.udp_over_tcp;
                CheckObfsTCP.Checked = server.obfs_tcp;
                CheckObfsUDP.Checked = server.obfs_udp;

                ServerGroupBox.Visible = true;
                ReconnectText.Text = _modifiedConfiguration.reconnectTimes.ToString();
                RandomComboBox.SelectedIndex = _modifiedConfiguration.randomAlgorithm;
                TTLText.Text = _modifiedConfiguration.TTL.ToString();

                CheckSocks5Proxy.Checked = _modifiedConfiguration.socks5enable;
                TextS5Server.Text = _modifiedConfiguration.socks5Host;
                TextS5Port.Text = _modifiedConfiguration.socks5Port.ToString();
                TextS5User.Text = _modifiedConfiguration.socks5User;
                TextS5Pass.Text = _modifiedConfiguration.socks5Pass;

                TextLink.Text = controller.GetSSLinkForServer(server);

                CheckAutoBan.Checked = _modifiedConfiguration.autoban;

                PasswordLabel.Checked = false;
                GenQR(TextLink.Text);
                //IPTextBox.Focus();
            }
            else
            {
                ServerGroupBox.Visible = false;
            }
        }

        private void LoadConfiguration(Configuration configuration)
        {
            if (ServersListBox.Items.Count != _modifiedConfiguration.configs.Count)
            {
                ServersListBox.Items.Clear();
                foreach (Server server in _modifiedConfiguration.configs)
                {
                    ServersListBox.Items.Add(server.FriendlyName());
                }
            }
            else
            {
                for (int i = 0; i < _modifiedConfiguration.configs.Count; ++i)
                {
                    ServersListBox.Items[i] = _modifiedConfiguration.configs[i].FriendlyName();
                }
            }
        }

        private void SetServerListSelectedIndex(int index)
        {
            int oldSelectedIndex = _oldSelectedIndex;
            int selIndex = Math.Min(index + 5, ServersListBox.Items.Count - 1);
            if (selIndex != index)
            {
                _oldSelectedIndex = selIndex;
                ServersListBox.SelectedIndex = selIndex;
                _oldSelectedIndex = oldSelectedIndex;
                ServersListBox.SelectedIndex = index;
            }
            else
            {
                ServersListBox.SelectedIndex = index;
            }
        }

        private void LoadCurrentConfiguration()
        {
            _modifiedConfiguration = controller.GetConfiguration();
            LoadConfiguration(_modifiedConfiguration);
            SetServerListSelectedIndex(_modifiedConfiguration.index);
            LoadSelectedServer();
        }

        private void ServersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_oldSelectedIndex == ServersListBox.SelectedIndex || ServersListBox.SelectedIndex == -1)
            {
                // we are moving back to oldSelectedIndex or doing a force move
                return;
            }
            int change = SaveOldSelectedServer();
            if (change == -1)
            {
                ServersListBox.SelectedIndex = _oldSelectedIndex; // go back
                return;
            }
            if (change == 1)
            {
                LoadConfiguration(_modifiedConfiguration);
            }
            LoadSelectedServer();
            _oldSelectedIndex = ServersListBox.SelectedIndex;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (SaveOldSelectedServer() == -1)
            {
                return;
            }
            Server server = Configuration.GetDefaultServer();
            _modifiedConfiguration.configs.Add(server);
            LoadConfiguration(_modifiedConfiguration);
            ServersListBox.SelectedIndex = _modifiedConfiguration.configs.Count - 1;
            _oldSelectedIndex = ServersListBox.SelectedIndex;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            _oldSelectedIndex = ServersListBox.SelectedIndex;
            if (_oldSelectedIndex >= 0 && _oldSelectedIndex < _modifiedConfiguration.configs.Count)
            {
                _modifiedConfiguration.configs.RemoveAt(_oldSelectedIndex);
            }
            if (_oldSelectedIndex >= _modifiedConfiguration.configs.Count)
            {
                // can be -1
                _oldSelectedIndex = _modifiedConfiguration.configs.Count - 1;
            }
            ServersListBox.SelectedIndex = _oldSelectedIndex;
            LoadConfiguration(_modifiedConfiguration);
            SetServerListSelectedIndex(_oldSelectedIndex);
            LoadSelectedServer();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (SaveOldSelectedServer() == -1)
            {
                return;
            }
            if (_modifiedConfiguration.configs.Count == 0)
            {
                MessageBox.Show(I18N.GetString("Please add at least one server"));
                return;
            }
            controller.SaveServersConfig(_modifiedConfiguration);
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConfigForm_Shown(object sender, EventArgs e)
        {
            IPTextBox.Focus();
        }

        private void ConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            controller.ConfigChanged -= controller_ConfigChanged;
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            _oldSelectedIndex = ServersListBox.SelectedIndex;
            int index = _oldSelectedIndex;
            SaveOldSelectedServer();
            if (index > 0 && index < _modifiedConfiguration.configs.Count)
            {
                Server server = _modifiedConfiguration.configs[index - 1].Clone();
                _modifiedConfiguration.configs.Reverse(index - 1, 2);
                _oldSelectedIndex = index - 1;
                ServersListBox.SelectedIndex = index - 1;
                LoadConfiguration(_modifiedConfiguration);
                ServersListBox.SelectedIndex = _oldSelectedIndex;
                //SetServerListSelectedIndex(_oldSelectedIndex);
                LoadSelectedServer();
            }
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            _oldSelectedIndex = ServersListBox.SelectedIndex;
            int index = _oldSelectedIndex;
            SaveOldSelectedServer();
            if (_oldSelectedIndex >= 0 && _oldSelectedIndex < _modifiedConfiguration.configs.Count - 1)
            {
                Server server = _modifiedConfiguration.configs[index + 1].Clone();
                _modifiedConfiguration.configs.Reverse(index, 2);
                _oldSelectedIndex = index + 1;
                ServersListBox.SelectedIndex = index + 1;
                LoadConfiguration(_modifiedConfiguration);
                ServersListBox.SelectedIndex = _oldSelectedIndex;
                //SetServerListSelectedIndex(_oldSelectedIndex);
                LoadSelectedServer();
            }
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            SaveOldSelectedServer();
            LoadSelectedServer();
            ((TextBox)sender).SelectAll();
        }

        private void TextBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void LinkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(updateChecker.LatestVersionURL);
        }

        private void PasswordLabel_CheckedChanged(object sender, EventArgs e)
        {
            if (PasswordLabel.Checked)
            {
                PasswordTextBox.PasswordChar = '\0';
            }
            else
            {
                PasswordTextBox.PasswordChar = '●';
            }
        }

    }
}
