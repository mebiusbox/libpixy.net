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
using System.Drawing;

namespace libpixy.net.Utils
{
    public class GeomUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rc"></param>
        /// <returns></returns>
        public static Point GetCenter(Rectangle rc)
        {
            return new Point(rc.X + rc.Width / 2, rc.Y + rc.Height / 2);
        }

        /// <summary>
        /// Žw’è‚µ‚½ˆÊ’u‚ª‹éŒ`—Ìˆæ“à‚É‚ ‚é‚©‚Ç‚¤‚©
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool PtInRect(Rectangle rc, int x, int y)
        {
            if (rc.Left <= x && x <= rc.Right && rc.Top <= y && y <= rc.Bottom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="gridsize"></param>
        /// <returns></returns>
        public static int SnapGrid(int value, int gridsize)
        {
            if (gridsize <= 1)
            {
                return value;
            }
            else
            {
                int mod = System.Math.Abs(value % gridsize);
                if (mod == 0)
                {
                    return value;
                }

                if (mod < gridsize / 2)
                {
                    return (value / gridsize) * gridsize;
                }
                else
                {
                    return (value / gridsize + 1) * gridsize;
                }
            }
        }
    }
}
