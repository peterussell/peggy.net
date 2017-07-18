using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeggyTheGameApp.src
{
    public class GameController
    {
        private GameWorld _world;

        public GameController()
        {
            _world = new GameWorld();
            _world.LoadMapFromJson(@"./data/map1.json");
        }

        public string HandleCommand(string cmd)
        {
            string[] origTokens = cmd.Split(' ');
            string[] lowerTokens = cmd.ToLower().Split(' ');
            switch (lowerTokens[0])
            {
                case "go":
                    return HandleGo(origTokens);
                case "look":
                    return HandleLook(origTokens);
                case "talk":
                    return HandleTalk(origTokens);
                case "take":
                    return HandleTake(origTokens);
                case "drop":
                    return HandleDrop(origTokens);
                case "give":
                    return HandleGive(origTokens);
                case "inventory":
                    return HandleLookInInventory(origTokens);
                default:
                    return "Use your words.\r\n";
            }
        }
        
        public bool CheckForEndGame()
        {
            return _world.IsEndGameSatisfied();
        }

        public string GetEndGameText()
        {
            return _world.GetEndGameText();
        }

        private string HandleGo(string[] tokens)
        {
            if (tokens.Length < 2)
            {
                return "But, go where?\r\n";
            }
            string direction = tokens[1];
            return _world.MovePeggy(direction);
        }

        private string HandleLook(string[] tokens)
        {
            if (tokens.Length > 1)
            {
                // "look in (x)"
                if (tokens[1] == "in")
                {
                    if (tokens.Length > 2)
                    {
                        string containerName = "";
                        for (int i = 2; i < tokens.Length; i++)
                        {
                            containerName += $"{tokens[i]} ";
                        }
                        return _world.LookInContainer(containerName.TrimEnd());
                    }
                }
            }

            return _world.Look();
        }

        private string HandleTalk(string[] tokens)
        {
            // "talk to (x)"
            if (tokens.Length > 2 && tokens[1] == "to")
            {
                string characterName = "";
                for (int i = 2; i < tokens.Length; i++)
                {
                    characterName += $"{tokens[i]} ";
                }
                return _world.TalkToCharacter(characterName.TrimEnd());
            }
            return "Who do you want to talk to?\r\n";
        }

        private string HandleTake(string[] tokens)
        {
            const string helpText = "Try \"take (thing) from (place)\".";
            if (tokens.Length < 4 || !tokens.Contains("from"))
            {
                return $"I need some more information. {helpText}\r\n";
            }

            string item = "";
            string takeFrom = "";
            int fromIndex = Array.FindIndex(tokens, t => t.ToLower().Equals("from"));
            for (int i=1; i<fromIndex; i++)
            {
                item += $"{tokens[i]} ";
            }
            item = item.TrimEnd();

            if (fromIndex + 1 == tokens.Length)
            {
                // No container specified after 'from'
                return $"Where do you want to take {item} from? {helpText}\r\n";
            }

            for (int i=fromIndex+1; i<tokens.Length; i++)
            {
                takeFrom += $"{tokens[i]} ";
            }
            takeFrom = takeFrom.TrimEnd();

            return _world.TakeItemFrom(item, takeFrom);
        }

        private string HandleDrop(string[] tokens)
        {
            // TODO: looks supsiciously similar to HandleTake() and HandleGive(),
            //       refactor these into a shared method.
            const string helpText = "Try \"drop (thing) in (place)\".";
            if (tokens.Length < 4 || !tokens.Contains("in"))
            {
                return $"I need some more information. {helpText}\r\n";
            }

            string item = "";
            string dropIn = "";
            int inIndex = Array.FindIndex(tokens, t => t.ToLower().Equals("in"));
            for (int i=1; i<inIndex; i++)
            {
                item += $"{tokens[i]} ";
            }
            item = item.TrimEnd();

            for (int i=inIndex+1; i<tokens.Length; i++)
            {
                dropIn += $"{tokens[i]} ";
            }
            dropIn = dropIn.TrimEnd();

            return _world.DropItemIn(item, dropIn);
        }

        private string HandleGive(string[] tokens)
        {
            // TODO: looks supsiciously similar to HandleTake() and HandleGive(),
            //       refactor these into a shared method.
            const string helpText = "Try \"give (thing) to (person)\".";
            if (tokens.Length < 4 || !tokens.Contains("to"))
            {
                return $"I need some more information. {helpText}\r\n";
            }

            string item = "";
            string giveTo = "";
            int inIndex = Array.FindIndex(tokens, t => t.ToLower().Equals("to"));
            for (int i = 1; i < inIndex; i++)
            {
                item += $"{tokens[i]} ";
            }
            item = item.TrimEnd();

            for (int i = inIndex + 1; i < tokens.Length; i++)
            {
                giveTo += $"{tokens[i]} ";
            }
            giveTo = giveTo.TrimEnd();

            return _world.GiveItemTo(item, giveTo);
        }

        private string HandleLookInInventory(string[] tokens)
        {
            return _world.LookInInventory();
        }
    }
}
