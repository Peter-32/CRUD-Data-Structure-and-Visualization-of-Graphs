using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Data_Structure_for_Graphs
{
    class GraphicManager
    {
        /*------Constants----------*/
        public const int CANVAS_HEIGHT = 700;
        public const int CANVAS_WIDTH = 1200;
        public const int ROOM_TILE_WIDTH = 24;
        public const int ROOM_TILE_HEIGHT = 10;
        public const int TILE_SIDE_LENGTH = 50;

        /*------Members------------*/
        private GEngine gEngine;

        // Load the room
        public void loadRoom()
        {
            Room.initRoom();
        }

        // Launches the graphics engine
        public void startGraphics(Graphics g)
        {
            gEngine = new GEngine(g);
            gEngine.init();
        }

        public void stopGraphics()
        {
            gEngine.stop();
        }
    }

    public enum TextureID
    {
        blank,
        node
    }
}
