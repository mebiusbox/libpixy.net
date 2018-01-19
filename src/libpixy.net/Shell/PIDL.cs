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
using System.Runtime.InteropServices;
using System.Collections;

namespace libpixy.net.Shell
{
    public class PIDL : IEnumerable
    {
        private IntPtr pidl = IntPtr.Zero;

        #region Constructors

        public PIDL(IntPtr pidl, bool clone)
        {
            if (clone)
            {
                this.pidl = ILClone(pidl);
            }
            else
            {
                this.pidl = pidl;
            }
        }

        public PIDL(PIDL pidl, bool clone)
        {
            if (clone)
            {
                this.pidl = ILClone(pidl.Ptr);
            }
            else
            {
                this.pidl = pidl.Ptr;
            }
        }

        #endregion

        #region Public

        public IntPtr Ptr { get { return pidl; } }

        public void Append(IntPtr appendPidl)
        {
            IntPtr newPidl = ILCombine(pidl, appendPidl);
            Marshal.FreeCoTaskMem(pidl);
            pidl = newPidl;
        }

        public void Insert(IntPtr insertPidl)
        {
            IntPtr newPidl = ILCombine(insertPidl, pidl);
            Marshal.FreeCoTaskMem(pidl);
            pidl = newPidl;
        }

        public static bool IsEmpty(IntPtr pidl)
        {
            if (pidl == IntPtr.Zero)
                return true;

            byte[] bytes = new byte[2];
            Marshal.Copy(pidl, bytes, 0, 2);
            int size = bytes[0] + bytes[1] * 256;
            return (size <= 2);
        }

