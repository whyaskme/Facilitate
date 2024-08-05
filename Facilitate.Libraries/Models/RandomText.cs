using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLPostLibrary
{
    public class RandomText
    {
        /// <summary>
        /// Create a pusedo-random number generator to be used for the instance of this object
        /// </summary>
        private Random _random;

        /// <summary>
        /// Create a list of possible end of sentance punctuation marks.  There are multiple
        /// periods so that more often than not a period ends the sentance.
        /// </summary>
        private char[] _endOfSentancePunctuation = new char[] { '.', '.', '.', '!', '.', '.', '?', '.', '.', '.' };

        /// <summary>
        /// Create a list of possible middle of the sentance punctuation marks.
        /// </summary>
        private char[] _middleOfSentancePunctuation = new char[] { ',' };

        /// <summary>
        /// Indicate whether or not to include random middle of the sentance punctuation
        /// marks in generated sentances
        /// </summary>
        public bool AddMiddleOfSentancePunctuationMarks = false;

        /// <summary>
        /// Indicates whether or not to add an end of sentance punctuation mark
        /// </summary>
        public bool AddEndOfSentancePunctuation = true;

        /// <summary>
        /// List of possible word lengths when user does not specify their own list
        /// </summary>
        private int[] _defaultWordLengths = new int[] { 1, 2, 3, 3, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 6, 7, 7, 8 };

        /// <summary>
        /// List of possible sentance lengths when user does not specify their own list
        /// </summary>
        private int[] _defaultSentanceLengths = new int[] { 3, 4, 5, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13 };

        /// <summary>
        /// List of possible paragrpah lengths when user does not specify their own list
        /// </summary>
        private int[] _defaultParagraphLengths = new int[] { 3, 4, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 10, 10, 11, 11, 11, 11, 12, 12, 13, 13, 14, 15 };

        public RandomText()
        {
            // Create a Random object for use during the existence of this object
            _random = new Random(DateTime.UtcNow.Millisecond);
        }     

        /// <summary>
        /// Generates a random string of a specfic length.
        /// </summary>        
        /// <returns>Returns a randomly generated string (lower case) of a specific length.</returns>
        public string String()
        {
            int wordLength = _defaultWordLengths.GetRandomElement<int>();
            return String(wordLength);
        }

        /// <summary>
        /// Generates a random string of a specfic length.
        /// </summary>
        /// <param name="length">The length of the random string to generate.</param>        
        /// <returns>Returns a randomly generated string (lower case) of a specific length.</returns>
        public string String(int length)
        {
            return String(length, false);
        }

        /// <summary>
        /// Generates a random string of a specfic length.
        /// </summary>
        /// <param name="length">The length of the random string to generate.</param>
        /// <param name="randomCharacterCase">If true, each character in the string will have
        /// an equal chance of being either upper case or lower case.  If false, the generated
        /// string will be all lower case.
        /// </param>
        /// <returns>Returns a randomly generated string of a specific length.</returns>
        public string String(int length, bool randomCharacterCase)
        {
            StringBuilder s = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                // Get a random integer in the range of ASCII lower case characters
                int c = _random.Next(97, 123);

                // If we are to randomize the case of the characters in the string
                // then if a random number is evenly divisible by two (a 1 in 2 chance)
                // then subtract 32 which converts the character to upper case
                if (randomCharacterCase && _random.Next() % 2 == 0)
                    c -= 32;

                // Add the character to the string
                s.Append(Convert.ToChar(c));
            }

            return s.ToString();
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>        
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be
        ///  greater than or equal to minValue.</param>               
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue;
        ///  that is, the range of return values includes minValue but not maxValue. If
        ///  minValue equals maxValue, minValue is returned.</returns>
        private int Number(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Generates a random sentance.
        /// </summary>        
        /// <returns>Returns a random sentance of random length and words from the default sentance and word lengths.</returns>
        public string Sentance()
        {
            int numberOfWords = _defaultSentanceLengths.GetRandomElement<int>();
            return Sentance(numberOfWords);
        }

        /// <summary>
        /// Generates a random sentance of a given number of words .
        /// </summary>
        /// <param name="numberOfWords">The number of words in the sentance</param>
        /// /// <returns>Returns a random sentance of the specified length.</returns>
        public string Sentance(int numberOfWords)
        {
            return Sentance(numberOfWords, _defaultWordLengths);
        }

        /// <summary>
        /// Generates a random sentance of a given number of words and possible word lengths.
        /// </summary>
        /// <param name="numberOfWords">The number of words in the sentance</param>
        /// <param name="possibleWordLengths">An array of integers representing the possible number of characters in each word</param>
        /// <returns>Returns a string containing a specified number of random words composed of random characters</returns>
        public string Sentance(int numberOfWords, int[] possibleWordLengths)
        {
            // Parameter validation
            if (possibleWordLengths == null)
                throw new ArgumentNullException("possibleWordLengths");
            else if (possibleWordLengths.Length < 1 || possibleWordLengths.Where(l => l < 0).Any())
                throw new ArgumentException("Parameter 'possibleWordLengths' must have one or more elements and cannot contain a negative number.");

            StringBuilder s = new StringBuilder();

            for (int i = 1; i <= numberOfWords; i++)
            {
                // Randomly choose a word length
                int wordLength = possibleWordLengths.GetRandomElement<int>();

                // Generate a random word of the indicated length
                s.Append(String(wordLength, false));

                if (AddMiddleOfSentancePunctuationMarks && i != numberOfWords && _random.Next() % 15 == 0)                    
                    s.Append(_middleOfSentancePunctuation.GetRandomElement<char>());

                // Add a space if we aren't at the end of the sentance;
                s.Append(i == numberOfWords ? string.Empty : " ");
            }

            // Capitalize the first letter
            if (s.Length > 0)
                s[0] = char.ToUpper(s[0]);

            // Add a puncuation mark to the end of the sentance if desired
            if (AddEndOfSentancePunctuation)                
                s.Append(_endOfSentancePunctuation.GetRandomElement<char>());

            return s.ToString();
        }

        /// <summary>
        /// Generates a random paragraph.
        /// </summary>
        public string Paragraph()
        {
            int numberOfSentances = _defaultParagraphLengths.GetRandomElement<int>();
            return Paragraph(numberOfSentances);
        }

        /// <summary>
        /// Generates a random paragraph of a given number of sentances.
        /// </summary>
        /// <param name="numberOfSentances">The number of sentances in the paragraph.</param>
        public string Paragraph(int numberOfSentances)
        {
            return Paragraph(numberOfSentances, _defaultSentanceLengths, _defaultWordLengths);
        }

        /// <summary>
        /// Generates a random paragraph of a given number of sentances.
        /// </summary>
        /// <param name="numberOfSentances">The number of sentances in the paragraph.</param>
        /// <param name="possibleSentanceLengths">An array of integers representing the possible number of words in each sentance.</param>
        public string Paragraph(int numberOfSentances, int[] possibleSentanceLengths)
        {
            return Paragraph(numberOfSentances, possibleSentanceLengths, _defaultWordLengths);
        }

        /// <summary>
        /// Generates a random paragraph of a given number of sentances.
        /// </summary>
        /// <param name="numberOfSentances">The number of sentances in the paragraph.</param>
        /// <param name="possibleSentanceLengths">An array of integers representing the possible number of words in each sentance.</param>
        /// <param name="possibleWordLengths">An array of integers representing the possible number of characters in each word</param>
        /// <returns>Returns a string containing a specified number of random sentances composed of random words and characters</returns>
        public string Paragraph(int numberOfSentances, int[] possibleSentanceLengths, int[] possibleWordLengths)
        {
            // Parameter validation
            if (possibleWordLengths == null)
                throw new ArgumentNullException("possibleWordLengths");
            else if (possibleWordLengths.Length < 1 || possibleWordLengths.Where(l => l < 0).Any())
                throw new ArgumentException("Parameter 'possibleWordLengths' must have one or more elements and cannot contain a negative number.");

            if (possibleSentanceLengths == null)
                throw new ArgumentNullException("possibleSentanceLengths");
            else if (possibleSentanceLengths.Length < 1 || possibleWordLengths.Where(l => l < 0).Any())
                throw new ArgumentException("Parameter 'possibleSentanceLengths' must have one or more elements and cannot contain a negative number.");

            StringBuilder s = new StringBuilder();

            for (int i = 1; i <= numberOfSentances; i++)
            {
                int numberOfWords = possibleSentanceLengths.GetRandomElement<int>();
                s.Append(Sentance(numberOfWords, possibleWordLengths));

                s.Append(i == numberOfSentances ? string.Empty : "  ");
            }            
            
            return s.ToString();
        }        
    }
    
    public static class Extensions
    {
        private static Random random = new Random();

        /// <summary>
        /// Gets a random element from a callection of elements that implments the IEnumerable interface
        /// </summary>
        /// <typeparam name="T">The type of each element in the collection</typeparam>
        /// <param name="list">A collection of elements of type T</param>
        /// <returns>Returns a random element from a collection</returns>        
        public static T GetRandomElement<T>(this IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            // Get the number of elements in the collection
            int count = list.Count();

            // If there are no elements in the collection, return the default value of T
            if (count == 0)
                return default(T);

            // Get a random index
            int index = random.Next(list.Count());

            // When the collection has 100 elements or less, get the random element
            // by traversing the collection one element at a time.
            if (count <= 100)
            {                
                using (IEnumerator<T> enumerator = list.GetEnumerator())
                {
                    // Move down the collection one element at a time.
                    // When index is -1 we are at the random element location
                    while (index >= 0 && enumerator.MoveNext())
                        index--;

                    // Return the current element
                    return enumerator.Current;
                }
            }
            
            // Get an element using LINQ which casts the collection
            // to an IList and indexes into it.
            return list.ElementAt(index);            
        }       
    }
}
