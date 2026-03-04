
public enum GuessStatus { OutOfRange, Contains, Positioned }; // Secret
//enum AlphabetStatus { Unused, Used, Contains, Positioned }; // Guesses & Secret

class Wordle {
   public Wordle () {
      mSecret = GetSecretWord ();
   }

   public bool AcceptGuess (string gword) {
      if (FoundSecret || GameOver || gword.Length != 5)
         throw new Exception ("Coding error");
      mGuesses[CGuess++] = gword;
      FoundSecret = gword == mSecret;
      return true;
   }

   public GuessStatus[] ComputeStatus (int guessIdx) {
      if (guessIdx >= CGuess) throw new Exception ("Coding error");
      var gchars = mGuesses[guessIdx].ToCharArray ();
      var schars = mSecret.ToCharArray ();
      GuessStatus[] statuses = new GuessStatus[5];
      for (int i = 0; i < 5; i++) {
         if (gchars[i] != schars[i]) continue;
         statuses[i] = GuessStatus.Positioned;
         gchars[i] = schars[i] = '-';
      }
      for (int i = 0; i < 5; i++) {
         var ch = gchars[i];
         if (ch == '-') continue;
         int idx = schars.IndexOf (ch);
         if (idx == -1) continue;
         statuses[i] = GuessStatus.Contains;
         schars[i] = '-';
      }
      return statuses;
   }

   public string this[int index] {
      get {
         if (index >= CGuess) throw new Exception ("Coding error");
         return mGuesses [index];
      }
   }

   public bool FoundSecret { get; private set; }
   public bool GameOver => CGuess == 6;

   static string GetSecretWord () => "HELLO";
   public static bool IsDictWord (string word) => true;

   public int CGuess { get; private set; }

   readonly string mSecret;
   readonly string[] mGuesses = new string[6];
}