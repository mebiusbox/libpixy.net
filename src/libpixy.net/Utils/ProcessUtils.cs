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

namespace libpixy.net.Utils
{
    public class ProcessUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workdir"></param>
        /// <param name="filename"></param>
        /// <param name="argument"></param>
        public static void RunConsole(string workdir, string filename, string argument)
        {
            System.Diagnostics.ProcessStartInfo startinfo = new System.Diagnostics.ProcessStartInfo();
            startinfo.WorkingDirectory = workdir;
            startinfo.FileName = filename;
            startinfo.Arguments = argument;
            //startinfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(startinfo);
            process.WaitForExit();
            process.Close();
            process.Dispose();
        }

        /// <summary>
        /// 出力をバッファリングするコマンド実行
        /// </summary>
        internal class BufferedCommandRunner
        {
            public string OutputStdout = "";
            public string OutputStderr = "";

            public void Run(string workdir, string filename, string argument)
            {
                Tools.CommandRunner runner = new Tools.CommandRunner();
                runner.OutputRedirected += new EventHandler<Tools.CommandRunner.RedirectEventArgs>(runner_OutputRedirected);
                runner.ErrorRedirected += new EventHandler<Tools.CommandRunner.RedirectEventArgs>(runner_ErrorRedirected);
                runner.RunConsole(workdir, filename, argument);
            }

            void runner_OutputRedirected(object sender, Tools.CommandRunner.RedirectEventArgs e)
            {
                OutputStdout += e.data + Environment.NewLine;
            }

            void runner_ErrorRedirected(object sender, Tools.CommandRunner.RedirectEventArgs e)
            {
                OutputStderr += e.data + Environment.NewLine;
            }
        };

        /// <summary>
        /// コンソールで実行（リダイレクトを受け取る）
        /// </summary>
        /// <param name="workdir"></param>
        /// <param name="filename"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static string RunConsoleRedirect(string workdir, string filename, string argument)
        {
            BufferedCommandRunner runner = new BufferedCommandRunner();
            runner.Run(workdir, filename, argument);
            return runner.OutputStdout;
        }
    }
}
