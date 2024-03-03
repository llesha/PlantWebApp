using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PlantLib;
using SvgNet;
using SvgNet.SvgElements;
using System.IO;
using System.Windows;
using Point = System.Drawing.Point;

namespace PleaseWorkPlant
{
    public partial class _Default : Page
    {
        public readonly List<Color> segmentLightColors = new List<Color>
        {Color.DarkSeaGreen, Color.PaleGreen, Color.Olive,
            Color.BlanchedAlmond, Color.Khaki, Color.Firebrick };
        public readonly List<Color> segmentShadowColors = new List<Color>
        {Color.Green, Color.SeaGreen, Color.DarkOliveGreen, Color.Moccasin,
            Color.DarkKhaki, Color.Maroon};
        public readonly List<Color> leafLightColors = new List<Color>
        { Color.LightGreen, Color.MediumAquamarine, Color.Gold, Color.Crimson };
        public readonly List<Color> leafShadowColors = new List<Color>
        { Color.MediumSeaGreen, Color.Teal, Color.Goldenrod, Color.DarkRed };
        public readonly List<Color> grainColors = new List<Color>
        { Color.Tan, Color.Wheat, Color.Goldenrod, Color.DarkGoldenrod, Color.SandyBrown };
        public readonly List<Color> berryLightColors = new List<Color>
        { Color.HotPink, Color.MediumTurquoise, Color.Crimson,
            Color.SeaShell, Color.IndianRed, Color.MediumBlue };
        public readonly List<Color> berryShadowColors = new List<Color>
        { Color.MediumVioletRed, Color.Teal, Color.Brown,
            Color.OldLace, Color.DarkRed, Color.DarkBlue };
        public readonly List<Color> flowerPetal1Colors = new List<Color>
        { Color.Yellow,Color.LightYellow, Color.Crimson,
            Color.PaleVioletRed, Color.Thistle, Color.DeepSkyBlue };
        public readonly List<Color> flowerPetal2Colors = new List<Color>
        { Color.Orange, Color.Yellow, Color.DarkRed,
            Color.Pink, Color.Snow, Color.AliceBlue };
        public readonly List<Color> flowerCenter1Colors = new List<Color>
        { Color.Yellow, Color.LightBlue, Color.GhostWhite,
            Color.FloralWhite, Color.NavajoWhite };
        public readonly List<Color> flowerCenter2Colors = new List<Color>
        { Color.Snow, Color.AliceBlue, Color.Khaki, Color.Bisque, Color.LemonChiffon };
        int Xoffset = 60;
        static Random rnd = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Image canvas = new Bitmap(89, 90);
            //Graphics gr = Graphics.FromImage(canvas);

            //gr.DrawLine(new Pen(Color.Black, 2f), new Point(0, 0), new Point(10, 10));
            //canvas.Save(Server.MapPath("FVDTest.jpg"));

            SvgLineElement f = new SvgLineElement();
            f.X1 = 10;
            f.X2 = 20;
            f.Y1 = 10;
            f.Y2 = 20;
            SvgEllipseElement ell = new SvgEllipseElement();
            ell.CX = 10;
            ell.CY = 10;
            ell.RX = 10;
            ell.RY = 20;
            //toWrite.AddChild(ell);

            SvgNet.SvgGdi.SvgGraphics g = new SvgNet.SvgGdi.SvgGraphics();

            //string toFile = g.WriteSVGString();
            //File.WriteAllText(Server.MapPath("other.svg"), toFile);
            //ImageContainer.ImageUrl ="other.svg";
        }

        protected void CreateNewPlant_Click(object sender, EventArgs e)
        {
            DrawPlant(250);
        }

        protected void DownloadPlant_Click(object sender, EventArgs e)
        {
            Response.ContentType = "plant.svg";
            Response.AppendHeader("Content-Disposition", "attachment; filename=plant.svg");
            Response.TransmitFile("plant.svg");
            Response.End();
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image = null;
            DrawPlant(Xoffset);

            Xoffset += 100;
        }

