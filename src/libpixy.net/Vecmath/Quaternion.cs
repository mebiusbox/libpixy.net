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
    public class Quaternion
    {
        #region Fields

        public float X;
        public float Y;
        public float Z;
        public float W;

        #endregion

        #region Constructor/Destructor

        public Quaternion()
        {
        }

        public Quaternion(float w, float x, float y, float z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        public Quaternion(Vector3 axis, float theta)
        {
            SetToRotateAboutAxis(axis, theta);
        }

        #endregion

        #region Methods

        public void SetIdentity()
        {
            W = 1.0f;
            X = Y = Z = 0.0f;
        }

        public void Normalize()
        {
            float mag = (float)Math.Sqrt(W * W + X * X + Y * Y + Z * Z);

            if (mag > 0.0f)
            {
                float oneOverMag = 1.0f / mag;
                W *= oneOverMag;
                X *= oneOverMag;
                Y *= oneOverMag;
                Z *= oneOverMag;
            }
            else
            {
                System.Diagnostics.Debug.Assert(false);

                SetIdentity();
            }
        }

        public float GetRotationAngle()
        {
            float thetaOver2 = Utils.SafeAcos(W);
            return thetaOver2 * 2.0f;
        }

        public Vector3 GetRotationAxis()
        {
            float sinThetaOver2Sq = 1.0f - W * W;

            if (sinThetaOver2Sq <= 0.0f)
            {
                return new Vector3(1.0f, 0.0f, 0.0f);
            }

            float oneOverSinThetaOver2 = 1.0f / (float)Math.Sqrt(sinThetaOver2Sq);

            return new Vector3(
                X * oneOverSinThetaOver2,
                Y * oneOverSinThetaOver2,
                Z * oneOverSinThetaOver2);
        }

        public void SetToRotateAboutX(float theta)
        {
            float thetaOver2 = theta * 0.5f;

            W = (float)Math.Cos(thetaOver2);
            X = (float)Math.Sin(thetaOver2);
            Y = 0.0f;
            Z = 0.0f;
        }

        public void SetToRatateAboutY(float theta)
        {
            float thetaOver2 = theta * 0.5f;

            W = (float)Math.Cos(thetaOver2);
            X = 0.0f;
            Y = (float)Math.Sin(thetaOver2);
            Z = 0.0f;
        }

        public void SetToRotateAboutZ(float theta)
        {
            float thetaOver2 = theta * 0.5f;

            W = (float)Math.Cos(thetaOver2);
            X = 0.0f;
            Y = 0.0f;
            Z = (float)Math.Sin(thetaOver2);
        }

        public void SetToRotateAboutAxis(Vector3 axis, float theta)
        {
            System.Diagnostics.Debug.Assert(Math.Abs(axis.Length() - 1.0f) < 0.01f);

            float thetaOver2 = theta * 0.5f;
            float sinThetaOver2 = (float)Math.Sin(thetaOver2);

            W = (float)Math.Cos(thetaOver2);
            X = axis.X * sinThetaOver2;
            Y = axis.Y * sinThetaOver2;
            Z = axis.Z * sinThetaOver2;
        }

        public void SetToRotateObjectToInertial(EulerAngles orientation)
        {
            float sp, sb, sh;
            float cp, cb, ch;
            sp = (float)Math.Sin(orientation.Pitch * 0.5f);
            cp = (float)Math.Cos(orientation.Pitch * 0.5f);
            sb = (float)Math.Sin(orientation.Bank * 0.5f);
            cb = (float)Math.Cos(orientation.Bank * 0.5f);
            sh = (float)Math.Sin(orientation.Heading * 0.5f);
            ch = (float)Math.Cos(orientation.Heading * 0.5f);

            W = ch * cp * cb + sh * sp * sb;
            X = ch * sp * cb + sh * cp * sb;
            Y = -ch * sp * sb + sh * cp * cb;
            Z = -sh * sp * cb + ch * cp * sb;
        }

        public void SetToRotateInertialToObject(EulerAngles orientation)
        {
            float sp, sb, sh;
            float cp, cb, ch;
            sp = (float)Math.Sin(orientation.Pitch * 0.5f);
            cp = (float)Math.Cos(orientation.Pitch * 0.5f);
            sb = (float)Math.Sin(orientation.Bank * 0.5f);
            cb = (float)Math.Cos(orientation.Bank * 0.5f);
            sh = (float)Math.Sin(orientation.Heading * 0.5f);
            ch = (float)Math.Cos(orientation.Heading * 0.5f);

            W = ch * cp * cb + sh * sp * sb;
            X =-ch * sp * cb - sh * cp * sb;
            Y = ch * sp * sb - sh * cp * cb;
            Z = sh * sp * cb - ch * cp * sb;
        }

        public Vector3 Rotate(Vector3 v)
        {
            Quaternion p = new Quaternion(0.0f, v.X, v.Y, v.Z);
            Quaternion p2 = Conjugate(this) * p * this;
            return new Vector3(p2.X, p2.Y, p2.Z);
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(W * W + X * X + Y * Y + Z * Z);
        }

        public float GetScalar()
        {
            return W;
        }

        public Vector3 GetVector()
        {
            return new Vector3(X, Y, Z);
        }

        #endregion

        #region Overload operators

        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
            return new Quaternion(
                lhs.W * rhs.W - lhs.X * rhs.X - lhs.Y * rhs.Y - lhs.Z * rhs.Z,
                lhs.W * rhs.X + lhs.X * rhs.W + lhs.Z * rhs.Y - lhs.Y * rhs.Z,
                lhs.W * rhs.Y + lhs.Y * rhs.W + lhs.X * rhs.Z - lhs.Z * rhs.X,
                lhs.W * rhs.Z + lhs.Z * rhs.W + lhs.Y * rhs.X - lhs.X * rhs.Y);
        }

        public static Quaternion operator *(Vector3 v, Quaternion q)
        {
            Quaternion p = new Quaternion(0.0f, v.X, v.Y, v.Z);
            return p * q;
        }

        public static Quaternion operator *(Quaternion q, Vector3 v)
        {
            Quaternion p = new Quaternion(0.0f, v.X, v.Y, v.Z);
            return p * q;
        }

        #endregion

        #region Static Methods
        
        public static float Dot(Quaternion a, Quaternion b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static Quaternion Slerp(Quaternion q0, Quaternion q1, float t)
        {
            if (t <= 0.0f) return q0;
            if (t >= 1.0f) return q1;

            float cosOmega = Dot(q0, q1);
            float q1w = q1.W;
            float q1x = q1.X;
            float q1y = q1.Y;
            float q1z = q1.Z;
            if (cosOmega < 0.0f)
            {
                q1w = -q1w;
                q1x = -q1x;
                q1y = -q1y;
                q1z = -q1z;
                cosOmega = -cosOmega;
            }

            System.Diagnostics.Debug.Assert(cosOmega < 1.1f);

            float k0, k1;
            if (cosOmega > 0.9999f)
            {
                k0 = 1.0f - t;
                k1 = t;
            }
            else
            {
                float sinOmega = (float)Math.Sqrt(1.0f - cosOmega * cosOmega);
                float omega = (float)Math.Atan2(sinOmega, cosOmega);
                float oneOverSinOmega = 1.0f / sinOmega;

                k0 = (float)Math.Sin((1.0f - t) * omega) * oneOverSinOmega;
                k1 = (float)Math.Sin(t * omega) * oneOverSinOmega;
            }

            return new Quaternion(
                k0 * q0.W + k1 * q1w,
                k0 * q0.X + k1 * q1x,
                k0 * q0.Y + k1 * q1y,
                k0 * q0.Z + k1 * q1z);
        }

        public static Quaternion Conjugate(Quaternion q)
        {
            return new Quaternion(
                q.W,// Same rotation amount
                -q.X, -q.Y, -q.Z);// Opposite axis of rotation
        }

        public static Quaternion Power(Quaternion q, float exponent)
        {
            if (Math.Abs(q.W) > 0.9999f)
            {
                return q;
            }

            float alpha = (float)Math.Acos(q.W);

            float newAlpha = alpha * exponent;

            float mult = (float)Math.Sin(newAlpha / alpha);
            return new Quaternion(
                (float)Math.Cos(newAlpha),
                q.X * mult,
                q.Y * mult,
                q.Z * mult);
        }

        #endregion
    }
}