        public static bool SplitPidl(IntPtr pidl, out IntPtr parent, out IntPtr child)
        {
            parent = ILClone(pidl);
            child = ILClone(ILFindLastID(pidl));

            if (!ILRemoveLastID(parent))
            {
                Marshal.FreeCoTaskMem(parent);
                Marshal.FreeCoTaskMem(child);
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void Write(IntPtr pidl)
        {
            StringBuilder path = new StringBuilder(256);
            libpixy.net.Shell.API.SHGetPathFromIDList(pidl, path);
            Console.Out.WriteLine("Pidl: {0}", path);
        }

        public static void WriteBytes(IntPtr pidl)
        {
            int size = Marshal.ReadByte(pidl, 0) + Marshal.ReadByte(pidl, 1) * 256 - 2;
            for (int i = 0; i < size; ++i)
            {
                Console.Out.WriteLine(Marshal.ReadByte(pidl, i + 2));
            }

            Console.Out.WriteLine(Marshal.ReadByte(pidl, size + 2));
            Console.Out.WriteLine(Marshal.ReadByte(pidl, size + 3));
        }

        public void Free()
        {
            if (pidl != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(pidl);
                pidl = IntPtr.Zero;
            }
        }

        #endregion

        #region Shell

        private static int ItemIDSize(IntPtr pidl)
        {
            if (!pidl.Equals(IntPtr.Zero))
            {
                byte[] buffer = new byte[2];
                Marshal.Copy(pidl, buffer, 0, 2);
                return buffer[1] * 256 + buffer[0];
            }
            else
            {
                return 0;
            }
        }

        private static int ItemIDListSize(IntPtr pidl)
        {
            if (pidl.Equals(IntPtr.Zero))
            {
                return 0;
            }
            else
            {
                int size = ItemIDSize(pidl);
                int nextSize = Marshal.ReadByte(pidl, size) + (Marshal.ReadByte(pidl, size + 1) * 256);
                while (nextSize > 0)
                {
                    size += nextSize;
                    nextSize = Marshal.ReadByte(pidl, size) + (Marshal.ReadByte(pidl, size + 1) * 256);
                }

                return size;
            }
        }

        // Clones as ITEMIDLIST structure
        public static IntPtr ILClone(IntPtr pidl)
        {
            int size = ItemIDListSize(pidl);

            byte[] bytes = new byte[size + 2];
            Marshal.Copy(pidl, bytes, 0, size);

            IntPtr newPidl = Marshal.AllocCoTaskMem(size + 2);
            Marshal.Copy(bytes, 0, newPidl, size + 2);

            return newPidl;
        }

        // Clones the first SHITEMID structure in an ITEMIDLIST structure
        public static IntPtr ILCloneFirst(IntPtr pidl)
        {
            int size = ItemIDSize(pidl);

            byte[] bytes = new byte[size + 2];
            Marshal.Copy(pidl, bytes, 0, size);

            IntPtr newPidl = Marshal.AllocCoTaskMem(size + 2);
            Marshal.Copy(bytes, 0, newPidl, size + 2);

            return newPidl;
        }

        // Gets the next SHITEMID structure in an ITEMIDLIST structure
        public static IntPtr ILGetNext(IntPtr pidl)
        {
            int size = ItemIDSize(pidl);
            IntPtr nextPidl = new IntPtr((int)pidl + size);
            return nextPidl;
        }

        // Returns a pointer to the last SHITEMID structure in an ITEMIDLIST structure
        public static IntPtr ILFindLastID(IntPtr pidl)
        {
            IntPtr ptr1 = pidl;
            IntPtr ptr2 = ILGetNext(ptr1);

            while (ItemIDSize(ptr2) > 0)
            {
                ptr1 = ptr2;
                ptr2 = ILGetNext(ptr1);
            }

            return ptr1;
        }

        // Removes the last SHITEMID structure from an ITEMIDLIST structure
        public static bool ILRemoveLastID(IntPtr pidl)
        {
            IntPtr lastPidl = ILFindLastID(pidl);
            if (lastPidl != pidl)
            {
                int newSize = (int)lastPidl - (int)pidl + 2;
                Marshal.ReAllocCoTaskMem(pidl, newSize);
                Marshal.Copy(new byte[] { 0, 0 }, 0, new IntPtr((int)pidl + newSize - 2), 2);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Conbines two ITEMIDLIST structure
        public static IntPtr ILCombine(IntPtr pidl1, IntPtr pidl2)
        {
            int size1 = ItemIDListSize(pidl1);
            int size2 = ItemIDListSize(pidl2);

            IntPtr newPidl = Marshal.AllocCoTaskMem(size1 + size2 + 2);
            byte[] bytes = new byte[size1 + size2 + 2];
            Marshal.Copy(pidl1, bytes, 0, size1);
            Marshal.Copy(pidl2, bytes, size1, size2);
            Marshal.Copy(bytes, 0, newPidl, bytes.Length);

            return newPidl;
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return new PIDLEnumerator(pidl);
        }

        #endregion

        #region Override

        public override bool Equals(object obj)
        {
            try
            {
                if (obj is IntPtr)
                    return libpixy.net.Shell.API.ILIsEqual(this.Ptr, (IntPtr)obj);
                if (obj is PIDL)
                    return libpixy.net.Shell.API.ILIsEqual(this.Ptr, ((PIDL)obj).Ptr);
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return pidl.GetHashCode();
        }

        #endregion

        #region

        public class PIDLEnumerator : IEnumerator
        {
            private IntPtr pidl;
            private IntPtr curPidl;
            private IntPtr clonePidl;
            private bool start;

            public PIDLEnumerator(IntPtr pidl)
            {
                start = true;
                this.pidl = pidl;
                curPidl = pidl;
                clonePidl = IntPtr.Zero;
            }

            public object Current
            {
                get
                {
                    if (clonePidl != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(clonePidl);
                        clonePidl = IntPtr.Zero;
                    }

                    clonePidl = PIDL.ILCloneFirst(curPidl);
                    return clonePidl;
                }
            }

            public bool MoveNext()
            {
                if (clonePidl != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(clonePidl);
                    clonePidl = IntPtr.Zero;
                }

                if (start)
                {
                    start = false;
                    return true;
                }
                else
                {
                    IntPtr newPidl = ILGetNext(curPidl);
                    if (!PIDL.IsEmpty(newPidl))
                    {
                        curPidl = newPidl;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            public void Reset()
            {
                start = true;
                curPidl = pidl;

                if (clonePidl != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(clonePidl);
                    clonePidl = IntPtr.Zero;
                }
            }

            #endregion
        }
    }
}
