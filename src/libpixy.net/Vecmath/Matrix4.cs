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
    public class Matrix4
    {
        #region Fields

        public float m11, m12, m13, m14;
        public float m21, m22, m23, m24;
        public float m31, m32, m33, m34;
        public float m41, m42, m43, m44;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Matrix4()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Matrix4(
            float n11, float n12, float n13, float n14,
            float n21, float n22, float n23, float n24,
            float n31, float n32, float n33, float n34,
            float n41, float n42, float n43, float n44)
        {
            m11 = n11; m12 = n12; m13 = n13; m14 = n14;
            m21 = n21; m22 = n22; m23 = n23; m24 = n24;
            m31 = n31; m32 = n32; m33 = n33; m34 = n34;
            m41 = n41; m42 = n42; m43 = n43; m44 = n44;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Matrix4(Matrix4 other)
        {
            m11 = other.m11; m12 = other.m12; m13 = other.m13; m14 = other.m14;
            m21 = other.m21; m22 = other.m22; m23 = other.m23; m24 = other.m24;
            m31 = other.m31; m32 = other.m32; m33 = other.m33; m34 = other.m34;
            m41 = other.m41; m42 = other.m42; m43 = other.m43; m44 = other.m44;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the matrix to identity
        /// </summary>
        public void SetIdentity()
        {
            m11 = m22 = m33 = m44 = 1.0f;
            m12 = m13 = m14 = 0.0f;
            m21 = m23 = m24 = 0.0f;
            m31 = m32 = m34 = 0.0f;
            m41 = m42 = m43 = 0.0f;
        }

        /// <summary>
        /// Zero the 4th row of the matrix, which contains the translation portion.
        /// </summary>
        public void SetZeroTranslation()
        {
            m41 = m42 = m43 = 0.0f;
        }

        /// <summary>
        /// Sets the translation portion of the matrix in vector form
        /// </summary>
        /// <param name="d"></param>
        public void SetTranslation(Vector3 d)
        {
            m41 = d.X;
            m42 = d.Y;
            m43 = d.Z;
        }

        /// <summary>
        /// Sets the translation portion of the matrix in vector form
        /// </summary>
        /// <param name="d"></param>
        public void SetupTranslation(Vector3 d)
        {
            SetIdentity();
            SetTranslation(d);
        }

        public void SetupLocalToParent(Vector3 pos, EulerAngles orient)
        {
            RotationMatrix orientMatrix = new RotationMatrix();
            orientMatrix.Setup(orient);
            SetupLocalToParent(pos, orientMatrix);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="orient"></param>
        public void SetupLocalToParent(Vector3 pos, RotationMatrix orient)
        {
            m11 = orient.m11; m12 = orient.m21; m13 = orient.m31; m14 = 0.0f;
            m21 = orient.m12; m22 = orient.m22; m23 = orient.m32; m24 = 0.0f;
            m31 = orient.m13; m32 = orient.m23; m33 = orient.m33; m34 = 0.0f;
            m41 = pos.X; m42 = pos.Y; m43 = pos.Z; m44 = 1.0f;
        }

        /// <summary>
        /// Setup the matrix to perform a parent -> local transformation, given
        /// the position and orientation of the local reference frame within the 
        /// parent reference frame.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="orient"></param>
        public void SetupParentToLocal(Vector3 pos, EulerAngles orient)
        {
            RotationMatrix orientMatrix = new RotationMatrix();
            orientMatrix.Setup(orient);
            SetupParentToLocal(pos, orientMatrix);
        }

        public void SetupParentToLocal(Vector3 pos, RotationMatrix orient)
        {
            m11 = orient.m11; m12 = orient.m12; m13 = orient.m13; m14 = 0.0f;
            m21 = orient.m21; m22 = orient.m22; m23 = orient.m23; m24 = 0.0f;
            m31 = orient.m31; m32 = orient.m32; m33 = orient.m33; m34 = 0.0f;
            m41 = -(pos.X*m11 + pos.Y*m21 + pos.Z * m31);
            m42 = -(pos.X*m12 + pos.Y*m22 + pos.Z * m32);
            m43 = -(pos.X*m13 + pos.Y*m23 + pos.Z * m33);
            m44 = 1.0f;
        }

        public void SetupRotateXYZ(libpixy.net.Vecmath.Vector3 theta)
        {
            SetupRotateXYZ(theta.X, theta.Y, theta.Z);
        }

        /// <summary>
        /// rotation (XYZ)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void SetupRotateXYZ(float x, float y, float z)
        {
            Matrix4 matRX = new Matrix4();
            Matrix4 matRY = new Matrix4();
            Matrix4 matRZ = new Matrix4();
            matRX.SetupRotate(1, x);
            matRY.SetupRotate(2, y);
            matRZ.SetupRotate(3, z);
            Matrix4 mat = matRX * matRY * matRZ;
            m11 = mat.m11; m12 = mat.m12; m13 = mat.m13; m14 = mat.m14;
            m21 = mat.m21; m22 = mat.m22; m23 = mat.m23; m24 = mat.m24;
            m31 = mat.m31; m32 = mat.m32; m33 = mat.m33; m34 = mat.m34;
            m41 = mat.m41; m42 = mat.m42; m43 = mat.m43; m44 = mat.m44;
        }

        /// <summary>
        /// rotation (ZYX)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void SetupRotateZYX(libpixy.net.Vecmath.Vector3 rot)
        {
            SetupRotateZYX(rot.X, rot.Y, rot.Z);
        }

        /// <summary>
        /// rotation (ZYX)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void SetupRotateZYX(float x, float y, float z)
        {
            Matrix4 matRX = new Matrix4();
            Matrix4 matRY = new Matrix4();
            Matrix4 matRZ = new Matrix4();
            matRX.SetupRotate(1, x);
            matRY.SetupRotate(2, y);
            matRZ.SetupRotate(3, z);
            Matrix4 mat = matRZ * matRY * matRX;
            m11 = mat.m11; m12 = mat.m12; m13 = mat.m13; m14 = mat.m14;
            m21 = mat.m21; m22 = mat.m22; m23 = mat.m23; m24 = mat.m24;
            m31 = mat.m31; m32 = mat.m32; m33 = mat.m33; m34 = mat.m34;
            m41 = mat.m41; m42 = mat.m42; m43 = mat.m43; m44 = mat.m44;
        }

        public void SetupRotate(int axis, float theta)
        {
            float s = (float)Math.Sin(theta);
            float c = (float)Math.Cos(theta);

            // Check which axis they are rotating about

            switch (axis)
            {
                case 1://Rotate about the x-axis
                    m11 = 1.0f; m12 = 0.0f; m13 = 0.0f;
                    m21 = 0.0f; m22 = c;    m23 = s;
                    m31 = 0.0f; m32 = -s;   m33 = c;
                    break;

                case 2://Rotate about the y-axis
                    m11 = c;    m12 = 0.0f; m13 = -s;
                    m21 = 0.0f; m22 = 1.0f; m23 = 0.0f;
                    m31 = s;    m32 = 0.0f; m33 = c;
                    break;

                case 3://Rotate about the z-axis
                    m11 = c;    m12 = s;    m13 = 0.0f;
                    m21 = -s;   m22 = c;    m23 = 0.0f;
                    m31 = 0.0f; m32 = 0.0f; m33 = 1.0f;
                    break;

                default:
                    // bogus axis index
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }

            m14 = m24 = m34 = 0.0f;
            m44 = 1.0f;
            m41 = m42 = m43 = 0.0f;
        }

        public void SetupRotate(Vector3 axis, float theta)
        {
            System.Diagnostics.Debug.Assert(Math.Abs(axis.Dot(axis) - 1.0f) < 0.01f);

            float s = (float)Math.Sin(theta);
            float c = (float)Math.Cos(theta);

            float a = 1.0f - c;
            float ax = a*axis.X;
            float ay = a*axis.Y;
            float az = a*axis.Z;

            m11 = ax*axis.X + c;
            m12 = ax*axis.Y + axis.Z*s;
            m13 = ax*axis.Z - axis.Y*s;

            m21 = ay*axis.X - axis.Z*s;
            m22 = ay*axis.Y + c;
            m23 = ay*axis.Z + axis.X*s;

            m31 = az*axis.X + axis.Y*s;
            m32 = az*axis.Y - axis.X*s;
            m33 = az*axis.Z + c;

            m41 = m42 = m43 = 0.0f;
            m44 = 1.0f;
            m14 = m24 = m34 = 0.0f;
        }

        public void FromQuaternion(Quaternion q)
        {
            float ww = 2.0f * q.W;
            float xx = 2.0f * q.X;
            float yy = 2.0f * q.Y;
            float zz = 2.0f * q.Z;

            m11 = 1.0f - yy*q.Y - zz*q.Z;
            m12 = xx*q.Y + ww*q.Z;
            m13 = xx*q.Z - ww*q.Y;

            m21 = xx*q.Y - ww*q.Z;
            m22 = 1.0f - xx*q.X - zz*q.Z;
            m23 = yy*q.Z + ww*q.X;

            m31 = xx*q.Z + ww*q.Y;
            m32 = yy*q.Z - ww*q.X;
            m33 = 1.0f - xx*q.X - yy*q.Y;

            m41 = m42 = m43 = 0.0f;
            m44 = 1.0f;
            m14 = m24 = m34 = 0.0f;
        }

        public void SetupScale(Vector3 s)
        {
            m11 = s.X; m12 = m13 = m14 = 0.0f;
            m22 = s.Y; m21 = m23 = m24 = 0.0f;
            m33 = s.Z; m31 = m32 = m34 = 0.0f;
            m41 = m42 = m43 = 0.0f;
            m44 = 1.0f;
        }

        public void SetupScaleAlongAxis(Vector3 axis, float k)
        {
            System.Diagnostics.Debug.Assert(Math.Abs(Vector3.DotProduct(axis, axis) - 1.0f) < 0.01f);

            float a = k - 1.0f;
            float ax = a * axis.X;
            float ay = a * axis.Y;
            float az = a * axis.Z;

            m11 = ax * axis.X + 1.0f;
            m22 = ay * axis.Y + 1.0f;
            m33 = az * axis.Z + 1.0f;

            m12 = m21 = ax * axis.Y;
            m13 = m31 = ax * axis.Z;
            m23 = m32 = ay * axis.Z;

            m14 = m24 = m34 = 0.0f;
            m44 = 1.0f;
        }

        public void SetupShear(int axis, float s, float t)
        {
            switch (axis)
            {
                case 1:
                    // Shear y and z using x
                    m11 = 1.0f; m12 = s;    m13 = t; m14 = 0.0f;
                    m21 = 0.0f; m22 = 1.0f; m23 = 0.0f; m24 = 0.0f;
                    m31 = 0.0f; m32 = 0.0f; m33 = 1.0f; m34 = 0.0f;
                    break;

                case 2:
                    // Shear x and z using y
                    m11 = 1.0f; m12 = 0.0f; m13 = 0.0f; m14 = 0.0f;
                    m21 = s;    m22 = 1.0f; m23 = t;    m24 = 0.0f;
                    m31 = 0.0f; m32 = 0.0f; m33 = 1.0f; m34 = 0.0f;
                    break;

                case 3:
                    // Shear x and y using z
                    m11 = 1.0f; m12 = 0.0f; m13 = 0.0f; m14 = 0.0f;
                    m21 = 0.0f; m22 = 1.0f; m23 = 0.0f; m24 = 0.0f;
                    m31 = s;    m32 = t;    m33 = 1.0f; m34 = 0.0f;
                    break;

                default:
                    System.Diagnostics.Debug.Assert(false);

                    break;
            }

            m41 = m42 = m43 = 0.0f;
            m44 = 1.0f;
        }

        public void SetupProject(Vector3 n)
        {
            System.Diagnostics.Debug.Assert(Math.Abs(Vector3.DotProduct(n,n)) < 0.01f);

            m11 = 1.0f - n.X*n.X;
            m22 = 1.0f - n.Y*n.Y;
            m33 = 1.0f - n.Z*n.Z;

            m12 = m21 = -n.X * n.Y;
            m13 = m32 = -n.X * n.Z;
            m23 = m32 = -n.Y * n.Z;

            m14 = m24 = m34 = 0.0f;
            
            m14 = m24 = m34 = 0.0f;
            m44 = 1.0f;
        }

        public void SetupReflect(int axis, float k)
        {
            switch (axis)
            {
                case 1:
                    // Reflect about the plane x=k
                    m11 = -1.0f; m12 = 0.0f; m13 = 0.0f; m14 = 0.0f;
                    m21 =  0.0f; m22 = 1.0f; m23 = 0.0f; m24 = 0.0f;
                    m31 =  0.0f; m32 = 0.0f; m33 = 1.0f; m34 = 0.0f;
                    m41 = 2.0f * k; m42 = 0.0f; m43 = 0.0f; m44 = 1.0f;
                    break;

                case 2:
                    // Reflect about the plane y=k
                    m11 = 1.0f; m12 =  0.0f; m13 = 0.0f; m14 = 0.0f;
                    m21 = 0.0f; m22 = -1.0f; m23 = 0.0f; m24 = 0.0f;
                    m31 = 0.0f; m32 =  0.0f; m33 = 1.0f; m34 = 0.0f;
                    m41 = 0.0f; m42 = 2.0f*k; m43 = 0.0f; m44 = 1.0f;
                    break;

                case 3:
                    // Reflect about the plane z=k
                    m11 = 1.0f; m12 = 0.0f; m13 =  0.0f; m14 = 0.0f;
                    m21 = 0.0f; m22 = 1.0f; m23 =  0.0f; m24 = 0.0f;
                    m31 = 0.0f; m32 = 0.0f; m33 = -1.0f; m34 = 0.0f;
                    m41 = 0.0f; m42 = 0.0f; m43 = 2.0f*k; m44 = 1.0f;
                    break;

                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }
        }

        public void SetupReflect(Vector3 n)
        {
            System.Diagnostics.Debug.Assert(Math.Abs(Vector3.DotProduct(n, n) - 1.0f) < 0.01f);

            float ax = -2.0f * n.X;
            float ay = -2.0f * n.Y;
            float az = -2.0f * n.Z;

            m11 = 1.0f + ax * n.X;
            m22 = 1.0f + ay * n.Y;
            m33 = 1.0f + az * n.Z;

            m12 = m21 = ax * n.Y;
            m13 = m31 = ax * n.Z;
            m23 = m32 = ay * n.Z;

            m14 = m24 = m34 = 0.0f;

            m41 = m42 = m43 = 0.0f;
            m44 = 1.0f;
        }

        public float Determinant33()
        {
            return m11 * (m22 * m33 - m23 * m32) + m12 * (m23 * m31 - m21 * m33) + m13 * (m21 * m32 - m22 * m31);
        }

        public float Determinant()
        {
            return m11 * (m22 * m33 - m23 * m32) + m12 * (m23 * m31 - m21 * m33) + m13 * (m21 * m32 - m22 * m31);
        }

        public Matrix4 Inverse()
        {
            float det = Determinant();

            System.Diagnostics.Debug.Assert(Math.Abs(det) > 0.000001f);

            float oneOverDet = 1.0f / det;

            Matrix4 r = new Matrix4();

            r.m11 = (m22 * m33 - m23 * m32) * oneOverDet;
            r.m12 = (m13 * m32 - m12 * m33) * oneOverDet;
            r.m13 = (m12 * m23 - m13 * m22) * oneOverDet;

            r.m21 = (m23 * m31 - m21 * m33) * oneOverDet;
            r.m22 = (m11 * m33 - m13 * m31) * oneOverDet;
            r.m23 = (m13 * m21 - m11 * m23) * oneOverDet;

            r.m31 = (m21 * m32 - m22 * m31) * oneOverDet;
            r.m32 = (m12 * m31 - m11 * m32) * oneOverDet;
            r.m33 = (m11 * m22 - m12 * m21) * oneOverDet;

            r.m41 = -(m41 * r.m11 + m42 * r.m21 + m43 * r.m31);
            r.m42 = -(m41 * r.m12 + m42 * r.m22 + m43 * r.m32);
            r.m43 = -(m41 * r.m13 + m42 * r.m23 + m43 * r.m33);

            return r;
        }

        public void Transpose()
        {
            float tmp = m12; m12 = m21; m21 = tmp;
            tmp = m13; m13 = m31; m31 = tmp;
            tmp = m14; m14 = m41; m41 = tmp;
            tmp = m23; m23 = m32; m32 = tmp;
            tmp = m24; m24 = m42; m42 = tmp;
            tmp = m34; m34 = m43; m43 = tmp;
        }

        public Vector3 GetTranslation()
        {
            return new Vector3(m41, m42, m43);
        }

        public Vector3 GetPositionFromParentToLocalMatrix()
        {
            return new Vector3(
                -(m41 * m11 + m42 * m12 + m43 * m13),
                -(m41 * m21 + m42 * m22 + m43 * m23),
                -(m41 * m31 + m43 * m32 + m43 * m33));
        }

        public Vector3 GetPositionFromLocalToParentMatrix()
        {
            return new Vector3(m41, m42, m43);
        }

        public Vector3 Transform(Vector3 v)
        {
            return v * this;
        }

        #endregion

        #region Overload operators

        public static Vector3 operator *(Vector3 p, Matrix4 m)
        {
            return new Vector3(
                p.X * m.m11 + p.Y * m.m21 + p.Z * m.m31 + m.m41,
                p.X * m.m12 + p.Y * m.m22 + p.Z * m.m32 + m.m42,
                p.X * m.m13 + p.Y * m.m23 + p.Z * m.m33 + m.m43);
        }

        public static Matrix4 operator *(Matrix4 a, Matrix4 b)
        {
            Matrix4 r = new Matrix4();

            r.m11 = a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31 + a.m14 * b.m41;
            r.m12 = a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32 + a.m14 * b.m42;
            r.m13 = a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33 + a.m14 * b.m43;
            r.m14 = a.m11 * b.m14 + a.m12 * b.m24 + a.m13 * b.m34 + a.m14 * b.m44;

            r.m21 = a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31 + a.m24 * b.m41;
            r.m22 = a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32 + a.m24 * b.m42;
            r.m23 = a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33 + a.m24 * b.m43;
            r.m24 = a.m21 * b.m14 + a.m22 * b.m24 + a.m23 * b.m34 + a.m24 * b.m44;

            r.m31 = a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31 + a.m34 * b.m41;
            r.m32 = a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32 + a.m34 * b.m42;
            r.m33 = a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33 + a.m34 * b.m43;
            r.m34 = a.m31 * b.m14 + a.m32 * b.m24 + a.m33 * b.m34 + a.m34 * b.m44;

            r.m41 = a.m41 * b.m11 + a.m42 * b.m21 + a.m43 * b.m31 + a.m44 * b.m41;
            r.m42 = a.m41 * b.m12 + a.m42 * b.m22 + a.m43 * b.m32 + a.m44 * b.m42;
            r.m43 = a.m41 * b.m13 + a.m42 * b.m23 + a.m43 * b.m33 + a.m44 * b.m43;
            r.m44 = a.m41 * b.m14 + a.m42 * b.m24 + a.m43 * b.m34 + a.m44 * b.m44;

            return r;
        }

        #endregion

        #region Constants

        public static Matrix4 Unit
        {
            get
            {
                return new Matrix4(
                    1.0f, 0.0f, 0.0f, 0.0f,
                    0.0f, 1.0f, 0.0f, 0.0f,
                    0.0f, 0.0f, 1.0f, 0.0f,
                    0.0f, 0.0f, 0.0f, 1.0f);
            }
        }

        public static Matrix4 Zero
        {
            get
            {
                return new Matrix4(
                    0.0f, 0.0f, 0.0f, 0.0f,
                    0.0f, 0.0f, 0.0f, 0.0f,
                    0.0f, 0.0f, 0.0f, 0.0f,
                    0.0f, 0.0f, 0.0f, 0.0f);
            }
        }

        #endregion Constants
    }
}
