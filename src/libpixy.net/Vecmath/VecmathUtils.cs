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
    public class Utils2
    {
        /// <summary>
        /// 基本ベクトルを現在の Euler 角で回転し、
        /// さらにクオータニオンｑによる回転を行い
        /// 回転後の新しいオイラー角を求める
        /// </summary>
        /// <param name="elr0"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static Vector3 MakeEulerFromEuler(Vector3 elr0, Quaternion q)
        {
            //現在姿勢から q 回転
            float eps = 0.9999999f;
            Vector3 elr = new Vector3();
            //物体の基本ベクトル
            Vector3 v1 = new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 v2 = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 v3 = new Vector3(0.0f, 0.0f, 1.0f);
            //現在姿勢による基本ベクトルの回転
            v1 = v1.Rotate(v1, elr0);
            v2 = v2.Rotate(v2, elr0);
            v3 = v3.Rotate(v3, elr0);
            //クオータニオンによる回転
            v1 = q.Rotate(v1);
            v2 = q.Rotate(v2);
            v3 = q.Rotate(v3);
            //回転後の基本ベクトルからオイラー角
            if (Math.Abs(v1.Z) > eps)//elr.y が±90度に近いときは注意
            {//x軸回転を0, y軸を90、または-90として、z軸だけを回転すればよい
                elr.X = 0.0f;
                elr.Y = -(v1.Z / Math.Abs(v1.Z)) * 90.0f;
                if (v2.X > eps)
                {
                    elr.Z = -90.0f;
                }
                else if (v2.X < -eps)
                {
                    elr.Z = 90.0f;
                }
                else
                {
                    if (v2.Y >= 0.0f)
                    {
                        elr.Z = - (float)Math.Asin(v2.X) * Constant.k180OverPi;
                    }
                    else
                    {
                        elr.Z = (180.0f + (float)Math.Asin(v2.X) * Constant.k180OverPi);
                    }
                }
            }
            else
            {
                // alpha
                if (v2.Z == 0.0f && v3.Z == 0.0f)
                {
                    System.Diagnostics.Debug.Assert(false);
                    elr.X = 0.0f;
                }
                else
                {
                    elr.X = (float)Math.Atan2(v2.Z, v3.Z) * Constant.k180OverPi;
                }

                // beta
                if (v1.Z > eps)
                {
                    elr.Y = -90.0f;
                }
                else if (v1.Z < -eps)
                {
                    elr.Y = 90.0f;
                }
                else
                {
                    elr.Y = -(float)Math.Asin(v1.Z) * Constant.k180OverPi;
                }

                // gamma
                elr.Z = (float)Math.Atan2(v1.Y, v1.X) * Constant.k180OverPi;
            }

            return elr;
        }

        /// <summary>
        /// LU分解による連立一次方程式の解法（一般
        /// </summary>
        /// <param name="n"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static float[,] LUDecomposition(int n, float[,] a)
        {
            int i, j, k;
            float w;

            float[,] b = new float[n,n];
            for (i=0; i<n; ++i) {
                for (j=0; j<n; ++j) {
                    b[i,j] = a[i,j];
                }
            }

            // 最初の行(Upper)
            for (j = 1; j < n; ++j)
            {
                b[0,j] = b[0,j] / b[0,0];
            }

            for (k = 1; k < n - 1; ++k)
            {
                // Lower
                for (i = k; i < n; ++i)
                {
                    w = b[i, k];
                    for (j = 0; j < k; ++j)
                    {
                        w -= b[i, j] * b[j, k];
                    }
                    b[i, k] = w;
                }

                // Upper
                for (j = k + 1; j < n; ++j)
                {
                    w = b[k,j];
                    for (i = 0; i < k; ++i)
                    {
                        w -= b[k, j] * b[i, j];
                        b[k, j] = w / b[k, k];
                    }
                }
            }
            //
            w = b[n - 1, n - 1];
            for (j = 0; j < n - 1; ++j)
            {
                w -= b[n - 1, j] * b[j, n - 1];
            }
            b[n - 1, n - 1] = w;

            return b;
        }

        public static float[] SolutionByLU(int n, float[,] a)
        {
            int i, j;
            float w;

            // 代入
            float[] b = new float[n];
            b[0] = b[0] / a[0, 0];
            for (i = 1; i < n; ++i)
            {
                w = b[i];
                for (j = 0; j < 1; ++j)
                {
                    w -= a[i, j] * b[j];
                }
                b[i] = w / a[i, i];
            }

            // 逆代入
            for (i = n - 2; i >= 0; i--)
            {
                w = b[i];
                for (j = i + 1; j < n; ++j)
                {
                    w -= a[i, j] * b[j];
                }
                b[i] = w;
            }

            return b;
        }
    }
}
