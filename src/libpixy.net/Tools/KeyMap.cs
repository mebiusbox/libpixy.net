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
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace libpixy.net.Tools
{
    /// <summary>
    /// アイテム
    /// </summary>
    public class KeyMapItem
    {
        /// <summary>
        /// コマンド名
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// スコープ
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// キーコード
        /// </summary>
        public Keys KeyCode { get; set; }

        /// <summary>
        /// 修飾キー
        /// </summary>
        public Keys Modifier { get; set; }

        #region Public Methods

        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="r"></param>
        public void Serialize(XmlWriter w)
        {
            w.WriteStartElement("KeyMapItem");
            w.WriteElementString("command", this.Command);
            w.WriteElementString("scope", this.Scope);
            w.WriteElementString("keycode", string.Format("{0}", (int)this.KeyCode));
            w.WriteElementString("modifier", string.Format("{0}", (int)this.Modifier));
            w.WriteEndElement();
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="r"></param>
        public void Deserialize(XmlReader r)
        {
            if (r.IsStartElement("KeyMapItem"))
            {
                r.ReadStartElement("KeyMapItem");
                this.Command = r.ReadElementString("command");
                this.Scope = r.ReadElementString("scope");
                this.KeyCode = (Keys)int.Parse(r.ReadElementString("keycode"));
                this.Modifier = (Keys)int.Parse(r.ReadElementString("modifier"));
                r.ReadEndElement();
            }
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Command : {0}  Scope : {1}  KeyCode : {2}  Modifier : {3}",
                this.Command, this.Scope, this.KeyCode.ToString(), this.Modifier.ToString());
        }

        /// <summary>
        /// キーストローク文字列を生成
        /// </summary>
        /// <returns></returns>
        public string ToKeyStrokeString()
        {
            List<string> parts = new List<string>();
            if ((this.Modifier & Keys.Shift) == Keys.Shift)
            {
                parts.Add("S");
            }
            if ((this.Modifier & Keys.Control) == Keys.Control)
            {
                parts.Add("C");
            }
            if ((this.Modifier & Keys.Alt) == Keys.Alt)
            {
                parts.Add("M");
            }

            parts.Add(this.KeyCode.ToString());

            return string.Join("-", parts.ToArray());
        }

        #endregion Public Methods
    }

    /// <summary>
    /// キーマップ
    /// </summary>
    public class KeyMap
    {
        #region Attributes

        public List<KeyMapItem> Items = new List<KeyMapItem>();
        public string FileName { get; set; }

        #endregion Attributes

        #region Public Events

        public event EventHandler Changed;

        /// <summary>
        /// 変更通知
        /// </summary>
        public void RaiseChanged()
        {
            if (Changed != null)
            {
                Changed(this, EventArgs.Empty);
            }
        }

        #endregion Public Events

        #region Public Methods

        /// <summary>
        /// ロード
        /// </summary>
        public void Load()
        {
            string filename = GetFilename(false);
            if (System.IO.File.Exists(filename) == false)
            {
                System.Diagnostics.Debug.WriteLine("Utils.KeyMap: Not found keymap file.");
                return;
            }

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;
            XmlReader reader = XmlReader.Create(filename, settings);
            reader.Read();

            if (reader.IsStartElement("KeyMap"))
            {
                int count = int.Parse(reader.GetAttribute("count"));
                reader.ReadStartElement("KeyMap");
                for (int i = 0; i < count; ++i)
                {
                    KeyMapItem item = new KeyMapItem();
                    item.Deserialize(reader);
                    this.Items.Add(item);
                }
                reader.ReadEndElement();
            }

            reader.Close();

#if DEBUG
            System.Diagnostics.Debug.WriteLine("KeyMap loaded. " + filename);
            System.Diagnostics.Debug.WriteLine("<KeyMap>");
            foreach (KeyMapItem item in Items)
            {
                System.Diagnostics.Debug.WriteLine("  " + item.ToString());
            }
            System.Diagnostics.Debug.WriteLine("</KeyMap>");
#endif//DEBUG
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            string filename = GetFilename(true);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            XmlWriter writer = XmlTextWriter.Create(filename, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("KeyMap");
            writer.WriteAttributeString("count", this.Items.Count.ToString());
            foreach (KeyMapItem item in Items)
            {
                item.Serialize(writer);
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

#if DEBUG
            System.Diagnostics.Debug.WriteLine("KeyMap saved. " + filename);
            System.Diagnostics.Debug.WriteLine("<KeyMap>");
            foreach (KeyMapItem item in Items)
            {
                System.Diagnostics.Debug.WriteLine("  " + item.ToString());
            }
            System.Diagnostics.Debug.WriteLine("</KeyMap>");
#endif//DEBUG
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="command"></param>
        /// <param name="scope"></param>
        /// <param name="keyCode"></param>
        /// <param name="modKey"></param>
        public void Regist(Keys keyCode, Keys modKey, string command, string scope)
        {
            KeyMapItem item = new KeyMapItem();
            item.Command = command;
            item.Scope = scope;
            item.KeyCode = keyCode;
            item.Modifier = modKey;
            this.Items.Add(item);
        }

        /// <summary>
        /// スコープからアイテムを選択して、そのリストを取得する
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public List<KeyMapItem> SelectByScope(string scope)
        {
            List<KeyMapItem> list = new List<KeyMapItem>();
            foreach (KeyMapItem item in Items)
            {
                if (item.Scope == scope)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// コマンド名からアイテムを選択して、そのリストを取得する
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public List<KeyMapItem> SelectByCommand(string command)
        {
            List<KeyMapItem> list = new List<KeyMapItem>();
            foreach (KeyMapItem item in Items)
            {
                if (item.Command == command)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// ファイル名を取得
        /// </summary>
        /// <returns></returns>
        private string GetFilename(bool create)
        {
            if (System.IO.File.Exists(FileName) == false && create == false)
            {
                // <executable path>/KeyMap.xml
                return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "KeyMap.xml");
            }

            return FileName;
        }

        #endregion Private methods

    }
}
