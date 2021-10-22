using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingTests
{
    [TestClass]
    public class BowlingTests
    {
        [TestMethod]
        public void AddSeparators()
        {
            string rawText = "11223344";

            var list = Enumerable.Range(0, rawText.Length / 2).Select(i => rawText.Substring(i * 2, 2)).ToList();
            string finalString = string.Join("|", list);
            Assert.AreEqual("|11|22|33|44|", "|" + finalString + "|");
        }

        [TestMethod]
        public void ReplaceX()
        {
            string[,] rawArray = new string[,] { { "0", "0" }, { "10", "" } };
            string[,] resultArray = new string[,] { { "0", "0" }, { "X", " " } };
            string itemFromArray = "-1";
            for (int i = 0; i < rawArray.GetLength(0); i++)
            {
                for (int j = 0; j < rawArray.GetLength(1); j++)
                {
                    itemFromArray = rawArray[i, j];
                    if (string.Equals(itemFromArray, "10"))
                    {
                        rawArray[i, j] = "X";
                        if (j < 1)
                            rawArray[i, j + 1] = " ";
                    }
                }
            }

            CollectionAssert.AreEqual(resultArray, rawArray);
        }
        [TestMethod]
        public void Replace0()
        {
            string[,] rawArray = new string[,] { { "-", "-" }, { "X", " " } };
            string[,] resultArray = new string[,] { { "-", "-" }, { "X", " " } };
            string itemFromArray = "-1";
            for (int i = 0; i < rawArray.GetLength(0); i++)
            {
                for (int j = 0; j < rawArray.GetLength(1); j++)
                {
                    itemFromArray = rawArray[i, j];
                    if (string.Equals(itemFromArray, "0"))
                        rawArray[i, j] = "-";
                }
            }
        }
        [TestMethod]
        public void CleanCommas()
        {
            string rawText = "0,1,2,3";
            string result = "0123";
            string finalText = rawText.Replace(",", "");
            Assert.AreEqual(result, finalText);
        }
        [TestMethod]
        public void GroupBy2()
        {
            string rawText = "-,-,1,1,2,2,X, ";
            string noCommas = rawText.Replace(",", "");
            string[,] array2D = new string[(noCommas.Length / 2), 2];
            string[,] result = new string[,] { { "-", "-" }, { "1", "1" }, { "2", "2" }, { "X", " " } };
            int x = 0;
            for (int i = 0; i < array2D.GetLength(0); i++)
            {
                for (int j = 0; j < array2D.GetLength(1); j++)
                {
                    array2D[i, j] = noCommas[x].ToString();
                    x++;
                }
            }
            CollectionAssert.AreEqual(result, array2D);
        }
        [TestMethod]
        public void CompareStrings()
        {
            string[,] rawArray = new string[,] { { "X", " " } };
            bool areEqual = false;
            var sstring = rawArray[0, 0];

            if (!rawArray[0, 0].Contains("X"))
                areEqual = true;
            else if (String.Equals(sstring, "X"))
                areEqual = true;
            Assert.IsTrue(areEqual);
        }
        [TestMethod]
        public void AddSpares()
        {
            string[,] rawArray = new string[,] { { "1", "9" }, { "-", "2" }, { "8", "2" }, { "X", " " } };
            string[,] resultArray = new string[,] { { "1", "/" }, { "-", "2" }, { "8", "/" }, { "X", " " } };
            string itemFromArray = "-1";
            int total = 0;

            for (int i = 0; i < rawArray.GetLength(0); i++)
            {
                total = 0;
                for (int j = 0; j < rawArray.GetLength(1); j++)
                {
                    itemFromArray = rawArray[i, j];
                    if (!String.Equals(itemFromArray, "X") && !String.Equals(itemFromArray, "-") && !String.IsNullOrWhiteSpace(itemFromArray))
                    {
                        total += Convert.ToInt32(itemFromArray);
                        if (total == 10)
                            rawArray[i, j] = "/";
                    }
                }
            }

            CollectionAssert.AreEqual(resultArray, rawArray);
        }
        [TestMethod]
        public void ReturnString()
        {
            string[,] rawArray = new string[,] { { "1", "/" }, { "7", "2" }, { "8", "/" }, { "X", " " } };
            string result = "|1,/|7,2|8,/|X, |";
            string final = "";
            string itemFromArray = "-1";
            bool comma;
            for (int i = 0; i < rawArray.GetLength(0); i++)
            {
                final += "|";
                comma = true;
                for (int j = 0; j < rawArray.GetLength(1); j++)
                {
                    itemFromArray = rawArray[i, j];
                    final += itemFromArray;
                    if (comma)
                    {
                        final += ",";
                        comma = false;
                    }
                }
            }
            Assert.AreEqual(result, final + "|");
        }
        [TestMethod]
        public void CalculateSpareScore()
        {
            string[,] rawArray = new string[,] { { "6", "4" }, { "8", "1" } };
            //The first frame must be 18 (6+4+8) + 9 of the 2nd frame
            int result = 27, totalFrame, score = 0;
            bool isSpare = false;

            for (int i = 0; i < rawArray.GetLength(0); i++)
            {
                totalFrame = 0;
                for (int j = 0; j < rawArray.GetLength(1); j++)
                {
                    totalFrame += Convert.ToInt32(rawArray[i, j]);
                    if (isSpare)
                    {
                        score += totalFrame;
                        isSpare = false;
                    }
                    if (totalFrame == 10)
                        isSpare = true;
                }
                score += totalFrame;
            }
            Assert.AreEqual(result, score);
        }
        [TestMethod]
        public void CalcualteStrikeScore()
        {
            string[,] rawArray = new string[,] { { "0", "10" }, { "7", "1" } };
            string[,] rawArray2 = new string[,] { { "10", "0" }, { "7", "1" } };
            //The first frame must be 18 (10+7+1) + 8 of the 2nd frame
            int result = 26, totalFrame, score = 0;
            bool isStrike = false;

            for (int i = 0; i < rawArray2.GetLength(0); i++)
            {
                totalFrame = 0;
                for (int j = 0; j < rawArray2.GetLength(1); j++)
                {
                    int pinsDown = Convert.ToInt32(rawArray2[i, j]);
                    if (isStrike)
                    {
                        totalFrame += pinsDown;
                        if (j == 1)
                            isStrike = false;
                    }
                    if (pinsDown == 10)
                    {
                        isStrike = true;
                        //End frame
                        if (j != 1)
                            j = 1;
                    }
                    totalFrame += pinsDown;
                }
                score += totalFrame;
            }
            Assert.AreEqual(result, score);
        }
        [TestMethod]
        public void CalculateSparesAndStrikes()
        {
            string[,] rawArray = new string[,] { { "6", "4" }, { "10", "0" }, { "3", "4" } };
            int result = 44;

            int totalFrame = 0;
            int score = 0;
            bool isStrike = false;
            bool isSpare = false;

            for (int i = 0; i < rawArray.GetLength(0); i++)
            {
                totalFrame = 0;
                for (int j = 0; j < rawArray.GetLength(1); j++)
                {
                    int pinsDown = Convert.ToInt32(rawArray[i, j]);
                    if (isStrike)
                    {
                        totalFrame += pinsDown;
                        if (j == 1)
                            isStrike = false;
                    }
                    if (pinsDown == 10)
                    {
                        isStrike = true;
                        //End frame
                        if (j != 1)
                            j = 1;
                    }

                    totalFrame += pinsDown;
                    if (isSpare)
                    {
                        score += totalFrame;
                        isSpare = false;
                    }
                    if (totalFrame == 10 && !isStrike)
                        isSpare = true;
                }
                score += totalFrame;
            }
            Assert.AreEqual(result, score);
        }
    }
}
