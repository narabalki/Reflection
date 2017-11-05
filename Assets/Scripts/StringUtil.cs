using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

public class StringUtil
{
	public static string RemoveCharacter (string ch, string str)
	{
		return ReplaceCharacter (ch, "", str);
	}

	public static string ReplaceCharacter (string oldCh, string newCh, string str)
	{
		string newStr = str.Replace(oldCh, newCh);
		return newStr;
	}

	/// <summary>
	/// Returns a list of strings no larger than the max length sent in.
	/// </summary>
	/// <remarks>useful function used to wrap string text for reporting.</remarks>
	/// <param name="text">Text to be wrapped into of List of Strings</param>
	/// <param name="maxLength">Max length you want each line to be.</param>
	/// <returns>List of Strings</returns>

	public static string AutoWrapSentenceAccoringToDevice (string text, int smallLen = 35, int largeLen = 45)
	{
		if (Settings.instance.isLargeDevice) {
			return WrapSentence (text, largeLen);
		} else {
			return WrapSentence (text, smallLen);
		}
	}

	public static string WrapSentence(string text, int maxLength)
	{
		// Return empty list of strings if the text was empty
		if (text.Length == 0) { return ""; }

		var words = text.Split (new char[]{' '}, System.StringSplitOptions.RemoveEmptyEntries);
		var lines = new List<string>();
		var currentLine = "";

		foreach (var word in words)
		{
			var currentWord = word.Trim ();

			if(currentWord.Length >= 1)
			{
				if ((currentLine.Length + 1 >= maxLength) || ((currentLine.Length + currentWord.Length + 1) >= maxLength))
				{
					//Debug.Log ("Length :" + currentLine.Length + ":::" + (currentLine.Length + currentWord.Length) );
					//Debug.Log ("1. Line broken!!!\n" + currentLine);

					lines.Add(currentLine);
					currentLine = "";
				}

				string[] splitWords = currentWord.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);

				//string printStr = "";
				//foreach(string tempstr in splitWords)
				//{
				//	printStr += "<" + tempstr + ">";
				//}
				//Debug.Log ("Current word :<" + currentWord + "> Split words :<" + printStr + ">");

				if(splitWords.Length <= 1)
				{
					if (currentLine.Length > 0)
					{
						currentLine += " " + currentWord;
					}
					else
					{
						currentLine += currentWord;
					}
				}
				else
				{
					//Debug.Log ("new line detected!!!");

					if( (currentLine.Length + splitWords[0].Length + 1) >= maxLength )
					{
						//Debug.Log ("2. Line broken!!!<" + currentLine + ">");
						lines.Add(currentLine);
						lines.Add(splitWords[0].Trim ());// + "\n");
						currentLine = splitWords[1].Trim ();
					}
					else
					{
						//Debug.Log ("3. Line broken!!! <" + currentLine + ">");
						if(currentLine.Length > 0) { lines.Add(currentLine + " " + splitWords[0].Trim ());/* + "\n");*/ }
						else { lines.Add(splitWords[0].Trim ());/* + "\n");*/ }

						currentLine = splitWords[1].Trim ();
					}
				}
			}
		}

		if (currentLine.Length > 0) { lines.Add(currentLine); }

		string sentence = "";
		foreach(string line in lines) {
			sentence += line + "\n";
		}

