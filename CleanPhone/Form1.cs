using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;
using Ookii.Dialogs;

namespace CleanPhone
{
    
    public partial class Form1 : Form
    {
        public string InputFolderPath { get; set; }
        public string OutputFolderPath { get; set; }
        public bool IsRecursive { get; set; }
        private bool _validDragDrop;
        public bool DoOverwrite { get; set; }
        public bool OpenFolderWhenComplete { get; set; }
        public bool IsProcessing { get; set; }
        public bool RemoveWhenComplete { get; set; }
        public bool HasSeparateOutputFolder { get; set; }
        private CancellationTokenSource _cancellationTokenSource;
        private int _totalCount;
        private readonly ConcurrentDictionary<string, double> _completionPercentages = new ConcurrentDictionary<string, double>(); 
        private readonly ConcurrentDictionary<string, TimeSpan> _durations = new ConcurrentDictionary<string, TimeSpan>();
        private readonly List<Process> _runningProcesses = new List<Process>();
        private readonly TaskbarManager _taskbarManager = TaskbarManager.Instance;
        private readonly ConcurrentDictionary<string, Stopwatch> _processRunningTimes = new ConcurrentDictionary<string, Stopwatch>();
        private readonly Stopwatch _globalRunningTime = new Stopwatch();
        public double Progress
        {
            get
            {
                if (_totalCount == 0)
                    return 0;
                var sum = _completionPercentages.Sum(x => x.Value);
                return sum / _totalCount;
            }
        }

        public Quality Quality { get; set; }

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.Fixed3D;
            IsRecursive = true;
            DoOverwrite = true;
            RemoveWhenComplete = true;
            HasSeparateOutputFolder = false;
            UpdateOutputFolderForm();
            qualityComboBox.DataSource = Enum.GetNames(typeof (Quality));
            LoadComboBox(qualityComboBox);
        }

