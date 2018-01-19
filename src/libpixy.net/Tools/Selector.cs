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
using System.Text.RegularExpressions;

namespace libpixy.net.Tools
{
    /// <summary>
    /// セレクタ
    /// </summary>
    public interface Selector
    {
        bool Match(string str);
    }

    /// <summary>
    /// 
    /// </summary>
    public class WildcardSelector : Selector
    {
        #region Fields
        private string m_pattern;
        #endregion Fields

        /// <summary>
        /// Constructor
        /// </summary>
        public WildcardSelector()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="selector"></param>
        public WildcardSelector(string pattern)
        {
            m_pattern = pattern;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public bool Setup(string pattern)
        {
            m_pattern = pattern;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool Match(string str)
        {
            StringBuilder pattern = new StringBuilder();
            int len = m_pattern.Length;
            for (int i = 0; i < len; ++i)
            {
                if (m_pattern[i] == '.')
                {
                    pattern.Append("[.]");
                }
                else if (m_pattern[i] == '*')
                {
                    pattern.Append(".*");
                }
                else if (m_pattern[i] == '[')
                {
                    pattern.Append("\\[");
                }
                else if (m_pattern[i] == ']')
                {
                    pattern.Append("\\]");
                }
                else if (m_pattern[i] == '+')
                {
                    pattern.Append("\\+");
                }
                else if (m_pattern[i] == '#')
                {
                    pattern.Append("\\*");
                }
                else if (m_pattern[i] == '@')
                {
                    pattern.Append("\\@");
                }
                else
                {
                    pattern.Append(m_pattern[i]);
                }
            }

            Regex regex = new Regex(pattern.ToString(), RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match matcher = regex.Match(str);
            return matcher.Success;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RegexpSelector : Selector
    {
        #region Fields
        private string m_pattern;
        #endregion Fields

        /// <summary>
        /// Constructor
        /// </summary>
        public RegexpSelector()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="selector"></param>
        public RegexpSelector(string pattern)
        {
            m_pattern = pattern;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public bool Setup(string pattern)
        {
            m_pattern = pattern;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool Match(string str)
        {
            Regex regex = new Regex(m_pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match matcher = regex.Match(str);
            return matcher.Success;
        }
    }

    
}
