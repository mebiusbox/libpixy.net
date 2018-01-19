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
using System.Windows.Forms;

namespace libpixy.net.Utils
{
    public class KeyUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsAlphabetKey(Keys key)
        {
            switch (key)
            {
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.G:
                case Keys.H:
                case Keys.I:
                case Keys.J:
                case Keys.K:
                case Keys.L:
                case Keys.M:
                case Keys.N:
                case Keys.O:
                case Keys.P:
                case Keys.Q:
                case Keys.R:
                case Keys.S:
                case Keys.T:
                case Keys.U:
                case Keys.V:
                case Keys.W:
                case Keys.X:
                case Keys.Y:
                case Keys.Z:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsDigitKey(Keys key)
        {
            switch (key)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsFunctionKey(Keys key)
        {
            switch (key)
            {
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public static bool IsPrintableSymbolKey(Keys key, bool shift)
        {
            if (shift)
            {
                switch (key)
                {
                    case Keys.D1:// !
                    case Keys.D2:// "
                    case Keys.D3:// #
                    case Keys.D4:// $
                    case Keys.D5:// %
                    case Keys.D6:// &
                    case Keys.D7:// '
                    case Keys.D8:// (
                    case Keys.D9:// )
                    case Keys.OemMinus:// =
                    case Keys.Oem7:// ~
                    case Keys.Oem5:// |
                    case Keys.Oemtilde:// `
                    case Keys.OemOpenBrackets:// {
                    case Keys.Oemplus:// +
                    case Keys.Oem1:// *
                    case Keys.Oem6:// }
                    case Keys.Oemcomma:// <
                    case Keys.OemPeriod:// >
                    case Keys.OemQuestion:// ?
                    case Keys.OemBackslash:// _
                        return true;
                }
            }
            else
            {
                switch (key)
                {
                    case Keys.OemMinus:// -
                    case Keys.Oem7:// ^
                    case Keys.Oem5:// \
                    case Keys.Oemtilde:// @
                    case Keys.OemOpenBrackets:// [
                    case Keys.Oemplus:// ;
                    case Keys.Oem1:// :
                    case Keys.Oem6:// ]
                    case Keys.Oemcomma:// ,
                    case Keys.OemPeriod:// .
                    case Keys.OemQuestion:// /
                    case Keys.OemBackslash:// \
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public static bool IsTextBoxAcceptsKey(Keys key, bool shift)
        {
            if (IsAlphabetKey(key) || IsDigitKey(key))
            {
                return true;
            }

            if (IsPrintableSymbolKey(key, shift))
            {
                return true;
            }

            if (key == Keys.Enter || key == Keys.Tab || key == Keys.Space ||
                key == Keys.Up || key == Keys.Down || key == Keys.Left || key == Keys.Right ||
                key == Keys.Home || key == Keys.End)
            {
                return true;
            }

            return false;
        }
    }
}
