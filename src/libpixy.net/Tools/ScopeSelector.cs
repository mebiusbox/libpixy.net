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
    /// 
    /// </summary>
    public class ScopeSelector : Selector
    {
        #region Fields
        private string[] m_elements = null;
        //private int m_start;
        //private int m_end;
        #endregion Fields

        /// <summary>
        /// Constructor
        /// </summary>
        public ScopeSelector()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="selector"></param>
        public ScopeSelector(string selector)
        {
            Setup(selector);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public bool Setup(string selector)
        {
            m_elements = selector.Split('.');
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool Match(string str)
        {
            if (m_elements == null)
            {
                return false;
            }

            string[] elm = str.Split('.');
            int i = 0;
            int j = 0;
            for (i = 0; i < m_elements.Length; ++i)
            {
                if (j >= elm.Length)
                {
                    break;
                }

                if (elm[j] == m_elements[i] || m_elements[i] == "*")
                {//pass
                    ++j;
                }
                else
                {
                    break;
                }
            }

            if (i == m_elements.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
