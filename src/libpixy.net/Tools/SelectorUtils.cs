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
    public class SelectorUtils
    {
        public static bool Match(string str, string pattern)
        {
            ScopeSelector sel = new ScopeSelector(pattern);
            return sel.Match(str);
        }

        public static bool WildcardMatch(string str, string pattern)
        {
            WildcardSelector sel = new WildcardSelector(pattern);
            return sel.Match(str);
        }

        public static bool RegexpMatch(string str, string pattern)
        {
            RegexpSelector sel = new RegexpSelector(pattern);
            return sel.Match(str);
        }

        /// <summary>
        /// 最初の要素を取得する. AAA.BBB.CCC だったら AAA を返す.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetFirstElement(string str)
        {
            string[] elm = str.Split('.');
            if (elm.Length > 0)
            {
                return elm[0];
            }

            return "";
        }

        /// <summary>
        /// 最後の要素を取得する. AAA.BBB.CCC だったら CCC を返す.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetLastElement(string str)
        {
            string[] elm = str.Split('.');
            if (elm.Length > 0)
            {
                return elm[elm.Length - 1];
            }

            return "";
        }

        /// <summary>
        /// 特定の要素を取得する. AAA.BBB.CCC で 1 を指定したとき BBB を返す.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetElementAt(string str, int i)
        {
            string[] elm = str.Split('.');
            if (i < elm.Length)
            {
                return elm[i];
            }

            return "";
        }
    }
}
