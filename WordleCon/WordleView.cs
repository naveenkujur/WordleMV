
class WordleView {
   public bool AcceptKey (ConsoleKeyInfo key) {
      return key.Key switch {
         ConsoleKey.Enter => mPlayer.EnterPressed (),
         ConsoleKey.Backspace => mPlayer.BackspacePressed (),
         _ => char.IsLetter (key.KeyChar) ? mPlayer.LetterPressed (key.KeyChar) : false,
      };
   }

   public static void Run () {
      var view = new WordleView ();
      view.Display ();
      Console.CursorVisible = false;
      while (view.NextTry) {
         var key = Console.ReadKey (intercept: true);
         if (!view.AcceptKey (key)) Console.Beep ();
         else view.Display ();
         if (key.Key == ConsoleKey.Escape) {
            Console.WriteLine ("Exited...");
            break;
         }
      }

   }

   public bool NextTry => mPlayer.NextTry;

   public void Display () {
      Console.Clear ();
      for (int row = 0; row < mPlayer.CGuess; row++) {
         char[] chars = mPlayer.GetGuess (row).ToCharArray ();
         GuessStatus[] statuses = mPlayer.GetGuessStatus (row);
         for (int i = 0; i < 5; i++) {
            var clr = statuses[i] switch {
               GuessStatus.Contains => ConsoleColor.Yellow,
               GuessStatus.Positioned => ConsoleColor.Green,
               _ => ConsoleColor.Gray,
            };
            Out (chars[i], clr);
         }
         Console.WriteLine ();
      }

      if (mPlayer.CGuess == 6) return;

      for (int i = 0; i < 5; i++) {
         char ch = i < mPlayer.CChars ? mPlayer[i] : '.';
         Out (ch, ConsoleColor.Gray);
      }
      Console.WriteLine ();

      for (int i = mPlayer.CGuess; i < 6; i++) {
         for (int j = 0; j < 5; j++)
            Out ('.', ConsoleColor.Gray);
         Console.WriteLine ();
      }
   }

   static void Out (char ch, ConsoleColor clr) {
      var oldClr = Console.ForegroundColor;
      Console.ForegroundColor = clr;
      Console.Write (ch); Console.Write (" ");
      Console.ForegroundColor = oldClr;
   }

   readonly WordlePlayer mPlayer = new WordlePlayer ();
}