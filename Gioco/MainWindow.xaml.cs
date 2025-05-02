using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Gioco
{
    public partial class MainWindow : Window
    {
        Label labelTempo;
        Label labelPunteggio;
        Button bottoneStart;
        Button bottoneGioco;
        DispatcherTimer timer;
        Random random = new Random();
        bool[,] matrice = new bool[4, 4];
        int riga, colonna;
        int tempo = 0;
        int punteggio = 0;

        public MainWindow()
        {
            InitializeComponent();

            // Imposta la finestra
            this.Title = "Acchiappa il Minion!";
            this.Width = 500;
            this.Height = 550;
            this.ResizeMode = ResizeMode.NoResize;
            this.Background = Brushes.AliceBlue;

            // Crea la griglia
            G1.Background = Brushes.Beige;
            G1.Margin = new Thickness(10);
            G1.ShowGridLines = false;

            for (int i = 0; i < 3; i++)
                G1.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < 4; i++)
                G1.RowDefinitions.Add(new RowDefinition());

            // Etichetta tempo
            labelTempo = new Label
            {
                Content = "Tempo: 0s",
                FontSize = 20,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Background = Brushes.LightSteelBlue,
                Padding = new Thickness(10)
            };
            Grid.SetRow(labelTempo, 0);
            Grid.SetColumn(labelTempo, 0);
            G1.Children.Add(labelTempo);

            // Etichetta punteggio
            labelPunteggio = new Label
            {
                Content = "Punteggio: 0",
                FontSize = 20,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Background = Brushes.LightSteelBlue,
                Padding = new Thickness(10)
            };
            Grid.SetRow(labelPunteggio, 0);
            Grid.SetColumn(labelPunteggio, 2);
            G1.Children.Add(labelPunteggio);

            // Bottone start
            bottoneStart = new Button
            {
                Content = "INIZIA!",
                FontSize = 18,
                Padding = new Thickness(10),
                Background = Brushes.LightGreen,
                Margin = new Thickness(5)
            };
            bottoneStart.Click += Start_Click;
            Grid.SetRow(bottoneStart, 0);
            Grid.SetColumn(bottoneStart, 1);
            G1.Children.Add(bottoneStart);

            // Bottone con immagine del gioco
            bottoneGioco = new Button
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("images/minion.jpg", UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                BorderThickness = new Thickness(0),
                Background = Brushes.Transparent,
                Padding = new Thickness(0),
            };
            bottoneGioco.Click += Minion_Click;

            // Timer
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(700)
            };
            timer.Tick += Timer_Tick;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            bottoneStart.IsEnabled = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            labelTempo.Content = $"Tempo: {++tempo}s";

            // Rimuove il bottone dalla posizione precedente
            G1.Children.Remove(bottoneGioco);

            // Scegli nuova posizione libera
            do
            {
                colonna = random.Next(0, 3);
                riga = random.Next(1, 4);
            }
            while (matrice[riga, colonna]);

            // Imposta posizione nuova
            Grid.SetRow(bottoneGioco, riga);
            Grid.SetColumn(bottoneGioco, colonna);
            G1.Children.Add(bottoneGioco);
        }

        private void Minion_Click(object sender, RoutedEventArgs e)
        {
            punteggio++;
            labelPunteggio.Content = $"Punteggio: {punteggio}";
            matrice[riga, colonna] = true;

            Image x = new Image
            {
                Source = new BitmapImage(new Uri("images/x.png", UriKind.Relative)),
                Stretch = Stretch.Fill
            };
            Grid.SetRow(x, riga);
            Grid.SetColumn(x, colonna);
            G1.Children.Add(x);
            if (punteggio % 2 == 0 && timer.Interval.TotalMilliseconds > 200)
            {
                timer.Interval = TimeSpan.FromMilliseconds(timer.Interval.TotalMilliseconds - 100);
            }
            if (punteggio >= 10)
            {
                timer.Stop();
                MessageBox.Show("Hai vinto!", "Complimenti!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }
    }
}
