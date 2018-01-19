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
    public class Random
    {
        private System.Random m_random = null;

        public Random()
        {
            m_random = new System.Random();
        }

        public Random(int seed)
        {
            m_random = new System.Random(seed);
        }

        public void SetSeed(int seed)
        {
            m_random = new System.Random(seed);
        }

        public int Next()
        {
            return m_random.Next();
        }

        public int Next(int max)
        {
            return m_random.Next(max);
        }

        public int Next(int min, int max)
        {
            if (min < max)
            {
                return m_random.Next(min, max);
            }
            else
            {
                return m_random.Next(max, min);
            }
        }

        public float NextFloat()
        {
            return (float)NextDouble();
        }

        public float NextFloat(float max)
        {
            return (float)NextDouble(max);
        }

        public float NextFloat(float min, float max)
        {
            return (float)NextDouble(min, max);
        }

        public double NextDouble()
        {
            return m_random.NextDouble();
        }

        public double NextDouble(double max)
        {
            return m_random.NextDouble() * max;
        }

        public double NextDouble(double min, double max)
        {
            double range = Math.Abs(max - min);
            if (min < max)
            {
                return m_random.NextDouble() * range + min;
            }
            else
            {
                return m_random.NextDouble() * range + max;
            }
        }
    }
}