        private void DrawFlower(SvgNet.SvgGdi.SvgGraphics g, Plant.Segment segm, Plant p,
            Plant.Flower fl, List<Color> palette)
        {
            float lcoef;
            int yOffset;

            lcoef = Convert.ToSingle(1 + rnd.NextDouble());
            yOffset = (int)(fl.width - (fl.width / 2 * lcoef));

            // CHANGING TO ZERO SLIGHTLY CHANGES FLOWER PERSPECTIVE.

            //yOffset = 0;

            GraphicsPath bigEl = new GraphicsPath();
            bigEl.AddEllipse(segm.finish.X - (fl.width * lcoef) / 2,
                segm.finish.Y - fl.width / 2 + yOffset,
                fl.width, fl.width);
            GraphicsPath smallEl = new GraphicsPath();
            smallEl.AddEllipse(segm.finish.X - fl.sWidth * lcoef / 2,
                segm.finish.Y - fl.sWidth / 2, fl.sWidth, fl.sWidth);

            PointF[] bigArr = bigEl.PathPoints;
            PointF[] smallArr = smallEl.PathPoints;
            GraphicsPath petals = new GraphicsPath();
            if (fl.petalsNum < 6)
            {
                for (int i = 0; i < bigArr.Length - 1; i += fl.petalsNum - 1)
                {
                    //petals.AddLine(bigArr[i], smallArr[i]);
                    petals.AddCurve(new PointF[] { smallArr[i], bigArr[i + 1],
                        smallArr[i + 2] }, 2);
                }
            }
            else
            {
                for (int i = 0; i < bigArr.Length - 2; i += 1)
                {
                    //petals.AddLine(bigArr[i], smallArr[i]);
                    petals.AddCurve(new PointF[] { smallArr[i], bigArr[i + 1],
                        smallArr[i + 2] }, 2);
                }
            }
            Matrix m = new Matrix();
            m.Translate(segm.finish.X, segm.finish.Y);
            m.Scale(lcoef, 1);
            m.Translate(-segm.finish.X, -segm.finish.Y);
            bigEl.Transform(m);
            smallEl.Transform(m);
            petals.Transform(m);
            Matrix m1 = new Matrix();
            m1.Translate(segm.finish.X, segm.finish.Y);
            m1.Rotate(rnd.Next(-90, 90));
            m1.Translate(-segm.finish.X, -segm.finish.Y);
            bigEl.Transform(m1);
            smallEl.Transform(m1);
            petals.Transform(m1);


            //g.DrawPath(new Pen(Color.Red, 2f), bigEl);
            //g.DrawPath(new Pen(Color.Red, 2f), smallEl);
            //g.DrawPath(new Pen(Color.Red, 2f), petals);
            //PathGradientBrush pgb = new PathGradientBrush(petals);
            //PathGradientBrush pgb2 = new PathGradientBrush(smallEl);
            //Color[] colors = new Color[] { palette[4] };
            //Color[] colors2 = new Color[] { palette[6] };
            //pgb.SurroundColors = colors;
            //pgb.CenterColor = palette[5];
            //pgb2.SurroundColors = colors2;
            //pgb2.CenterColor = palette[7];
            g.FillPath(new SolidBrush(palette[5]), petals);
            g.FillPath(new SolidBrush(palette[7]), smallEl);
            //g.FillPath(new SolidBrush(Color.Yellow), smallEl);
        }

        private void DrawGrain(SvgNet.SvgGdi.SvgGraphics g, Plant.Segment segm,
            Plant p, List<Color> palette)
        {
            int grainAngle = rnd.Next(10, 60);
            // = new Plant.Segment(segm.dir, 10, segm.finish, 5);
            Plant.Segment s = p.CreateSegment(segm, 1f, 0);
            for (int i = 0; i < 10; i++)
            {

                Plant.Leaf l = new Plant.Leaf(10, s.dir, new Point(s.finish.X * (i) / 9
                    + s.start.X * (9 - i) / 9,
                s.finish.Y * (i) / 9 + s.start.Y * (9 - i) / 9), 1, grainAngle);
                Plant.Leaf l1 = new Plant.Leaf(10, s.dir, new Point(s.finish.X * (i) / 9
                    + s.start.X * (9 - i) / 9,
                s.finish.Y * (i) / 9 + s.start.Y * (9 - i) / 9), 1, -grainAngle);
                l.CreateLeaf();
                l1.CreateLeaf();
                GraphicsPath lPath = new GraphicsPath();
                GraphicsPath rPath = new GraphicsPath();
                lPath.AddClosedCurve(l.leftSide.ToArray());
                rPath.AddClosedCurve(l.rightSide.ToArray());
                g.FillPath(new SolidBrush(palette[5]), lPath);
                g.FillPath(new SolidBrush(palette[4]), rPath);
                GraphicsPath l1Path = new GraphicsPath();
                GraphicsPath r1Path = new GraphicsPath();
                l1Path.AddClosedCurve(l1.leftSide.ToArray());
                r1Path.AddClosedCurve(l1.rightSide.ToArray());
                g.FillPath(new SolidBrush(palette[4]), l1Path);
                g.FillPath(new SolidBrush(palette[5]), r1Path);
            }

            g.DrawLine(new Pen(palette[4], 2f), s.start, s.finish);
        }

