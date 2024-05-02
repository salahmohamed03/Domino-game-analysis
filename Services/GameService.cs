


using MauiApp1.Data;
using Microsoft.AspNetCore.Components;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MauiApp1.Services
{
    public class GameService 
	{
        public struct PossiblePiece
        {
            public bool certain;
            public Tuple<int, int> piece;
        }
        public Dictionary<Player, (List<PossiblePiece>, int)> possiblePieces = new Dictionary<Player, (List<PossiblePiece>, int)>()
        {
            {Player.first,(new List<PossiblePiece>(), 7) },
            {Player.second,(new List<PossiblePiece>(), 7) },
            {Player.third,(new List<PossiblePiece>(), 7) },
            {Player.fourth,(new List<PossiblePiece>(), 7) }
        };
        public Settings settings;
        public Mode mode = Mode.start;
        public Player turn = Player.first;
        public Player prvTurn = Player.first;
        public List<Move> moves = new List<Move>();
        public Dictionary<string,List<string>> ConvertToDict(List<PossiblePiece> possiblePieces) 
        {
            var table = new Dictionary<string, List<string>>()
            {
                {"0",new List<string>()},
                {"1", new List<string>()},
                {"2", new List<string>()},
                {"3", new List<string>()},
                {"4", new List<string>()},
                {"5", new List<string>()},
                {"6", new List<string>()}
            };
            foreach(var piece in possiblePieces)
            {
                var color = piece.certain ? "1" : "0";
                table[piece.piece.Item1.ToString()].Add(piece.piece.Item2.ToString()+$",{color}");
                if(piece.piece.Item1 != piece.piece.Item2)
                    table[piece.piece.Item2.ToString()].Add(piece.piece.Item1.ToString()+$",{color}");
            }
            foreach (var item in table)
                item.Value.Sort();
            return table;
        }
        public List<Tuple<int,int>> PossiblePieces(Player player)
        {
            return possiblePieces[player].Item1.Select(x => x.piece).ToList();
        }
        public List<PossiblePiece> CanPlay(Player player ,int half1,int half2)
        {
            var canPlay = CouldBePlayed(half1,half2);
            return possiblePieces[player].Item1.Where(x => canPlay.Contains(x.piece)).ToList();
        }
        public void InitializePossiblePieces(List<Tuple<int, int>> myPieces)
        {
            var myPossiblePieces = myPieces.Select(x => new PossiblePiece { certain = true, piece = x }).ToList();
            var allPieces = Pieces.Select(x => x.Key).ToList();
            allPieces = allPieces.Except(myPossiblePieces.Select(p => p.piece)).ToList();
            var allPossiblePieces = allPieces.Select(x => new PossiblePiece { certain = false, piece = x}).ToList();

            possiblePieces[Player.first] = (myPossiblePieces,7);
            possiblePieces[Player.second] = (allPossiblePieces,7);
            possiblePieces[Player.third] = (allPossiblePieces,7);
            possiblePieces[Player.fourth] = (allPossiblePieces,7);
        }
        public void RemovePieces(Player player, int half)
        {
            var playerPieces = new List<PossiblePiece>(possiblePieces[player].Item1);
            playerPieces.RemoveAll(x => (x.piece.Item1 == half || x.piece.Item2 == half)&&!x.certain);
            possiblePieces[player] = (playerPieces, possiblePieces[player].Item2);
            UpdateAllPossiblePieces(player);
        }
        public void RemovePiece(Player player, Tuple<int, int> piece)
        {
			var playerPieces = new List<PossiblePiece>(possiblePieces[player].Item1);
			playerPieces.RemoveAll(x => x.piece.Equals(piece));
			possiblePieces[player] = (playerPieces, possiblePieces[player].Item2);
			UpdateAllPossiblePieces(player);
		}
        public void UpdateCertainty(Player player, Tuple<int, int> piece)
        {
            var playerPieces = possiblePieces[player];
            var pieceIndex = playerPieces.Item1.FindIndex(x => x.piece.Equals(piece));
            var updatedPiece = playerPieces.Item1[pieceIndex];
            updatedPiece.certain = true;
            playerPieces.Item1[pieceIndex] = updatedPiece;
            possiblePieces[player] = playerPieces;
            foreach(var p in possiblePieces)
            {
                if(p.Key != player)
                {
                    RemovePiece(p.Key, piece);
                }
            }
        }
        public void PiecePlayed(Tuple<int, int> piece)
        {
            foreach (var player in Enum.GetValues(typeof(Player)).Cast<Player>())
            {
                RemovePiece(player, piece);
            }
        }
        public Dictionary<Tuple<int, int>, int> Pieces = new Dictionary<Tuple<int, int>, int>
            {
                { new Tuple<int, int>(6, 6) ,0},
                { new Tuple<int, int>(5, 5) ,1},
                { new Tuple<int, int>(3, 4) ,2},
                { new Tuple<int, int>(0, 3) ,3},
                { new Tuple<int, int>(5, 6) ,4},
                { new Tuple<int, int>(4, 5) ,5},
                { new Tuple<int, int>(2, 4) ,6},
                { new Tuple<int, int>(2, 2) ,7},
                { new Tuple<int, int>(4, 6) ,8},
                { new Tuple<int, int>(3, 5) ,9},
                {  new Tuple<int, int>(1, 4),10 },
                {  new Tuple<int, int>(1, 2),11 },
                {  new Tuple<int, int>(3, 6),12 },
                {  new Tuple<int, int>(2, 5),13 },
                {  new Tuple<int, int>(0, 4),14 },
                {  new Tuple<int, int>(0, 2),15 },
                {  new Tuple<int, int>(2, 6),16 },
                {  new Tuple<int, int>(1, 5),17 },
                {  new Tuple<int, int>(3, 3),18 },
                {  new Tuple<int, int>(1, 1),19 },
                {  new Tuple<int, int>(1, 6),20 },
                {  new Tuple<int, int>(0, 5),21 },
                {  new Tuple<int, int>(2, 3),22 },
                {  new Tuple<int, int>(0, 1),23 },
                {  new Tuple<int, int>(0, 6),24 },
                {  new Tuple<int, int>(4, 4),25 },
                {  new Tuple<int, int>(1, 3),26 },
                {  new Tuple<int, int>(0, 0),27 }
            };
		public void SetSettings(Settings settings)
		{
			this.settings = settings;
		}
        public string ShowTable(Dictionary<string, List<string>> table)
        {
            StringBuilder htmlTable = new StringBuilder();

            htmlTable.AppendLine("<table style='width:40%; border: 1px solid black; border-collapse: collapse;'>");

            // Header row
            htmlTable.AppendLine("<tr style='background-color: #f2f2f2;'>");
            foreach (var key in table.Keys)
            {
                htmlTable.AppendLine($"<th style='border: 1px solid black; padding: 5px; text-align: left;'>{key}</th>");
            }
            htmlTable.AppendLine("</tr>");

            // Data rows
            int rowCount = table.Values.MaxBy(x => x.Count).Count;
            for (int i = 0; i < rowCount; i++)
            {
                htmlTable.AppendLine("<tr>");
                foreach (var column in table.Values)
                {
                    var cell = i < column.Count ? column[i] : "";
                    var cellParts = cell.Split(',');
                    var cellValue = cellParts[0];
                    var cellColor = cellParts.Length > 1 && cellParts[1] == "1" ? "white" : "black";
                    htmlTable.AppendLine($"<td style='border: 1px solid black; padding: 5px; text-align: left; color: {cellColor};'>{cellValue}</td>");
                }
                htmlTable.AppendLine("</tr>");
            }

            htmlTable.AppendLine("</table>");

            return htmlTable.ToString();
        }
		public string ShowTableTranspose(Dictionary<string, List<string>> table)
		{
			StringBuilder htmlTable = new StringBuilder();

			htmlTable.AppendLine("<table style='border: 1px solid black; border-collapse: collapse; margin:10px;'>");

			// Data rows
			foreach (var key in table.Keys)
			{
				htmlTable.AppendLine("<tr>");
				htmlTable.AppendLine($"<td style='border: 1px solid black; padding: 5px; text-align: left;background-color:white'>{key}</td>");
				for (int i = 0; i < table[key].Count; i++)
				{
					var cell = table[key][i];
					var cellParts = cell.Split(',');
					var cellValue = cellParts[0];
					var cellColor = cellParts.Length > 1 && cellParts[1] == "1" ? "white" : "black";
					htmlTable.AppendLine($"<td style='border: 1px solid black; padding: 5px; text-align: left; color: {cellColor};'>{cellValue}</td>");
				}
				htmlTable.AppendLine("</tr>");
			}

			htmlTable.AppendLine("</table>");

			return htmlTable.ToString();
		}
		public string Show(string question)
        {
            return "<h3>" + question + "</h3>";
        }
        public void Next()
        {
            turn = (turn != Player.fourth) ? turn + 1 : 0;
        }
        public void Previous()
        {
            turn = (turn != Player.first) ? turn - 1 : Player.fourth;
        }
        public void SetFirstMove(Move move)
        {
            moves.Add(move);
        }
        public List<Move> PlayerMoves(Player player)
        {
            return moves.Where(x => x.player == player).ToList();
        }
        public bool IsOnTheGround(int a, int b)
        {
            int min = Math.Min(a, b);
            int max = Math.Max(a, b);
            return moves.Any(x => x.min == min && x.max == max);
        }
        public Player PieceOwner(int a , int b)
        {
            int min = Math.Min(a , b);
            int max = Math.Max(a , b);
            var move = moves.FirstOrDefault(x => x.min == min && x.max == max);
            return move.player;
        }
		public void UpdateAllPossiblePieces(Player player)
		{
            if(player == Player.first)
				return;
			var playerPieces = possiblePieces[player];
			var diff = playerPieces.Item1.Select(x=>x.piece);
			foreach (var p in possiblePieces.Keys)
			{
				if (p != player)
				{
					var otherPlayerPieces = possiblePieces[p].Item1.Select(x=>x.piece);
					diff = diff.Except(otherPlayerPieces).ToList();
				}
			}
            for(int i = 0;i < playerPieces.Item1.Count;i++)
            {
                if(playerPieces.Item1[i].certain)
					continue;
				if (diff.Contains(playerPieces.Item1[i].piece))
                {
                    PossiblePiece update = new PossiblePiece()
                    {
                        certain = true,
                        piece = playerPieces.Item1[i].piece
                    };
					playerPieces.Item1[i] = update;
                }
			}
			possiblePieces[player] = playerPieces;
		}
        public List<Tuple<int,int>> RemainingMoves()
        {
            var playedMoves = moves.Select(x => x.Piece()).ToList();
            var remainingMoves = Pieces.Select(x => x.Key).ToList().Except(playedMoves).ToList();
            return remainingMoves;
        }
        public List<Tuple<int,int>> CertainPieces(Player player)
        {
            var certainPieces = possiblePieces[player].Item1.Where(x => x.certain).Select(x=>x.piece);
            return certainPieces.ToList();
        }
        public List<Tuple<int,int>> CouldBePlayed(int half1,int half2)
        {
			var remainingMoves = RemainingMoves();
			var couldBePlayed = remainingMoves.Where(x => x.Item1 == half1 || x.Item2 == half1 || x.Item1 == half2 || x.Item2 == half2).ToList();
			return couldBePlayed;
		}
		public int CalculateScore()
		{
			Func<Tuple<int, int>, int> PieceValue = (p) => p.Item1 + p.Item2;
			int score = 0;
			foreach (var move in RemainingMoves())
			{
				score += PieceValue(move);
			}
			return score;
		}
		public void NextMode()
		{
            do
            {
                mode = (mode != Mode.possibleMoves) ? mode + 1 : Mode.start;
            }while (mode == Mode.start || mode == Mode.firstMove);
		}
		public void PreviousMode()
		{
            do
            {
                mode = (mode != Mode.start) ? mode - 1 : Mode.possibleMoves;
            }while (mode == Mode.start || mode == Mode.firstMove);
		}
	}
    public class Move
    {
        public Player player;
        public int min;
        public int max;
        public int leftOpen;
        public int closed;
        public int opened;
        public Tuple<int,int> Piece()
        {
            return new Tuple<int,int>(min, max);
        }
    }
    public enum Mode
    {
        start,
        firstMove,
        mainStream,
        possiblePieces,
        possibleMoves
    }
    public enum Player
    {
        first,
        second,
        third,
        fourth,
    }
}

