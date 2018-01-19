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

namespace libpixy.net.Vecmath
{
    /// <summary>
    /// マトリクススタック
    /// </summary>
    public class MatrixStack
    {
        #region fields
        private Stack<Matrix4> m_stack = new Stack<Matrix4>();
        #endregion fields

        /// <summary>
        /// Constructor
        /// </summary>
        public MatrixStack()
        {
            m_stack.Push(Matrix4.Unit);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Push()
        {
            m_stack.Push(m_stack.Peek());
        }

        /// <summary>
        /// Push
        /// </summary>
        /// <param name="m"></param>
        public void Push(Matrix4 m)
        {
            m_stack.Push(m);
        }

        /// <summary>
        /// Pop
        /// </summary>
        public void Pop()
        {
            if (m_stack.Count > 0)
            {
                m_stack.Pop();
            }
        }

        /// <summary>
        /// Top
        /// </summary>
        /// <returns></returns>
        public Matrix4 Top()
        {
            return m_stack.Peek();
        }

        /// <summary>
        /// Mult
        /// </summary>
        /// <param name="m"></param>
        public void Mult(Matrix4 m)
        {
            Matrix4 top = m_stack.Pop();
            m_stack.Push(top * m);
        }
    }
}
