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
            string[] tokens = cmd.ToLower().Split(' ');
            switch (tokens[0])
            {
                case "go":
                    return HandleGo(tokens);
                case "look":
                    return HandleLook(tokens);
                case "talk":
                    return HandleTalk(tokens);
                default:
                    return "Use your words.\r\n";
            }
        }

        private string HandleGo(string[] tokens)
        {
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
                            containerName += tokens[i] + " ";
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
                    characterName += tokens[i] + " ";
                }
                return _world.TalkToCharacter(characterName.TrimEnd());
            }
            return "Who do you want to talk to?\r\n";
        }
    }
}
