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
using System.Drawing;
using System.ComponentModel;

namespace libpixy.net.Exif
{
    /// <summary>
    /// 
    /// </summary>
    public class ExifFormat
    {
        /// <summary>
        /// 
        /// </summary>
        public class NotConvertException : ApplicationException
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string ParseAsciiString(System.Drawing.Imaging.PropertyItem prop)
        {
            if (prop.Type == 2)
            {
                string val = System.Text.Encoding.ASCII.GetString(prop.Value);
                val = val.Trim(new char[] { '\0' });
                return val;
            }

            throw new NotConvertException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static ushort ParseUShort(System.Drawing.Imaging.PropertyItem prop)
        {
            if (prop.Type == 3)
            {
                return BitConverter.ToUInt16(prop.Value, 0);
            }

            throw new NotConvertException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static uint ParseULong(System.Drawing.Imaging.PropertyItem prop)
        {
            if (prop.Type == 4)
            {
                return BitConverter.ToUInt32(prop.Value, 0);
            }

            throw new NotConvertException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static uint ParseUInt(System.Drawing.Imaging.PropertyItem prop)
        {
            if (prop.Type == 3)
            {
                return BitConverter.ToUInt16(prop.Value, 0);
            }

            if (prop.Type == 4)
            {
                return BitConverter.ToUInt32(prop.Value, 0);
            }

            throw new NotConvertException();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="num"></param>
        /// <param name="denom"></param>
        public static void ParseUnsignedRational(
            System.Drawing.Imaging.PropertyItem prop,
            ref uint num,
            ref uint denom,
            int index)
        {
            if (prop.Type == 5)
            {
                num = BitConverter.ToUInt32(prop.Value, index*8+0);
                denom = BitConverter.ToUInt32(prop.Value, index*8+4);
            }
            else
            {
                throw new NotConvertException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="num"></param>
        /// <param name="denom"></param>
        public static void ParseUnsignedRational(
            System.Drawing.Imaging.PropertyItem prop,
            ref int num,
            ref int denom,
            int index)
        {
            if (prop.Type == 5)
            {
                num = (int)BitConverter.ToUInt32(prop.Value, index * 8 + 0);
                denom = (int)BitConverter.ToUInt32(prop.Value, index * 8 + 4);
            }
            else
            {
                throw new NotConvertException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static uint ParseUnsignedRationalEval(
            System.Drawing.Imaging.PropertyItem prop, int index)
        {
            uint num = 0, denom = 0;
            ParseUnsignedRational(prop, ref num, ref denom, index);
            return num / denom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static float ParseUnsignedRationalEvalF(
            System.Drawing.Imaging.PropertyItem prop, int index)
        {
            uint num = 0, denom = 0;
            ParseUnsignedRational(prop, ref num, ref denom, index);
            return (float)num / (float)denom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ParseUndefined(System.Drawing.Imaging.PropertyItem prop, int length)
        {
            if (prop.Type == 7)
            {
                if (length < 0)
                {
                    string val = System.Text.Encoding.ASCII.GetString(prop.Value, 0, prop.Value.Length);
                    val = val.Trim(new char[] { '\0' });
                    return val;
                }
                else
                {
                    string val = System.Text.Encoding.ASCII.GetString(prop.Value, 0, length);
                    val = val.Trim(new char[] { '\0' });
                    return val;
                }
            }

            throw new NotConvertException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] ParseUndefinedAsByte(System.Drawing.Imaging.PropertyItem prop, int length)
        {
            if (prop.Type == 7)
            {
                if (length < 0)
                {
                    byte[] bytes = new byte[prop.Value.Length];
                    prop.Value.CopyTo(bytes, 0);
                    return bytes;
                }
                else
                {
                    byte[] bytes = new byte[length];
                    for (int i=0; i<length; ++i)
                    {
                        bytes[i] = prop.Value[i];
                    }
                    return bytes;
                }
            }

            throw new NotConvertException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool CompareBytes(byte[] a, byte[] b, int length)
        {
            if (a.Length < length)
            {
                return false;
            }

            if (b.Length < length)
            {
                return false;
            }

            for (int i = 0; i < length; ++i)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ParseComment(System.Drawing.Imaging.PropertyItem prop)
        {
            if (prop.Type == 7)
            {
                if (prop.Value.Length < 8)
                {
                    return "";
                }

                byte[] CodeAscii = { 0x41, 0x53, 0x43, 0x49, 0x49, 0x00, 0x00, 0x00 };
                byte[] CodeJis = { 0x4a, 0x49, 0x53, 0x00, 0x00, 0x00, 0x00, 0x00 };
                byte[] CodeUnicode = { 0x55, 0x4e, 0x49, 0x43, 0x4f, 0x44, 0x45, 0x00 };
                byte[] CodeUndefined = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                if (CompareBytes(CodeAscii, prop.Value, 8))
                {
                    byte[] bytes = new byte[prop.Value.Length - 8];
                    prop.Value.CopyTo(bytes, 8);
                    string val = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    val = val.Trim(new char[] { '\0' });
                    return val;
                }

                if (CompareBytes(CodeJis, prop.Value, 8))
                {
                    byte[] bytes = new byte[prop.Value.Length - 8];
                    prop.Value.CopyTo(bytes, 8);
                    Encoding enc = Encoding.GetEncoding("iso-2022-jp");
                    string val = enc.GetString(bytes, 0, bytes.Length);
                    val = val.Trim(new char[] { '\0' });
                    return val;
                }

                if (CompareBytes(CodeUnicode, prop.Value, 8))
                {
                    byte[] bytes = new byte[prop.Value.Length - 8];
                    prop.Value.CopyTo(bytes, 8);
                    string val = System.Text.Encoding.Unicode.GetString(bytes, 0, bytes.Length);
                    val = val.Trim(new char[] { '\0' });
                    return val;
                }

                // Undefined = SJIS

                if (CompareBytes(CodeUndefined, prop.Value, 8))
                {
                    byte[] bytes = new byte[prop.Value.Length - 8];
                    prop.Value.CopyTo(bytes, 8);
                    Encoding enc = Encoding.GetEncoding("Shift_JIS");
                    string val = enc.GetString(bytes, 0, bytes.Length);
                    val = val.Trim(new char[] { '\0' });
                    return val;
                }

#if false
                // ? = ascii
                {
                    string val = System.Text.Encoding.ASCII.GetString(prop.Value, 0, prop.Value.Length);
                    val = val.Trim(new char[] { '\0' });
                    return val;
                }
#endif
            }
            else if (prop.Type == 2)
            {//Ascii string
                return ParseAsciiString(prop);
            }

            throw new NotConvertException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static short ParseShort(System.Drawing.Imaging.PropertyItem prop)
        {
            if (prop.Type == 8)
            {
                return BitConverter.ToInt16(prop.Value, 0);
            }

            throw new NotConvertException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        private static int ParseLong(System.Drawing.Imaging.PropertyItem prop)
        {
            if (prop.Type == 9)
            {
                return BitConverter.ToInt32(prop.Value, 0);
            }

            throw new NotConvertException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="num"></param>
        /// <param name="denom"></param>
        public static void ParseSignedRational(
            System.Drawing.Imaging.PropertyItem prop,
            ref int num,
            ref int denom,
            int index)
        {
            if (prop.Type == 10)
            {
                num = BitConverter.ToInt32(prop.Value, index*8+0);
                denom = BitConverter.ToInt32(prop.Value, index*8+4);
            }
            else
            {
                throw new NotConvertException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        public static int ParseSignedRationalEval(
            System.Drawing.Imaging.PropertyItem prop, int index)
        {
            int num = 0, denom = 0;
            ParseSignedRational(prop, ref num, ref denom, index);
            return num / denom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        public static float ParseSignedRationalEvalF(
            System.Drawing.Imaging.PropertyItem prop, int index)
        {
            int num = 0, denom = 0;
            ParseSignedRational(prop, ref num, ref denom, index);
            return (float)num / (float)denom;
        }
    }

    namespace Tag
    {
        public enum Orientation
        {
            TOP_LEFT = 1,
            TOP_RIGHT,
            BOTTOM_LEFT,
            BOTTOM_RIGHT,
            LEFT_TOP,
            RIGHT_TOP,
            LEFT_BOTTOM,
            RIGHT_BOTTOM
        }

        public enum ResolutionUnit
        {
            NOUNIT = 1,
            INCH,
            CENTIMETER
        }

        public enum YCbCrPositioning
        {
            UNKNOWN = 0,
            CENTERED,
            COSITED
        }

        public enum ComponentsConfiguration
        {
            UNKNOWN = 0,
            RGB,
            YCbCr
        }

        public enum MeteringMode
        {
            UNKNOWN = 0,
            AVERAGE,
            CWA, // CENTER_WEIGHTED_AVERAGE
            SPOT,
            MULTI_SPOT,
            PATTERN,
            PARTIAL,
            RESERVE,
            OTHER = 255
        }

        public enum LightSource
        {
            UNKNOWN = 0,
            DAYLIGHT = 1,
            FLUORESCENT = 2,
            TUNGSTEN = 3,
            FLASH = 4,
            FINE_WEATHER = 9,
            CLOUDY_WEATHER = 10,
            SHADE = 11,
            DAYLIGHT_FLUORESCENT = 12,
            DAY_WHITE_FLUORESCENT = 13,
            COOL_WHITE_FLUORESCENT = 14,
            WHITE_FLUORESCENT = 15,
            STANDARD_LIGHT_A = 17,
            STANDARD_LIGHT_B = 18,
            STANDARD_LIGHT_C = 19,
            D55 = 20,
            D65 = 21,
            D75 = 22,
            D50 = 23,
            ISO_STUDIO_TUNGSTEN = 24,
            OTHER_LIGHT_SOURCE = 255
        }

        public enum FlashFlash
        {
            NO_FLASH = 0,
            FLASH
        }

        public enum FlashReturn
        {
            NO_RETURN = 0,
            RESERVE,
            NOT_DETECTED,
            DETECTED
        }

        public enum FlashFlashMode
        {
            UNKNOWN = 0,
            COMPULSORY_FLASH_FIRING,
            COMPULSORY_FLASH_SUPPRESSION,
            AUTO_MODE
        }

        public enum FlashFlashFunction
        {
            PRESENT = 0,
            NO_PRESENT
        }

        public enum FlashRedEye
        {
            NO_REDEYE = 0,
            REDUCTION
        }

        public struct FlashParam
        {
            public FlashFlash Flash;
            public FlashReturn Return;
            public FlashFlashMode FlashMode;
            public FlashFlashFunction FlashFunction;
            public FlashRedEye RedEye;
        }

        public enum ExposureProgram
        {
            NOT_DEFINED = 0,
            MANUAL,
            NORMAL_PROGRAM,
            APERTURE_PRIORITY,
            SHUTTER_PRIORITY,
            CREATIVE_PROGRAM,
            ACTION_PROGRAM,
            PORTRAIT_MODE,
            LANDSCAPE_MODE
        }

        public enum SensingMethod
        {
            NOT_DEFINED = 1,
            ONE_CHIP_COLOR_AREA_SENSOR,
            TWO_CHIP_COLOR_AREA_SENSOR,
            THREE_CHIP_COLOR_AREA_SENSOR,
            COLOR_SEQUENCIAL_AREA_SENSOR,
            TRILINEAR_SENSOR,
            COLOR_SEQUENCIAL_LINEAR_SENSOR
        }

        public enum ColorSpace
        {
            sRGB = 1,
            UNCALIBRATED = 0xffff
        }

        public enum FocalPlaneResolutionUnit
        {
            NOUNIT = 1,
            INCH,
            CENTIMETER
        }

        public struct Rational
        {
            public int Num;
            public int Denom;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExifTags
    {
        public class Param<T>
        {
            public T Value;
        }

        #region Properties

        public Param<string> ImageDescription = null;
        public Param<string> Make = null;
        public Param<string> Model = null;
        public Param<Tag.Orientation> Orientation = null;
        public Param<Tag.Rational> XResolution = null;
        public Param<Tag.Rational> YResolution = null;
        public Param<Tag.ResolutionUnit> ResolutionUnit = null;
        public Param<string> Software = null;
        public Param<string> DateTime = null;
        public Param<string> Artist = null;
        public Param<Tag.Rational[]> WhitePoint = null;
        public Param<Tag.Rational[]> PrimaryChromaticities = null;
        public Param<Tag.Rational[]> YCbCrCoefficients = null;
        public Param<Tag.YCbCrPositioning> YCbCrPositioning = null;
        public Param<Tag.Rational[]> ReferenceBlackWhite = null;
        public Param<string> Copyright = null;
        public Param<uint> ExifIFDPointer = null;

        public Param<Tag.Rational> ExposureTime = null;
        public Param<Tag.Rational> FNumber = null;
        public Param<Tag.ExposureProgram> ExposureProgram = null;
        public Param<uint> ISOSpeedRatings = null;
        public Param<string> ExifVersion = null;
        public Param<string> DateTimeOriginal = null;
        public Param<string> DateTimeDigitized = null;
        public Param<Tag.ComponentsConfiguration> ComponentsConfiguration = null;
        public Param<Tag.Rational> CompressedBitsPerPixel = null;
        public Param<Tag.Rational> ShutterSpeedValue = null;
        public Param<Tag.Rational> ApertureValue = null;
        public Param<Tag.Rational> BrightnessValue = null;
        public Param<Tag.Rational> ExposureBiasValue = null;
        public Param<Tag.Rational> MaxApertureValue = null;
        public Param<Tag.Rational> SubjectDistance = null;
        public Param<Tag.MeteringMode> MeteringMode = null;
        public Param<Tag.LightSource> LightSource = null;
        public Param<Tag.FlashParam> Flash = null;
        public Param<Tag.Rational> FocalLength = null;
        public Param<string> MakerNote = null;
        public Param<string> UserComment = null;
        public Param<string> SubsecTime = null;
        public Param<string> SubsecTimeOriginal = null;
        public Param<string> SubsecTimeDigitized = null;
        public Param<string> FlashPixVersion = null;
        public Param<Tag.ColorSpace> ColorSpace = null;
        public Param<uint> ExifImageWidth = null;
        public Param<uint> ExifImageHeight = null;
        public Param<string> RelatedSoundFile = null;
        public Param<uint> InteroperabilityIFDPointer = null;
        public Param<Tag.Rational> FocalPlaneXResolution = null;
        public Param<Tag.Rational> FocalPlaneYResolution = null;
        public Param<Tag.FocalPlaneResolutionUnit> FocalPlaneResolutionUnit = null;
        public Param<Tag.Rational> ExposureIndex = null;
        public Param<Tag.SensingMethod> SensingMethod = null;
        public Param<uint> FileSource = null;
        public Param<uint> SceneType = null;
        public Param<string> CFAPattern = null;

        #endregion

        #region Ctor/Dtor

        /// <summary>
        /// 
        /// </summary>
        public ExifTags()
        {
        }

        #endregion

        #region Text

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string RationalAsText(Tag.Rational value)
        {
            if (value.Denom != 0)
            {
                return string.Format("{0}", value.Num / value.Denom);
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string RationalFAsText(Tag.Rational value)
        {
            if (value.Denom != 0)
            {
                float valueF = (float)value.Num / (float)value.Denom;
                return valueF.ToString("0.##");
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string RationalValuesAsText(Tag.Rational[] values)
        {
            StringBuilder s = new StringBuilder();
            foreach (libpixy.net.Exif.Tag.Rational value in values)
            {
                if (s.Length > 0)
                {
                    s.Append(", ");
                }

                s.Append(RationalFAsText(value));
            }

            return s.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string OrientationAsText()
        {
            if (this.Orientation == null)
            {
                return "";
            }

            switch (this.Orientation.Value)
            {
                case Tag.Orientation.TOP_LEFT: return "Top Left";
                case Tag.Orientation.TOP_RIGHT: return "Top Right";
                case Tag.Orientation.BOTTOM_RIGHT: return "Bottom Right";
                case Tag.Orientation.BOTTOM_LEFT: return "Bottom Left";
                case Tag.Orientation.LEFT_TOP: return "Left Top";
                case Tag.Orientation.LEFT_BOTTOM: return "Left Bottom";
                case Tag.Orientation.RIGHT_TOP: return "Right Top";
                case Tag.Orientation.RIGHT_BOTTOM: return "Right Bottom";
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string XResolutionAsText()
        {
            if (this.XResolution == null)
            {
                return "";
            }
            else
            {
                return RationalAsText(this.XResolution.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string YResolutionAsText()
        {
            if (this.YResolution == null)
            {
                return "";
            }
            else
            {
                return RationalAsText(this.YResolution.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ResolutionUnitAsText()
        {
            if (this.ResolutionUnit == null)
            {
                return "";
            }

            switch (this.ResolutionUnit.Value)
            {
                case libpixy.net.Exif.Tag.ResolutionUnit.NOUNIT:
                    return "";

                case libpixy.net.Exif.Tag.ResolutionUnit.INCH:
                    return "インチ(inch)";

                case libpixy.net.Exif.Tag.ResolutionUnit.CENTIMETER:
                    return "センチメートル(cm)";
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string WhitePointAsText()
        {
            if (this.WhitePoint == null)
            {
                return "";
            }

            return RationalValuesAsText(this.WhitePoint.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string PrimaryChromaticitiesAsText()
        {
            if (this.PrimaryChromaticities == null)
            {
                return "";
            }

            return RationalValuesAsText(this.PrimaryChromaticities.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string YCbCrCoefficientsAsText()
        {
            if (this.YCbCrCoefficients == null)
            {
                return "";
            }

            return RationalValuesAsText(this.YCbCrCoefficients.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string YCbCrPositioningAsText()
        {
            if (this.YCbCrPositioning == null)
            {
                return "";
            }

            switch (this.YCbCrPositioning.Value)
            {
                case libpixy.net.Exif.Tag.YCbCrPositioning.UNKNOWN:
                    return "";

                case libpixy.net.Exif.Tag.YCbCrPositioning.CENTERED:
                    return "中心(Centered)";

                case libpixy.net.Exif.Tag.YCbCrPositioning.COSITED:
                    return "一致(Cosited)";
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReferenceBlackWhiteAsText()
        {
            if (this.ReferenceBlackWhite == null)
            {
                return "";
            }

            return RationalValuesAsText(this.ReferenceBlackWhite.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ExposureTimeAsText()
        {
            if (this.ExposureTime == null)
            {
                return "";
            }

            if (this.ExposureTime.Value.Denom != 0)
            {
                return string.Format("{0}/{1} seconds", this.ExposureTime.Value.Num, this.ExposureTime.Value.Denom);
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string FNumberAsText()
        {
            if (this.FNumber == null)
            {
                return "";
            }
            else
            {
                return RationalFAsText(this.FNumber.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ExposureProgramAsText()
        {
            if (this.ExposureProgram == null)
            {
                return "";
            }

            switch (this.ExposureProgram.Value)
            {
                case libpixy.net.Exif.Tag.ExposureProgram.NOT_DEFINED:
                    return "未定義";
                    
                case libpixy.net.Exif.Tag.ExposureProgram.MANUAL:
                    return "マニュアル";

                case libpixy.net.Exif.Tag.ExposureProgram.NORMAL_PROGRAM:
                    return "ノーマルプログラム";

                case libpixy.net.Exif.Tag.ExposureProgram.APERTURE_PRIORITY:
                    return "露出優先";

                case libpixy.net.Exif.Tag.ExposureProgram.SHUTTER_PRIORITY:
                    return "シャッター優先";

                case libpixy.net.Exif.Tag.ExposureProgram.CREATIVE_PROGRAM:
                    return "create プログラム";

                case libpixy.net.Exif.Tag.ExposureProgram.ACTION_PROGRAM:
                    return "action プログラム";

                case libpixy.net.Exif.Tag.ExposureProgram.PORTRAIT_MODE:
                    return "ポートレイトモード";

                case libpixy.net.Exif.Tag.ExposureProgram.LANDSCAPE_MODE:
                    return "ランドスケープモード";
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ComponentsConfigurationAsText()
        {
            if (this.ComponentsConfiguration == null)
            {
                return "";
            }

            switch (this.ComponentsConfiguration.Value)
            {
                case libpixy.net.Exif.Tag.ComponentsConfiguration.RGB:
                    return "RGB";

                case libpixy.net.Exif.Tag.ComponentsConfiguration.YCbCr:
                    return "YCbCr";
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ISOSpeedRatingsAsText()
        {
            if (this.ISOSpeedRatings == null)
            {
                return "";
            }
            else
            {
                return string.Format("{0}", this.ISOSpeedRatings.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CompressedBitsPerPixelAsText()
        {
            if (this.CompressedBitsPerPixel == null)
            {
                return "";
            }
            else
            {
                return RationalAsText(this.CompressedBitsPerPixel.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ShutterSpeedValueAsText()
        {
            if (this.ShutterSpeedValue == null)
            {
                return "";
            }

            if (this.ShutterSpeedValue.Value.Denom != 0)
            {
                return string.Format("1/{0} seconds",
                    GetShutterSpeedValue().ToString("#"));
            }

            return "";
        }

        /// <summary>
        /// "1/n seconds" の n を返す
        /// </summary>
        /// <returns></returns>
        public double GetShutterSpeedValue()
        {
            if (this.ShutterSpeedValue == null)
            {
                return 0.0;
            }

            if (this.ShutterSpeedValue.Value.Denom != 0)
            {
                return Math.Pow(2.0, (double)this.ShutterSpeedValue.Value.Num / (double)this.ShutterSpeedValue.Value.Denom);
            }

            return 0.0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ApertureValueAsText()
        {
            if (this.ApertureValue == null)
            {
                return "";
            }

            if (this.ApertureValue.Value.Denom == 0)
            {
                return "";
            }

            return "F " + GetApertureValue().ToString("0.##");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetApertureValue()
        {
            if (this.ApertureValue == null)
            {
                return 0.0;
            }

            if (this.ApertureValue.Value.Denom == 0)
            {
                return 0.0;
            }

            return Math.Pow(Math.Sqrt(2.0), (double)this.ApertureValue.Value.Num / (double)this.ApertureValue.Value.Denom);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string BrightnessValueAsText()
        {
            if (this.BrightnessValue == null)
            {
                return "";
            }
            else
            {
                return RationalFAsText(this.BrightnessValue.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ExposureBiasValueAsText()
        {
            if (this.ExposureBiasValue == null)
            {
                return "";
            }
            else
            {
                return RationalFAsText(this.ExposureBiasValue.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string MaxApertureValueAsText()
        {
            if (this.MaxApertureValue == null)
            {
                return "";
            }

            if (this.MaxApertureValue.Value.Denom == 0)
            {
                return "";
            }

            return "F " + GetMaxApertureValue().ToString("0.##");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetMaxApertureValue()
        {
            if (this.MaxApertureValue == null)
            {
                return 0.0;
            }

            if (this.MaxApertureValue.Value.Denom == 0)
            {
                return 0.0;
            }

            return Math.Pow(Math.Sqrt(2.0), (double)this.MaxApertureValue.Value.Num / (double)this.MaxApertureValue.Value.Denom);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SubjectDistanceAsText()
        {
            if (this.SubjectDistance == null)
            {
                return "";
            }
            else
            {
                return RationalAsText(this.SubjectDistance.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string MeteringModeAsText()
        {
            if (this.MeteringMode == null)
            {
                return "";
            }

            switch (this.MeteringMode.Value)
            {
                case libpixy.net.Exif.Tag.MeteringMode.UNKNOWN:
                    return "不明(Unknown)";

                case libpixy.net.Exif.Tag.MeteringMode.AVERAGE:
                    return "平均(Average)";

                case libpixy.net.Exif.Tag.MeteringMode.CWA:
                    return "中央重点(CenterWeightedAverage)";

                case libpixy.net.Exif.Tag.MeteringMode.SPOT:
                    return "スポット(Spot)";

                case libpixy.net.Exif.Tag.MeteringMode.MULTI_SPOT:
                    return "マルチスポット(MultiSpot)";

                case libpixy.net.Exif.Tag.MeteringMode.PATTERN:
                    return "分割測光(Pattern)";

                case libpixy.net.Exif.Tag.MeteringMode.PARTIAL:
                    return "部分測光(Partial)";

                case libpixy.net.Exif.Tag.MeteringMode.RESERVE:
                    return "予約(Reserve)";

                case libpixy.net.Exif.Tag.MeteringMode.OTHER:
                    return "その他(Other)";
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string LightSourceAsText()
        {
            if (this.LightSource == null)
            {
                return "";
            }

            switch (this.LightSource.Value)
            {
                case libpixy.net.Exif.Tag.LightSource.UNKNOWN:
                    return "不明(Unknown)";

                case libpixy.net.Exif.Tag.LightSource.DAYLIGHT:
                    return "昼光(Daylight)";

                case libpixy.net.Exif.Tag.LightSource.FLUORESCENT:
                    return "蛍光灯(Fluorescent)";
                    
                case libpixy.net.Exif.Tag.LightSource.TUNGSTEN:
                    return "タングステン(Tungsten)";

                case libpixy.net.Exif.Tag.LightSource.FLASH:
                    return "フラッシュ(Flash)";

                case libpixy.net.Exif.Tag.LightSource.FINE_WEATHER:
                    return "晴れ(Fine weather)";

                case libpixy.net.Exif.Tag.LightSource.CLOUDY_WEATHER:
                    return "曇り(Cloudy weather)";

                case libpixy.net.Exif.Tag.LightSource.SHADE:
                    return "日陰(Shade)";

                case libpixy.net.Exif.Tag.LightSource.DAYLIGHT_FLUORESCENT:
                    return "蛍光灯(D 5700-7100K)";

                case libpixy.net.Exif.Tag.LightSource.DAY_WHITE_FLUORESCENT:
                    return "蛍光灯(N 4600-5400K)";

                case libpixy.net.Exif.Tag.LightSource.COOL_WHITE_FLUORESCENT:
                    return "蛍光灯(W 3900-4500K)";

                case libpixy.net.Exif.Tag.LightSource.WHITE_FLUORESCENT:
                    return "蛍光灯(WW 3200-3700K)";

                case libpixy.net.Exif.Tag.LightSource.STANDARD_LIGHT_A:
                    return "標準光A";

                case libpixy.net.Exif.Tag.LightSource.STANDARD_LIGHT_B:
                    return "標準光B";

                case libpixy.net.Exif.Tag.LightSource.STANDARD_LIGHT_C:
                    return "標準光C";

                case libpixy.net.Exif.Tag.LightSource.D55:
                    return "D55";

                case libpixy.net.Exif.Tag.LightSource.D65:
                    return "D65";

                case libpixy.net.Exif.Tag.LightSource.D75:
                    return "D75";

                case libpixy.net.Exif.Tag.LightSource.D50:
                    return "D50";
                    
                case libpixy.net.Exif.Tag.LightSource.ISO_STUDIO_TUNGSTEN:
                    return "ISOスタジオ タングステン(ISO studio tungsten)";

                case libpixy.net.Exif.Tag.LightSource.OTHER_LIGHT_SOURCE:
                    return "その他(Other)";
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        public static int[] FlashHexTable = {
            #region Data
            0x0000,
            0x0001,
            0x0005,
            0x0007,
            0x0009,
            0x000D,
            0x000F,
            0x0010,
            0x0018,
            0x0019,
            0x001D,
            0x001F,
            0x0020,
            0x0041,
            0x0045,
            0x0047,
            0x0049,
            0x004D,
            0x004F,
            0x0059,
            0x005D,
            0x005F
            #endregion
        };

        /// <summary>
        /// 
        /// </summary>
        public static string[] FlashTextTable = {
            #region Data
            /* 0x0000 */"Flash did not fire",
            /* 0x0001 */"Flash fired",
            /* 0x0005 */"Strobe return light not detected",
            /* 0x0007 */"Strobe return light detected",
            /* 0x0009 */"Flash fired, compulsory flash mode",
            /* 0x000D */"Flash fired, compulsory flash mode, return light not detected",
            /* 0x000F */"Flash fired, compulsory flash mode, return light detected",
            /* 0x0010 */"Flash did not fire, compulsory flash mode",
            /* 0x0018 */"Flash did not fire, auto mode",
            /* 0x0019 */"Flash fired, auto mode",
            /* 0x001D */"Flash fired, auto mode, return light not detected",
            /* 0x001F */"Flash fired, auto mode, return light detected",
            /* 0x0020 */"No flash function",
            /* 0x0041 */"Flash fired, red-eye reduction mode",
            /* 0x0045 */"Flash fired, red-eye reduction mode, return light not detected",
            /* 0x0047 */"Flash fired, red-eye reduction mode, return light detected",
            /* 0x0049 */"Flash fired, compulsory flash mode, red-eye reduction mode",
            /* 0x004D */"Flash fired, compulsory flash mode, red-eye reduction mode, return light not detected",
            /* 0x004F */"Flash fired, compulsory flash mode, red-eye reduction mode, return light detected",
            /* 0x0059 */"Flash fired, auto mode, red-eye reduction mode",
            /* 0x005D */"Flash fired, auto mode, return light not detected, red-eye reduction mode",
            /* 0x005F */"Flash fired, auto mode, return light detected, red-eye reduction mode"
            #endregion
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetFlashBits(Tag.FlashParam data)
        {
            int bits = 0;
            switch (data.Flash)
            {
                case libpixy.net.Exif.Tag.FlashFlash.NO_FLASH:
                    break;

                case libpixy.net.Exif.Tag.FlashFlash.FLASH:
                    bits |= 0x1;
                    break;
            }

            switch (data.Return)
            {
                case libpixy.net.Exif.Tag.FlashReturn.NO_RETURN:
                    break;

                case libpixy.net.Exif.Tag.FlashReturn.RESERVE:
                    bits |= (0x1 << 1);
                    break;

                case libpixy.net.Exif.Tag.FlashReturn.NOT_DETECTED:
                    bits |= (0x2 << 1);
                    break;

                case libpixy.net.Exif.Tag.FlashReturn.DETECTED:
                    bits |= (0x3 << 1);
                    break;
            }

            switch (data.FlashMode)
            {
                case libpixy.net.Exif.Tag.FlashFlashMode.UNKNOWN:
                    break;

                case libpixy.net.Exif.Tag.FlashFlashMode.COMPULSORY_FLASH_FIRING:
                    bits |= (0x1 << 3);
                    break;

                case libpixy.net.Exif.Tag.FlashFlashMode.COMPULSORY_FLASH_SUPPRESSION:
                    bits |= (0x2 << 3);
                    break;

                case libpixy.net.Exif.Tag.FlashFlashMode.AUTO_MODE:
                    bits |= (0x3 << 3);
                    break;
            }

            switch (data.FlashFunction)
            {
                case libpixy.net.Exif.Tag.FlashFlashFunction.NO_PRESENT:
                    bits |= (0x1 << 5);
                    break;

                case libpixy.net.Exif.Tag.FlashFlashFunction.PRESENT:
                    break;
            }

            switch (data.RedEye)
            {
                case libpixy.net.Exif.Tag.FlashRedEye.NO_REDEYE:
                    break;

                case libpixy.net.Exif.Tag.FlashRedEye.REDUCTION:
                    bits |= (0x1 << 6);
                    break;
            }

            return bits;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string FlashAsText()
        {
            if (this.Flash == null)
            {
                return "";
            }

            int bits = GetFlashBits(this.Flash.Value);
            for (int i = 0; i < FlashHexTable.Length; ++i)
            {
                if (bits == FlashHexTable[i])
                {
                    return FlashTextTable[i];
                }
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string FocalLengthAsText()
        {
            if (this.FocalLength == null)
            {
                return "";
            }

            string str = RationalFAsText(this.FocalLength.Value);
            if (str.Length>0)
            {
                return str + " mm";
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ColorSpaceAsText()
        {
            if (this.ColorSpace == null)
            {
                return "";
            }

            switch (this.ColorSpace.Value)
            {
                case libpixy.net.Exif.Tag.ColorSpace.sRGB:
                    return "sRGB";

                case libpixy.net.Exif.Tag.ColorSpace.UNCALIBRATED:
                    return "Uncalibrated";
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ExifImageWidthAsText()
        {
            if (this.ExifImageWidth == null)
            {
                return "";
            }
            else
            {
                return string.Format("{0}", this.ExifImageWidth.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ExifImageHeightAsText()
        {
            if (this.ExifImageHeight == null)
            {
                return "";
            }
            else
            {
                return string.Format("{0}", this.ExifImageHeight.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string FocalPlaneXResolutionAsText()
        {
            if (this.FocalPlaneXResolution == null)
            {
                return "";
            }
            else
            {
                return RationalFAsText(this.FocalPlaneXResolution.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string FocalPlaneYResolutionAsText()
        {
            if (this.FocalPlaneYResolution == null)
            {
                return "";
            }
            else
            {
                return RationalFAsText(this.FocalPlaneYResolution.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string FocalPlaneResolutionUnitAsText()
        {
            if (this.FocalPlaneResolutionUnit == null)
            {
                return "";
            }

            switch (this.FocalPlaneResolutionUnit.Value)
            {
                case libpixy.net.Exif.Tag.FocalPlaneResolutionUnit.NOUNIT:
                    return "";

                case libpixy.net.Exif.Tag.FocalPlaneResolutionUnit.INCH:
                    return "インチ(inch)";

                case libpixy.net.Exif.Tag.FocalPlaneResolutionUnit.CENTIMETER:
                    return "センチメートル(cm)";
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ExposureIndexAsText()
        {
            if (this.ExposureIndex == null)
            {
                return "";
            }
            else
            {
                return RationalFAsText(this.ExposureIndex.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SensingMethodAsText()
        {
            if (this.SensingMethod == null)
            {
                return "";
            }
            
            switch (this.SensingMethod.Value)
            {
                case libpixy.net.Exif.Tag.SensingMethod.NOT_DEFINED:
                    return "単版カラーセンサー";

                case libpixy.net.Exif.Tag.SensingMethod.ONE_CHIP_COLOR_AREA_SENSOR:
                    return "２板カラーセンサー";

                case libpixy.net.Exif.Tag.SensingMethod.TWO_CHIP_COLOR_AREA_SENSOR:
                    return "３板カラーセンサー";

                case libpixy.net.Exif.Tag.SensingMethod.THREE_CHIP_COLOR_AREA_SENSOR:
                    return "色順次カラーセンサー";

                case libpixy.net.Exif.Tag.SensingMethod.COLOR_SEQUENCIAL_AREA_SENSOR:
                    return "３線リニアセンサー";

                case libpixy.net.Exif.Tag.SensingMethod.TRILINEAR_SENSOR:
                    return "色順次リニアセンサー";

                case libpixy.net.Exif.Tag.SensingMethod.COLOR_SEQUENCIAL_LINEAR_SENSOR:
                    return "";
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string FileSourceAsText()
        {
            if (this.FileSource == null)
            {
                return "";
            }

            return "0x" + this.FileSource.Value.ToString("X");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SceneTypeAsText()
        {
            if (this.SceneType == null)
            {
                return "";
            }

            return "0x" + this.SceneType.Value.ToString("X");
        }

        #endregion

        #region Time

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Param<DateTime> ToDateTime(Param<string> p)
        {
            if (p == null)
            {
                return null;
            }

            try
            {
                Param<DateTime> ret = new Param<DateTime>();
                ret.Value = System.DateTime.ParseExact(p.Value, "yyyy:MM:dd HH:mm:ss", null);
                return ret;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Param<DateTime> GetDateTime()
        {
            return ToDateTime(this.DateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Param<DateTime> GetDateTimeOriginal()
        {
            return ToDateTime(this.DateTimeOriginal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Param<DateTime> GetDateTimeDigitized()
        {
            return ToDateTime(this.DateTimeDigitized);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Param<DateTime> GetSubsecTime()
        {
            return ToDateTime(this.SubsecTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Param<DateTime> GetSubsecTimeOriginal()
        {
            return ToDateTime(this.SubsecTimeOriginal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Param<DateTime> GetSubsecTimeDigitized()
        {
            return ToDateTime(this.SubsecTimeDigitized);
        }

        #endregion
    }
}
