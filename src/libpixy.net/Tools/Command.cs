/*
 * Copyright (c) 2018 mebiusbox software. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
 * OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 * SUCH DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace libpixy.net.Tools
{
    /// <summary>
    /// コマンド
    /// </summary>
    public class Command
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// イベントハンドラ
        /// </summary>
        public event EventHandler Handler;

        public Command(string name, EventHandler handler)
        {
            this.Name = name;
            this.Handler += handler;
        }

        public void AddHandler(EventHandler handler)
        {
            Handler += handler;
        }

        public void RemoveHandler(EventHandler handler)
        {
            handler -= handler;
        }

        public void RaiseEvent(object sender, EventArgs e)
        {
            if (Handler != null)
            {
                Handler(sender, e);
            }
        }
    }

    /// <summary>
    /// コマンドマネージャ
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// コマンドリスト
        /// </summary>
        public List<Command> Items = new List<Command>();

        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="cmdname"></param>
        public bool ExecCommand(string cmdname)
        {
            foreach (Command cmd in this.Items)
            {
                if (cmd.Name == cmdname)
                {
                    cmd.RaiseEvent(this, EventArgs.Empty);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handler"></param>
        public void Regist(string name, EventHandler handler)
        {
            this.Items.Add(new Command(name, handler));
        }
    }

    /// <summary>
    /// コマンド情報
    /// </summary>
    public class CommandInfo
    {
        /// <summary>
        /// コマンド名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// スコープ
        /// </summary>
        public string Scope { get; set; }
    }

    /// <summary>
    /// コマンドデータベース
    /// </summary>
    public class CommandDatabase
    {
        /// <summary>
        /// コマンドリスト
        /// </summary>
        private List<CommandInfo> Items = new List<CommandInfo>();
        public string FileName { get; set; }

        #region Public Methods

        /// <summary>
        /// ロード
        /// </summary>
        public void Load()
        {
            string filename = FileName;
            if (string.IsNullOrEmpty(filename) || System.IO.File.Exists(filename) == false)
            {
                System.Diagnostics.Debug.WriteLine("Utils.CommanDatabase: Invalid filename. Use <ExecutablePath>/Command.xml");
                // <executable path>/Command.xml
                filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Command.xml");
                if (System.IO.File.Exists(filename) == false)
                {
                    System.Diagnostics.Debug.WriteLine("Utils.CommanDatabase: Not found command database file.");
                    return;
                }
            }

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;
            XmlReader reader = XmlReader.Create(filename, settings);
            reader.Read();

            if (reader.IsStartElement("Command"))
            {
                reader.ReadStartElement("Command");
                for (; ; )
                {
                    if (reader.IsStartElement("CommandInfo"))
                    {
                        reader.ReadStartElement("CommandInfo");
                        CommandInfo cmdinfo = new CommandInfo();
                        cmdinfo.Name = reader.ReadElementString("name");
                        cmdinfo.Scope = reader.ReadElementString("scope");
                        cmdinfo.Description = reader.ReadElementString("desc");
                        this.Items.Add(cmdinfo);
                        reader.ReadEndElement();
                    }
                    else
                    {
                        break;
                    }
                }
                reader.ReadEndElement();
            }

            reader.Close();

#if DEBUG
            System.Diagnostics.Debug.WriteLine("CommandDatabase loaded. " + filename);
            System.Diagnostics.Debug.WriteLine("<CommandDatabase>");
            foreach (CommandInfo cmdinfo in Items)
            {
                System.Diagnostics.Debug.WriteLine("  " + cmdinfo.Name + " : " + cmdinfo.Description + " : " + cmdinfo.Scope);
            }
            System.Diagnostics.Debug.WriteLine("</CommandDatabase>");
#endif//DEBUG
        }

        /// <summary>
        /// コマンド名からコマンド情報を検索する
        /// </summary>
        /// <param name="cmdname"></param>
        /// <returns></returns>
        public CommandInfo Find(string cmdname)
        {
            foreach (CommandInfo cmdinfo in Items)
            {
                if (cmdinfo.Name == cmdname)
                {
                    return cmdinfo;
                }
            }

            return null;
        }

        /// <summary>
        /// 指定したスコープをもつコマンドのリストを取得する
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public List<CommandInfo> SelectByScope(string scope)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            foreach (CommandInfo cmdinfo in Items)
            {
                if (cmdinfo.Scope == scope)
                {
                    list.Add(cmdinfo);
                }
            }

            return list;
        }

        #endregion Public Methods
    }

}
