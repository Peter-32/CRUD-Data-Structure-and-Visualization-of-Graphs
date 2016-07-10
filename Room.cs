using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Structure_for_Graphs
{
    class Room
    {
        private static TextureID[,] blocks = new TextureID[GraphicManager.ROOM_TILE_WIDTH, GraphicManager.ROOM_TILE_HEIGHT];

        public static TextureID[,] Blocks
        {
            get { return blocks; }
            set { blocks = value; }
        }

        public static void initRoom()
        {
            for (int x = 0; x < GraphicManager.ROOM_TILE_WIDTH; x++)
            {
                for (int y = 0; y < GraphicManager.ROOM_TILE_HEIGHT; y++)
                {
                    if (Controller.isNode(x,y))
                    {
                        blocks[x, y] = TextureID.node;
                    }
                    else
                    {
                        blocks[x, y] = TextureID.blank;
                    }
                }
            }
        }

    }
}
