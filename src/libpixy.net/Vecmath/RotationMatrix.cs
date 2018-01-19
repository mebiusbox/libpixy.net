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
    public class RotationMatrix
    {
        #region Fields

        public float m11, m12, m13;
        public float m21, m22, m23;
        public float m31, m32, m33;

        #endregion

        #region Constructor/Destructor

        public RotationMatrix()
        {
        }

        #endregion

        #region Methods

        public void SetIdentity()
        {
            m11 = m22 = m33 = 1.0f;
            m12 = m13 = 0.0f;
            m21 = m23 = 0.0f;
            m31 = m32 = 0.0f;
        }

        public void Setup(EulerAngles orientation)
        {
            float sh = (float)Math.Sin(orientation.Heading);
            float ch = (float)Math.Cos(orientation.Heading);
            float sp = (float)Math.Sin(orientation.Pitch);
            float cp = (float)Math.Cos(orientation.Pitch);
            float sb = (float)Math.Sin(orientation.Bank);
            float cb = (float)Math.Cos(orientation.Bank);

            m11 = ch * cb + sh * sp * sb;
            m12 = -ch * sb + sh * sp * cb;
            m13 = sh * cp;

            m21 = sb * cp;
            m22 = cb * cp;
            m23 = -sp;

            m31 = -sh * cb + ch * sp * sb;
            m32 = sb * sh + ch * sp * cb;
            m33 = ch * cp;
        }

        public void FromInertialToObjectQuaternion(Quaternion q)
        {
            m11 = 1.0f - 2.0f * (q.Y * q.Y + q.Z * q.Z);
            m12 = 2.0f * (q.X * q.X + q.W * q.Z);
            m13 = 2.0f * (q.X * q.Z - q.W * q.Y);

            m21 = 2.0f * (q.X * q.Y - q.W * q.Z);
            m22 = 1.0f - 2.0f * (q.X * q.X + q.Z * q.Z);
            m23 = 2.0f * (q.Y * q.Z + q.W * q.X);

            m31 = 2.0f * (q.X * q.Z + q.W * q.Y);
            m32 = 2.0f * (q.Y * q.Z - q.W * q.X);
            m33 = 1.0f - 2.0f * (q.X * q.X + q.Y * q.Y);
        }

        public void FromObjectToInertialQuaternion(Quaternion q)
        {
            m11 = 1.0f - 2.0f * (q.Y * q.Y + q.Z * q.Z);
            m12 = 2.0f * (q.X * q.X - q.W * q.Z);
            m13 = 2.0f * (q.X * q.Z + q.W * q.Y);

            m21 = 2.0f * (q.X * q.Y + q.W * q.Z);
            m22 = 1.0f - 2.0f * (q.X * q.X + q.Z * q.Z);
            m23 = 2.0f * (q.Y * q.Z - q.W * q.X);

            m31 = 2.0f * (q.X * q.Z - q.W * q.Y);
            m32 = 2.0f * (q.Y * q.Z + q.W * q.X);
            m33 = 1.0f - 2.0f * (q.X * q.X + q.Y * q.Y);
        }

        public Vector3 InertialToObject(Vector3 v)
        {
            return new Vector3(
                m11 * v.X + m21 * v.Y + m31 * v.Z,
                m12 * v.X + m22 * v.Y + m32 * v.Z,
                m13 * v.X + m23 * v.Y + m33 * v.Z);
        }

        public Vector3 ObjectToInertial(Vector3 v)
        {
            return new Vector3(
                m11 * v.X + m12 * v.Y + m13 * v.Z,
                m21 * v.X + m22 * v.Y + m23 * v.Z,
                m31 * v.X + m32 * v.Y + m33 * v.Z);
        }

        #endregion
    }
}
