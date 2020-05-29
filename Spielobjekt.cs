using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Asteroids
{
    abstract class Spielobjekt
    {
        public double x { get; private set; }
        public double y { get; private set; }

        protected double vx;
        protected double vy;

        public Spielobjekt(double x, double y, double vx, double vy)
        {
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
        }

        public abstract void Draw(Canvas Zeichenfläche);

        public void Move(TimeSpan interval, Canvas Zeichenfläche)
        {
            x += vx * interval.TotalSeconds;
            y += vy * interval.TotalSeconds;

            if (x > Zeichenfläche.ActualWidth) x = 0;
            if (x < 0) x = Zeichenfläche.ActualWidth;

            if (y > Zeichenfläche.ActualHeight) y = 0;
            if (y < 0) y = Zeichenfläche.ActualHeight;
        }

    }

    class Asteroid : Spielobjekt
    {
        static Random rnd = new Random();
        Polygon umriss;
        public Asteroid(Canvas Zeichenfläche)
            : base( rnd.NextDouble() * Zeichenfläche.ActualWidth,
                    rnd.NextDouble() * Zeichenfläche.ActualHeight,
                    (rnd.NextDouble() - 0.5) * 200,
                    (rnd.NextDouble() - 0.5) * 200)
        {
            umriss = new Polygon();
            for (int i = 0; i < 15; i++)
            {
                double winkel = 2 * Math.PI / 15 * i;
                double radius = 8 + rnd.NextDouble() * 7;
                umriss.Points.Add(new Point(radius * Math.Cos(winkel), radius * Math.Sin(winkel)));
            }
            umriss.Fill = Brushes.Gray;
        }

        public override void Draw(Canvas Zeichenfläche)
        {
            Zeichenfläche.Children.Add(umriss);
            Canvas.SetTop(umriss, y);
            Canvas.SetLeft(umriss, x);
        }
    }

    //to do:
    class Raumschiff : Spielobjekt
    {
        Polygon umriss;
        public Raumschiff(Canvas Zeichenfläche)
            :base(Zeichenfläche.ActualWidth/2,
                  Zeichenfläche.ActualHeight/2,
                  0.1,
                  -1)
        {
            umriss = new Polygon();
            umriss.Points.Add(new Point(0 , -10));
            umriss.Points.Add(new Point(5 , 7));
            umriss.Points.Add(new Point(-5 , 7));
            umriss.Fill = Brushes.Blue;
        }

        

        public override void Draw(Canvas Zeichenfläche)
        {
            double winkel = Math.Atan2(vy, vx) * 180.0 / Math.PI+90;
            umriss.RenderTransform = new RotateTransform(winkel);
            Zeichenfläche.Children.Add(umriss);
            Canvas.SetTop(umriss, y);
            Canvas.SetLeft(umriss, x);
        }

        public void Accelerate(bool faster)
        {
            double faktor = faster ? 1.1 : 0.9;
            vx *= faktor;
            vy *= faktor;
        }

        public void Steer(bool left)
        {
            double winkel = (left ? -5 : 5) * Math.PI/ 180.0;
            double sin = Math.Sin(winkel);
            double cos = Math.Cos(winkel);
            double vxn, vyn;
            vxn = vx * cos - vy * sin;
            vyn = vx * sin + vy * cos;
            vx = vxn;
            vy = vyn;
        }
    }

    //class Torpedo : Spielobjekt
    //{

    //}
}
