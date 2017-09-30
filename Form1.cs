using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace Labels
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var myPane = zedGraphControl1.GraphPane;
            var sinWave = new PointPairList();
            var cosWave = new PointPairList();

            const int count = 50;

            myPane.YAxis.Scale.Min = -1.2;
            myPane.YAxis.Scale.Max = 1.2;
            myPane.YAxis.Scale.MajorStep = 0.2;
            myPane.YAxis.Scale.MinorStep = 0.2;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.Color = Color.Gray;
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            myPane.YAxis.MajorGrid.DashOff = 0;

            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = count;
            myPane.XAxis.Scale.MajorStep = count * 0.1;
            myPane.XAxis.Scale.MinorStep = myPane.XAxis.Scale.MajorStep;
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.Color = Color.Gray;
            myPane.XAxis.MajorGrid.IsZeroLine = false;
            myPane.XAxis.MajorGrid.DashOff = 0;

            myPane.Legend.IsVisible = false;

            var divisor = 1.0 / count;
            for (var i = 0; i <= count; i++)
            {
                sinWave.Add(i, Math.Sin(2 * Math.PI * i * divisor));
                cosWave.Add(i, Math.Cos(2 * Math.PI * i * divisor));
            }

            var sinLine = myPane.AddCurve("Sine Wave", sinWave, Color.Blue, SymbolType.None);
            sinLine.Line.Width = 3;
            sinLine.Line.DashOn = 1;
            sinLine.Line.DashOff = 1;
            sinLine.Line.Style = DashStyle.Dash;
            CreateLabel(myPane, sinLine, 25, 0.5, 22);
            var cosLine = myPane.AddCurve("Cosine Wave", cosWave, Color.Red, SymbolType.None);
            cosLine.Line.Width = 3;
            cosLine.Line.Style = DashStyle.Solid;
            CreateLabel(myPane, cosLine, 13, -0.7, 17);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Refresh();

        }

        private void CreateLabel(GraphPane pane, LineItem curve, double locationX, double locationY, int pointToIndex)
        {
            var textObject = new TextObj(curve.Label.Text, locationX, locationY, CoordType.AxisXYScale)
            {
                FontSpec =
                {
                    Border =
                    {
                        Color = curve.Color,
                        Width = curve.Line.Width,
                        Style = curve.Line.Style,
                        IsVisible = curve.Line.IsVisible,
                        InflateFactor = 1.5f
                    }
                },
                Location =
                {
                    AlignH = curve.Points[pointToIndex].X > locationX
                        ? AlignH.Right
                        : AlignH.Left,
                    AlignV = curve.Points[pointToIndex].Y > locationY
                        ? AlignV.Top
                        : AlignV.Bottom
                }
            };

            var lineObject = new LineObj(locationX, locationY, curve.Points[pointToIndex].X, curve.Points[pointToIndex].Y)
            {
                Line =
                {
                    Color = curve.Color,
                    Width = curve.Line.Width,
                    Style = curve.Line.Style,
                    IsVisible = curve.Line.IsVisible
                }
            };

            pane.GraphObjList.Add(textObject);
            pane.GraphObjList.Add(lineObject);
        }

    }
}
