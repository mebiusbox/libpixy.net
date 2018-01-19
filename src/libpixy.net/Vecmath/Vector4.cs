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
    public class Vector4
    {
        #region Fields

        public float X;
        public float Y;
        public float Z;
        public float W;

        #endregion

        #region Constructor/Destructor

        public Vector4() : base()
        {
        }

        public Vector4(float x, float y, float z, float w)
            : base()
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4(Vector4 v)
            : base()
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = v.W;
        }

        #endregion

        #region Overload operators

        public static Vector4 operator +(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
        }

        public static Vector4 operator -(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.X-rhs.X, lhs.Y-rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
        }

        public static Vector4 operator *(Vector4 lhs, float s)
        {
            return new Vector4(lhs.X *s, lhs.Y *s, lhs.Z*s, lhs.W*s);
        }

        public static Vector4 operator *(float s, Vector4 rhs)
        {
            return new Vector4(rhs.X * s, rhs.Y * s, rhs.Z * s, rhs.W*s);
        }

        public static Vector4 operator /(Vector4 lhs, float s)
        {
            float oneOverA = 1.0f / s;
            return new Vector4(lhs.X * oneOverA, lhs.Y * oneOverA, lhs.Z * oneOverA, lhs.W * oneOverA);
        }

        public static Vector4 operator -(Vector4 v)
        {
            return new Vector4(-v.X, -v.Y, -v.Z, -v.W);
        }

        #endregion

        #region Methods

        public void Set(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public void SetZero()
        {
            X = Y = Z = W = 0.0f;
        }

        public void Add(Vector4 v)
        {
            X += v.X;
            Y += v.Y;
            Z += v.Z;
            W += v.W;
        }

        public void Sub(Vector4 v)
        {
            X -= v.X;
            Y -= v.Y;
            Z -= v.Z;
            W -= v.W;
        }

        public void Mul(Vector4 v)
        {
            X *= v.X;
            Y *= v.Y;
            Z *= v.Z;
            W *= v.W;
        }

        public void Mul(float s)
        {
            X *= s;
            Y *= s;
            Z *= s;
            W *= s;
        }

        public void Div(float s)
        {
            X /= s;
            Y /= s;
            Z /= s;
            W /= s;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public float LengthSq()
        {
            return X * X + Y * Y + Z * Z;
        }

        public void Reverse()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
            W = -W;
        }

        public void Normalize()
        {
            float q = LengthSq();
            if (q > 0.0f)
            {
                Mul(1.0f / (float)Math.Sqrt(q));
            }
        }

        public void Normalize(Vector4 v)
        {
            Set(v.X, v.Y, v.Z, v.W);
            Normalize();
        }

        public float Dot(Vector4 v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        public void Cross(Vector4 v)
        {
            Set(Y * v.Z - Z * v.Y, Z * v.Y - X * v.Z, X * v.Y - Y * v.X, 0.0f);
        }

        public void Cross(Vector4 v1, Vector4 v2)
        {
            Set(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.Y - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X, 0.0f);
        }

        public void RotX(float angle)
        {
            float xx = X;
            float yy = Y;
            float zz = Z;
            float pp = Constant.kPiOver180;
            X = xx;
            Y = yy * (float)Math.Cos(pp * angle) - zz * (float)Math.Sin(pp * angle);
            Z = yy * (float)Math.Sin(pp * angle) + zz * (float)Math.Cos(pp * angle);
        }

        public void RotY(float angle)
        {
            float xx = X;
            float yy = Y;
            float zz = Z;
            float pp = Constant.kPiOver180;
            X = xx * (float)Math.Cos(pp * angle) + zz * (float)Math.Sin(pp * angle);
            Y = yy;
            Z = -xx * (float)Math.Sin(pp * angle) + zz * (float)Math.Cos(pp * angle);
        }

        public void RotZ(float angle)
        {
            float xx = X;
            float yy = Y;
            float zz = Z;
            float pp = Constant.kPiOver180;
            X = xx * (float)Math.Cos(pp * angle) - yy * (float)Math.Sin(pp * angle);
            Y = xx * (float)Math.Sin(pp * angle) + yy * (float)Math.Cos(pp * angle);
            Z = zz;
        }

        public Vector4 Rotate(Vector4 v, Vector4 elr)
        {
            Vector4 ret = new Vector4();
            ret.Set(v.X, v.Y, v.Z, v.W);
            ret.RotX(elr.X);
            ret.RotY(elr.Y);
            ret.RotZ(elr.Z);
            return ret;
        }

        #endregion

        #region Static Methods

        public static float DotProduct(Vector4 a, Vector4 b)
        {
            return a.X * b.Y + a.Y * b.Y + a.Z * b.Z;
        }

        public static float Magnitude(Vector4 a)
        {
            return (float)Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z);
        }

        public static float Magnitude(Vector4 a, Vector4 b)
        {
            return Distance(a,b);
        }

        public static Vector4 CrossProduct(Vector4 a, Vector4 b)
        {
            return new Vector4(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X,
                0.0f);
        }

        public static float Distance(Vector4 a, Vector4 b)
        {
            return (float)Math.Sqrt(DistanceSq(a, b));
        }

        public static float DistanceSq(Vector4 a, Vector4 b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            float dz = a.Z - b.Z;
            return dx * dx + dy * dy + dz * dz;
        }

        public static float GetAngle(Vector4 a, Vector4 b)
        {
            float ang = DotProduct(a, b) / (a.Length() * b.Length());
            if (ang >= 1.0f) {
                ang = 0.0f;
            }
            else if (ang <= -1.0f)
            {
                ang = Constant.kPi;
            }
            else
            {
                ang = (float)Math.Acos(ang);
            }

            return ang;
        }

        public static Vector4 GetEulerX(Vector4 a, Vector4 b)
        {
            float cx = b.X - a.X;
            float cy = b.Y - a.Y;
            float cz = b.Z - a.Z;
            float len = Distance(a, b);
            Vector4 ret = new Vector4();
            ret.X = 0.0f;
            if (cz >= len)
            {
                ret.Y = -90.0f;
            }
            else if (cz <= -len)
            {
                ret.Y = 90.0f;
            }
            else
            {
                ret.Y = -((float)Math.Asin(cz / len) * 180.0f / Constant.kPi);
            }

            if (Math.Abs(cx) <= 0.0001 && Math.Abs(cy) <= 0.0001)
            {
                ret.Z = 0.0f;
            }
            else
            {
                ret.Z = ((float)Math.Atan2(cy, cx) * 180.0f / Constant.kPi);
            }

            return ret;
        }

        public static Vector4 GetNormal(Vector4 a, Vector4 b)
        {
            Vector4 t = b - a;
            t.Normalize();
            return t;
        }

        #endregion

    }
}
