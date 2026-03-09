namespace Wordle;

/// <summary>Wordle player interface</summary>
public class WordlePlayer {
   /// <summary>Handles incoming chars and composes guess word</summary>
   /// <returns>False if incoming char could not be accommodated</returns>
   public bool LetterPressed (char ch) {
      if (CChars == WordleGame.WORDLENGTH) return false;
      mChars[CChars++] = char.ToUpper (ch);
      return true;
   }

   /// <summary>User pressed ENTER or wants composed guess word to be considered as solution</summary>
   /// <returns>False if guess word is not fully composed OR the guess word is not a valid dictionary word</returns>
   public bool EnterPressed () {
      if (CChars != WordleGame.WORDLENGTH || !mWordle.AcceptGuess (new string (mChars))) return false;
      CChars = 0;
      return true;
   }

   /// <summary>User pressed BACKSPACE key or wants to undo the last char input</summary>
   /// <returns>False if there are no more char inputs to be undone</returns>
   public bool BackspacePressed () {
      if (CChars == 0) return false;
      CChars--;
      return true;
   }

   /// <summary>Count of char inputs</summary>
   public int CChars { get; private set; }

   /// <summary>Gets the Nth char in the guess word being composed</summary>
   public char this[int index] {
      get {
         if (index >= CChars) throw new Exception ("Coding error");
         return mChars[index];
      }
   }

   //public int CGuess => mWordle.CGuess;
   //public string GetGuess (int idx) => mWordle[idx];
   //public GuessStatus[] GetGuessStatus (int idx) => mWordle.ComputeGuessStatus (idx);
   ///// <summary>Indicates if the player may be allowed another guess</summary>
   //public bool SecretCracked => mWordle.SecretCracked;
   public bool CanRetry => !mWordle.SecretCracked && !mWordle.GameOver;

   readonly char[] mChars = new char[WordleGame.WORDLENGTH]; // Char inputs to be considered for guess word composition
   public readonly WordleGame mWordle = WordleGame.CreateNew ();
}
