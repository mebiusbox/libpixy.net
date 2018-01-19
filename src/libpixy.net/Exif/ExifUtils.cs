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
using System.IO;

namespace libpixy.net.Exif
{
    public class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static libpixy.net.Exif.ExifTags GetProperty(libpixy.net.Shell.ShellItem item)
        {
            libpixy.net.Exif.ExifTags exifTags = new libpixy.net.Exif.ExifTags();
            libpixy.net.Shell.ShellStreamReader strmReader = new libpixy.net.Shell.ShellStreamReader(item, null, FileAccess.Read);
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(strmReader);
                if (image.PropertyItems.Length > 0)
                {
                    foreach (System.Drawing.Imaging.PropertyItem prop in image.PropertyItems)
                    {
                        AddExifTags(ref exifTags, prop);
                    }
                }
            }
            catch
            {
                return null;
            }

            return exifTags;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exifProp"></param>
        /// <param name="prop"></param>
        public static void AddExifTags(ref libpixy.net.Exif.ExifTags exifTags, System.Drawing.Imaging.PropertyItem prop)
        {
            switch (prop.Id)
            {
                case 0x010e:
                    exifTags.ImageDescription = new ExifTags.Param<string>();
                    exifTags.ImageDescription.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0x010f:
                    exifTags.Make = new ExifTags.Param<string>();
                    exifTags.Make.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0x0110:
                    exifTags.Model = new ExifTags.Param<string>();
                    exifTags.Model.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0x0112:
                    exifTags.Orientation = new ExifTags.Param<libpixy.net.Exif.Tag.Orientation>();
                    exifTags.Orientation.Value = (libpixy.net.Exif.Tag.Orientation)ExifFormat.ParseUShort(prop);
                    break;

                case 0x011a:
                    exifTags.XResolution = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.XResolution.Value.Num,
                        ref exifTags.XResolution.Value.Denom,
                        0);
                    break;

                case 0x011b:
                    exifTags.YResolution = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.YResolution.Value.Num,
                        ref exifTags.YResolution.Value.Denom,
                        0);
                    break;

                case 0x0128:
                    exifTags.ResolutionUnit = new ExifTags.Param<libpixy.net.Exif.Tag.ResolutionUnit>();
                    exifTags.ResolutionUnit.Value = (libpixy.net.Exif.Tag.ResolutionUnit)ExifFormat.ParseUShort(prop);
                    break;

