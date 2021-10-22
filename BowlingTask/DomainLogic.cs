using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BowlingTask
{
    class DomainLogic : IFileManager
    {
        #region File Management
        //This will get the current project directory + find the "ScoreFiles" sub-directory
        static readonly string FolderPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\ScoreFiles\";

        string[] FilePathArray;
        public int GetArrayLength()
        {
            FilePathArray = Directory.GetFiles(FolderPath);

            return FilePathArray.Length;
        }
        public string GetFileNameFromArray(int position)
        {
            return Path.GetFileNameWithoutExtension(FilePathArray[position]);
        }

        public string ReadFile(int position)
        {
            //User always sees one number ahead to be userfriendly, therefore, we must substract one from them choice
            string rawText = File.ReadAllText(FilePathArray[position - 1]);

            //Clean any white space character
            rawText = Regex.Replace(rawText, @"\s", "");

            return ReplaceTenForX(rawText);
        }
        #endregion

        #region Text modification
        //Since this comment. Methods are all about string/char modification and all of them are concatenated
        string ReplaceTenForX(string rawText)
        {
            rawText = rawText.Replace("10", "X ");
            return GroupRollsByTwo(rawText);
        }
        string GroupRollsByTwo(string rawText)
        {
            //Clean Commas
            string noCommas = rawText.Replace(",", "");
            //Get the total size from the cleanned strings divided by two since the items will be grouped by two
            //Create two separate arrays, otherwise they will keep changing values among themselves no matter what
            string[,] array2D = new string[noCommas.Length / 2, 2];
            calculationArray = new string[noCommas.Length / 2, 2];

            int x = 0;
            for (int i = 0; i < array2D.GetLength(0); i++)
            {
                for (int j = 0; j < array2D.GetLength(1); j++)
                {
                    array2D[i, j] = noCommas[x].ToString();
                    calculationArray[i, j] = noCommas[x].ToString();
                    //Keep increasing the char index on the cleanned string
                    x++;
                }
            }
            return ReplaceCeroForDash(array2D);
        }
        string ReplaceCeroForDash(string[,] rawArray)
        {
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
            return AddSpares(rawArray);
        }
        string AddSpares(string[,] rawArray)
        {
            string itemFromArray = "-1";
            int totalFrame = 0;

            for (int i = 0; i < rawArray.GetLength(0); i++)
            {
                totalFrame = 0;
                for (int j = 0; j < rawArray.GetLength(1); j++)
                {
                    itemFromArray = rawArray[i, j];
                    //Be sure to skip any time an "X", "-" and white space as they are automatically 10 or other meaning
                    if (!String.Equals(itemFromArray, "X") && !String.Equals(itemFromArray, "-") && !String.IsNullOrWhiteSpace(itemFromArray))
                    {
                        totalFrame += Convert.ToInt32(itemFromArray);
                        if (totalFrame == 10)
                            rawArray[i, j] = "/";
                    }
                }
            }
            return ConvertToOutput(rawArray);
        }
        string ConvertToOutput(string[,] rawArray)
        {
            //If the length is above 20 means there are extra rolls at the end
            int length = rawArray.Length;
            //Count 10 sticks, 11th is for extraroll
            int stickCount = 0;
            string final = "";
            string itemFromArray = "-1";
            bool comma;
            //The comma will alternate on each loop so it will appear just between the numbers
            for (int i = 0; i < rawArray.GetLength(0); i++)
            {
                //Any two items, separate them with |
                if (stickCount < 10)
                {
                    final += "|";
                    stickCount++;
                }
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
            return final + "|";
        }
        #endregion

        #region Score Calculation
        //Since this comment. All methods are all about calculating the score
        int score = 0;
        string[,] calculationArray;
        public int CalculateScore()
        {
            int totalFrame = 0;
            bool isStrike = false;
            bool isSpare = false;
            string itemFromArray = "-1";

            for (int i = 0; i < calculationArray.GetLength(0); i++)
            {
                //totalFrame calculates the sum of the frame. Reset it to 0 when change to another frame
                totalFrame = 0;
                for (int j = 0; j < calculationArray.GetLength(1); j++)
                {
                    //The array comes with X instead 10. Indicate how to interpret those Xs and the following empty space
                    itemFromArray = calculationArray[i, j];
                    if (string.Equals(itemFromArray, "X"))
                        calculationArray[i, j] = "10";
                    else if (string.Equals(itemFromArray, " "))
                        calculationArray[i, j] = "0";

                    int pinsDown = Convert.ToInt32(calculationArray[i, j]);
                    //If it was a strike, calculate the strike operation before anything else
                    if (isStrike)
                    {
                        totalFrame += pinsDown;
                        if (j == 1)
                            isStrike = false;
                    }
                    //Check for the total amount of pins down at once, if = 10 means its a strike
                    if (pinsDown == 10)
                    {
                        isStrike = true;
                        //End frame
                        //if (j != 1)
                        // j = 1;
                    }
                    //Add to the frame calculation the sum of the pins down per roll
                    totalFrame += pinsDown;
                    //If it was a spare, calculate the spare operation before anything else
                    if (isSpare)
                    {
                        score += totalFrame;
                        isSpare = false;
                    }
                    //Be sure that the sum of the roll is 10 and it is not because of a strike
                    if (totalFrame == 10 && !isStrike)
                        isSpare = true;
                }
                //Once the sum of the frame are done, add it to the final score
                score += totalFrame;
            }
            return score;
        }
        #endregion
    }
}
