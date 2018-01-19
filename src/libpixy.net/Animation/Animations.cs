﻿/*
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

namespace libpixy.net.Animation
{
    /// <summary>
    /// アニメーションセット
    /// </summary>
    public class Animations : Tools.BaseObject, ICloneable
    {
        #region Fields
        #endregion Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<FCurve> Items = new List<FCurve>();

        #endregion Properties

        #region Constructor, Destructor

        /// <summary>
        /// Ctor
        /// </summary>
        public Animations()
            : base("anim", "anim")
        {
        }

        #endregion Constructor, Destructor

        #region Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public FCurve Find(string scriptName)
        {
            foreach (FCurve curve in this.Items)
            {
                if (curve.ScriptName == scriptName)
                {
                    return curve;
                }
            }

            return null;
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Animations clone = new Animations();
            foreach (FCurve curve in this.Items)
            {
                clone.Items.Add((FCurve)curve.Clone());
            }

            return clone;
        }

        #endregion ICloneable
    }
}
