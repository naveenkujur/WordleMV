namespace WordleCon;

using Wordle;

/// <summary>Interaction b/w view (Console) and model space (Reusable Wordle business logic)</summary>
static class WordleView {
   /// <summary>Handler for all the key presses</summary>
   static bool AcceptKey (ConsoleKeyInfo key) {
      return key.Key switch {
         ConsoleKey.Enter => mPlayer.EnterPressed (),
         ConsoleKey.Backspace => mPlayer.BackspacePressed (),
         _ => char.IsAsciiLetter (key.KeyChar) && mPlayer.LetterPressed (key.KeyChar),
      };
   }

   /// <summary>Entry point for the application</summary>
   public static void Run () {
      Console.Clear ();
      Display ();
      Console.CursorVisible = false;
      while (true) {
         var key = Console.ReadKey (intercept: true);
         if (key.Key == ConsoleKey.Escape) {
            Console.WriteLine ("Player exited the game...");
            break;
         }

         if (!AcceptKey (key)) {
            Console.WriteLine ("Key input ignored!");
            continue;
         } else Display ();

         if (mPlayer.mWordle.SecretCracked) {
            Console.WriteLine ("Congratulations, you have cracked the secret.");
            break;
         }
         if (!mPlayer.CanRetry) {
            Console.WriteLine ("No more tries... Game over.");
            break;
         }
      }
      Console.ResetColor ();
      Console.CursorVisible = true;
   }

   /// <summary>Refreshes the game display</summary>
   static public void Display () {
      Console.SetCursorPosition (0, 0);
      WordleGame w = mPlayer.mWordle;
      for (int row = 0; row < w.CGuess; row++) {
         char[] chars = w[row].ToCharArray ();
         GuessStatus[] statuses = w.ComputeGuessStatus (row);
         for (int i = 0; i < 5; i++) {
            var clr = statuses[i] switch {
               GuessStatus.Present => ConsoleColor.Yellow,
               GuessStatus.Positioned => ConsoleColor.Green,
               _ => ConsoleColor.DarkGray,
            };
            Out (chars[i], clr);
         }
         Console.WriteLine ();
      }

      if (w.CGuess == WordleGame.MAXGUESSES) return;

      for (int i = 0; i < WordleGame.WORDLENGTH; i++) {
         char ch = i < mPlayer.CChars ? mPlayer[i] : '.';
         Out (ch, ConsoleColor.Gray);
      }
      Console.WriteLine ();

      for (int i = w.CGuess + 1; i < WordleGame.MAXGUESSES; i++) {
         for (int j = 0; j < 5; j++)
            Out ('.', ConsoleColor.Gray);
         Console.WriteLine ();
      }

      Console.WriteLine ();
      Console.WriteLine ();

      // Alphabet letter statuses
      {
         var statuses = w.ComputeAlphabetStatus ();
         for (int i = 0; i < 26; i++) {
            if (i == 13) Console.WriteLine ();
            var clr = statuses[i] switch {
               AlphabetStatus.Present => ConsoleColor.Magenta,
               AlphabetStatus.Positioned => ConsoleColor.Green,
               AlphabetStatus.Used => ConsoleColor.DarkGray,
               _ => ConsoleColor.Gray,
            };
            Out ((char)('A' + i), clr);
         }
      }
      Console.WriteLine ();
   }

   static void Out (char ch, ConsoleColor clr) {
      var oldClr = Console.ForegroundColor;
      Console.ForegroundColor = clr;
      Console.Write (ch); Console.Write (" ");
      Console.ForegroundColor = oldClr;
   }

   static readonly WordlePlayer mPlayer = new ();
}
