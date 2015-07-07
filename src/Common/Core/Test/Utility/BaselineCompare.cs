﻿using System;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Languages.Core.Test.Utility
{
    public static class BaselineCompare
    {
        public static string CompareStrings(string expected, string actual)
        {
            var result = new StringBuilder();

            int length = Math.Min(expected.Length, actual.Length);
            for (int i = 0; i < length; i++)
            {
                if (expected[i] != actual[i])
                {
                    result.AppendFormat("Position: {0}: expected: '{1}', actual '{2}'\r\n", i, expected[i], actual[i]);
                    if (i > 6 && i < length - 6)
                    {
                        result.AppendFormat("Context: {0} -> {1}", expected.Substring(i - 6, 12), actual.Substring(i - 6, 12));
                    }
                    break;
                }

            }

            if (expected.Length != actual.Length)
                result.AppendFormat("\r\nLength different. Expected: '{0}' , actual '{1}'", expected.Length, actual.Length);

            return result.ToString();
        }

        public static int CompareLines(string expected, string actual, out string baseLine, out string newLine, bool ignoreCase = false)
        {
            var newReader = new StringReader(actual);
            var baseReader = new StringReader(expected);

            int lineNum = 1;
            for (lineNum = 1; ; lineNum++)
            {
                baseLine = baseReader.ReadLine();
                newLine = newReader.ReadLine();

                if (baseLine == null || newLine == null)
                    break;

                if (String.Compare(baseLine, newLine, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) != 0)
                    return lineNum;
            }

            if (baseLine == null && newLine == null)
            {
                baseLine = String.Empty;
                newLine = String.Empty;

                return 0;
            }

            return lineNum;
        }

        public static void CompareFiles(string baselineFile, string actual, bool regenerateBaseline, bool ignoreCase = false)
        {
            try
            {
                if (regenerateBaseline)
                {
                    if (File.Exists(baselineFile))
                        File.SetAttributes(baselineFile, FileAttributes.Normal);

                    using (var sw = new StreamWriter(baselineFile))
                    {
                        sw.Write(actual);
                    }
                }
                else
                {
                    using (var sr = new StreamReader(baselineFile))
                    {
                        string expected = sr.ReadToEnd();

                        string baseLine, newLine;
                        int line = CompareLines(expected, actual, out baseLine, out newLine, ignoreCase);

                        Assert.AreEqual(0, line,
                            String.Format(
                             CultureInfo.InvariantCulture, "\r\nDifferent at line {0}\r\n\tExpected:\t{1}\r\n\tActual:\t{2}\r\n", line, baseLine.Trim(), newLine.Trim()));
                    }
                }
            }
            catch (AssertFailedException ex)
            {
                Assert.Fail(string.Format(CultureInfo.InvariantCulture, "Test {0} has thrown an exception: {1}", baselineFile.Substring(baselineFile.LastIndexOf('\\') + 1), ex.Message));
            }
        }
    }
}