        private void DrawLeaf(int typeLeaf, Vector dir,
            int len, Plant.Segment s, SvgNet.SvgGdi.SvgGraphics g, bool withSegment, Plant p,
            int grainAngle, List<Color> palette)
        {
            Plant.Leaf l;
            if (withSegment)
            {
                Plant.Segment segm = p.CreateSegment(s, 0.5f, 0);
                g.DrawLine(new Pen(Color.Green, 1f), segm.start,
                    segm.finish);
                g.DrawLine(new Pen(Color.DarkSeaGreen, 1f), segm.start, segm.finish);
                l = new Plant.Leaf(len, -segm.dir, segm.finish, typeLeaf, 0);
            }
            else if (grainAngle == 0) l = new Plant.Leaf(len, new Vector(dir.Y, dir.X),
                new Point((int)(s.start.X +
                 rnd.NextDouble() * (s.finish.X - s.start.X)), (int)(s.start.Y +
                 rnd.NextDouble() * (s.finish.Y - s.start.Y))), typeLeaf, 0);
            else l = new Plant.Leaf(len, new Vector(dir.Y, dir.X), new Point((int)(s.start.X +
                rnd.NextDouble() * (s.finish.X - s.start.X)), (int)(s.start.Y +
                rnd.NextDouble() * (s.finish.Y - s.start.Y))), typeLeaf, grainAngle);
            l.CreateLeaf();
            //g.DrawPolygon(new Pen(Color.Aqua, 2.0f), l.leftSide.ToArray());
            //g.DrawCurve(new Pen(Color.Aqua, 2.0f), l.rightSide.ToArray());
            GraphicsPath lPath = new GraphicsPath();
            GraphicsPath rPath = new GraphicsPath();
            lPath.AddClosedCurve(l.leftSide.ToArray());
            rPath.AddClosedCurve(l.rightSide.ToArray());
            g.FillPath(new SolidBrush(palette[2]), lPath);
            g.FillPath(new SolidBrush(palette[3]), rPath);
        }

