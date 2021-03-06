﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.File;
using Model.Pictures;
using Util;

namespace Tests
{
    [TestClass]
    public class VariableParserTests
    {
        [TestMethod]
        public void VariableParserTest_Group()
        {
            const string input = " 01 FOO-1-BAR.       ";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual(typeof(PicGroup), actual["FOO-1-BAR"].Picture.GetType());
        }

        [TestMethod]
        public void VariableParserTest_Simple()
        {
            var strings = new List<string>
            {
                " 01 FOO-1-BAR PICTURE X.                   ",
                " 01 FOO-1-BAR PICTURE IS X.                ",
                " 01 FOO-1-BAR PIC X.                       ",
                " 01 FOO-1-BAR PIC IS X.                    ",
                " 01 FOO-1-BAR PIC X(04).                   ",
                " 01 FOO-1-BAR PIC 9.                       ",
                " 01 FOO-1-BAR PIC 999.                     ",
                " 01 FOO-1-BAR PIC 999 COMP.                ",
                " 01 FOO-1-BAR PIC 999 COMP-3.              ",
                " 01 FOO-1-BAR PIC 999 COMPUTATIONAL-4.     ",
                " 01 FOO-1-BAR PIC 9(5).                    ",
                " 01 FOO-1-BAR PIC S9.                      ",
                " 01 FOO-1-BAR PIC S9(51).                  ",
                " 01 FOO-1-BAR PIC 9V9.                     ",
                " 01 FOO-1-BAR PIC 9(3)V99.                 ",
                " 01 FOO-1-BAR PIC S9V99.                   ",
                " 01 FOO-1-BAR PIC V99.                     "
            };

            foreach (var input in strings)
            {
                var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

                Assert.AreEqual(1, actual.Count, input);
            }
        }

        [TestMethod]
        public void VariableParserTest_WithValue_String()
        {
            const string input = " 01 FOO-1-BAR PIC X VALUE \"asdjh\".       ";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual("FOO-1-BAR", actual["FOO-1-BAR"].VariableName);
        }

        [TestMethod]
        public void VariableParserTest_WithValue_Spaces()
        {
            const string input = " 01 FOO-1-BAR PIC X(04) VALUE SPACES.     ";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual("FOO-1-BAR", actual["FOO-1-BAR"].VariableName);
        }

        [TestMethod]
        public void VariableParserTest_WithValue_Int()
        {
            const string input = " 01 FOO-1-BAR PIC 9 VALUE 3.";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual("FOO-1-BAR", actual["FOO-1-BAR"].VariableName);
        }

        [TestMethod]
        public void VariableParserTest_WithValue_Decimal()
        {
            const string input = " 01 FOO-1-BAR PIC S9V99 VALUE 2.43.";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual("FOO-1-BAR", actual["FOO-1-BAR"].VariableName);
        }


        [TestMethod]
        public void VariableParserTest_WithValue_More()
        {
            var strings = new List<string>
            {
                " 01 FOO-1-BAR PIC X VALUE HIGH-VALUES.     ",
                // " 01 FOO-1-BAR PIC 9 VALUE HIGH-VALUES.     ", // PIC 9 does not currently support high or low values
                " 01 FOO-1-BAR PIC X VALUE LOW-VALUES.      ",
                // " 01 FOO-1-BAR PIC 9 VALUE LOW-VALUES.      ", // PIC 9 does not currently support high or low values
                " 01 FOO-1-BAR PIC 9 VALUE ZERO.            ",
                " 01 FOO-1-BAR PIC 999 VALUE 432.           ",
                " 01 FOO-1-BAR PIC 9(5) VALUE 12397.        ",
                " 01 FOO-1-BAR PIC S9 VALUE 4.              ",
                " 01 FOO-1-BAR PIC S9(51) VALUE 99283468723.",
                " 01 FOO-1-BAR PIC 9V9 VALUE 9.3.           ",
                " 01 FOO-1-BAR PIC 9(3)V99 VALUE 123.54.    ",
                " 01 FOO-1-BAR PIC V99 VALUE .43.           "
            };

            foreach (var input in strings)
            {
                var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

                Assert.AreEqual(1, actual.Count, input);
            }
        }

        [TestMethod]
        public void VariableParserTest_WithOccurs()
        {
            const string input = " 01 FOO-1-BAR PIC 999 OCCURS 20.";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual("FOO-1-BAR", actual["FOO-1-BAR"].VariableName);
            Assert.AreEqual(20, actual["FOO-1-BAR"].Occurs);

        }

        [TestMethod]
        public void VariableParserTest_WithOccurs_2()
        {
            const string input = " 03 FOO-1-BAR           OCCURS 10  PIC S9 COMP.";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual("FOO-1-BAR", actual["FOO-1-BAR"].VariableName);
            Assert.AreEqual(10, actual["FOO-1-BAR"].Occurs);
        }

        [TestMethod]
        public void VariableParserTest_WithOccurs_3()
        {
            const string input = " 03 FOO-1-BAR           OCCURS 10.";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual("FOO-1-BAR", actual["FOO-1-BAR"].VariableName);
            Assert.AreEqual(10, actual["FOO-1-BAR"].Occurs);
        }

