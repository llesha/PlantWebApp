using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Point = System.Drawing.Point;

namespace PlantLib
{
    public class Plant
    {
        bool withBerry;
        static Random rnd = new Random();
        public int height;
        int numSegments;
        public List<List<Segment>> lineArr;
        public List<List<Segment>> lineArrShadow;
        public Point root;
        public int stem;
        public Leaf leaf;
        public Flower fl;
        public int plantType;

        public Plant(int h, int n, Point r, int s, bool w, int plType)
        {
            lineArr = new List<List<Segment>>();
            lineArrShadow = new List<List<Segment>>();
            height = h;
            numSegments = n;
            root = r;
            stem = s;
            withBerry = w;
            plantType = plType;
        }

        public Segment CreateSegment(Segment prev, float len, int forWhat)
        {
            if (forWhat == 0)
            {
                double a = rnd.NextDouble() * 0.5 * (rnd.Next(0, 2) == 0 ? 1 : -1);
                // What segment will be on what side.
                int leftOrRight = -1;
                if (rnd.Next(0, 2) == 1)
                    leftOrRight = 1;
                double b = rnd.NextDouble() * 0.5 * (rnd.Next(0, 2) == 0 ? 1 : -1) + 0.1;
                Segment segm1 = new Segment(prev.dir *
                        new System.Windows.Media.Matrix(Math.Cos(a + b * leftOrRight),
                        -Math.Sin(a + b * leftOrRight),
                        Math.Sin(a + b * leftOrRight),
                        Math.Cos(a + b * leftOrRight), 0, 0),
                        prev.length,
                        prev.finish,
                        prev.segmNum + 1);
                segm1.finish = new Point(Convert.ToInt32(
                    (segm1.start.X + len * (segm1.dir * 15).X)),
                   Convert.ToInt32((segm1.start.Y + len * (segm1.dir * 15).Y)));
                return segm1;
            }
            else return new Segment(new Vector(1, 1), 1, new Point(10, 10), 0);
        }




        public void CreatePlant()
        {
            fl = new Flower();
            int whatArr = 0;
            lineArr.Add(new List<Segment>());
            lineArr[0].Add(new Segment(new Vector(0, -2), 10, root, 0));
            lineArr[0][0].finish = new Point(root.X, root.Y - 10);
            lineArrShadow.Add(new List<Segment>());
            lineArrShadow[0].Add(new Segment(new Vector(0, -2), 10,
                new Point(root.X + 2, root.Y), 0));
            lineArrShadow[0][0].finish = new Point(root.X + 2, root.Y - 10);
            while (whatArr < lineArr.Count && lineArr.Count < 5)
            {
                double chance = rnd.NextDouble();
                //double a = rnd.NextDouble() * 0.5 * (rnd.Next(0, 2) == 0 ? 1 : -1);
                if (lineArr[whatArr].Count == numSegments)
                {
                    whatArr++;
                    continue;
                    // Chance to leave while before the end.

                    //if (rnd.NextDouble() > 0.7)
                    //    break;
                }
                if (chance > 10.9)
                {
                    // Create three.
                }
                else if (chance > 0.7)
                {
                    // Create two.
                    lineArr.Add(new List<Segment>());
                    lineArrShadow.Add(new List<Segment>());
                    lineArr[whatArr].Add(CreateSegment(lineArr[whatArr].Last(), 1, 0));
                    Segment segm1Shadow = new Segment(lineArr[whatArr].Last().dir,
                        lineArr[whatArr].Last().length,
                        new Point(lineArr[whatArr].Last().start.X + 2,
                        lineArr[whatArr].Last().start.Y), lineArr[whatArr].Last().segmNum);
                    segm1Shadow.finish = new Point(lineArr[whatArr].Last().finish.X + 2,
                        lineArr[whatArr].Last().finish.Y);
                    lineArrShadow[whatArr].Add(segm1Shadow);

                    lineArr.Last().Add(CreateSegment
                        (lineArr[whatArr][lineArr[whatArr].Count - 2], 1, 0));
                    Segment segm2Shadow = new Segment(lineArr.Last().Last().dir,
                        lineArr.Last().Last().length,
                        new Point(lineArr.Last().Last().start.X + 2,
                        lineArr.Last().Last().start.Y),
                        lineArr.Last().Last().segmNum);
                    segm2Shadow.finish = new Point(lineArr.Last().Last().finish.X + 2,
                        lineArr.Last().Last().finish.Y);
                    lineArrShadow.Last().Add(segm2Shadow);

                }
                else
                {
                    // Create one.

                    lineArr[whatArr].Add(CreateSegment(lineArr[whatArr].Last(), 1, 0));
                    Segment segmShadow = new Segment(lineArr[whatArr].Last().dir,
                        lineArr[whatArr].Last().length,
                        new Point(lineArr[whatArr].Last().start.X + 2,
                        lineArr[whatArr].Last().start.Y),
                        lineArr[whatArr].Last().segmNum);
                    segmShadow.finish = new Point(lineArr[whatArr].Last().finish.X + 2,
                        lineArr[whatArr].Last().finish.Y);
                    lineArrShadow[whatArr].Add(segmShadow);
                }
            }
        }

        public class Flower
        {

