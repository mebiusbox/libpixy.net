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

namespace libpixy.net.Tools
{
    /// <summary>
    /// イベントマネージャ
    /// </summary>
    public class TEventManager<type>
    {
        /// <summary>
        /// Delegater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventHandler(object sender, TEventArgs<type> e);

        /// <summary>
        /// ハンドラ保持
        /// </summary>
        private class EventHandlerHolder
        {
            public EventHandlerHolder()
            {
            }

            public void Fire(object sender, TEventArgs<type> e)
            {
                if (this.Handlers != null)
                {
                    this.Handlers(sender, e);
                }
            }

            public event EventHandler Handlers;
        }

        /// <summary>
        /// イベントリスナ
        /// </summary>
        private Dictionary<string, EventHandlerHolder> m_handlers;

        /// <summary>
        /// Constructor
        /// </summary>
        public TEventManager()
        {
            m_handlers = new Dictionary<string, EventHandlerHolder>();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        /// <param name="name"></param>
        public void CreateEvent(string name)
        {
            if (m_handlers.ContainsKey(name))
            {
                return;
            }

            try
            {
                m_handlers.Add(name, new EventHandlerHolder());
            }
            catch (ArgumentException /*e*/)
            {
                //Already exists
            }
        }

        /// <summary>
        /// イベント削除
        /// </summary>
        /// <param name="name"></param>
        public void DeleteEvent(string name)
        {
            if (m_handlers.ContainsKey(name) == false)
            {
                return;
            }

            try
            {
                m_handlers.Remove(name);
            }
            catch { }
        }

        /// <summary>
        /// ハンドラ設定
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handler"></param>
        public void SetHandler(string name, EventHandler handler)
        {
            if (m_handlers.ContainsKey(name) == false)
            {
                CreateEvent(name);
            }

            try
            {
                EventHandlerHolder holder = m_handlers[name];
                holder.Handlers += handler;
            }
            catch (ArgumentException /* e */)
            {
                // this key is not exists
                CreateEvent(name);
            }
            catch { }
        }

        /// <summary>
        /// ハンドラ削除
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handler"></param>
        public void RemoveHandler(string name, EventHandler handler)
        {
            if (m_handlers.ContainsKey(name) == false)
            {
                return;
            }

            try
            {
                EventHandlerHolder holder = m_handlers[name];
                holder.Handlers -= handler;
            }
            catch { }
        }

        /// <summary>
        /// イベント起動
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Fire(string name, object sender, TEventArgs<type> e)
        {
            if (m_handlers.ContainsKey(name) == false)
            {
                return;
            }

            try
            {
                EventHandlerHolder holder = m_handlers[name];
                holder.Fire(sender, e);
            }
            catch { }
        }

        /// <summary>
        /// イベント起動
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Fire(string name, object sender, type value)
        {
            if (m_handlers.ContainsKey(name) == false)
            {
                return;
            }

            try
            {
                EventHandlerHolder holder = m_handlers[name];
                holder.Fire(sender, new TEventArgs<type>(value));
            }
            catch { }
        }
    }

    /// <summary>
    /// イベントマネージャ
    /// </summary>
    public class EventManager
    {
        /// <summary>
        /// Delegater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventHandler(object sender, EventTagArgs e);

        /// <summary>
        /// ハンドラ保持
        /// </summary>
        private class EventHandlerHolder
        {
            public EventHandlerHolder()
            {
            }

            public void Fire(object sender, EventTagArgs e)
            {
                if (this.Handlers != null)
                {
                    this.Handlers(sender, e);
                }
            }

            public event EventHandler Handlers;
        }

        /// <summary>
        /// イベントリスナ
        /// </summary>
        private Dictionary<string, EventHandlerHolder> m_handlers;

        /// <summary>
        /// Constructor
        /// </summary>
        public EventManager()
        {
            m_handlers = new Dictionary<string,EventHandlerHolder>();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        /// <param name="name"></param>
        public void CreateEvent(string name)
        {
            if (m_handlers.ContainsKey(name))
            {
                return;
            }

            try
            {
                m_handlers.Add(name, new EventHandlerHolder());
            }
            catch (ArgumentException /*e*/)
            {
                //Already exists
            }
        }

        /// <summary>
        /// イベント削除
        /// </summary>
        /// <param name="name"></param>
        public void DeleteEvent(string name)
        {
            if (m_handlers.ContainsKey(name) == false)
            {
                return;
            }

            try
            {
                m_handlers.Remove(name);
            }
            catch { }
        }

        /// <summary>
        /// ハンドラ設定
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handler"></param>
        public void SetHandler(string name, EventHandler handler)
        {
            if (m_handlers.ContainsKey(name) == false)
            {
                CreateEvent(name);
            }

            try
            {
                EventHandlerHolder holder = m_handlers[name];
                holder.Handlers += handler;
            }
            catch (ArgumentException /* e */)
            {
                // this key is not exists
                CreateEvent(name);
            }
            catch { }
        }

        /// <summary>
        /// ハンドラ削除
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handler"></param>
        public void RemoveHandler(string name, EventHandler handler)
        {
            if (m_handlers.ContainsKey(name) == false)
            {
                return;
            }

            try
            {
                EventHandlerHolder holder = m_handlers[name];
                holder.Handlers -= handler;
            }
            catch { }
        }

        /// <summary>
        /// イベント起動
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Fire(string name, object sender, EventTagArgs e)
        {
            if (m_handlers.ContainsKey(name) == false)
            {
                return;
            }

            try
            {
                EventHandlerHolder holder = m_handlers[name];
                holder.Fire(sender, e);
            }
            catch { }
        }

        /// <summary>
        /// イベント起動
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Fire(string name, object sender, object tag)
        {
            if (m_handlers.ContainsKey(name) == false)
            {
                return;
            }

            try
            {
                EventHandlerHolder holder = m_handlers[name];
                holder.Fire(sender, new EventTagArgs(tag));
            }
            catch { }
        }
    }
}
