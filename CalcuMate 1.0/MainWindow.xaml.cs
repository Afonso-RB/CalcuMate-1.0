using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.IO;
using System.Windows.Media.Imaging;

namespace CalcuMate_1._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Classe que faz a solicitação à API do Wolfram Alpha
        private WolframAlphaClient wolframAlphaClient = new WolframAlphaClient();


        private double currentAngle = 0;
        private int menu = 1;
        private bool readyToRun = false;
        private bool canRun = true;

        //
        public MainWindow()
        {
            InitializeComponent();

           // tbAreaExibicao.Focus();
        }

        private void AumentarPainelOpd()
        {
            wPainelOperadores.Height = 200;
            borderWpOperadores.Height = 200;
            borderWpOperadores.Width = 128;
            Canvas.SetBottom(borderWpOperadores, 25);
            Canvas.SetLeft(borderWpOperadores, 10);
        }
        private void DiminuirPainelOpd()
        {
            wPainelOperadores.Height = 101;
            borderWpOperadores.Height = 101;
            borderWpOperadores.Width = 101;
            Canvas.SetBottom(borderWpOperadores, 137);
            Canvas.SetLeft(borderWpOperadores, 20);
        }
        //Botões Numéricos
        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
                Color specialColor = Color.FromRgb(255, 99, 71);
                button.Background = new SolidColorBrush(specialColor);

                tbAreaExibicao.Text += button.Content;
            }
        }

        private void Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
                Color normalColor = Color.FromRgb(78, 78, 78);
                button.Background = new SolidColorBrush(normalColor);
            }
        }
       
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                Color normalColor = Color.FromRgb(78, 78, 78);
                button.Background = new SolidColorBrush(normalColor);
            }
        } 


        //Botões Aicionais

        private void AditionalButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Button button = (Button)sender;
            Color normalColor = Color.FromRgb(0, 255, 127);
            button.Background = new SolidColorBrush(normalColor);
        }
        private void AditionalButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Button button = (Button)sender;
            Color normalColor = Color.FromRgb(50, 205, 50);
            button.Background = new SolidColorBrush(normalColor);
        }

        private void AditionalButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            Color normalColor = Color.FromRgb(50, 205, 50);
            button.Background = new SolidColorBrush(normalColor);
        }

        //Botão de Igual
        private async void btnIgual_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string input = "2 + 2";
            string result = await wolframAlphaClient.QueryWolframAlpha(input);
            tbAreaExibicao.Text = result;

            if (e.ChangedButton == MouseButton.Left)
            {
                Color specialColor = Color.FromRgb(192, 57, 43);
                btnIgual.Background = new SolidColorBrush(specialColor);
            }

            try
            {
                imgLedRed.Source = new BitmapImage(new Uri("pack://application:,,,/assets/led_red_on.png"));
                imgLedYellow.Source = new BitmapImage(new Uri("pack://application:,,,/assets/led_yellow_off.png"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar a imagem: {ex.Message}");
            }
        }
        private void btnIgual_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Color normalColor = Color.FromRgb(255, 69, 0);
            btnIgual.Background = new SolidColorBrush(normalColor);
        }

        private void btnIgual_MouseLeave(object sender, MouseEventArgs e)
        {
            Color normalColor = Color.FromRgb(255, 69, 0);
            btnIgual.Background = new SolidColorBrush(normalColor);
        }

       //Painel e Botões de Operadores
       private void OperatorButton_PreviewMouseDown(object sender, MouseEventArgs e)
        {
            tbAreaExibicao.Focus();
            Button button = (Button)sender;
            Color specialColor = Color.FromRgb(192, 57, 43);
            button.Background = new SolidColorBrush(specialColor);
            tbAreaExibicao.Text += button.Content;
        }
        private void OperatorButton_PreviewMouseUp(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            Color specialColor = Color.FromRgb(255, 69, 0);
            button.Background = new SolidColorBrush(specialColor);
        }

        private void btnMenuOperadores_Click(object sender, RoutedEventArgs e)
        {
            currentAngle += 180;

            DoubleAnimation rotateAnimation = new DoubleAnimation
            {
                To = currentAngle,
                Duration=new Duration(TimeSpan.FromSeconds(0.2)),
               FillBehavior=FillBehavior.HoldEnd
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

            //Menu de Operações
            menu++;
            switch (menu)
            {
                case 1:
                    wpConversoesDeUnidades.Visibility = Visibility.Collapsed;
                    wpAritmetica.Visibility = Visibility.Visible;
                    DiminuirPainelOpd();
                    break;
                case 2:
                    wpAritmetica.Visibility = Visibility.Collapsed;
                    wpTrigonometria.Visibility = Visibility.Visible;
                    break;
                case 3:
                    wpTrigonometria.Visibility = Visibility.Collapsed;
                    wpEquações.Visibility = Visibility.Visible;
                    AumentarPainelOpd();
                    break;
                case 4:
                    wpEquações.Visibility = Visibility.Collapsed;
                    wpLogaritmosEExponênciação.Visibility = Visibility.Visible;
                    DiminuirPainelOpd();
                    break;
                case 5:
                    wpLogaritmosEExponênciação.Visibility = Visibility.Collapsed;
                    wpRaizesEPotencias.Visibility = Visibility.Visible;
                    break;
                default:
                    wpRaizesEPotencias.Visibility = Visibility.Collapsed;
                    wpConversoesDeUnidades.Visibility = Visibility.Visible;
                    wPainelOperadores.Height = 200;
                    AumentarPainelOpd();
                    menu = 0;
                    break;
            }
        }

        private void btnMenuOperadores_MouseEnter(object sender, MouseEventArgs e)
        {
            btnMenuOperadores.Width = 28;
            btnMenuOperadores.Height = 28;
            Color specialColor = Color.FromRgb(21, 101, 179);
            btnMenuOperadores.Background = borderMenuOp.Background = new SolidColorBrush(specialColor);
        }

        private void btnMenuOperadores_MouseLeave(object sender, MouseEventArgs e)
        {
            btnMenuOperadores.Width = 25;
            btnMenuOperadores.Height = 25;
            Color normalColor = Color.FromRgb(30, 144, 255);
            btnMenuOperadores.Background = borderMenuOp.Background = new SolidColorBrush(normalColor);
        }
        
        //Alternância de Leds
        private void tbAreaExibicao_GotFocus(object sender, RoutedEventArgs e)
        {
            if (canRun)
                tbAreaExibicao.Clear();
            canRun = false;
        }

        private void tbAreaExibicao_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void tbAreaExibicao_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (readyToRun)
            {
                try
                {
                    imgLedYellow.Source = new BitmapImage(new Uri("pack://application:,,,/assets/led_yellow_on.png"));
                    imgLedRed.Source = new BitmapImage(new Uri("pack://application:,,,/assets/led_red_off.png"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar a imagem: {ex.Message}");
                }
                tbAreaExibicao.Focus();
            }
              
            readyToRun = true;
        }

        private void btnClean_Click(object sender, RoutedEventArgs e)
        {
            tbAreaExibicao.Clear();
        }

        private void btnBackSpace_Click(object sender, RoutedEventArgs e)
        {
            int caretPosition = tbAreaExibicao.CaretIndex;
            if (!tbAreaExibicao.IsFocused)
                tbAreaExibicao.CaretIndex = tbAreaExibicao.Text.Length;
            
            tbAreaExibicao.Focus();

            if (caretPosition > 0)
            {
                tbAreaExibicao.Text = tbAreaExibicao.Text.Remove(caretPosition - 1, 1);
                tbAreaExibicao.CaretIndex = caretPosition - 1;
            }
        }

      
    }
}