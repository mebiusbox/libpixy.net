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

namespace libpixy.net.Tools
{
    /// <summary>
    /// キーストロークアイテム
    /// </summary>
    public class KeyStrokeItem
    {
        public Keys KeyCode;
        public Keys Modifier;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keyData"></param>
        /// <param name="modifier"></param>
        public KeyStrokeItem(Keys KeyCode, Keys modifier)
        {
            this.KeyCode = KeyCode;
            this.Modifier = modifier;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            KeyStrokeItem rhs = obj as KeyStrokeItem;
            if (this.KeyCode == rhs.KeyCode &&
                this.Modifier == rhs.Modifier)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// キーストロークアイテムコレクション
    /// </summary>
    public class KeyStrokeItemCollection
    {
        /// <summary>
        /// キーストロークリスト
        /// </summary>
        public List<KeyStrokeItem> Items = new List<KeyStrokeItem>();

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            KeyStrokeItemCollection ksc = obj as KeyStrokeItemCollection;
            if (Items.Count != ksc.Items.Count)
            {
                return false;
            }

            for (int i=0; i<Items.Count; ++i)
            {
                if (Items[i].Equals(ksc.Items[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// キーストローク
    /// </summary>
    public class KeyStroke
    {
        /// <summary>
        /// キー
        /// </summary>
        public KeyStrokeItemCollection Keys = new KeyStrokeItemCollection();

        /// <summary>
        /// コマンド名
        /// </summary>
        public string Command;
    }

    /// <summary>
    /// キーストロークマネージャ
    /// </summary>
    public class KeyStrokeManager
    {
        /// <summary>
        /// キーストロークライブラリ
        /// </summary>
        public List<KeyStroke> Items = new List<KeyStroke>();

        #region Public Methods

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="keyData"></param>
        /// <param name="modifier"></param>
        /// <param name="cmdname"></param>
        public void Regist(Keys keyCode, Keys modifier, string cmdname)
        {
            KeyStroke ks = new KeyStroke();
            ks.Keys.Items.Add(new KeyStrokeItem(keyCode, modifier));
            ks.Command = cmdname;
            this.Items.Add(ks);
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="items"></param>
        public void Regist(List<KeyMapItem> items)
        {
            foreach (KeyMapItem item in items)
            {
                Regist(item.KeyCode, item.Modifier, item.Command);
            }
        }

        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string Find(KeyStrokeItemCollection item)
        {
            foreach (KeyStroke ks in this.Items)
            {
                if (ks.Keys.Equals(item))
                {
                    return ks.Command;
                }
            }

            return null;
        }

        #endregion Public Methods
    }
}