        [TestMethod]
        public void VariableParserTest_WithRedefines()
        {
            const string input = " 01 FOO-1-BAR PIC 9. \n 03 FOO-2-BAR  REDEFINES  FOO-1-BAR  PIC 9.";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(2, actual.Count, input);
            Assert.AreEqual("FOO-2-BAR", actual["FOO-2-BAR"].VariableName);
            Assert.AreEqual("FOO-1-BAR", actual["FOO-2-BAR"].Redefines.VariableName);
        }

        [TestMethod]
        public void VariableParserTest_Level88_Int()
        {
            const string input = " 88 FOO-1-BAR VALUE 1.";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual(typeof(Pic88), actual["FOO-1-BAR"].Picture.GetType());
            Assert.AreEqual("FOO-1-BAR", actual["FOO-1-BAR"].VariableName);
            Assert.AreEqual("1", actual["FOO-1-BAR"].Picture.Value);
        }

        [TestMethod]
        public void VariableParserTest_Level88_String()
        {
            const string input = " 88 FOO-1-BAR VALUE \"foo\".                     ";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual(typeof(Pic88), actual["FOO-1-BAR"].Picture.GetType());
            Assert.AreEqual("FOO-1-BAR", actual["FOO-1-BAR"].VariableName);
            Assert.AreEqual("\"foo\"", actual["FOO-1-BAR"].Picture.Value);
        }

        [TestMethod]
        public void VariableParserTest_Level88_EmptyString()
        {
            const string input = " 88 FOO-1-BAR VALUE \" \".";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual(typeof(Pic88), actual["FOO-1-BAR"].Picture.GetType());
            Assert.AreEqual("FOO-1-BAR", actual["FOO-1-BAR"].VariableName);
            Assert.AreEqual("\" \"", actual["FOO-1-BAR"].Picture.Value);
        }

        [TestMethod]
        public void VariableParserTest_Level88_THRU()
        {
            const string input = " 88  FOO-1-BAR VALUE \" \"   THRU \"2\".";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual(typeof(Pic88), actual["FOO-1-BAR"].Picture.GetType());
            Assert.AreEqual("FOO-1-BAR", actual["FOO-1-BAR"].VariableName);
            Assert.AreEqual("\" \" THRU \"2\"", actual["FOO-1-BAR"].Picture.Value);
        }

        [TestMethod]
        public void VariableParserTest_Level88()
        {
            var strings = new List<string>
            {
                " 88 FOO-1-BAR VALUE 1.                           ",
                " 88 FOO-1-BAR VALUE \"@\".                       ",
                " 88 FOO-1-BAR VALUE IS 1.                        ",
                " 88 FOO-1-BAR VALUE IS ZERO.                     ",
                " 88 FOO-1-BAR VALUE IS ZEROS.                    ",
                " 88 FOO-1-BAR VALUE IS ZEROES.                   ",
                " 88 FOO-1-BAR VALUE IS SPACE.                    ",
                " 88 FOO-1-BAR VALUE IS SPACES.                   ",
                " 88 FOO-1-BAR VALUE IS HIGH-VALUES.              ",
                " 88 FOO-1-BAR VALUE IS LOW-VALUES.               ",
                " 88 FOO-1-BAR VALUES ARE HIGH-VALUES.            ",
                " 88 FOO-1-BAR VALUES ARE LOW-VALUES.             ",
                " 88 FOO-1-BAR VALUES ARE ZEROS.                  ",
                " 88 FOO-1-BAR VALUES ARE ZEROES.                 ",
                " 88 FOO-1-BAR VALUES ARE HIGH-VALUES THROUGH 5.  ",
                " 88 FOO-1-BAR VALUES ARE HIGH-VALUES THRU 5.     ",
                " 88 FOO-1-BAR VALUES HIGH-VALUES THROUGH 5.      ",
                " 88 FOO-1-BAR VALUES HIGH-VALUES THRU 5.         ",
                " 88 FOO-1-BAR VALUES ARE 4 THROUGH 6.            ",
                " 88 FOO-1-BAR VALUES ARE 4 THRU 6.               ",
                " 88 FOO-1-BAR VALUES 4 THRU 6.                   ",
                " 88 FOO-1-BAR VALUE \"foo\", \"bar\".            "
            };

            foreach (var input in strings)
            {
                var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

                Assert.AreEqual(1, actual.Count, input);
            }
        }

        [TestMethod]
        public void VariableParserTest_Multiline_1()
        {
            const string input = " 03  BAR                   PIC S9(11)V99 COMP \n                                OCCURS 5.";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual(typeof(PicS9V9), actual["BAR"].Picture.GetType());
            Assert.AreEqual("BAR", actual["BAR"].VariableName);
            Assert.AreEqual(5, actual["BAR"].Occurs);
        }

        [TestMethod]
        public void VariableParserTest_Multiline_2()
        {
            const string input = " 88  BAR              VALUE \"0\", \"1\",\n \"2\", \"3\", \"4\", \"A\", \"B\".";

            var actual = VariablesUtil.Instance.AnalyzeVariables(new CobolFile(input, ""), false);

            Assert.AreEqual(1, actual.Count, input);
            Assert.AreEqual(typeof(Pic88), actual["BAR"].Picture.GetType());
            Assert.AreEqual("BAR", actual["BAR"].VariableName);
            Assert.AreEqual("\"0\", \"1\", \"2\", \"3\", \"4\", \"A\", \"B\"", actual["BAR"].Picture.Value); // TODO
        }
    }
}
