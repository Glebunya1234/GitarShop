using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace WindowsFormsApp1
{
    internal static class Animation
    {
        public static bool flagHeight = true;
        public static bool flagWight = true;
        public static void AnimationFormHeight(this Form Formheight, int HeightMax,int HeightDefolt, int DelayMax, int DelayMin)
        {
           
            if (flagHeight)
            {
                int originalWidth = Formheight.Height;
                int step = (HeightMax - originalWidth) / (DelayMax / 10); // вычисляем шаг изменения ширины

                Timer timer = new Timer();
                timer.Interval = 10; // задаем интервал таймера в миллисекундах
                timer.Tick += (s, ev) =>
                {
                    Formheight.Height += step; // изменяем ширину формы на шаг
                    if (Math.Abs(HeightMax - Formheight.Height) <= Math.Abs(step)) // проверяем, достигли ли целевой ширины
                    {
                        Formheight.Height = HeightMax; // устанавливаем точную целевую ширину
                        timer.Stop(); // останавливаем таймер
                    }
                };

                timer.Start(); // запускаем таймер
                flagHeight = false;
            }
            else
            {
                
                
                int originalWidth = Formheight.Height;
                int step = (HeightDefolt + originalWidth) / (DelayMin / 10);

                Timer timer = new Timer();
                timer.Interval = 10;
                timer.Tick += (s, ev) =>
                {
                    Formheight.Height -= step;
                    if (Math.Abs(HeightDefolt - Formheight.Height) <= Math.Abs(step))
                    {
                        Formheight.Height = HeightDefolt;
                        timer.Stop();
                    }
                };

                timer.Start(); // запускаем таймер
                flagHeight = true;
            }

        }
        public static void AnimationFormWight(this Form FormWight, int WightMax, int WightDefolt, int DelayMax, int DelayMin)
        {
           
            if (flagWight)
            {
                int originalWidth = FormWight.Width;
                int step = (WightMax - originalWidth) / (DelayMax / 10); // вычисляем шаг изменения ширины

                Timer timer = new Timer();
                timer.Interval = 10; // задаем интервал таймера в миллисекундах
                timer.Tick += (s, ev) =>
                {
                    FormWight.Width += step; // изменяем ширину формы на шаг
                    if (Math.Abs(WightMax - FormWight.Width) <= Math.Abs(step)) // проверяем, достигли ли целевой ширины
                    {
                        FormWight.Width = WightMax; // устанавливаем точную целевую ширину
                        timer.Stop(); // останавливаем таймер
                    }
                };

                timer.Start(); // запускаем таймер
                flagWight = false;
            }
            else
            {


                int originalWidth = FormWight.Width;
                int step = (WightDefolt + originalWidth) / (DelayMin / 10);

                Timer timer = new Timer();
                timer.Interval = 10;
                timer.Tick += (s, ev) =>
                {
                    FormWight.Width -= step;
                    if (Math.Abs(WightDefolt - FormWight.Width) <= Math.Abs(step))
                    {
                        FormWight.Width = WightDefolt;
                        timer.Stop();
                    }
                };

                timer.Start(); // запускаем таймер
                flagWight = true;
            }

        }
    }
}
