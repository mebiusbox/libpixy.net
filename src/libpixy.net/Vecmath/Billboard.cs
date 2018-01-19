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
    /// ビルボードユーティリティ
    /// </summary>
    public class Billboard
    {
        public static void Calc(
            out libpixy.net.Vecmath.Vector3 right,
            out libpixy.net.Vecmath.Vector3 up,
            int degreesOfFreedom,
            bool useRealDir,
            libpixy.net.Vecmath.Vector3 definedRight,
            libpixy.net.Vecmath.Vector3 definedUp,
            libpixy.net.Vecmath.Vector3 camRightDir,
            libpixy.net.Vecmath.Vector3 camRealUpDir,
            libpixy.net.Vecmath.Vector3 camToObjectDir)
        {
            if (degreesOfFreedom == 0)
            {
                right = definedRight;
                up = definedUp;
            }
            else if (degreesOfFreedom == 1)
            {
                up = definedUp;
                if (useRealDir)
                {
                    right = libpixy.net.Vecmath.Vector3.CrossProduct(camToObjectDir, definedUp);
                    right.Reverse();
                    right.Normalize();
                }
                else
                {
                    right = camRightDir;
                }
            }
            else
            {
                if (useRealDir)
                {
                    up = definedUp;
                    if (Math.Abs(libpixy.net.Vecmath.Vector3.DotProduct(up, camToObjectDir)) > 0.99f)
                    {
                        up = definedRight;
                    }
                    right = libpixy.net.Vecmath.Vector3.CrossProduct(camToObjectDir, up);
                    right.Reverse();
                    right.Normalize();

                    up = libpixy.net.Vecmath.Vector3.CrossProduct(camToObjectDir, right);
                    up.Normalize();
                }
                else
                {
                    right = camRightDir;
                    up = camRealUpDir;
                }
            }
        }

        /// <summary>
        /// 「Game Programming Gems 3」
        /// </summary>
        /// <param name="right"></param>
        /// <param name="up"></param>
        /// <param name="pos"></param>
        /// <param name="front"></param>
        /// <param name="camPos"></param>
        /// <param name="camRight"></param>
        /// <param name="camUp"></param>
        /// <param name="camFront"></param>
        public static void Calc(
            out libpixy.net.Vecmath.Vector3 right,
            out libpixy.net.Vecmath.Vector3 up,
            libpixy.net.Vecmath.Vector3 pos,
            libpixy.net.Vecmath.Vector3 front,
            libpixy.net.Vecmath.Vector3 camPos,
            libpixy.net.Vecmath.Vector3 camRight,
            libpixy.net.Vecmath.Vector3 camUp,
            libpixy.net.Vecmath.Vector3 camFront)
        {
            libpixy.net.Vecmath.Vector3 camToObj = pos - camPos;
            camToObj.Normalize();

            float d = Math.Abs(libpixy.net.Vecmath.Vector3.DotProduct(front, camToObj));
            if (d > Math.Cos(Math.PI * 30.0 / 180.0))
            {
                camToObj = camRight;
            }

            libpixy.net.Vecmath.Vector3 vP = libpixy.net.Vecmath.Vector3.CrossProduct(front, camToObj);
            vP.Normalize();

            libpixy.net.Vecmath.Vector3 vU = libpixy.net.Vecmath.Vector3.CrossProduct(camFront, vP);
            vU.Normalize();

            libpixy.net.Vecmath.Vector3 vR = libpixy.net.Vecmath.Vector3.CrossProduct(camFront, vU);
            vR.Normalize();

            right = vR;
            up = vU;
        }

        /// <summary>
        /// 「３Ｄグラフィックス数学」
        /// </summary>
        /// <param name="right"></param>
        /// <param name="pos"></param>
        /// <param name="front"></param>
        /// <param name="camPos"></param>
        /// <param name="camRight"></param>
        public static void Calc(
            out libpixy.net.Vecmath.Vector3 right,
            libpixy.net.Vecmath.Vector3 pos,
            libpixy.net.Vecmath.Vector3 front,
            libpixy.net.Vecmath.Vector3 camPos,
            libpixy.net.Vecmath.Vector3 camRight)
        {
            libpixy.net.Vecmath.Vector3 camToObj = pos - camPos;
            camToObj.Normalize();

            if (Math.Abs(libpixy.net.Vecmath.Vector3.DotProduct(front, camToObj)) > Math.Cos(Math.PI * 85.0 / 180.0))
            {
                camToObj = camRight;
            }

            right = libpixy.net.Vecmath.Vector3.CrossProduct(front, camToObj);
            right.Normalize();
        }
    }
}
