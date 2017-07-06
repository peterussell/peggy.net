using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeggyTheGameApp.src.Global
{
    public static class Utils
    {
        public static Direction GetDirectionFromString(string direction)
        {
            var directionMap = new Dictionary<string, Direction>();
            directionMap.Add("north", Direction.North);
            directionMap.Add("east", Direction.East);
            directionMap.Add("south", Direction.South);
            directionMap.Add("west", Direction.West);

            string d = direction.ToLower();
            if (!directionMap.Keys.Contains(d))
            {
                return Direction.Invalid;
            }
            return directionMap[d];
        }

        public static string Capitalize(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
