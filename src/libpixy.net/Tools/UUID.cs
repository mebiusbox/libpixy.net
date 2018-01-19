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
    /// ユニークＩＤ
    /// </summary>
    public class UUID
    {
        #region Fields

        private UInt32 m_data1 = 0;
        private UInt32 m_data2 = 0;
        private UInt32 m_data3 = 0;
        private UInt32 m_data4 = 0;

        #endregion Fields

        #region Constructor, Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        public UUID()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <param name="data3"></param>
        /// <param name="data4"></param>
        public UUID(UInt32 data1, UInt32 data2, UInt32 data3, UInt32 data4)
        {
            m_data1 = data1;
            m_data2 = data2;
            m_data3 = data3;
            m_data4 = data4;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uuid"></param>
        public UUID(string uuid)
        {
            Parse(uuid);
        }

        #endregion

        #region Public Methds

        /// <summary>
        /// 文字列から UUID を解析
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public bool Parse(string uuid)
        {
            string[] parts = uuid.Split(new char[] { '-' });
            if (parts.Length != 4)
            {
                return false;
            }

            try
            {
                this.m_data1 = Convert.ToUInt32(parts[0], 16);
                this.m_data2 = Convert.ToUInt32(parts[1], 16);
                this.m_data3 = Convert.ToUInt32(parts[2], 16);
                this.m_data4 = Convert.ToUInt32(parts[3], 16);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 同値かどうか
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            return Equals(this, obj as UUID);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 同値かどうか
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool Equals(UUID lhs, UUID rhs)
        {
            return (
                lhs.m_data1 == rhs.m_data1 &&
                lhs.m_data2 == rhs.m_data2 &&
                lhs.m_data3 == rhs.m_data3 &&
                lhs.m_data4 == rhs.m_data4);
        }

        #endregion Public Methods

        #region Public Properties

        public UInt32 Data1
        {
            get { return m_data1; }
        }

        public UInt32 Data2
        {
            get { return m_data2; }
        }

        public UInt32 Data3
        {
            get { return m_data3; }
        }

        public UInt32 Data4
        {
            get { return m_data4; }
        }

        #endregion // Public Properties

        #region ToString

        /// <summary>
        /// 文字列に変換
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0:X}-{1:X}-{2:X}-{3:X}", m_data1, m_data2, m_data3, m_data4);
        }

        #endregion

        /// <summary>
        /// 次の UUID 
        /// </summary>
        public static UUID NextUUID = new UUID(0, 0, 0, 0);

        /// <summary>
        /// UUID を生成
        /// </summary>
        /// <returns></returns>
        public static UUID Generate()
        {
            if (NextUUID.m_data4 == UInt32.MaxValue)
            {
                if (NextUUID.m_data3 == UInt32.MaxValue)
                {
                    if (NextUUID.m_data2 == UInt32.MaxValue)
                    {
                        if (NextUUID.m_data1 == UInt32.MaxValue)
                        {
                            System.Diagnostics.Debug.Assert(false);
                        }

                        NextUUID.m_data1++;
                        NextUUID.m_data2 = 0;
                    }
                    else
                    {
                        NextUUID.m_data2++;
                    }

                    NextUUID.m_data3 = 0;
                }
                else
                {
                    NextUUID.m_data3++;
                }

                NextUUID.m_data4 = 0;
            }
            else
            {
                NextUUID.m_data4++;
            }

            return new UUID(NextUUID.m_data1, NextUUID.m_data2, NextUUID.m_data3, NextUUID.m_data4);
        }
    }
}