            public int width;
            public int sWidth;
            public int petalsNum;

            public Flower()
            {
                width = rnd.Next(10, 50);
                petalsNum = rnd.Next(3, 7);
                sWidth = width - rnd.Next(5, (int)(width * 3 / 4) + 1);
            }
        }

        public class Berry
        {
            public int sRadius;
            public int bRadius;
            public Point smallP;
            public Point bigP;
            public Point center;
            public Point left;
            public Point right;
            public Point bigCenter;
            public float sbAngle;
            public float ebAngle;

            public Berry(int size, Point c, float sAngle, float eAngle)
            {
                sRadius = size / 2;
                center = c;
                smallP = new Point(center.X - sRadius, center.Y - sRadius);
                left = new Point(Convert.ToInt32(center.X + Math.Cos(sAngle * Math.PI / 180) * sRadius),
                    Convert.ToInt32(center.Y + Math.Sin(sAngle * Math.PI / 180) * sRadius));
                right = new Point(Convert.ToInt32(center.X +
                    Math.Cos((sAngle + eAngle) * Math.PI / 180) * sRadius),
                    Convert.ToInt32(center.Y + Math.Sin((sAngle + eAngle) * Math.PI / 180) * sRadius));
                Point mid = new Point((left.X + right.X) / 2, (left.Y + right.Y) / 2);
                if (eAngle < 180)
                {
                    bigCenter = new Point((center.X + 3 * (center.X - mid.X)),
                        (center.Y + 3 * (center.Y - mid.Y)));
                }
                else bigCenter = new Point((center.X + 3 * (-center.X + mid.X)),
                       (center.Y + 3 * (-center.Y + mid.Y)));
                bRadius = Convert.ToInt32(Math.Sqrt(Math.Pow(right.X - bigCenter.X, 2)
                    + Math.Pow(right.Y - bigCenter.Y, 2)));
                bigP = new Point(bigCenter.X - bRadius, bigCenter.Y - bRadius);
                sbAngle = (float)Math.Atan2(Convert.ToSingle(left.Y - bigCenter.Y),
                    Convert.ToSingle(left.X - bigCenter.X));
                ebAngle = (float)Math.Atan2(Convert.ToSingle(right.Y - bigCenter.Y),
                    Convert.ToSingle(right.X - bigCenter.X));
            }
        }

        public class Leaf
        {
            public int length;
            public Vector dir;
            public Point start;
            public List<Point> leftSide = new List<Point>();
            public List<Point> rightSide = new List<Point>();
            public int leafType;
            public int grainAngle;

            public Leaf(int l, Vector d, Point p, int t, int gA)
            {
                length = l;
                dir = d;
                start = p;
                leafType = t;
                grainAngle = gA;
            }

            public void CreateLeaf()
            {
                double a = rnd.NextDouble() * 0.5 * (rnd.Next(0, 2) == 0 ? 1 : -1);
                int coef;
                int c2 = 10;
                if (leafType == 0)
                {
                    coef = rnd.Next(5, 15);
                    c2 = 2;
                }
                else coef = rnd.Next(10, 25);
                if (grainAngle != 0)
                {
                    coef = 10;
                    c2 = 7;
                    dir = dir * new System.Windows.Media.Matrix
                        (Math.Cos(grainAngle * Math.PI / 180),
                        -Math.Sin(grainAngle * Math.PI / 180),
                    Math.Sin(grainAngle * Math.PI / 180),
                    Math.Cos(grainAngle * Math.PI / 180), 0,
                         rnd.Next(0, 2) == 0 ? 1 : -1);
                }
                else
                    dir = dir * new System.Windows.Media.Matrix(Math.Cos(a), -Math.Sin(a),
                    Math.Sin(a), Math.Cos(a), 0, 0) * (rnd.Next(0, 2) == 0 ? 1 : -1);
                leftSide.Add(start);
                leftSide.Add(new Point(
                    (int)(leftSide[0].X + dir.X * coef),
                    (int)(leftSide[0].Y + dir.Y * coef)));
                Point mid = new Point((start.X + leftSide[1].X) / 2,
                    (start.Y + leftSide[1].Y) / 2);
                rightSide.Add(leftSide[0]);
                rightSide.Add(leftSide[1]);
                leftSide.Add(new Point((int)(dir.Y * coef / c2 + rnd.Next(-20, 20) / c2 + mid.X),
                    (int)(dir.X * coef / c2 + rnd.Next(-20, 20) / c2 + mid.Y)));
                rightSide.Add(new Point((int)(-dir.Y * coef / c2 + rnd.Next(-20, 20) / c2 + mid.X),
                    (int)(dir.X * coef / c2 + rnd.Next(-20, 20) / c2 + mid.Y)));
                //leftSide.Insert(2, new Point(leftSide[1].X / 2 + leftSide[2].X / 2,
                //leftSide[1].Y / 2 + leftSide[2].Y / 2));
            }
        }

        public class Segment
        {
            public int length;
            public Vector dir;
            public int segmNum;
            public Point start;
            public Point finish;
            bool taken;

            public Segment(Vector d, int l, Point s, int segmN)
            {
                taken = false;
                dir = d;
                length = l;
                start = s;
                segmNum = segmN;
            }
        }
    }
}
