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
    /// This class represents a heading-pitch-bank Euler angle triple.
    /// </summary>
    public class EulerAngles
    {
        #region Fields

        public float Heading;
        public float Pitch;
        public float Bank;

        #endregion

        #region Constructor/Destructor

        public EulerAngles()
        {
        }

        public EulerAngles(float h, float p, float b)
        {
            Heading = h;
            Pitch = p;
            Bank = b;
        }

        #endregion

        #region Methods

        public void SetIdentity()
        {
            Heading = Pitch = Bank = 0.0f;
        }

        public void Canonize()
        {
            Pitch = Utils.WrapPi(Pitch);

            if (Pitch < -Constant.kPiOver2)
            {
                Pitch = -Constant.kPi - Pitch;
                Heading += Constant.kPi;
                Bank += Constant.kPi;
            }
            else if (Pitch > Constant.kPiOver2)
            {
                Pitch = Constant.kPi - Pitch;
                Heading += Constant.kPi;
                Bank += Constant.kPi;
            }

            if (Math.Abs(Pitch) > Constant.kPiOver2 - 1e-4)
            {
                Heading += Bank;
                Bank = 0.0f;
            }
            else
            {
                Bank = Utils.WrapPi(Bank);
            }

            Heading = Utils.WrapPi(Heading);
        }

        public void FromObjectToInertialQuaternion(Quaternion q)
        {
            float sp = -2.0f * (q.Y * q.Z - q.W * q.X);

            if (Math.Abs(sp) > 0.9999f)
            {
                Pitch = Constant.kPiOver2 * sp;
                Heading = (float)Math.Atan2(-q.X * q.Z + q.W * q.Y, 0.5f - q.Y * q.Y - q.Z * q.Z);
                Bank = 0.0f;
            }
            else
            {
                Pitch = (float)Math.Asin(sp);
                Heading = (float)Math.Atan2(q.X * q.Z + q.W * q.Y, 0.5f - q.X * q.X - q.Y * q.Y);
                Bank = (float)Math.Atan2(q.X * q.Y + q.W * q.Z, 0.5f - q.X * q.X - q.Z * q.Z);
            }
        }

        public void FromInertialToObjectQuaternion(Quaternion q)
        {
            float sp = -2.0f * (q.Y * q.Z + q.W * q.X);

            if (Math.Abs(sp) > 0.9999f)
            {
                Pitch = Constant.kPiOver2 * sp;
                Heading = (float)Math.Atan2(-q.X * q.Z - q.W * q.Y, 0.5f - q.Y * q.Y - q.Z * q.Z);
                Bank = 0.0f;
            }
            else
            {
                Pitch = (float)Math.Asin(sp);
                Heading = (float)Math.Atan2(q.X * q.Z - q.W * q.Y, 0.5f - q.X * q.X - q.Y * q.Y);
                Bank = (float)Math.Atan2(q.X * q.Y - q.W * q.Z, 0.5f - q.X * q.X - q.Z * q.Z);
            }
        }

        public void FromObjectToWorldMatrix(Matrix4 m)
        {
            float sp = -m.m32;

            if (Math.Abs(sp) > 9.9999f)
            {
                Pitch = Constant.kPiOver2 * sp;
                Heading = (float)Math.Atan2(-m.m23, m.m11);
                Bank = 0.0f;
            }
            else
            {
                Heading = (float)Math.Atan2(m.m31, m.m33);
                Pitch = (float)Math.Asin(sp);
                Bank = (float)Math.Atan2(m.m12, m.m22);
            }
        }

        public void FromWorldToObjectMatrix(Matrix4 m)
        {
            float sp = -m.m23;

            if (Math.Abs(sp) > 9.9999f)
            {
                Pitch = Constant.kPiOver2 * sp;
                Heading = (float)Math.Atan2(-m.m31, m.m11);
                Bank = 0.0f;
            }
            else
            {
                Heading = (float)Math.Atan2(m.m13, m.m33);
                Pitch = (float)Math.Asin(sp);
                Bank = (float)Math.Atan2(m.m21, m.m22);
            }
        }

        public void FromRotationMatrix(RotationMatrix m)
        {
            float sp = -m.m23;

            if (Math.Abs(sp) > 9.9999f)
            {
                Pitch = Constant.kPiOver2 * sp;
                Heading = (float)Math.Atan2(-m.m31, m.m11);
                Bank = 0.0f;
            }
            else
            {
                Heading = (float)Math.Atan2(m.m13, m.m33);
                Pitch = (float)Math.Asin(sp);
                Bank = (float)Math.Atan2(m.m21, m.m22);
            }
        }

        #endregion
    }
}
