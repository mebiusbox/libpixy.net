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
    public class CommandRunner
    {
        private object m_lock = new object();

        /// <summary>
        /// コンソール実行
        /// </summary>
        /// <param name="workdir"></param>
        /// <param name="filename"></param>
        /// <param name="argument"></param>
        public void RunConsole(string workdir, string filename, string argument)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.WorkingDirectory = workdir;
                process.StartInfo.FileName = "cmd";
                process.StartInfo.Arguments = "/C " + filename + " " + argument;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(m_process_OutputDataReceived);
                process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(m_process_ErrorDataReceived);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                process.Close();
                process.Dispose();
                process = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            OnErrorRedirected(e.Data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            OnOutputRedirected(e.Data);
        }

        /// <summary>
        /// リダイレクトされたデータを受け取ったときのイベントパラメータ
        /// </summary>
        public class RedirectEventArgs : EventArgs
        {
            /// <summary>
            /// データ
            /// </summary>
            public string data = "";

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="indata"></param>
            public RedirectEventArgs(string indata)
            {
                data = indata;
            }
        };

        /// <summary>
        /// リダイレクトイベント
        /// </summary>
        public event EventHandler<RedirectEventArgs> OutputRedirected;
        public event EventHandler<RedirectEventArgs> ErrorRedirected;

        /// <summary>
        /// リダイレクトイベント（内部用）
        /// </summary>
        /// <param name="indata"></param>
        private void OnOutputRedirected(string indata)
        {
            lock (m_lock)
            {
                if (OutputRedirected != null)
                {
                    OutputRedirected(this, new RedirectEventArgs(indata));
                }
            }
        }

        /// <summary>
        /// リダイレクトイベント（内部用）
        /// </summary>
        /// <param name="indata"></param>
        private void OnErrorRedirected(string indata)
        {
            lock (m_lock)
            {
                if (ErrorRedirected != null)
                {
                    ErrorRedirected(this, new RedirectEventArgs(indata));
                }
            }
        }
    }
}
