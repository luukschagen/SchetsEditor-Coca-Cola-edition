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
        //protected void Tekenitem(Graphics g);
    }

    class TextItem : SchetsItem
    {
        protected Char c;

        public TextItem(Point p, Brush kleur, char letter)
        {
            startpunt = p;
            kwast = kleur;
            c = letter;
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
        bool gevuld;

        public RechthoekItem(Point p1, Point p2, bool vulling, Brush kleur) : 
        base(p1, p2, kleur)
        {
            gevuld = vulling;
        }
    }

    class CirkelItem : RechthoekItem
    {
        public CirkelItem(Point p1, Point p2, bool vulling, Brush kleur) :
        base(p1, p2, vulling, kleur)
        {}
    }

    class LijnItem : TweepuntItem
    {
        public LijnItem(Point p1, Point p2, Brush kleur): base(p1, p2, kleur)
        {}
    }
}