        private void DrawBerry(Point start, Plant p, Plant.Segment s,
            SvgNet.SvgGdi.SvgGraphics g, List<Color> palette,
            float sAngle, float eAngle, bool withSegment)
        {
            Plant.Berry berry1;
            if (withSegment)
            {
                Plant.Segment segm = p.CreateSegment(s, 0.5f, 0);
                g.DrawLine(new Pen(Color.Black, 1f), segm.start,
                    new Point(2 * segm.start.X - segm.finish.X, 2 * segm.start.Y - segm.finish.Y));
                g.DrawLine(new Pen(Color.Brown, 1f), segm.start,
                    new Point(2 * segm.start.X - segm.finish.X + 1,
                    2 * segm.start.Y - segm.finish.Y));
                berry1 = new Plant.Berry(rnd.Next(5, 15),
                    new Point(2 * segm.start.X - segm.finish.X, 2 * segm.start.Y - segm.finish.Y),
                    sAngle, eAngle);
            }
            else berry1 = new Plant.Berry(rnd.Next(5, 15), s.start, sAngle, eAngle);
            GraphicsPath berry = new GraphicsPath();
            berry.AddEllipse(new Rectangle(berry1.smallP.X, berry1.smallP.Y,
                berry1.sRadius * 2, berry1.sRadius * 2));
            g.FillPath(new SolidBrush(palette[4]), berry);
            GraphicsPath berryShadow = new GraphicsPath();
            berryShadow.FillMode = FillMode.Alternate;
            berryShadow.AddArc(new Rectangle(berry1.smallP.X, berry1.smallP.Y,
               berry1.sRadius * 2, berry1.sRadius * 2),
               sAngle, eAngle);
            berryShadow.AddArc(new Rectangle(berry1.bigP.X, berry1.bigP.Y,
                berry1.bRadius * 2, berry1.bRadius * 2),
                berry1.sbAngle * 180 / (float)Math.PI,
                (berry1.ebAngle - berry1.sbAngle) * 180 / (float)Math.PI);
            g.FillPath(new SolidBrush(palette[5]), berryShadow);
            //g.DrawArc(new Pen(Color.Brown, 2f), new Rectangle(berry1.smallP.X, berry1.smallP.Y,
            //    berry1.sRadius * 2, berry1.sRadius * 2),
            //    sAngle, eAngle);
            //g.DrawArc(new Pen(Color.Black, 2f), new Rectangle(berry1.bigP.X, berry1.bigP.Y,
            //    berry1.bRadius * 2, berry1.bRadius * 2),
            //    berry1.sbAngle * 180 / (float)Math.PI,
            //(berry1.ebAngle - berry1.sbAngle) * 180 / (float)Math.PI);
            //g.DrawLine(new Pen(Color.BlanchedAlmond, 2f),
            //new Point(berry1.bigCenter.X - berry1.bRadius,
            //    berry1.bigCenter.Y - berry1.bRadius),
            //    berry1.bigCenter);
            //g.DrawLine(new Pen(Color.DarkMagenta, 2f), berry1.left, berry1.right);
            //g.DrawLine(new Pen(Color.BlueViolet, 2f), berry1.center, berry1.smallP);

        }

        private void DrawPlant(int X)
        {
            //Image canvas = new Bitmap(89, 90);
            //Graphics gr = Graphics.FromImage(canvas);

            //gr.DrawLine(new Pen(Color.Black, 2f), new Point(0, 0), new Point(10, 10));
            //canvas.Save(Server.MapPath("FVDTest.jpg"));
            //Graphics g = Graphics.FromImage(canvas);
            SvgNet.SvgGdi.SvgGraphics g = new SvgNet.SvgGdi.SvgGraphics();
            g.SmoothingMode = SmoothingMode.HighQuality;
            Plant pl1 = new Plant(10, 5, new Point(X, 400), rnd.Next(0, 2), false, rnd.Next(0, 3));
            List<Color> palette = CreateColors(pl1.plantType);
            for (int i = 0; i < palette.Count; i++)
            {
                palette[i] = RandomizeColor(palette[i], 10);
            }
            Pen plantPen = new Pen(palette[2], 2.0f);
            Pen shadowPen = new Pen(palette[3], 2.0f);
            g.DrawCurve(plantPen, new Point[] { new Point(0, 100), new Point(0, 100) });
            pl1.CreatePlant();
            int typeLeaf = rnd.Next(0, 2);
            bool isStraight = rnd.Next(0, 2) == 1 ? true : false;
            bool withSegmentBerry = rnd.Next(0, 2) == 1 ? true : false;
            float sAngle = (float)(-60 + rnd.NextDouble() * 60);
            float eAngle = (float)(rnd.NextDouble() * 100 + 150);
            for (int i = 0; i < pl1.lineArr.Count; i++)
            {
                for (int j = 0; j < pl1.lineArr[i].Count(); j++)
                {
                    if (isStraight)
                    {
                        g.DrawLine(shadowPen, pl1.lineArr[i][j].start, pl1.lineArr[i][j].finish);
                        g.DrawLine(plantPen, pl1.lineArrShadow[i][j].start,
                            pl1.lineArrShadow[i][j].finish);
                    }
                    if (rnd.Next(0, 3) == 2 && (i != 0 || j != 0))
                    {
                        // Draw leaf.
                        // Place on the segment.
                        double place = rnd.NextDouble();
                        DrawLeaf(typeLeaf, pl1.lineArr[i][j].dir, pl1.lineArr[i][j].length,
                            pl1.lineArr[i][j], g,
                            rnd.Next(0, 2) == 1 ? true : false, pl1, 0, palette);
                    }
                    if (rnd.Next(0, 2) == 1 && (i != 0 || j != 0) && pl1.plantType == 2)
                    {
                        double place = rnd.NextDouble();
                        DrawBerry(new Point((int)(pl1.lineArr[i][j].start.X +
                            place * (pl1.lineArr[i][j].finish.X - pl1.lineArr[i][j].start.X)),
                            (int)(pl1.lineArr[i][j].start.Y +
                            place * (pl1.lineArr[i][j].finish.Y - pl1.lineArr[i][j].start.Y))),
                            pl1, pl1.lineArr[i][j], g, palette,
                            sAngle, eAngle, withSegmentBerry);
                    }
                    if (j == pl1.lineArr[i].Count - 1)
                    {
                        if (pl1.plantType == 0)
                            DrawFlower(g, pl1.lineArr[i][j], pl1, pl1.fl, palette);
                        else if (pl1.plantType == 1)
                            DrawGrain(g, pl1.lineArr[i][j], pl1, palette);
                    }
                }
            }
            // DrawCurve.
            if (!isStraight)
            {
                for (int i = 0; i < pl1.lineArr.Count; i++)
                {
                    Point[] pointsForCurve = new Point[pl1.lineArr[i].Count + 1];
                    Point[] pointsForCurveShadow = new Point[pl1.lineArrShadow[i].Count + 1];
                    for (int j = 0; j < pl1.lineArr[i].Count(); j++)
                    {
                        pointsForCurve[j] = pl1.lineArr[i][j].start;
                        pointsForCurveShadow[j] = pl1.lineArrShadow[i][j].start;
                    }
                    pointsForCurve[pointsForCurve.Length - 1] = pl1.lineArr[i].Last().finish;
                    pointsForCurveShadow[pointsForCurve.Length - 1]
                        = pl1.lineArrShadow[i].Last().finish;
                    g.DrawCurve(shadowPen, pointsForCurveShadow.ToArray());
                    g.DrawCurve(plantPen, pointsForCurve.ToArray());
                }
            }
            string toFile = g.WriteSVGString();
            File.WriteAllText(Server.MapPath("plant.svg"), toFile);
            ImageContainer.Width = 500;
            ImageContainer.Height = 500;
            ImageContainer.ImageUrl = "plant.svg";
        }

