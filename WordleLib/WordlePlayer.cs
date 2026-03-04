
public class WordlePlayer {
   public bool LetterPressed (char ch) {
      if (CChars == 5) return false;
      mChars[CChars++] = char.ToUpper (ch);
      return true;
   }

   public bool EnterPressed () {
      var word = new string (mChars);
      if (CChars != 5 || !Wordle.IsDictWord (word) || !mWordle.AcceptGuess (word)) return false;
      CChars = 0;
      return true;
   }

   public bool BackspacePressed () {
      if (CChars == 0) return false;
      CChars--;
      return true;
   }

   public char this[int index] {
      get {
         if (index >= CChars) throw new Exception ("Coding error");
         return mChars[index];
      }
   }

   public bool NextTry => !mWordle.FoundSecret || !mWordle.GameOver;

   public int CChars { get; private set; }

   public int CGuess => mWordle.CGuess;
   public string GetGuess (int idx) => mWordle[idx];
   public GuessStatus[] GetGuessStatus (int idx) => mWordle.ComputeStatus (idx);

   readonly char[] mChars = new char[5];
   readonly Wordle mWordle = new Wordle ();
}