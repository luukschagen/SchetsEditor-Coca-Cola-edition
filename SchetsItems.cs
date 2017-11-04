using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchetsEditor
{
    abstract class SchetsItem
    {
        protected Point startpunt;
        protected Brush kwast;
        //Hieronder twee methoden die nodig zijn voor de gum
        public abstract void Tekenitem(Graphics g);
        public abstract bool Raaktpunt(Point p, Graphics g);
    }

    class TextItem : SchetsItem
    {
        protected Char c;
        protected Font f;

        public TextItem(Point p, Brush kleur, char letter, Font font)
        {
            startpunt = p;
            kwast = kleur;
            c = letter;
            f = font;
        }

        public override void Tekenitem(Graphics g)
        {
            g.DrawString(c.ToString(), f, kwast, startpunt);
        }

        //Controleert of de gum de letter raakt
        public override bool Raaktpunt(Point p, Graphics g)
        {
            SizeF size = g.MeasureString(c.ToString(), f);

            return (p.X > startpunt.X && p.X < startpunt.X + size.Width &&
                    p.Y > startpunt.Y && p.Y < startpunt.Y + size.Height);
        }
    }

    abstract class TweepuntItem : SchetsItem
    {
        protected Point eindpunt;

        public TweepuntItem(Point p1, Point p2, Brush kleur)
        {
            startpunt = p1;
            eindpunt = p2;
            kwast = kleur;
        }
    }

    class RechthoekItem : TweepuntItem
    {
        protected bool gevuld;

        public RechthoekItem(Point p1, Point p2, bool vulling, Brush kleur) : 
        base(p1, p2, kleur)
        {
            gevuld = vulling;
        }

        public override void Tekenitem(Graphics g)
        {
            if (gevuld)
                g.FillRectangle(kwast, TweepuntTool.Punten2Rechthoek(startpunt, eindpunt));
            else 
                g.DrawRectangle(new Pen(kwast, 3), TweepuntTool.Punten2Rechthoek(startpunt, eindpunt));
        }

        public override bool Raaktpunt(Point p, Graphics g)
        {
            Rectangle rect = TweepuntTool.Punten2Rechthoek(startpunt, eindpunt);
            if (p.X > rect.Location.X - 4 && p.X < rect.Location.X + rect.Width + 4 &&
                p.Y > rect.Location.Y - 4 && p.Y < rect.Location.Y + rect.Height + 4)
            {
                if (gevuld)
                    return true;

                else
                    return !(p.X > rect.Location.X + 4 && p.X < rect.Location.X + rect.Width - 4 &&
                             p.Y > rect.Location.Y + 4 && p.Y < rect.Location.Y + rect.Height - 4);
            }
            return false;
        }
    }

    class CirkelItem : RechthoekItem
    {
        public CirkelItem(Point p1, Point p2, bool vulling, Brush kleur) :
        base(p1, p2, vulling, kleur)
        {}

        public override void Tekenitem(Graphics g)
        {
            if (gevuld)
                g.FillEllipse(kwast, TweepuntTool.Punten2Rechthoek(startpunt, eindpunt));
            else
                g.DrawEllipse(new Pen(kwast, 3), TweepuntTool.Punten2Rechthoek(startpunt, eindpunt)); 
        }

        public override bool Raaktpunt(Point p, Graphics g)
        {
            Rectangle rect = TweepuntTool.Punten2Rechthoek(startpunt, eindpunt);
            double h = rect.Location.X + rect.Width / 2;
            double k = rect.Location.Y + rect.Height / 2;
            double rx = rect.Width / 2;
            double ry = rect.Height / 2;

            double d = Math.Pow((p.X - h), 2) / Math.Pow(rx, 2) + 
                       Math.Pow((p.Y - k), 2) / Math.Pow(ry, 2);

            if (d <= 1.1)
            {
                if (gevuld)
                    return true;

                else
                    return d >= 0.9;
            }

            return false;
        }
    }

    class LijnItem : TweepuntItem
    {
        public LijnItem(Point p1, Point p2, Brush kleur): base(p1, p2, kleur)
        {}

        public override void Tekenitem(Graphics g)
        {
            g.DrawLine(TweepuntTool.MaakPen(kwast, 3), startpunt, eindpunt);
        }

        public override bool Raaktpunt(Point p, Graphics g)
        {
            double x0 = p.X;
            double y0 = p.Y;
            double x1 = startpunt.X;
            double y1 = startpunt.Y;
            double x2 = eindpunt.X;
            double y2 = eindpunt.Y;
            //hihi, dubbel d - dit is overigens helemaal gekopieerd van de formule van wikipedia
            //werkt verassend goed
            double d = Math.Abs((y2 - y1) * x0 - (x2 - x1)* y0 + x2 * y1 - y2 * x1) /
                           Math.Sqrt(Math.Pow((y2 - y1), 2) + Math.Pow((x2 - x1), 2));

            if (d < 5 && x0 > Math.Min(x1, x2) - 4 && x0 < Math.Max(x1, x2) + 4 &&
                         y0 > Math.Min(y1, y2) - 4 && y0 < Math.Max(y1, y2) + 4)
                return true;

            else
                return false;
        }
    }
}
