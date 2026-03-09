namespace Wordle;

/// <summary>Status of guess word letter (whether present in the secret word or not)</summary>
public enum GuessStatus {
   /// <summary>This guess word letter is not present in the secret word</summary>
   Outlier,
   /// <summary>This guess word letter is present in the secret word</summary>
   Present,
   /// <summary>This guess word letter occurred at the same position as in the secret word</summary>
   Positioned
};

/// <summary>Status of each letter in the alphabet (considering their use in guess words)</summary>
public enum AlphabetStatus {
   /// <summary>This letter is not yet used in any guess words</summary>
   Unused,
   /// <summary>This letter is used in some guess words</summary>
   Used,
   /// <summary>This letter is used in some guess words, and is present in the secret word</summary>
   Present,
   /// <summary>This letter is used in some guess words, and it occurred at the same position as in the secret word</summary>
   Positioned
};

/// <summary>Wordle game state manager</summary>
public class WordleGame {
   /// <summary>Constructs a new Wordle game state</summary>
   WordleGame (string secret) => mSecret = secret;

   /// <summary>Creates a new Wordle game</summary>
   public static WordleGame CreateNew () => new (WordsList.It.GetRandomWord ());

   /// <summary>Number of letters making up a word</summary>
   public const int WORDLENGTH = 5;
   /// <summary>Number of guesses allowed</summary>
   public const int MAXGUESSES = 6;

   /// <summary>Accepts given guess word after due validation</summary>
   /// <returns>false if the given guess word is invalid</returns>
   internal bool AcceptGuess (string gword) {
      if (!WordsList.It.Contains (gword)) return false;
      if (SecretCracked || GameOver || gword.Length != WORDLENGTH || !gword.All (char.IsAsciiLetterUpper))
         throw new Exception ("Coding error");
      mGuesses[CGuess++] = gword;
      SecretCracked = gword == mSecret;
      return true;
   }

   /// <summary>Compute guess letter status, for a particular guess</summary>
   public GuessStatus[] ComputeGuessStatus (int guessIdx) {
      if (guessIdx >= CGuess) throw new Exception ("Coding error");
      var gchars = this[guessIdx].ToCharArray ();
      var schars = mSecret.ToCharArray ();
      GuessStatus[] statuses = new GuessStatus[5]; // Note: All guess letter statuses are set to Outlier!
      for (int i = 0; i < WORDLENGTH; i++) {
         if (gchars[i] != schars[i]) continue;
         statuses[i] = GuessStatus.Positioned;
         schars[i] = '-';
      }
      for (int i = 0; i < WORDLENGTH; i++) {
         if (statuses[i] == GuessStatus.Positioned) continue;
         var ch = gchars[i];
         int idx = schars.IndexOf (ch);
         if (idx == -1) continue;
         statuses[i] = GuessStatus.Present;
         schars[idx] = '-';
      }
      return statuses;
   }

   /// <summary>Compute alphabet letter coverage status</summary>
   public AlphabetStatus[] ComputeAlphabetStatus () {
      AlphabetStatus[] statuses = new AlphabetStatus[26]; // All are marked Unused
      // Positioned status is quite involved! [Need to walk the letters/statuses making up each guess word]
      var schars = mSecret.ToCharArray ();
      for (int guessIdx = 0; guessIdx < CGuess; guessIdx++) {
         var gstatuses = ComputeGuessStatus (guessIdx);
         var gchars = this[guessIdx].ToCharArray ();
         for (int letterIdx = 0; letterIdx < WORDLENGTH; letterIdx++) {
            switch (gstatuses[letterIdx]) {
               case GuessStatus.Present: {
                  var idx = gchars[letterIdx] - 'A';
                  if (statuses[idx] != AlphabetStatus.Positioned)
                     statuses[idx] = AlphabetStatus.Present;
                  break;
               }
               case GuessStatus.Positioned:
                  statuses[schars[letterIdx] - 'A'] = AlphabetStatus.Positioned;
                  break;
               case GuessStatus.Outlier: {
                  var idx = gchars[letterIdx] - 'A';
                  if (statuses[idx] == AlphabetStatus.Unused)
                     statuses[idx] = AlphabetStatus.Used;
                  break;
               }
               default: throw new IndexOutOfRangeException ();
            }
         }
      }
      return statuses;
   }

   /// <summary>Gets the Nth accepted guess word</summary>
   public string this[int index] {
      get {
         if (index >= CGuess) throw new Exception ("Coding error");
         return mGuesses[index];
      }
   }

   /// <summary>Has the secret been cracked?</summary>
   public bool SecretCracked { get; private set; }
   /// <summary>No more guess tries left</summary>
   public bool GameOver => CGuess == MAXGUESSES;

   /// <summary>Count of guesses accepted</summary>
   public int CGuess { get; private set; }

   readonly string mSecret;
   readonly string[] mGuesses = new string[MAXGUESSES];
}

/// <summary>Valid words dictionary (singleton)</summary>
class WordsList {
   WordsList () {
      // Load and initialize the words list!
   }

   public static readonly WordsList It = new ();

   public bool Contains (string _) => true;
   public string GetRandomWord () => "HELLO";
}
