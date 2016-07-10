using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Data_Structure_for_Graphs
{
    class GEngine
    {
        /*----Members---------------*/
        private Graphics drawHandle;
        private Thread renderThread;

        // load assets here


        /*----Functions-------------*/
        public GEngine (Graphics g)
        {
            drawHandle = g;
        }

        // Takes care of initializing all important things
        public void init()
        {
            // Starts the render thread
            renderThread = new Thread(new ThreadStart(render));
            renderThread.Start();
        }

        public void stop()
        {
            renderThread.Abort();
        }

        private void render()
        {
            // Objects used for constructing the individual frames of the game
            Bitmap frame = new Bitmap(GraphicManager.CANVAS_WIDTH, GraphicManager.CANVAS_HEIGHT);
            Graphics frameGraphics = Graphics.FromImage(frame);

            TextureID[,] textures = Room.Blocks;
            Pen pen = new System.Drawing.Pen(Color.Black,5);
            Pen lightPen = new System.Drawing.Pen(Color.Black, 3);
            Font font = new Font("Courier New", 20);

            int edgeListLength = Model.edgeList.Count();
            int nodeListLength = Model.nodeList.Count();
            Point startPoint;
            Point endPoint;

            do
            {
                // Background Color
                frameGraphics.FillRectangle(new SolidBrush(Color.White), 0, 0, GraphicManager.CANVAS_WIDTH, GraphicManager.CANVAS_HEIGHT);

                // Draw edges
                // Use the + Model.TILE_SIDE_LENGTH / 2; in order to select the center of the nodes.
                for (int i = 0; i < edgeListLength; i++)
                {
                    startPoint = new Point(Model.edgeList[i].node1.xLocation * GraphicManager.TILE_SIDE_LENGTH + GraphicManager.TILE_SIDE_LENGTH / 2, 
                        Model.edgeList[i].node1.yLocation * GraphicManager.TILE_SIDE_LENGTH + GraphicManager.TILE_SIDE_LENGTH / 2);
                    endPoint = new Point(Model.edgeList[i].node2.xLocation * GraphicManager.TILE_SIDE_LENGTH + GraphicManager.TILE_SIDE_LENGTH / 2,
                        Model.edgeList[i].node2.yLocation * GraphicManager.TILE_SIDE_LENGTH + GraphicManager.TILE_SIDE_LENGTH / 2);
                    frameGraphics.DrawLine(lightPen, startPoint, endPoint);
                }

                // Draw node or blank
                for (int x = 0; x < GraphicManager.ROOM_TILE_WIDTH; x++)
                {
                    for (int y = 0; y < GraphicManager.ROOM_TILE_HEIGHT; y++)
                    {
                        switch (textures[x, y])
                        {
                            case TextureID.blank:
                                break;
                            case TextureID.node:
                                frameGraphics.FillEllipse(new SolidBrush(Color.White), x * GraphicManager.TILE_SIDE_LENGTH, y * GraphicManager.TILE_SIDE_LENGTH, GraphicManager.TILE_SIDE_LENGTH, GraphicManager.TILE_SIDE_LENGTH);
                                frameGraphics.DrawEllipse(pen, x * GraphicManager.TILE_SIDE_LENGTH, y * GraphicManager.TILE_SIDE_LENGTH, GraphicManager.TILE_SIDE_LENGTH, GraphicManager.TILE_SIDE_LENGTH);
                                break;
                        }
                    }
                }

                // Draw labels on nodes
                for (int i = 0; i < nodeListLength; i++)
                {
                    frameGraphics.DrawString(Model.nodeList[i].key, font, new SolidBrush(Color.Black), 
                        new Point(Model.nodeList[i].xLocation * GraphicManager.TILE_SIDE_LENGTH + GraphicManager.TILE_SIDE_LENGTH / 4, 
                                  Model.nodeList[i].yLocation * GraphicManager.TILE_SIDE_LENGTH + GraphicManager.TILE_SIDE_LENGTH / 4));
                }
                


                // Draw the frame on the canvas
                drawHandle.DrawImage(frame, 0, 0);
            }
            while (false);
        }
    }
}
