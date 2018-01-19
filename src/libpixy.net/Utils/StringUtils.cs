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
    public class StringUtils
    {
        const string hanTableNumber = "0123456789";
        const string hanTableAlphaL = "abcdefghijklmnopqrstuvwxyz";
        const string hanTableAlphaH = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string hanTableSimbol = "!\"#$%&'()=~|`{+*}<>?_-^\\@[;:],./";
        const string hanTableKana = "ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾗﾘﾙﾚﾛﾜｦﾝ";
        const string hanTableKana1 = "ｱｲｳｴｵﾅﾆﾇﾈﾉﾏﾐﾑﾒﾓﾗﾘﾙﾚﾛﾜｦﾝ";
        const string hanTableKana2 = "ｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾊﾋﾌﾍﾎ";
        const string hanTableKana3 = "ｶﾞｷﾞｸﾞｹﾞｺﾞｻﾞｼﾞｽﾞｾﾞｿﾞﾀﾞﾁﾞﾂﾞﾃﾞﾄﾞﾊﾞﾋﾞﾌﾞﾍﾞﾎﾞﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ";
        const string zenTableNumber = "０１２３４５６７８９";
        const string zenTableAlphaL = "ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ";
        const string zenTableAlphaH = "ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ";
        const string zenTableSimbol = "！”＃＄％＆’（）＝～｜‘｛＋＊｝＜＞？＿－＾￥＠［；：］，．／";
        const string zenTableKana = "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモラリルレロワヲン";
        const string zenTableKana1 = "アイウエオナニヌネノマミムメモラリルレロワヲン";
        const string zenTableKana2 = "カキクケコサシスセソタチツテトハヒフヘホハヒフヘホ";
        //a                           ０１２３４５６７８９０１２３４５６７８９  
        const string zenTableKana3 = "ガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポ";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public static string ToZenkaku(string str, char escape)
        {
            string ret = str;
            ret = ToZenHan(ret, escape, hanTableNumber, zenTableNumber);
            ret = ToZenHan(ret, escape, hanTableAlphaL, zenTableAlphaL);
            ret = ToZenHan(ret, escape, hanTableAlphaH, zenTableAlphaH);
            ret = ToZenHan(ret, escape, hanTableSimbol, zenTableSimbol);
            ret = ToZenHan(ret, escape, hanTableKana1, zenTableKana1);
            ret = ToZenkakuKana2(ret, escape);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public static string ToZenHan(string str, char escape, string hanTable, string zenTable)
        {
            bool escaped = false;
            StringBuilder ret = new StringBuilder();
            foreach (char c in str)
            {
                if (escaped)
                {
                    ret.Append(c);
                    escaped = false;
                }
                else if (c == escape)
                {
                    escaped = true;
                }
                else
                {
                    int i;
                    for (i = 0; i < hanTable.Length; ++i)
                    {
                        if (hanTable[i] == c)
                        {
                            ret.Append(zenTable[i]);
                            break;
                        }
                    }

                    if (i >= hanTable.Length)
                    {
                        ret.Append(c);
                    }
                }
            }

            return ret.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public static string ToZenkakuKana2(string str, char escape)
        {
            bool escaped = false;
            bool daku = false;
            int idx = 0;
            char c = ' ';
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < str.Length; ++i)
            {
                c = str[i];

                if (escaped)
                {
                    ret.Append(c);
                    escaped = false;
                }
                else if (c == escape)
                {
                    escaped = true;
                }
                else if (daku)
                {
                    if (c == '"')
                    {
                        ret.Append(zenTableKana3[idx]);
                    }
                    else if (c == 'ﾟ' && idx >= 15 && idx <= 19)
                    {
                        ret.Append(zenTableKana3[idx + 5]);
                    }
                    else
                    {
                        ret.Append(zenTableKana2[idx]);
                        ret.Append(c);
                    }

                    daku = false;
                }
                else
                {
                    int j;
                    for (j = 0; j < hanTableKana2.Length; ++j)
                    {
                        if (hanTableKana2[j] == c)
                        {
                            daku = true;
                            idx = j;
                            break;
                        }
                    }

                    if (j >= hanTableKana2.Length)
                    {
                        ret.Append(c);
                    }
                }
            }

            if (daku)
            {
                ret.Append(c);
            }

            return ret.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public static string ToHankaku(string str, char escape)
        {
            string ret = str;
            ret = ToZenHan(ret, escape, zenTableNumber, hanTableNumber);
            ret = ToZenHan(ret, escape, zenTableAlphaL, hanTableAlphaL);
            ret = ToZenHan(ret, escape, zenTableAlphaH, hanTableAlphaH);
            ret = ToZenHan(ret, escape, zenTableSimbol, hanTableSimbol);
            ret = ToZenHan(ret, escape, zenTableKana1, hanTableKana1);
            ret = ToHankakuKana2(ret, escape);
            return ret.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public static string ToHankakuKana2(string str, char escape)
        {
            bool escaped = false;
            StringBuilder ret = new StringBuilder();
            char c;
            for (int i = 0; i < str.Length; ++i)
            {
                c = str[i];

                if (escaped)
                {
                    ret.Append(c);
                    escaped = false;
                }
                else if (c == escape)
                {
                    escaped = true;
                }
                else
                {
                    int j;
                    for (j = 0; j < zenTableKana2.Length; ++j)
                    {
                        if (zenTableKana2[j] == c)
                        {
                            ret.Append(hanTableKana2[j * 2 + 0]);
                            ret.Append(hanTableKana2[j * 2 + 1]);
                            break;
                        }
                    }

                    if (j >= zenTableKana2.Length)
                    {
                        ret.Append(c);
                    }
                }
            }

            return ret.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string WildcardToRegexp(string str)
        {
            string ret = "";
            foreach (char c in str)
            {
                if (c == '?')
                {
                    ret += '.';
                }
                else if (c == '*')
                {
                    ret += ".*";
                }
                else if (c == '.')
                {
                    ret += "[.]";
                }
                else
                {
                    ret += c;
                }
            }

            ret = '^' + ret + '$';
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EscapeWildcard(string str)
        {
            string ret = "";
            foreach (char c in str)
            {
                if (c == '?' || c == '*' || c == '\\')
                {
                    ret += '\\';
                }

                ret += c;
            }

            return ret;
        }

        public static string Right(string text, int length)
        {
            if (text.Length < length)
            {
                return text.Substring(0);
            }
            else
            {
                return text.Substring(text.Length - length - 1, length);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToHexDumpFormat(byte[] data, int len)
        {
            string ret = "";
            for (int i = 0; i < len / 16 + 1; ++i)
            {
                for (int j = 0; j < 16; ++j)
                {
                    if (i * 16 + j < len)
                    {
                        ret += string.Format("{0:X2} ", data[i * 16 + j]);
                    }
                    else
                    {
                        ret += "00 ";
                    }
                }

                ret += "\n";
            }

            return ret;
        }

        /// <summary>
        /// ダブルクォーテーションで囲む
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Quote(string str)
        {
            if (str.Length >= 2 && str[0] == '"' && str[str.Length - 1] == '"')
            {
                return str;
            }

            return "\"" + str + "\"";
        }

        public static int ToInt32(string str, int default_value)
        {
            int value = 0;
            if (Int32.TryParse(str, out value))
            {
                return value;
            }

            return default_value;
        }

        public static uint ToUInt32(string str, uint default_value)
        {
            uint value = 0;
            if (UInt32.TryParse(str, out value))
            {
                return value;
            }

            return default_value;
        }

        public static float ToFloat(string str, float default_value)
        {
            float value = 0;
            if (float.TryParse(str, out value))
            {
                return value;
            }

            return default_value;
        }

        public static double ToDouble(string str, double default_value)
        {
            double value = 0;
            if (double.TryParse(str, out value))
            {
                return value;
            }

            return default_value;
        }
    }
}
