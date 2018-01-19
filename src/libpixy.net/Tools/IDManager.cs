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
using System.Text;

namespace libpixy.net.Tools
{
    /// <summary>
    /// ���j�[�N�h�c���Ǘ�����
    /// </summary>
    public class IDManager
    {
        private int m_next = 0;
        private List<int> m_list = new List<int>();

        /// <summary>
        /// 
        /// </summary>
        public IDManager()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public IDManager(int start)
        {
            m_next = start;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            m_next = 0;
            m_list.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        public void Reset(int start)
        {
            m_next = start;
            m_list.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetNextId()
        {
            if (m_list.Count > 0)
            {
                int tmp = m_list[0];
                m_list.RemoveAt(0);
                return tmp;
            }

            return m_next++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void PutId(int id)
        {
            m_list.Add(id);
        }
    }
}