		return sentence.Trim ();
	}

	static char[] splitChars = new char[] { ' ', '\n', '\t' };
	
	public static string WrapText_Hyphenate(string str, int width)
	{
		str = str.Replace ("\n", " ");

		string[] words = Explode(str, splitChars);
		
		int curLineLength = 0;
		StringBuilder strBuilder = new StringBuilder();
		for(int i = 0; i < words.Length; i += 1)
		{
			string word = words[i];
			// If adding the new word to the current line would be too long,
			// then put it on a new line (and split it up if it's too long).
			if (curLineLength + word.Length > width)
			{
				// Only move down to a new line if we have text on the current line.
				// Avoids situation where wrapped whitespace causes emptylines in text.
				if (curLineLength > 0)
				{
					strBuilder.Append(System.Environment.NewLine);
					curLineLength = 0;
				}
				
				// If the current word is too long to fit on a line even on it's own then
				// split the word up.
				while (word.Length > width)
				{
					strBuilder.Append(word.Substring(0, width - 1) + "-");
					word = word.Substring(width - 1);
					
					strBuilder.Append(System.Environment.NewLine);
				}
				
				// Remove leading whitespace from the word so the new line starts flush to the left.
				word = word.TrimStart();
			}
			strBuilder.Append(word);
			curLineLength += word.Length;
		}
		
		return strBuilder.ToString();
	}
	
	private static string[] Explode(string str, char[] splitChars)
	{
		List<string> parts = new List<string>();
		int startIndex = 0;
		while (true)
		{
			int index = str.IndexOfAny(splitChars, startIndex);
			
			if (index == -1)
			{
				parts.Add(str.Substring(startIndex));
				return parts.ToArray();
			}
			
			string word = str.Substring(startIndex, index - startIndex);
			char nextChar = str.Substring(index, 1)[0];
			// Dashes and the likes should stick to the word occuring before it. Whitespace doesn't have to.
			if (char.IsWhiteSpace(nextChar))
			{
				parts.Add(word);
				parts.Add(nextChar.ToString());
			}
			else
			{
				parts.Add(word + nextChar);
			}
			
			startIndex = index + 1;
		}
	}

	/// <summary>
	/// Word wraps the given text to fit within the specified width.
	/// </summary>
	/// <param name="text">Text to be word wrapped</param>
	/// <param name="width">Width, in characters, to which the text
	/// should be word wrapped</param>
	/// <returns>The modified text</returns>
	public static string WrapText_NonHyphenate(string text, int length)
	{
		int pos, next;
		StringBuilder sb = new StringBuilder();
		
		// Lucidity check
		if (length < 1)
			return text;
		
		// Parse each line of text
		for (pos = 0; pos < text.Length; pos = next)
		{
			// Find end of line
			int eol = text.IndexOf(System.Environment.NewLine, pos);
			if (eol == -1)
				next = eol = text.Length;
			else
				next = eol + System.Environment.NewLine.Length;
			
			// Copy this line of text, breaking into smaller lines as needed
			if (eol > pos)
			{
				do
				{
					int len = eol - pos;
					if (len > length)
						len = BreakLine(text, pos, length);
					sb.Append(text, pos, len);
					sb.Append(System.Environment.NewLine);
					
					// Trim whitespace following break
					pos += len;
					while (pos < eol && char.IsWhiteSpace(text[pos]))
						pos++;
				} while (eol > pos);
			}
			else sb.Append(System.Environment.NewLine); // Empty line
		}
		return sb.ToString();
	}
	
	/// <summary>
	/// Locates position to break the given line so as to avoid
	/// breaking words.
	/// </summary>
	/// <param name="text">String that contains line of text</param>
	/// <param name="pos">Index where line of text starts</param>
	/// <param name="max">Maximum line length</param>
	/// <returns>The modified line length</returns>
	private static int BreakLine(string text, int pos, int max)
	{
		// Find last whitespace in line
		int i = max;
		while (i >= 0 && !char.IsWhiteSpace(text[pos + i]))
			i--;
		
		// If no whitespace found, break at maximum length
		if (i < 0)
			return max;
		
		// Find start of whitespace
		while (i >= 0 && char.IsWhiteSpace(text[pos + i]))
			i--;
		
		// Return length of text before whitespace
		return i + 1;
	}
	
	public static string SplitSentence (string sentence , int lineLength, string startNewLineWithString)
	{
		ArrayList splitSentenceList = new ArrayList();
		string sentenceString = "";
		
		splitSentenceList.Add (sentence);
		
		while( ((string)splitSentenceList[splitSentenceList.Count-1]).Length > lineLength) 
		{
			string tempSentence = (string)splitSentenceList[splitSentenceList.Count-1];
			
			int indexToSplit = ( tempSentence.Substring(lineLength - 1) ).IndexOf(' ');//first occurance of 'space' after line length
			
			if(indexToSplit != -1) 
			{
				indexToSplit += lineLength;
				
				//Debug.Log ("Index to split :" + indexToSplit);
				splitSentenceList[splitSentenceList.Count-1] = tempSentence.Substring(0, indexToSplit);
				splitSentenceList.Add (tempSentence.Substring(indexToSplit));
			} 
			else { break; }
			
			if(IsInfiniteLoop ()) { break; }
		} 
		
		foreach(string tempSentence in splitSentenceList)
		{
			sentenceString += tempSentence.Trim () + "\n" + startNewLineWithString;
		}
		
		return sentenceString.Trim ();
	}

	public static string SplitSentence (string sentence , int lineLength)
	{
		return SplitSentence (sentence , lineLength, "");
	}

	public static string ChangeColor (string sentence, string colorName)
	{
		string newSentence = "<color=" + colorName + ">" + sentence + "</color>";
		return newSentence;
	}
	
	public static string FindAndReplaceString(string sentence, string existingString, string newString)
	{
		string newSentence = sentence.Replace(existingString, newString);
		
		//Debug.Log ("Replaced Sentence :" + newSentence);
		return newSentence;
	}
	
	public static string FindAndReplaceSlashN(string sentence)
	{
		//Debug.Log ("sentence :"+sentence);
		string newSentence = "";
		
		if(sentence.Length > 0)
		{
			int startIndex = 0;
			int index = -1;
			index = sentence.IndexOf ("\\", startIndex);
			
			//Debug.Log ("startIndex :" + startIndex + ",index :" + index + ",new Sentence :" + newSentence);
			//Debug.Log (sentence[index+1]);
			
			int x = 0;
			
			while(index != -1)
			{
				if(sentence[index+1] == 'n')
				{
					newSentence += sentence.Substring (startIndex, (index- startIndex)) + "\n";
					//Debug.Log ("_startIndex :" + startIndex + ",index :" + index + ",new Sentence :" + newSentence);
					startIndex = index + 2;
				}
				else
				{
					startIndex = index + 1;
				}
				index = sentence.IndexOf ("\\", startIndex);
				
				//Debug.Log ("--startIndex :" + startIndex + ",index :" + index + ",new Sentence :" + newSentence);
				
				if(x++ > 100)
				{break;}
			}
			
			if(index == -1)
			{
				newSentence += sentence.Substring (startIndex, (sentence.Length- startIndex));
			}
		}
		return newSentence;
	}
	
	public static string SplitWordsWithNewLine(string sentence)
	{
		string newString = "";
		
		for(int i = 0; i < sentence.Length; i++)
		{
			string letter = sentence[i].ToString ();
			
			if(letter.Equals(" "))
			{
				letter = "\n";
			}
			newString  += letter;
		}
		
		return newString;
	}
	
	public static string ReplaceSpacesWithUnderScore (string sentence)
	{
		string newString = "";
		
		for(int i = 0; i < sentence.Length; i++)
		{
			string letter = sentence[i].ToString ();
			
			if(letter.Equals(" "))
			{
				letter = "_";
			}
			newString  += letter;
		}
		
		return newString;
	}

	static Regex ValidEmailRegex = CreateValidEmailRegex();

    /// <summary>
    /// Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
    /// </summary>
    /// <returns></returns>
    private static Regex CreateValidEmailRegex()
    {
        string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
    }

    public static bool IsEmailValid(string emailAddress)
    {
        bool isValid = ValidEmailRegex.IsMatch(emailAddress);

        return isValid;
    }

	public static int GetWordCount(string sentence)
	{
		string[] words = sentence.Split (' ');
		
		return words.Length;
	}

	private static int x = 0;
	private static bool IsInfiniteLoop ()
	{
		x += 1;
		if (x > 100) {
			Debug.LogWarning ("Infinite Loop...");
			x = 0;
			return true;
		}

		return false;
	}


	public static string ConvetToString(List<string> list) {
		string res = "[ "+ string.Join(",",list.ToArray()) +" ]";
		return res;
	}
	
}