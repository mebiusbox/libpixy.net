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
    public class ScopeName
    {
        #region Private Member Variables

        private List<string> m_elements = new List<string>();

        #endregion // Private Member Variables

        #region Public Types

        public enum MatchMode
        {
            Scope,
            Wildcard,
            Regexp
        }

        #endregion // Public Types

        #region Public Properties

        public int Count
        {
            get { return m_elements.Count; }
        }

        public string[] Elements
        {
            get { return m_elements.ToArray(); }
            set
            {
                m_elements.Clear();
                m_elements.AddRange(value);
            }
        }

        #endregion // Public Properties

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScopeName()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        public ScopeName(string name)
        {
            Parse(name);
        }

        #endregion // Constructor

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public bool Parse(string selector)
        {
            m_elements.Clear();
            m_elements.AddRange(selector.Split('.'));
            return true;
        }

        /// <summary>
        /// パターン比較.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool Match(string pattern)
        {
            return Match(pattern, MatchMode.Scope);
        }

        /// <summary>
        /// パターン比較.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool Match(string pattern, MatchMode mode)
        {
            switch (mode)
            {
                case MatchMode.Scope:
                    return SelectorUtils.Match(this.ToString(), pattern);

                case MatchMode.Wildcard:
                    return SelectorUtils.WildcardMatch(this.ToString(), pattern);

                case MatchMode.Regexp:
                    return SelectorUtils.RegexpMatch(this.ToString(), pattern);
            }

            return false;
        }

        /// <summary>
        /// 文字列化.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(".", m_elements.ToArray());
        }

        /// <summary>
        /// 最初の要素を取得.
        /// </summary>
        /// <returns></returns>
        public string GetFirstElement()
        {
            if (this.Count > 0)
            {
                return m_elements[0];
            }

            return "";
        }

        /// <summary>
        /// 最後の要素を取得.
        /// </summary>
        /// <returns></returns>
        public string GetLastElement()
        {
            if (this.Count > 0)
            {
                return m_elements[this.Count - 1];
            }

            return "";
        }

        /// <summary>
        /// 最後の要素を削除.
        /// </summary>
        public void RemoveLastElement()
        {
            if (this.Count > 0)
            {
                m_elements.RemoveAt(this.Count - 1);
            }
        }

        /// <summary>
        /// 要素を挿入.
        /// </summary>
        /// <param name="at"></param>
        /// <param name="element"></param>
        public void InsertElement(int at, string element)
        {
            m_elements.Insert(at, element);
        }

        /// <summary>
        /// 末尾に要素を追加.
        /// </summary>
        /// <param name="element"></param>
        public void AppendElement(string element)
        {
            m_elements.Add(element);
        }

        #endregion // Public Methods
    }
}
