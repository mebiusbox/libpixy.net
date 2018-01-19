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
using System.Threading;
using System.Collections;

namespace libpixy.net.Tools
{
    /// <summary>
    /// バリア
    /// </summary>
    public class Barrier
    {
        /// <summary>
        /// ロガー
        /// </summary>
        //private NLog.Logger m_logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 名前
        /// </summary>
        private string m_name;

        /// <summary>
        /// ミューテックス
        /// </summary>
        private Mutex m_mutex = new Mutex();

        /// <summary>
        /// 
        /// </summary>
        private ArrayList m_collection = new ArrayList();

        private object m_synclock = new object();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="unique">ユニーク名</param>
        public Barrier(string unique)
        {
            m_name = unique;
        }

        /// <summary>
        /// リソースをロック
        /// </summary>
        /// <param name="enter">ユニーク名</param>
        public void Lock(string enter)
        {
            for (; ; )
            {
                if (m_mutex.WaitOne(5000))
                {
                    break;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("libpixy.net.Tool.Barrier: timeout. name=" + m_name + " enter=" + enter);
                    //m_logger.Debug("timeout. name=" + m_name + " enter=" + enter);

                    lock (m_synclock)
                    {
                        foreach (string value in m_collection)
                        {
                            System.Diagnostics.Debug.WriteLine("libpixy.net.Tool.Barrier: " + value);
                            //m_logger.Debug("timeout. " + value);
                        }
                    }
                }
            }

            lock (m_synclock)
            {
                m_collection.Add(enter);
            }
        }

        /// <summary>
        /// アンロック
        /// </summary>
        public void Unlock(string enter)
        {
            m_mutex.ReleaseMutex();
            lock (m_synclock)
            {
                m_collection.Remove(enter);
            }
        }
    }

    /// <summary>
    /// スコープバリア
    /// </summary>
    public class ScopedBarrier : IDisposable
    {
        /// <summary>
        /// 同期オブジェクト
        /// </summary>
        private Barrier m_barrier = null;

        /// <summary>
        /// ユニーク名
        /// </summary>
        private string m_unique = "";

        /// <summary>
        /// アンロックしたかどうか
        /// </summary>
        private bool m_unlocked = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="barrier"></param>
        /// <param name="enter"></param>
        public ScopedBarrier(Barrier barrier, string enter)
        {
            m_unique = DateTime.Now.ToString() + "." + DateTime.Now.Ticks.ToString() + "." + Thread.CurrentThread.ManagedThreadId.ToString() + ":" + enter;
            m_barrier = barrier;
            m_barrier.Lock(m_unique);
        }

        /// <summary>
        /// 強制アンロック
        /// </summary>
        public void Unlock()
        {
            if (m_unlocked == false)
            {
                m_barrier.Unlock(m_unique);
                m_unlocked = true;
            }
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        public virtual void Dispose()
        {
            if (m_unlocked == false)
            {
                m_barrier.Unlock(m_unique);
                m_unlocked = true;
            }
        }
    };
}
