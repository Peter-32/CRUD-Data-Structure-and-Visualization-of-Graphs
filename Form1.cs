using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Data_Structure_for_Graphs
{
    public partial class GraphWind : Form
    {
        private GraphicManager graphicsManager = new GraphicManager();
        
        public GraphWind()
        {
            InitializeComponent();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            graphicsManager.loadRoom();
            Graphics g = canvas.CreateGraphics();
            graphicsManager.startGraphics(g);
        }

        private void GraphWind_FormClosing(object sender, FormClosingEventArgs e)
        {
            graphicsManager.stopGraphics();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
