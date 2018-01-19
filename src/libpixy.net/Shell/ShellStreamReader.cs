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
using System.IO;
using System.Runtime.InteropServices;

namespace libpixy.net.Shell
{
    public class ShellStreamReader : Stream, IDisposable
    {
        #region Fields

        private ShellItem shellItem;

        private IntPtr streamPtr;
        private IStream stream;

        private bool canRead, canWrite;

        private libpixy.net.Shell.API.STATSTG streamInfo;
        private bool streamInfoRead = false;

        private long currentPos;

        private bool disposed = false;

        #endregion

        public ShellStreamReader(ShellItem shellItem, IStorage parentStorage, FileAccess access)
        {
            this.shellItem = shellItem;

            OpenStream(parentStorage, ref access);

            this.canRead = (access == FileAccess.Read || access == FileAccess.ReadWrite);
            this.canWrite = (access == FileAccess.Write || access == FileAccess.ReadWrite);
            currentPos = 0;
        }

        ~ShellStreamReader()
        {
            CloseStream();
        }

        #region Stream Members

        public override bool CanRead
        {
            get { return canRead; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return canWrite; }
        }

        public override void Flush()
        {

        }

        public override long Length
        {
            get
            {
                if (CanRead)
                    return StreamInfo.cbSize;
                else
                    return 0;
            }
        }

        public override long Position
        {
            get
            {
                return currentPos;
            }
            set
            {
                Seek(value, SeekOrigin.Begin);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (CanRead)
            {
                byte[] readBytes = new byte[count];

                IntPtr readNrPtr = Marshal.AllocCoTaskMem(32);
                stream.Read(readBytes, count, readNrPtr);

                int readNr = (int)Marshal.PtrToStructure(readNrPtr, typeof(Int32));
                Marshal.FreeCoTaskMem(readNrPtr);

                Array.Copy(readBytes, 0, buffer, offset, readNr);
                currentPos += readNr;
                return readNr;
            }
            else
                return 0;
        }

        public override void SetLength(long value)
        {
            if (CanWrite)
                stream.SetSize(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (CanWrite)
            {
                byte[] writeBytes = new byte[count];
                Array.Copy(buffer, offset, writeBytes, 0, count);

                IntPtr writtenNrPtr = Marshal.AllocCoTaskMem(64);
                stream.Write(writeBytes, count, writtenNrPtr);

                long writtenNr = (long)Marshal.PtrToStructure(writtenNrPtr, typeof(Int64));
                Marshal.FreeCoTaskMem(writtenNrPtr);

                currentPos += writtenNr;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (CanSeek)
            {
                IntPtr newPosPtr = Marshal.AllocCoTaskMem(64);
                stream.Seek(offset, origin, newPosPtr);

                long newPos = (long)Marshal.PtrToStructure(newPosPtr, typeof(Int64));
                Marshal.FreeCoTaskMem(newPosPtr);

                return (currentPos = newPos);
            }
            else
                return -1;
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (!disposed)
            {
                disposed = true;

                if (stream != null)
                {
                    Marshal.ReleaseComObject(stream);
                    stream = null;
                }

                if (streamPtr != IntPtr.Zero)
                {
                    Marshal.Release(streamPtr);
                    streamPtr = IntPtr.Zero;
                }

                GC.SuppressFinalize(this);
            }
        }

        #endregion

        #region Open/Close Stream

        public override void Close()
        {

        }

        internal void CloseStream()
        {
            base.Close();

            if (stream != null)
            {
                Marshal.ReleaseComObject(stream);
                Marshal.Release(streamPtr);
            }
        }

        internal libpixy.net.Shell.API.STATSTG StreamInfo
        {
            get
            {
                if (!streamInfoRead)
                {
                    stream.Stat(out streamInfo, libpixy.net.Shell.API.STATFLAG.NONAME);
                    streamInfoRead = true;
                }
                return streamInfo;
            }
        }

        private void OpenStream(IStorage parentStorage, ref FileAccess access)
        {
            libpixy.net.Shell.API.STGM grfmode = libpixy.net.Shell.API.STGM.SHARE_DENY_WRITE;

            switch (access)
            {
                case FileAccess.ReadWrite:
                    grfmode |= libpixy.net.Shell.API.STGM.READWRITE;
                    break;

                case FileAccess.Write:
                    grfmode |= libpixy.net.Shell.API.STGM.WRITE;
                    break;
            }

            if (parentStorage != null)
            {
                if (parentStorage.OpenStream(
                        shellItem.Text + (shellItem.Flags.isLink ? ".lnk" : string.Empty),
                        IntPtr.Zero,
                        grfmode,
                        0,
                        out streamPtr) == libpixy.net.Shell.API.S_OK)
                {
                    stream = (IStream)Marshal.GetTypedObjectForIUnknown(streamPtr, typeof(IStream));
                }
                else if (access != FileAccess.Read)
                {
                    if (parentStorage.OpenStream(
                        shellItem.Text + (shellItem.Flags.isLink ? ".lnk" : string.Empty),
                        IntPtr.Zero,
                        libpixy.net.Shell.API.STGM.SHARE_DENY_WRITE,
                        0,
                        out streamPtr) == libpixy.net.Shell.API.S_OK)
                    {
                        access = FileAccess.Read;
                        stream = (IStream)Marshal.GetTypedObjectForIUnknown(streamPtr, typeof(IStream));
                    }
                    else
                        throw new IOException(String.Format("Can't open stream: {0}", shellItem));
                }
                else
                    throw new IOException(String.Format("Can't open stream: {0}", shellItem));
            }
            else
            {
                access = FileAccess.Read;

                if (!ShellHelper.GetIStream(shellItem, out streamPtr, out stream))
                    throw new IOException(String.Format("Can't open stream: {0}", shellItem));
            }
        }

        #endregion
    }
}
