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
    /// オブジェクト
    /// </summary>
    public class BaseObject
    {
        #region Fields

        private string m_name = "";
        private string m_scriptName = "";
        private object m_tag = null;
        private Tools.UUID m_uuid = null;

        #endregion

        #region Properties

        /// <summary>
        /// 名前
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// スクリプト名
        /// </summary>
        public string ScriptName
        {
            get { return m_scriptName; }
            set { m_scriptName = value; }
        }

        /// <summary>
        /// タグ名
        /// </summary>
        public object Tag
        {
            get { return m_tag; }
            set { m_tag = value; }
        }

        /// <summary>
        /// UUID
        /// </summary>
        public Tools.UUID UUID
        {
            get { return m_uuid; }
            set { m_uuid = value; }
        }

        #endregion

        #region Attributes
        #endregion Attributes

        #region Constructor, Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected BaseObject()
        {
            m_uuid = Tools.UUID.Generate();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">名前</param>
        protected BaseObject(string name)
        {
            m_name = name;
            m_uuid = Tools.UUID.Generate();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="scriptName">スクリプト名</param>
        protected BaseObject(string name, string scriptName)
        {
            m_uuid = Tools.UUID.Generate();
            m_name = name;
            m_scriptName = scriptName;
        }

        #endregion Constructor, Destructor

        #region Copy

        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="dst"></param>
        public void Copy(BaseObject dst)
        {
            dst.m_name = this.Name;
            dst.m_scriptName = this.ScriptName;
            dst.m_tag = this.Tag;
            //dst.m_uuid = this.UUID;
        }

        #endregion
    }
}
