using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // Перелічуваний тип (enum) для стану м’яча на полі
    public enum stanMjacha
    {
        vGriA,     // М'яч у грі у команди А
        vGriB,     // М'яч у грі у команди В
        pozaGroju, // М'яч поза грою
        vCentri,   // М'яч в центрі поля
        vVorotahA, // М'яч у воротах команди А
        vVorotahB  // М'яч у воротах команди В
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); // Ініціалізація компонентів форми
        }

        /* Опис концепції програми:
         * Ця програма ілюструє принципи ООП: наслідування та поліморфізм.
         * Є базовий клас Sphere і його нащадки:
         * - mjach (м'яч)
         * - povitrjana_kulja (повітряна куля)
         * - futbol_mjach (футбольний м'яч) як нащадок класу mjach
         * Методи всіх класів виводять інформацію про свою роботу у змінну sumText,
         * яка потім відображається у Label2 на формі. Label1 показує план роботи.
         */

        public static string newline = "\r"; // Перехід на новий рядок у повідомленнях
        public static string sumText = "";    // Змінна для накопичення результатів виконання методів
        public static string PlanText = "";   // Змінна для накопичення плану роботи
        public static int j = 1;              // Лічильник порядку виконання методів

        // Базовий клас "Куля" (Sphere)
        public class Sphere
        {
            public const double Pi = Math.PI; // Константа Pi
            double r, l, v, s, m; // Поля: радіус, довжина кола, об'єм, площа поверхні, маса

            // Властивість для встановлення радіусу кулі
            public double r_kuli
            {
                get { return r; }
                set
                {
                    r = value;
                    l = 2 * Pi * r;          // Довжина кола
                    v = 4 * Pi * r * r * r;  // Об'єм кулі
                    s = 4 * Pi * r * r;      // Площа поверхні
                }
            }

            // Тільки для читання довжина кола
            public double l_kola { get { return l; } }

            // Тільки для читання об'єм кулі
            public double v_kuli { get { return v; } }

            // Тільки для читання площа поверхні
            public double s_kuli { get { return v; } } // Можливо, тут мала бути s?

            // Властивість маси кулі
            public double masa
            {
                get { return m; }
                set { m = value; }
            }

            // Метод котіння кулі (віртуальний, для поліморфізму)
            virtual public double kotytys(double t, double v)
            {
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано метод kotytys(double,double); класу Sphere" + Form1.newline;
                Form1.j++;
                return 2 * Pi * r * t * v;
            }

            // Метод польоту кулі (віртуальний)
            virtual public double letity(double t, double v)
            {
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано метод letity(double,double) класу Sphere" + Form1.newline;
                Form1.j++;
                return t * v;
            }

            // Метод удару кулі (віртуальний), повертає пройдений шлях
            virtual public double udar(double t, out double v, double f, double t1)
            {
                v = f * t1 / m; // Обчислюємо швидкість після удару
                Form1.sumText += Convert.ToString(Form1.j) +
                    ". Виконано метод udar(double, out double,double,double); класу Sphere s = v * t= " +
                    Convert.ToString(t * v) + Form1.newline;
                Form1.j++;
                return t * v;
            }
        }

        // Клас "м'яч" як нащадок Sphere
        public class mjach : Sphere
        {
            // Метод попадання по м'ячу
            virtual public void popav(bool je, ref int kilkist)
            {
                if (je) kilkist++;
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано метод popav(bool, ref int); класу mjach. kilkist=" + Convert.ToString(kilkist) + Form1.newline;
                Form1.j++;
            }

            // Метод польоту м'яча з додатковою силою тертя
            virtual public double letity(double t, double v, double f_tertja)
            {
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано метод letity(double, double, double) класу mjach" + Form1.newline;
                Form1.j++;
                return t * v - f_tertja / masa * t * t / 2;
            }

            // Виклик базового методу letity з класу Sphere
            public double letityBase(double t, double v)
            {
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано метод letityBase(double, double) класу mjach" + Form1.newline;
                Form1.j++;
                return base.letity(t, v);
            }
        }

        // Клас "повітряна куля"
        public class povitrjana_kulja : Sphere
        {
            double tysk, maxTysk; // Поточний і максимальний тиск

            public double tyskGazu { get { return tysk; } set { tysk = value; } }
            public double maxTyskGazu { get { return maxTysk; } set { maxTysk = value; } }

            // Перевірка, чи лопнула куля
            public bool lopatys()
            {
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано метод lopatys(); класу povitrjana_kulja" + Form1.newline;
                Form1.j++;
                return tysk > maxTysk;
            }

            // Метод польоту кулі з врахуванням вітру
            public double letity(double t, double v, double v_Vitru, double kutVitru)
            {
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано метод letity(double t, double v, double v_Vitru, double kutVitru); класу povitrjana_kulja" + Form1.newline;
                Form1.j++;
                return t * v - v_Vitru * Math.Sin(kutVitru);
            }

            // Перевизначення методу letity з базовою сигнатурою
            new public double letity(double t, double v)
            {
                double s = t * v;
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано метод letity(double t, double v ) класу povitrjana_kulja. Ми пролетіли " + Convert.ToString(s) + " метрів!" + Form1.newline;
                Form1.j++;
                return s;
            }
        }

        // Клас "футбольний м'яч" як нащадок mjach
        public class futbol_mjach : mjach
        {
            stanMjacha stanM; // Поточний стан м'яча

            public stanMjacha tstanMjacha { get { return stanM; } set { stanM = value; } }

            // Визначає, чи м'яч поза грою
            public bool standout { get { return stanM == stanMjacha.pozaGroju; } }

            // Конструктор з параметром
            public futbol_mjach(stanMjacha sm)
            {
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано конструктор futbol_mjach(stanMjacha sm) " + Form1.newline;
                Form1.j++;
                stanM = sm;
                masa = 0.5F;
                r_kuli = 0.1F;
            }

            // Перевантажений метод popav для двох команд
            public void popav(bool je, ref int kilkistA, ref int kilkistB)
            {
                if (je)
                {
                    if (stanM == stanMjacha.vGriA) kilkistA++;
                    else if (stanM == stanMjacha.vGriB) kilkistB++;
                }
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано метод popav класу futbol_mjach з сигнатурою (bool, ref int, ref int)" + Form1.newline;
                Form1.j++;
            }

            // Виклик базового методу popav з класу mjach
            public void popavBase(bool b, ref int i)
            {
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано метод popavBase(bool b, ref int i) класу futbol_mjach, який викликає метод popav(b, ref i) класу mjach " + Form1.newline;
                Form1.j++;
                base.popav(b, ref i);
            }

            // Override методу popav з базового класу
            override public void popav(bool je, ref int kilkist)
            {
                if (je) kilkist++;
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано метод popav(bool je, ref int kilkist) (override) класу futbol_mjach " + Form1.newline;
                Form1.j++;
            }

            // Override методу letity з базового класу
            override public double letity(double t, double v, double ftertja)
            {
                Form1.sumText += Convert.ToString(Form1.j) + ". Виконано метод letity(double t, double v, double ftertja) (override) класу futbol_mjach " + Form1.newline;
                Form1.j++;
                return t * v - ftertja / masa * t * t / 2;
            }
        }

        // Обробка натискання кнопки 1
        private void button1_Click(object sender, EventArgs e)
        {
            // Ініціалізація плану роботи та результату
            PlanText = "Планується зроботи:" + newline;
            sumText = "Початок роботи" + newline;
            int i = 1;
            label2.Text = sumText + newline;

            // Створення екземпляру класу futbol_mjach
            PlanText += Convert.ToString(i) + ". Плануємо створити екземпляр класу futbol_mjach, викликавши конструктор з параметром " + newline;
            i++;
            futbol_mjach fm = new futbol_mjach(stanMjacha.vCentri);

            int GolA = 0, GolB = 0; // Лічильники голів
            fm.masa = 0.6F;
            fm.r_kuli = 0.12F;
            fm.tstanMjacha = stanMjacha.vGriA;

            // Виклик методів класу та формування плану
            PlanText += Convert.ToString(i) + ". Плануємо викликати метод popav(true, ref GolA); (override)" + newline; i++;
            fm.popav(true, ref GolA);

            PlanText += Convert.ToString(i) + ". Плануємо викликати метод popav(true, ref GolA, ref GolB) " + newline; i++;
            fm.popav(true, ref GolA, ref GolB);

            PlanText += Convert.ToString(i) + ", " + Convert.ToString(i + 1) + ". Плануємо викликати метод popavBase(true, ref GolA), який викликає базовий метод popav" + newline; i += 2;
            fm.popavBase(true, ref GolA);

            double s, v;
            PlanText += Convert.ToString(i) + ". Плануємо викликати метод udar(2, out v, 200, 0.1) класу Sphere" + newline; i++;
            s = fm.udar(2, out v, 200, 0.1);

            PlanText += Convert.ToString(i) + ". Плануємо викликати метод kotytys(5, 1) класу Sphere" + newline; i++;
            s = fm.kotytys(5, 1);

            PlanText += Convert.ToString(i) + ". Плануємо викликати метод letity(20, 30) класу Sphere" + newline; i++;
            s = fm.letity(20, 30);

            PlanText += Convert.ToString(i) + ". Плануємо викликати перевизначений метод letity(20, 30, 5) класу futbol_mjach" + newline; i++;
            s = fm.letity(20, 30, 5);

            // Вивід плану та результатів у Label1 та Label2
            label1.Text = PlanText;
            label2.Text = sumText;
        }

        // Обробка кнопки виходу
        private void button2_Click(object sender, EventArgs e)
        {
            Close(); // Закриття форми
        }
    }
}
