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
using System.Text.RegularExpressions;

namespace libpixy.net.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public class PatternMatch
    {
        public enum MatchAlgo
        {
            LIKE = 0,
            WILDCARD,
            REGEXP
        }

        public static string[] MatchAlgoTextJp = {
            "あいまい",
            "ワイルドカード",
            "正規表現"
        };

        public static string[] MatchAlgoTextEn = {
            "like",
            "wildcard",
            "regexp"
        };

        public bool OptionCaseSensitive = false;
        public bool OptionZenkakuSensitive = false;
        public MatchAlgo OptionAlgo = MatchAlgo.LIKE;
        private string Pattern = "";

        /// <summary>
        /// 
        /// </summary>
        public PatternMatch()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void Setup(string s)
        {
            if (OptionCaseSensitive)
            {
                Pattern = s;
            }
            else
            {
                Pattern = s.ToLower();
            }

            if (!OptionZenkakuSensitive)
            {
                if (OptionAlgo != MatchAlgo.REGEXP)
                {
                    if (OptionAlgo == MatchAlgo.WILDCARD)
                    {
                        Pattern = libpixy.net.Utils.StringUtils.EscapeWildcard(Pattern);
                        Pattern = libpixy.net.Utils.StringUtils.ToZenkaku(Pattern, '\\');
                    }
                    else
                    {
                        Pattern = libpixy.net.Utils.StringUtils.ToZenkaku(Pattern, (char)0x00);
                    }
                }
            }

            if (OptionAlgo == MatchAlgo.WILDCARD)
            {
                Pattern = libpixy.net.Utils.StringUtils.WildcardToRegexp(Pattern);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool Match(string s)
        {
            string str = s;

            if (!OptionCaseSensitive)
            {
                str = str.ToLower();
            }

            if (!OptionZenkakuSensitive)
            {
                str = libpixy.net.Utils.StringUtils.ToZenkaku(str, (char)0x00);
            }

            if (OptionAlgo == MatchAlgo.LIKE)
            {
                if (str.IndexOf(Pattern) >= 0)
                {
                    return true;
                }
            }
            else
            {
                Regex regex = new Regex(Pattern);
                Match m = regex.Match(str);
                if (m.Success)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