        public List<Color> CreateColors(int plType)
        {
            List<Color> res = new List<Color>();
            int num = rnd.Next(0, segmentLightColors.Count());
            res.Add(segmentLightColors[num]);
            res.Add(segmentShadowColors[num]);
            num = rnd.Next(0, leafLightColors.Count());
            res.Add(leafLightColors[num]);
            res.Add(leafShadowColors[num]);
            // Flower.
            if (plType == 0)
            {
                num = rnd.Next(0, flowerPetal1Colors.Count());
                res.Add(flowerPetal1Colors[num]);
                res.Add(flowerPetal2Colors[num]);
                if (rnd.Next(0, 2) == 1)
                {
                    res.Reverse(res.Count() - 2, 2);
                }
                num = rnd.Next(0, flowerCenter1Colors.Count());
                res.Add(flowerCenter1Colors[num]);
                res.Add(flowerCenter2Colors[num]);
                if (rnd.Next(0, 2) == 1)
                {
                    res.Reverse(res.Count() - 2, 2);
                }
            }
            else if (plType == 1)
            {
                num = rnd.Next(0, grainColors.Count());
                res.Add(grainColors[num]);
                num = rnd.Next(0, grainColors.Count());
                res.Add(grainColors[num]);
            }
            else
            {
                num = rnd.Next(0, berryLightColors.Count());
                res.Add(berryLightColors[num]);
                res.Add(berryShadowColors[num]);
            }
            return res;
        }

        private Color RandomizeColor(Color c, int offset)
        {
            int[] rgb = new int[3];
            int[] rgbRef = new int[3] { c.R, c.G, c.B };
            for (int i = 0; i < 3; i++)
            {
                rgb[i] = rgbRef[i] + rnd.Next(-offset, offset + 1);
                if (rgb[i] > 255)
                    rgb[i] = 255;
                else if (rgb[i] < 0)
                    rgb[i] = 0;
            }
            return Color.FromArgb(255, rgb[0], rgb[1], rgb[2]);
        }
    }
}