using System;
using System.Threading;
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
using Life;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StandardLife lifeMatrix;
        Thread iterateThread;

        public MainWindow()
        {
            InitializeComponent();
            lifeMatrix = new StandardLife(20);
            
        }

        private void stopIterate(Object sender = null, RoutedEventArgs rea = null)
        {            
            if (iterateThread != null)
            {
                this.iterateForeverButton.IsEnabled = true;
                this.iterateButton.IsEnabled = true;
                iterateThread.Abort();
            }
        }


        private void disableButtons()
        {
            this.iterateForeverButton.IsEnabled = false;
            this.iterateButton.IsEnabled = false;

        }
        private void iterateForever()
        {
            Action disableAction = disableButtons;
            Dispatcher.Invoke(disableAction);
            try
            {
                while (true)
                {

                    iterate();
                    Thread.Sleep(500);
                }
            }
            catch (ThreadAbortException tae)
            {
                //this is a normal when they press the stop button, no need to do anything
            }
            catch (TaskCanceledException tce)
            {
                //this is normal when the user closes the window, no need to do anything
            }
        }

        private void startIterateForeverThread(object sender = null, RoutedEventArgs e = null)
        {
            ThreadStart job = new ThreadStart(iterateForever);
            iterateThread = new Thread(job);
            iterateThread.Start();

        }

        
        private void updateDisplay()
        {

            lifeMatrix.UpdateDisplay(matrix);
        }

        private void iterate(object sender=null, RoutedEventArgs e=null) {
            lifeMatrix.Iterate();
            Action updateAction = updateDisplay;
            this.Dispatcher.Invoke(updateDisplay);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            matrix.RowDefinitions.Clear();

            for (int rowIndex = 0; rowIndex < lifeMatrix.MatrixSize; rowIndex++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength((int)(matrix.ActualHeight / lifeMatrix.MatrixSize));
                matrix.RowDefinitions.Add(rowDef);
            }
            matrix.ColumnDefinitions.Clear();
            for (int columnIndex = 0; columnIndex < lifeMatrix.MatrixSize; columnIndex++)
            {
                ColumnDefinition colDef = null;
                colDef = new ColumnDefinition();
                colDef.Width = new GridLength(matrix.ActualWidth / lifeMatrix.MatrixSize);
                matrix.ColumnDefinitions.Add(colDef);
            }
            lifeMatrix.Randomize();
            lifeMatrix.Iterate();
            updateDisplay();
        }


        private void Window_Closed(object sender, EventArgs e)
        {

        }
    }


}