                case 0x0131:
                    exifTags.Software = new ExifTags.Param<string>();
                    exifTags.Software.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0x0132:
                    exifTags.DateTime = new ExifTags.Param<string>();
                    exifTags.DateTime.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0x013b:
                    exifTags.Artist = new ExifTags.Param<string>();
                    exifTags.Artist.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0x013e:
                    exifTags.WhitePoint = new ExifTags.Param<libpixy.net.Exif.Tag.Rational[]>();
                    exifTags.WhitePoint.Value = new libpixy.net.Exif.Tag.Rational[2];
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.WhitePoint.Value[0].Num,
                        ref exifTags.WhitePoint.Value[0].Denom,
                        0);
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.WhitePoint.Value[1].Num,
                        ref exifTags.WhitePoint.Value[1].Denom,
                        1);
                    break;

                case 0x013f:
                    exifTags.PrimaryChromaticities = new ExifTags.Param<libpixy.net.Exif.Tag.Rational[]>();
                    exifTags.PrimaryChromaticities.Value = new libpixy.net.Exif.Tag.Rational[6];
                    for (int i = 0; i < 6; ++i)
                    {
                        ExifFormat.ParseUnsignedRational(
                            prop,
                            ref exifTags.PrimaryChromaticities.Value[i].Num,
                            ref exifTags.PrimaryChromaticities.Value[i].Denom,
                            i);
                    }
                    break;

                case 0x0211:
                    exifTags.YCbCrCoefficients = new ExifTags.Param<libpixy.net.Exif.Tag.Rational[]>();
                    exifTags.YCbCrCoefficients.Value = new libpixy.net.Exif.Tag.Rational[3];
                    for (int i = 0; i < 3; ++i)
                    {
                        ExifFormat.ParseUnsignedRational(
                            prop,
                            ref exifTags.YCbCrCoefficients.Value[i].Num,
                            ref exifTags.YCbCrCoefficients.Value[i].Denom,
                            i);
                    }
                    break;

                case 0x0213:
                    exifTags.YCbCrPositioning = new ExifTags.Param<libpixy.net.Exif.Tag.YCbCrPositioning>();
                    exifTags.YCbCrPositioning.Value = (libpixy.net.Exif.Tag.YCbCrPositioning)ExifFormat.ParseUShort(prop);
                    break;

                case 0x0214:
                    exifTags.ReferenceBlackWhite = new ExifTags.Param<libpixy.net.Exif.Tag.Rational[]>();
                    exifTags.ReferenceBlackWhite.Value = new libpixy.net.Exif.Tag.Rational[6];
                    for (int i = 0; i < 6; ++i)
                    {
                        ExifFormat.ParseUnsignedRational(
                            prop,
                            ref exifTags.ReferenceBlackWhite.Value[i].Num,
                            ref exifTags.ReferenceBlackWhite.Value[i].Denom,
                            i);
                    }
                    break;

                case 0x8298:
                    exifTags.Copyright = new ExifTags.Param<string>();
                    exifTags.Copyright.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0x8769:
                    exifTags.ExifIFDPointer = new ExifTags.Param<uint>();
                    exifTags.ExifIFDPointer.Value = ExifFormat.ParseULong(prop);
                    break;

                case 0x829a:
                    exifTags.ExposureTime = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.ExposureTime.Value.Num,
                        ref exifTags.ExposureTime.Value.Denom,
                        0);
                    break;

                case 0x829d:
                    exifTags.FNumber = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.FNumber.Value.Num,
                        ref exifTags.FNumber.Value.Denom,
                        0);
                    break;

                case 0x8822:
                    exifTags.ExposureProgram = new ExifTags.Param<libpixy.net.Exif.Tag.ExposureProgram>();
                    exifTags.ExposureProgram.Value = (libpixy.net.Exif.Tag.ExposureProgram)ExifFormat.ParseUShort(prop);
                    break;

                case 0x8827:
                    exifTags.ISOSpeedRatings = new ExifTags.Param<uint>();
                    exifTags.ISOSpeedRatings.Value = ExifFormat.ParseUShort(prop);
                    break;

                case 0x9000:
                    exifTags.ExifVersion = new ExifTags.Param<string>();
                    exifTags.ExifVersion.Value = ExifFormat.ParseUndefined(prop, 4);
                    break;

                case 0x9003:
                    exifTags.DateTimeOriginal = new ExifTags.Param<string>();
                    exifTags.DateTimeOriginal.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0x9004:
                    exifTags.DateTimeDigitized = new ExifTags.Param<string>();
                    exifTags.DateTimeDigitized.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0x9101:
                    {
                        exifTags.ComponentsConfiguration = new ExifTags.Param<libpixy.net.Exif.Tag.ComponentsConfiguration>();
                        if (prop.Value[0] == 0x04 && prop.Value[1] == 0x05 && prop.Value[2] == 0x06 && prop.Value[3] == 0x00)
                        {
                            exifTags.ComponentsConfiguration.Value = libpixy.net.Exif.Tag.ComponentsConfiguration.RGB;
                        }
                        else if (prop.Value[0] == 0x01 && prop.Value[1] == 0x02 && prop.Value[2] == 0x03 && prop.Value[3] == 0x00)
                        {
                            exifTags.ComponentsConfiguration.Value = libpixy.net.Exif.Tag.ComponentsConfiguration.YCbCr;
                        }
                        else
                        {
                            exifTags.ComponentsConfiguration.Value = libpixy.net.Exif.Tag.ComponentsConfiguration.UNKNOWN;
                        }
                    }
                    break;

                case 0x9102:
                    exifTags.CompressedBitsPerPixel = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.CompressedBitsPerPixel.Value.Num,
                        ref exifTags.CompressedBitsPerPixel.Value.Denom,
                        0);
                    break;

                case 0x9201:
                    exifTags.ShutterSpeedValue = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseSignedRational(
                        prop,
                        ref exifTags.ShutterSpeedValue.Value.Num,
                        ref exifTags.ShutterSpeedValue.Value.Denom,
                        0);
                    break;

                case 0x9202:
                    exifTags.ApertureValue = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.ApertureValue.Value.Num,
                        ref exifTags.ApertureValue.Value.Denom,
                        0);
                    break;

                case 0x9203:
                    exifTags.BrightnessValue = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseSignedRational(
                        prop,
                        ref exifTags.BrightnessValue.Value.Num,
                        ref exifTags.BrightnessValue.Value.Denom,
                        0);
                    break;

                case 0x9204:
                    exifTags.ExposureBiasValue = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseSignedRational(
                        prop,
                        ref exifTags.ExposureBiasValue.Value.Num,
                        ref exifTags.ExposureBiasValue.Value.Denom,
                        0);
                    break;

                case 0x9205:
                    exifTags.MaxApertureValue = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.MaxApertureValue.Value.Num,
                        ref exifTags.MaxApertureValue.Value.Denom,
                        0);
                    break;

                case 0x9206:
                    exifTags.SubjectDistance = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseSignedRational(
                        prop,
                        ref exifTags.SubjectDistance.Value.Num,
                        ref exifTags.SubjectDistance.Value.Denom,
                        0);
                    break;

                case 0x9207:
                    exifTags.MeteringMode = new ExifTags.Param<libpixy.net.Exif.Tag.MeteringMode>();
                    exifTags.MeteringMode.Value = (libpixy.net.Exif.Tag.MeteringMode)ExifFormat.ParseUShort(prop);
                    break;

                case 0x9208:
                    exifTags.LightSource = new ExifTags.Param<libpixy.net.Exif.Tag.LightSource>();
                    exifTags.LightSource.Value = (libpixy.net.Exif.Tag.LightSource)ExifFormat.ParseUShort(prop);
                    break;

                case 0x9209:
                    {
                        exifTags.Flash = new ExifTags.Param<libpixy.net.Exif.Tag.FlashParam>();
                        ushort bits = ExifFormat.ParseUShort(prop);
                        exifTags.Flash.Value.Flash = (libpixy.net.Exif.Tag.FlashFlash)(bits & 0x1);
                        exifTags.Flash.Value.Return = (libpixy.net.Exif.Tag.FlashReturn)((bits>>1)&0x3);
                        exifTags.Flash.Value.FlashMode = (libpixy.net.Exif.Tag.FlashFlashMode)((bits>>3)&0x3);
                        exifTags.Flash.Value.FlashFunction = (libpixy.net.Exif.Tag.FlashFlashFunction)((bits>>5)&0x1);
                        exifTags.Flash.Value.RedEye = (libpixy.net.Exif.Tag.FlashRedEye)((bits>>6)&0x1);
                    }

                    break;

                case 0x920a:
                    exifTags.FocalLength = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.FocalLength.Value.Num,
                        ref exifTags.FocalLength.Value.Denom,
                        0);
                    break;

                case 0x927c:
                    //exifTags.MakerNote = new ExifTags.Param<string>();
                    //exifTags.MakerNote.Value = "(available)";
                    break;

                case 0x9286:
                    exifTags.UserComment = new ExifTags.Param<string>();
                    exifTags.UserComment.Value = ExifFormat.ParseComment(prop);
                    break;

                case 0x9290:
                    exifTags.SubsecTime = new ExifTags.Param<string>();
                    exifTags.SubsecTime.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0x9291:
                    exifTags.SubsecTimeOriginal = new ExifTags.Param<string>();
                    exifTags.SubsecTimeOriginal.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0x9292:
                    exifTags.SubsecTimeDigitized = new ExifTags.Param<string>();
                    exifTags.SubsecTimeDigitized.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0xa000:
                    exifTags.FlashPixVersion = new ExifTags.Param<string>();
                    exifTags.FlashPixVersion.Value = ExifFormat.ParseUndefined(prop, 4);
                    break;

                case 0xa001:
                    exifTags.ColorSpace = new ExifTags.Param<libpixy.net.Exif.Tag.ColorSpace>();
                    exifTags.ColorSpace.Value = (libpixy.net.Exif.Tag.ColorSpace)ExifFormat.ParseUShort(prop);
                    break;

                case 0xa002:
                    exifTags.ExifImageWidth = new ExifTags.Param<uint>();
                    exifTags.ExifImageWidth.Value = ExifFormat.ParseUInt(prop);
                    break;

                case 0xa003:
                    exifTags.ExifImageHeight = new ExifTags.Param<uint>();
                    exifTags.ExifImageHeight.Value = ExifFormat.ParseUInt(prop);
                    break;

                case 0xa004:
                    exifTags.RelatedSoundFile = new ExifTags.Param<string>();
                    exifTags.RelatedSoundFile.Value = ExifFormat.ParseAsciiString(prop);
                    break;

                case 0xa005:
                    exifTags.InteroperabilityIFDPointer = new ExifTags.Param<uint>();
                    exifTags.InteroperabilityIFDPointer.Value = ExifFormat.ParseULong(prop);
                    break;

                case 0xa20e:
                    exifTags.FocalPlaneXResolution = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.FocalPlaneXResolution.Value.Num,
                        ref exifTags.FocalPlaneXResolution.Value.Denom,
                        0);
                    break;

                case 0xa20f:
                    exifTags.FocalPlaneYResolution = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.FocalPlaneYResolution.Value.Num,
                        ref exifTags.FocalPlaneYResolution.Value.Denom,
                        0);
                    break;

                case 0xa210:
                    exifTags.FocalPlaneResolutionUnit = new ExifTags.Param<libpixy.net.Exif.Tag.FocalPlaneResolutionUnit>();
                    exifTags.FocalPlaneResolutionUnit.Value = (libpixy.net.Exif.Tag.FocalPlaneResolutionUnit)ExifFormat.ParseUShort(prop);
                    break;

                case 0xa215:
                    exifTags.ExposureIndex = new ExifTags.Param<libpixy.net.Exif.Tag.Rational>();
                    ExifFormat.ParseUnsignedRational(
                        prop,
                        ref exifTags.ExposureIndex.Value.Num,
                        ref exifTags.ExposureIndex.Value.Denom,
                        0);
                    break;

                case 0xa217:
                    exifTags.SensingMethod = new ExifTags.Param<libpixy.net.Exif.Tag.SensingMethod>();
                    exifTags.SensingMethod.Value = (libpixy.net.Exif.Tag.SensingMethod)ExifFormat.ParseUShort(prop);
                    break;

                case 0xa300:
                    exifTags.FileSource = new ExifTags.Param<uint>();
                    exifTags.FileSource.Value = ExifFormat.ParseUndefinedAsByte(prop, 1)[0];
                    break;

                case 0xa301:
                    exifTags.SceneType = new ExifTags.Param<uint>();
                    exifTags.SceneType.Value = ExifFormat.ParseUndefinedAsByte(prop, 1)[0];
                    break;
            }
        }
    }
}
