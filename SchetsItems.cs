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
        public abstract void Tekenitem(Graphics g);
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
    }

    class LijnItem : TweepuntItem
    {
        public LijnItem(Point p1, Point p2, Brush kleur): base(p1, p2, kleur)
        {}

        public override void Tekenitem(Graphics g)
        {
            g.DrawLine(new Pen(kwast, 3), startpunt, eindpunt);
        }
    }
}
