using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchetsEditor
{
    class SchetsItem
    {
        protected Point startpunt;
        protected Brush kwast;
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

    class TweepuntItem : SchetsItem
    {
        protected Point eindpunt;

        public TweepuntItem(Point p1, Point p2, Brush kleur)
        {
            startpunt = p1;
            eindpunt = p2;
            kwast = kleur;
        }
    }


}
