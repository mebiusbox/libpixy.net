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

namespace libpixy.net.Tools
{
    /// <summary>
    /// デバッグ出力
    /// </summary>
    public class DebugPrinter
    {
        #region Fields
        private string m_indent;
        #endregion Fields

        /// <summary>
        /// 書き込み（改行）
        /// </summary>
        /// <param name="str"></param>
        public void WriteLine(string str)
        {
            System.Diagnostics.Debug.WriteLine(m_indent + str);
        }

        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="str"></param>
        public void Write(string str)
        {
            System.Diagnostics.Debug.Write(m_indent + str);
        }

        /// <summary>
        /// インデント
        /// </summary>
        public void Indent()
        {
            m_indent += "  ";
        }

        /// <summary>
        /// インデント解除
        /// </summary>
        public void Unindent()
        {
            if (m_indent.Length > 2)
            {
                m_indent = m_indent.Substring(2);
            }
        }
    }
}
