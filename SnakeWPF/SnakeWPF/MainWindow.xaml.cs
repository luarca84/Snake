using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            timer.Tick += Timer_Tick;
            KeyDown += MainWindow_KeyDown;
            NuevoJuego();
        }

        private void NuevoJuego()
        {
            timer.Stop();

            movimiento = TypeMovement.Right;

            snake = new List<Point>();
            snake.Add(new Point(CTE_SIDE * 10, CTE_SIDE));
            snake.Add(new Point(CTE_SIDE * 10, CTE_SIDE * 2));
            snake.Add(new Point(CTE_SIDE * 10, CTE_SIDE * 3));
            snake.Add(new Point(CTE_SIDE * 10, CTE_SIDE * 4));

            food = new List<Point>();
            for (int i = 0; i < NUM_FOOD; i++)
                food.Add(new Point(CTE_SIDE * r.Next(0, 30), CTE_SIDE * r.Next(0, 30)));

            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NuevoJuego();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Up))
                movimiento = TypeMovement.Up;
            if (Keyboard.IsKeyDown(Key.Down))
                movimiento = TypeMovement.Down;
            if (Keyboard.IsKeyDown(Key.Left))
                movimiento = TypeMovement.Left;
            if (Keyboard.IsKeyDown(Key.Right))
                movimiento = TypeMovement.Right;
        }

        const int CTE_SIDE = 10;
        const int NUM_FOOD = 10;
        List<Point> snake = new List<Point>();
        List<Point> food = new List<Point>();
        enum TypeMovement { Right,Left,Up,Down};
        TypeMovement movimiento = TypeMovement.Right;
        Random r = new Random();
        DispatcherTimer timer = new DispatcherTimer();

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
            CheckSnakeEatFood();
            myCanvas.Children.Clear();
            foreach (Point p in snake)
            {
                Ellipse newEllipse = new Ellipse();
                newEllipse.Fill = Brushes.Black;
                newEllipse.Width = CTE_SIDE;
                newEllipse.Height = CTE_SIDE;
                Canvas.SetTop(newEllipse, p.Y);
                Canvas.SetLeft(newEllipse, p.X);
                myCanvas.Children.Add(newEllipse);
            }

            foreach (Point p in food)
            {
                Ellipse newEllipse = new Ellipse();
                newEllipse.Fill = Brushes.Brown;
                newEllipse.Width = CTE_SIDE;
                newEllipse.Height = CTE_SIDE;
                Canvas.SetTop(newEllipse, p.Y);
                Canvas.SetLeft(newEllipse, p.X);
                myCanvas.Children.Add(newEllipse);
            }

            CheckVictory();
            CheckLose();
        }

        private void CheckLose()
        {
            Point cabeza = snake[0];

            if (cabeza.Y > myCanvas.Height || cabeza.Y <0 || cabeza.X<0 || cabeza.X > myCanvas.Width)
            {
                timer.Stop();
                MessageBox.Show("You Lose");
            }
        }

        private void CheckVictory()
        {
            if (food.Count == 0)
            {
                timer.Stop();
                MessageBox.Show("You Win");
            }
        }

        private void CheckSnakeEatFood()
        {
            Point cabeza = snake[0];
            int index = -1;
            for (int f = 0; f < food.Count; f++)
            {
                Point p = food[f];
                if (p.X == cabeza.X && p.Y == cabeza.Y)
                {
                    index = f;
                    break;
                }
            }

            if (index >= 0)
            {
                food.RemoveAt(index);
                Point nuevaCabeza = new Point();
                if (movimiento == TypeMovement.Right)
                    nuevaCabeza = new Point(cabeza.X + CTE_SIDE, cabeza.Y);
                if (movimiento == TypeMovement.Left)
                    nuevaCabeza = new Point(cabeza.X - CTE_SIDE, cabeza.Y);
                if (movimiento == TypeMovement.Up)
                    nuevaCabeza = new Point(cabeza.X, cabeza.Y - CTE_SIDE);
                if (movimiento == TypeMovement.Down)
                    nuevaCabeza = new Point(cabeza.X, cabeza.Y + CTE_SIDE);
                snake.Insert(0, nuevaCabeza);
            }
        }

        private void MoveSnake()
        {
            if (movimiento == TypeMovement.Right)
            {
                Point cabeza = snake[0];
                Point nuevaCabeza = new Point(cabeza.X + CTE_SIDE, cabeza.Y );
                snake.Insert(0, nuevaCabeza);
                snake.RemoveAt(snake.Count - 1);
            }
            if (movimiento == TypeMovement.Left)
            {
                Point cabeza = snake[0];
                Point nuevaCabeza = new Point(cabeza.X - CTE_SIDE, cabeza.Y );
                snake.Insert(0, nuevaCabeza);
                snake.RemoveAt(snake.Count - 1);
            }
            if (movimiento == TypeMovement.Up)
            {
                Point cabeza = snake[0];
                Point nuevaCabeza = new Point(cabeza.X, cabeza.Y - CTE_SIDE);
                snake.Insert(0, nuevaCabeza);
                snake.RemoveAt(snake.Count - 1);
            }
            if (movimiento == TypeMovement.Down)
            {
                Point cabeza = snake[0];
                Point nuevaCabeza = new Point(cabeza.X, cabeza.Y + CTE_SIDE);
                snake.Insert(0, nuevaCabeza);
                snake.RemoveAt(snake.Count - 1);
            }
        }

        
    }
}
