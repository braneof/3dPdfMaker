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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.IO;

namespace ThreeDPdfMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSTLFile_Click(object sender, RoutedEventArgs e)
        {
            // Run the tests
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Choose an .stl file";
            //dlg.IsFolderPicker = true;
            //dlg.InitialDirectory = TextBoxSessionFile.Text;  // uses default or recent directory if text is empty or invalid

            //dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            //dlg.DefaultDirectory = Environment.CurrentDirectory;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = true;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var stlFile = dlg.FileName;
                LabelSessionFile.Content = stlFile;
                /* debug */
                string outputFile = System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + ".stl";
                //string outputDir = System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\";
                SetDebugTextAndClipboard(outputFile);
                /* end debug */
            }
        }


        private void ButtonMakePDF_Click(object sender, RoutedEventArgs e)
        {
            string basePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //string cmd = ssprintf("\"%s/R:\OrthoVis Releases\OtherTools\WebSessionConverter_latest\SessionConverter.exe\" \"%s/data\" \"%s\" \"%s.websession.zip\"", appPath.c_str(), appPath.c_str(), sessionFile.c_str(), sessionNoExtension.c_str());
            string fullPath = basePath + "\\StlToU3d\\StlToU3d.exe";
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = System.IO.Path.GetFileName(fullPath);
            process.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(fullPath);
            process.StartInfo.Arguments = "\"" + LabelSessionFile.Content.ToString() + "\"";
            process.Start();
            process.WaitForExit();// Waits here for the process to exit.

            string outputU3d = System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + ".u3d";
            string stlToU3dPath = basePath + "\\StlToU3d";
            string pdfDirPath = basePath + "\\pdf";
            File.Move(stlToU3dPath + "\\" + outputU3d, pdfDirPath + "\\" + "models.u3d");
            SetDebugTextAndClipboard("Moved " + stlToU3dPath + "\\" + outputU3d + " to " + pdfDirPath + "\\" + outputU3d);
            //fullPath = @"R:\OrthoVis Releases\OtherTools\StlToU3d_latest\StlToU3d.exe";
            process.StartInfo.FileName = "pdflatex.exe";
            process.StartInfo.WorkingDirectory = basePath;
            //string outputZip = System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + ".zip";
            //string outputDir = System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\";
            string pdfDir = basePath + "\\pdf";
            process.StartInfo.Arguments = String.Format("-output-directory \"{0}\" \"{0}\\OrthoVis3dModelPlan.tex\"", pdfDir);
            process.Start();
            process.WaitForExit();// Waits here for the process to exit.
        }

 

        private void SetDebugTextAndClipboard(string text)
        {
            LabelDebug.Content = text;
            System.Windows.Clipboard.SetText(text);
        }

    }
}
