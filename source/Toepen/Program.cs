// Game game = new Game();
//
// if (!game.AddPlayer(new Player("Niels")) ||
//     !game.AddPlayer(new Player("Mylo")) ||
//     !game.AddPlayer(new Player("Jens")) ||
//     !game.AddPlayer(new Player("Sam")) ||
//     !game.AddPlayer(new Player("Bas")) ||
//     !game.AddPlayer(new Player("Gijs"))
//    )
// {
//     Console.WriteLine("Too many players! Max 6");
//     return;
// }
//
// game.Start();
//
// // foreach (Card card in game.Deck)
// // {
// //     Console.WriteLine(card);
// // }
// foreach (Player player in game.Players)
// {
//     Console.WriteLine("--------------------------"); 
//     Console.WriteLine($"{player.Id} {player.Name}");
//     foreach (Card card in player.Hand)
//     {
//         Console.WriteLine(card);
//     }
//
//     if (player.HasDirtyLaundry())
//     {
//         Console.WriteLine("Heeft vuile was");
//     }
//
//     if (player.HasWhiteLaundry())
//     {
//         Console.WriteLine("Heeft witte was");
//     }
// }
//
// Console.WriteLine();
// Console.WriteLine();
// Console.WriteLine("---------------------");
// Console.WriteLine("Available commands:");
// Console.WriteLine("---------------------");
// Console.WriteLine("vuilewas");
// Console.WriteLine("wittewas");
// Console.WriteLine("turnslaundry");
// Console.WriteLine("check");
// Console.WriteLine("fold");
// Console.WriteLine("knock");
// Console.WriteLine("---------------------");
// while (true)
// {
//     // =========================================================
//     // readline
//     // =========================================================
//     string? command = Console.ReadLine();
//     
//     if (command == "vuilewas")
//     {
//         game.DirtyLaundry(1);
//         Console.WriteLine();
//         Console.WriteLine("---------------------");
//         Console.WriteLine("Player 1 zegt vuilewas te hebben");
//         Console.WriteLine("---------------------");
//     }
//     else if (command == "wittewas")
//     {
//         game.WhiteLaundry(1);
//         Console.WriteLine();
//         Console.WriteLine("---------------------");
//         Console.WriteLine("Player 1 zegt wittewas te hebben");
//         Console.WriteLine("---------------------");
//     }
//     else if (command == "turnslaundry")
//     {
//         Thread.Sleep(1000);
//         Console.WriteLine(1);
//         Thread.Sleep(1000);
//         Console.WriteLine(2);
//         Thread.Sleep(1000);
//         Console.WriteLine(3);
//         Thread.Sleep(1000);
//         Console.WriteLine(4);
//         Thread.Sleep(1000);
//         Console.WriteLine(5);
//         
//         game.TurnsLaundry(2);
//         Console.WriteLine();
//         Console.WriteLine("---------------------");
//         Console.WriteLine("Player 2 draait de was om");
//         Console.WriteLine("---------------------");
//     }
//     else if (command == "check")
//     {
//         game.Check(2);
//         Console.WriteLine();
//         Console.WriteLine("---------------------");
//         Console.WriteLine("Player 2 checkt");
//         Console.WriteLine("---------------------");
//     }
//     else if (command == "fold")
//     {
//         game.Fold(2);
//         Console.WriteLine();
//         Console.WriteLine("---------------------");
//         Console.WriteLine("Player 2 fold");
//         Console.WriteLine("---------------------");
//     }
//     else if (command == "knock")
//     {
//         game.Knock(1);
//         Console.WriteLine();
//         Console.WriteLine("---------------------");
//         Console.WriteLine("Player 1 klopt");
//         Console.WriteLine("---------------------");
//     }
//     else
//     {
//         Console.WriteLine();
//         Console.WriteLine();
//         Console.WriteLine("Incorrect commands:");
//         Console.WriteLine("---------------------");
//         Console.WriteLine("vuilewas");
//         Console.WriteLine("wittewas");
//         Console.WriteLine("turnslaundry");
//         Console.WriteLine("check");
//         Console.WriteLine("fold");
//         Console.WriteLine("knock");
//         Console.WriteLine("---------------------");
//     }
// }