        public static void LoadComboBox(ComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(Quality))
                .Cast<Enum>()
                .Select(value => new
                {
                    ((DescriptionAttribute) Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute))).Description,
                    value
                })
                .OrderBy(item => item.value)
                .ToList();
            cbo.DisplayMember = "Description";
            cbo.ValueMember = "value";
        }

        private string GetQualityCommand()
        {
            return Quality
                .GetType()
                .GetTypeInfo()
                .GetDeclaredField(Quality.ToString())
                .GetCustomAttribute<Command>()
                .CommandString;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            HandleFormErrors();

            Quality = (Quality)Enum.Parse(typeof(Quality), qualityComboBox.SelectedValue.ToString());

            var files = GetInputFiles();

            if (!files.Any())
            {
                DisplayStatusBarError(@"Found 0 valid files in the directory, check recursion option and try again");
                return;
            }

            _totalCount = files.Count();

            startButton.Enabled = false;
            cancelButton.Enabled = true;
            outputBrowse.Enabled = false;
            inputBrowse.Enabled = false;

            _cancellationTokenSource = new CancellationTokenSource();

            Text = string.Format("Processing: {0}%", Progress);
            toolStripStatusLabel1.Text = string.Format("Completed {0} of {1}", 0, _totalCount);

            IsProcessing = true;
            StartStopwatch();

            await Task.Run(() => ProcessSongs(files));

            IsProcessing = false;
            Reset();
            if (OpenFolderWhenComplete)
            {
                Process.Start(OutputFolderPath);
            }
            outputBrowse.Enabled = true;
            inputBrowse.Enabled = true;
        }

        private void HandleFormErrors()
        {
            if (!CheckDependencies())
            {
                DisplayStatusBarError("Could not find FFmpeg, please verify that it is installed.");
            }
            if (!Directory.Exists(InputFolderPath) || !Directory.Exists(OutputFolderPath))
            {
                DisplayStatusBarError("Input or output directory is an invalid directory");
            }
        }

        private async void DisplayStatusBarError(string error)
        {
            toolStripStatusLabel1.Text = error;
            await Task.Delay(1500);
            toolStripStatusLabel1.Text = "";
        }

        public async void StartStopwatch()
        {
            _globalRunningTime.Restart();
            await Task.Run(() =>
            {
                var calculateEstimate = false;
                var lastEstimateUpdate = TimeSpan.Zero;
                while (IsProcessing)
                {
                    var span = _globalRunningTime.Elapsed;
                    if (TimeSpan.FromTicks(span.Ticks - lastEstimateUpdate.Ticks) <= TimeSpan.FromSeconds(1))
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    var timeString = String.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
                    Invoke(new Action(() => toolStripStatusLabel2.Text = string.Format("Elapsed: {0}", timeString)));

                    lastEstimateUpdate = span;

                    if (_globalRunningTime.Elapsed > TimeSpan.FromSeconds(4) && Progress < .6d)
                    {
                        calculateEstimate = true;
                    }

                    if (!calculateEstimate) continue;

                    var currentTotal = _processRunningTimes.Sum(x => x.Value.ElapsedTicks);
                    var estimate = TimeSpan.FromTicks((long)(currentTotal / Progress) - currentTotal);
                    timeString = String.Format("{0:00}:{1:00}:{2:00}", estimate.Hours, estimate.Minutes,
                        estimate.Seconds);
                    Invoke(new Action(() => toolStripStatusLabel3.Text = string.Format("Remaining: {0}", timeString)));
                    
                }
            });
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";
        }

        private void ProcessSongs(IEnumerable<FileInfo> files)
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                CancellationToken = _cancellationTokenSource.Token
            };

            try
            {
                Parallel.ForEach(files, options, (x, state) =>
                {
                    ProcessSong(x, GetQualityCommand());
                    Invoke((MethodInvoker)(UpdateStatusBar));
                    if (options.CancellationToken.IsCancellationRequested)
                    {
                        state.Stop();
                    }
                });
            }
            catch(OperationCanceledException)
            {
                
            }
            finally
            {
                _cancellationTokenSource.Dispose();
            }
        }

        private void UpdateStatusBar()
        {
            var completedCount = _completionPercentages.Count(x => x.Value.Equals(1) || Math.Abs(x.Value - 1) < 0.001d);
            toolStripStatusLabel1.Text = string.Format("Completed {0} of {1}", completedCount, _totalCount);
        }

        private async void OnCancellation()
        {
            var runningProcessesCopy = new List<Process>(_runningProcesses);
            foreach (var runningProcess in runningProcessesCopy)
            {
                try
                {
                    runningProcess.Kill();
                }
                // Already exited, do nothing
                catch (InvalidOperationException)
                {
                }
                var match = Regex.Match(runningProcess.StartInfo.Arguments, @"-i ([""'])(?:(?=(\\?))\2.)*?\1");
                var inputFilePath = match.Value.Replace("-i ", "").Replace("\"", "");
                var outputFilePath = Path.ChangeExtension(inputFilePath, ".mp3");
                await Task.Run(() =>
                {
                    while (IsFileLocked(outputFilePath)) ;
                });
                File.Delete(outputFilePath);
            }
            progressBar1.Value = 0;
            Reset();
        }

        private void Reset()
        {
            startButton.Enabled = true;
            cancelButton.Enabled = false;
            Text = @"Flac to MP3 Converter";
            progressBar1.Value = 0;
            _completionPercentages.Clear();
            _durations.Clear();
            _processRunningTimes.Clear();
            toolStripStatusLabel1.Text = "";
            _taskbarManager.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        private void ReadFFMpegOutput(object sender, string e)
        {
            if (string.IsNullOrWhiteSpace(e)) return;

            var process = (Process) sender;
            var file = process.StartInfo.Arguments;
            if (!_durations.ContainsKey(file))
            {
                var duration = GetDuration(e);
                if (duration.Equals(TimeSpan.Zero))
                    return;
                _durations.GetOrAdd(file, duration);
            }
            else
            {
                var completePercent = GetPercentageFileComplete(e, _durations[file]);
                if (completePercent.Equals(0))
                {
                    return;
                }
                _completionPercentages[file] = completePercent;
                Invoke((MethodInvoker)(UpdateProgressBar));
            }
        }

        private static TimeSpan GetDuration(string output)
        {
            const string pattern = @"Duration: \d{2}:\d{2}:\d{2}\.\d{2}";
            var regex = new Regex(pattern);
            var duration = regex.Match(output);
            if (!duration.Success) return TimeSpan.Zero;
            var spanString = duration.Value.Replace("Duration: ", "");
            return TimeSpan.Parse(spanString);
        }

        private static double GetPercentageFileComplete(string output, TimeSpan totalDuration)
        {
            const string pattern = @"time=\d{2}:\d{2}:\d{2}\.\d{2}";
            var regex = new Regex(pattern);
            var match = regex.Match(output);
            if (!match.Success) return 0;
            var spanString = match.Value.Replace("time=", "");
            var completeDuration = TimeSpan.Parse(spanString);
            return (double)completeDuration.Ticks / totalDuration.Ticks;
        }

        private void UpdateProgressBar()
        {
            progressBar1.Value = (int) (Progress*100);
            
            _taskbarManager.SetProgressState(TaskbarProgressBarState.Normal);
            _taskbarManager.SetProgressValue((int) (Progress * 100), 100);
            Text = string.Format("Processing: {0}%", (int) (Progress*100));
        }

        public IList<FileInfo> GetInputFiles()
        {
            var searchOption = IsRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var extensions = new [] {"*.flac", ".*m4a", "*.wav", "*.aiff"};
            var files = new List<FileInfo>();
            foreach (var ext in extensions)
            {
                files.AddRange(Directory.EnumerateFiles(InputFolderPath, ext, searchOption).ToList().ConvertAll(x => new FileInfo(x)));
            }
            return files;
        }

        public void ProcessSong(FileInfo fi, string qualityCommand)
        {
            var inputPath = fi.FullName;
            var outputPath = Path.ChangeExtension(fi.FullName, ".mp3");
            if (HasSeparateOutputFolder)
            {
                outputPath = Path.Combine(OutputFolderPath,
                    Path.ChangeExtension(fi.FullName.Replace(InputFolderPath + "\\", ""), ".mp3"));
                Directory.CreateDirectory(new FileInfo(outputPath).DirectoryName);
            }
            var overwrite = DoOverwrite ? "-y" : "-n";
            var transcodeArgs = string.Format("-i \"{0}\" -map 0:0 {1} \"{2}\" {3}", inputPath, qualityCommand, outputPath, overwrite);

            _processRunningTimes.TryAdd(transcodeArgs, Stopwatch.StartNew());

            var transcode = new Process
            {
                StartInfo = new ProcessStartInfo("ffmpeg")
                {
                    Arguments = transcodeArgs,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            transcode.Start();
            _runningProcesses.Add(transcode);
            while (!transcode.HasExited)
            {
                while (!transcode.StandardError.EndOfStream)
                {
                    var line = transcode.StandardError.ReadLine();
                    ReadFFMpegOutput(transcode, line);
                }
            }
            try
            {
                _runningProcesses.Remove(transcode);
            }
            // Already Deleted
            catch (ArgumentOutOfRangeException)
            {
            }
            Stopwatch stopwatch;
            _processRunningTimes.TryGetValue(transcodeArgs, out stopwatch);
            if (stopwatch != null)
            {
                stopwatch.Stop();
            }

            if (RemoveWhenComplete)
            {
                Task.Run(() =>
                {
                    while (IsFileLocked(inputPath)) ;
                    File.Delete(inputPath);
                });
            }
        }

        public bool CheckDependencies()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "ffmpeg",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }
                };
                process.Start();
                return true;
            }
            catch (Win32Exception)
            {
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();
            var closeResult = dialog.ShowDialog();

            if (closeResult != DialogResult.OK)
                return;

            InputFolderPath = dialog.SelectedPath;
            inputFolderTextBox.Text = InputFolderPath;
            if (!HasSeparateOutputFolder)
            {
                outputFolderTextBox.Text = InputFolderPath;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            IsRecursive = !IsRecursive;
        }

        private void haveOutputFolderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            HasSeparateOutputFolder = !HasSeparateOutputFolder;
            UpdateOutputFolderForm();
        }

        private void UpdateOutputFolderForm()
        {
            if (HasSeparateOutputFolder)
            {
                outputBrowse.Enabled = true;
                outputFolderTextBox.ReadOnly = false;
                outputFolderTextBox.Text = OutputFolderPath;
                removeWhenCompleteCheckbox.Checked = false;
                removeWhenCompleteCheckbox.Enabled = false;
                RemoveWhenComplete = false;
            }

            else
            {
                outputBrowse.Enabled = false;
                outputFolderTextBox.ReadOnly = true;
                outputFolderTextBox.Text = InputFolderPath;
                removeWhenCompleteCheckbox.Enabled = true;
                removeWhenCompleteCheckbox.Checked = true;
            }
        }

        private void outputBrowse_Click(object sender, EventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();
            var closeResult = dialog.ShowDialog();

            if (closeResult != DialogResult.OK)
                return;

            OutputFolderPath = dialog.SelectedPath;
            outputFolderTextBox.Text = OutputFolderPath;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!IsProcessing) return;

            _cancellationTokenSource.Cancel();
            OnCancellation();
        }

        private void overwriteCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            DoOverwrite = !DoOverwrite;
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            RemoveWhenComplete = !RemoveWhenComplete;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
            OnCancellation();
        }

        public static bool IsFileLocked(string path)
        {
            FileStream stream = null;
            var file = new FileInfo(path);
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }

        private void inputFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            InputFolderPath = inputFolderTextBox.Text;
            if (!HasSeparateOutputFolder) outputFolderTextBox.Text = InputFolderPath;
        }

        private void outputFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            OutputFolderPath = outputFolderTextBox.Text;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            _validDragDrop = false;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var data = (string[]) e.Data.GetData(DataFormats.FileDrop);
            if (data.Length != 1)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            if (!Directory.Exists(data[0]))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            _validDragDrop = true;
            e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (!_validDragDrop) return;

            InputFolderPath = ((string[]) e.Data.GetData(DataFormats.FileDrop))[0];
            inputFolderTextBox.Text = InputFolderPath;

            if (HasSeparateOutputFolder) return;

            OutputFolderPath = InputFolderPath;
            outputFolderTextBox.Text = InputFolderPath;
        }

        private void openFolderWhenCompleteCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            OpenFolderWhenComplete = !OpenFolderWhenComplete;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class Command : Attribute
    {
        public Command(string command)
        {
            CommandString = command;
        }

        public string CommandString { get; private set; }
    }

    public enum Quality
    {
        [Description("MP3 V2")]
        [Command("-q:a 2")]
        MP3_V2,

        [Description("MP3 V0")]
        [Command("-q:a 0")]
        MP3_V0,

        [Description("MP3 320")]
        [Command("-b:a 320k")]
        MP3_320
    }
}
