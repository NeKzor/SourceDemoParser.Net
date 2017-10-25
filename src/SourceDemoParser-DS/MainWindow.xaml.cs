using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Series;

namespace SourceDemoParser_DS
{
	public partial class MainWindow : Window
	{
		public FunctionSeries YzFunction { get; set; }
		public FunctionSeries XzFunction { get; set; }
		public FunctionSeries XyFunction { get; set; }
		public FunctionSeries MxyFunction { get; set; }
		public FunctionSeries YzStart { get; set; }
		public FunctionSeries XzStart { get; set; }
		public FunctionSeries XyStart { get; set; }
		public FunctionSeries YzEnding { get; set; }
		public FunctionSeries XzEnding { get; set; }
		public FunctionSeries XyEnding { get; set; }

		private DemoSimulation _ds;
		private CancellationTokenSource _source;
		private bool _isPlaying;
		private bool _isPaused;
		private bool _showCommands;
		private bool _showButtons;
		private double _sleepFactor;
		private int _jumpTick;
		private bool _jump;

#if DEBUG
		private const string _file = @"C:\Program Files (x86)\Steam\steamapps\common\Portal 2\portal2\CeilingButton_1861_Zypeh.dem";
#endif
		private const LineStyle _lineStyle = LineStyle.Solid;
		private const MarkerType _markerType = MarkerType.None;
		private const double _markerSize = 1.5;
		private const double _delayFactor = 0.1;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void MenuItem_Play_Click(object sender, RoutedEventArgs e)
		{
			if (!_isPlaying)
			{
				_source = new CancellationTokenSource();
				Task.Factory.StartNew(Simulation, Dispatcher, _source.Token);
			}
		}

		private void MenuItem_Stop_Click(object sender, RoutedEventArgs e)
		{
			if (_isPlaying)
				_isPlaying = false;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_source = new CancellationTokenSource();
#if DEBUG
			_ds = new DemoSimulation(_file);
#endif

			YzFunction = new FunctionSeries() { Title = "Y/Z", Color = OxyColors.DarkRed, LineStyle = _lineStyle, MarkerType = _markerType, MarkerSize = _markerSize };
			XzFunction = new FunctionSeries() { Title = "X/Z", Color = OxyColors.DarkRed, LineStyle = _lineStyle, MarkerType = _markerType, MarkerSize = _markerSize };
			XyFunction = new FunctionSeries() { Title = "X/Y", Color = OxyColors.DarkRed, LineStyle = _lineStyle, MarkerType = _markerType, MarkerSize = _markerSize };
			MxyFunction = new FunctionSeries() { Title = "X/Y", Color = OxyColors.DarkRed, LineStyle = LineStyle.Solid, MarkerType = _markerType, MarkerSize = _markerSize };
			YzStart = new FunctionSeries() { Title = "Start", MarkerType = MarkerType.Circle, MarkerSize = 6, Color = OxyColors.DarkGreen };
			XzStart = new FunctionSeries() { Title = "Start", MarkerType = MarkerType.Circle, MarkerSize = 6, Color = OxyColors.DarkGreen };
			XyStart = new FunctionSeries() { Title = "Start", MarkerType = MarkerType.Circle, MarkerSize = 6, Color = OxyColors.DarkGreen };
			YzEnding = new FunctionSeries() { Title = "End", MarkerType = MarkerType.Circle, MarkerSize = 6, Color = OxyColors.Blue };
			XzEnding = new FunctionSeries() { Title = "End", MarkerType = MarkerType.Circle, MarkerSize = 6, Color = OxyColors.Blue };
			XyEnding = new FunctionSeries() { Title = "End", MarkerType = MarkerType.Circle, MarkerSize = 6, Color = OxyColors.Blue };
#if DEBUG
			SetStartingPoints(_ds.GetStartingPoints());
			SetEndingPoints(_ds.GetEndingPoints());
#endif
			ViewOriginA.Model = new PlotModel { Title = "ViewOrigin A" };
			ViewOriginB.Model = new PlotModel { Title = "ViewOrigin B" };
			ViewOriginC.Model = new PlotModel { Title = "ViewOrigin C" };
			ViewAngles.Model = new PlotModel { Title = "Mouse" };

			ViewOriginA.Model.Series.Add(YzStart);
			ViewOriginB.Model.Series.Add(XzStart);
			ViewOriginC.Model.Series.Add(XyStart);
			ViewOriginA.Model.Series.Add(YzEnding);
			ViewOriginB.Model.Series.Add(XzEnding);
			ViewOriginC.Model.Series.Add(XyEnding);
			ViewOriginA.Model.Series.Add(YzFunction);
			ViewOriginB.Model.Series.Add(XzFunction);
			ViewOriginC.Model.Series.Add(XyFunction);
			ViewAngles.Model.Series.Add(MxyFunction);
		}

		private void Window_Unloaded(object sender, RoutedEventArgs e)
		{
			_source.Cancel();
		}

		private void Simulation(object d)
		{
			if (_ds == null)
				return;

			var dispatcher = (Dispatcher)d;
			var tps = _ds.GetTps();

			var tick = _jumpTick;
			while (!_source.IsCancellationRequested)
			{
				_isPlaying = true;
				tick = 0;
				Dispatcher.Invoke(ClearAll);
				Dispatcher.Invoke(RefreshAll);

				while (tick <= _ds.Demo.PlaybackTicks)
				{
					Dispatcher.Invoke(() => Title = $"Demo Simulator (SourceDemoParser.Net) - Current Tick: {tick}");
					var data = _ds.GetNextData();

					// Process commands
					var commands = string.Empty;
					if ((_showCommands) && (data.Commands.Count != 0))
						commands = data.Commands.GetCommands();

					// Process points
					if (data.Info.Count != 0)
						Dispatcher.Invoke(() => SetNewDataPoints(data.Info[0].GetPoints()));

					// Process cmd
					var buttons = string.Empty;
					if ((_showButtons) && (data.User.Count != 0))
					{
						if (data.User[0].IsKeyPressed(KeyType.W)) buttons += "W";
						if (data.User[0].IsKeyPressed(KeyType.A)) buttons += "A";
						if (data.User[0].IsKeyPressed(KeyType.S)) buttons += "S";
						if (data.User[0].IsKeyPressed(KeyType.D)) buttons += "D";
					}
					if (data.User.Count != 0)
						Dispatcher.Invoke(() => SetNewMousePoint(data.User[0].GetNewMousePoint(MxyFunction.Points.Last())));

					Dispatcher.Invoke(() => CommandLabel.Content = commands);
					Dispatcher.Invoke(() => UserLabel.Content = buttons);
					Dispatcher.Invoke(RefreshAll);
					tick++;

					if (_isPaused)
					{
						Dispatcher.Invoke(() => Title = "Demo Simulator (SourceDemoParser.Net) - Paused");
						while (_isPaused)
						{
							Task.Yield();
							if (_source.IsCancellationRequested)
								goto end;
						}
					}

					// Goto
					if (_jump)
					{
						_jump = false;
						tick = 0;
						Dispatcher.Invoke(ClearAll);
						while (tick <= _jumpTick)
						{
							_ds.SetTick(tick);
							data = _ds.GetNextData();
							if (data.Info.Count != 0)
								Dispatcher.Invoke(() => SetNewDataPoints(data.Info[0].GetPoints()));
							if (data.User.Count != 0)
								Dispatcher.Invoke(() => SetNewMousePoint(data.User[0].GetNewMousePoint(MxyFunction.Points.Last())));
							tick++;
						}
					}

					// Stop
					if (!_isPlaying)
					{
						while (tick <= _ds.Demo.PlaybackTicks)
						{
							data = _ds.GetNextData();
							if (data.Info.Count != 0)
								Dispatcher.Invoke(() => SetNewDataPoints(data.Info[0].GetPoints()));
							if (data.User.Count != 0)
								Dispatcher.Invoke(() => SetNewMousePoint(data.User[0].GetNewMousePoint(MxyFunction.Points.Last())));
							tick++;
						}
						break;
					}

					// Oh well, this isn't accurate :(
					Thread.Sleep(TimeSpan.FromSeconds(tps / _sleepFactor));
				}
				if (!_isPlaying)
					break;
			}
end:
			Dispatcher.Invoke(() => Title = "Demo Simulator (SourceDemoParser.Net)");
			Dispatcher.Invoke(RefreshAll);
			Dispatcher.Invoke(() => CommandLabel.Content = string.Empty);
			Dispatcher.Invoke(() => UserLabel.Content = string.Empty);
			_isPlaying = false;
		}

		private void RefreshAll()
		{
			ViewOriginA.Model.InvalidatePlot(true);
			ViewOriginB.Model.InvalidatePlot(true);
			ViewOriginC.Model.InvalidatePlot(true);
			ViewAngles.Model.InvalidatePlot(true);
		}

		private void ClearAll()
		{
			YzFunction.Points.Clear();
			XzFunction.Points.Clear();
			XyFunction.Points.Clear();
			MxyFunction.Points.Clear();
			MxyFunction.Points.Add(new DataPoint(0, 0));
		}

		private void SetNewDataPoints((DataPoint Yz, DataPoint Xz, DataPoint Xy, DataPoint Mxy) points)
		{
			YzFunction.Points.Add(points.Yz);
			XzFunction.Points.Add(points.Xz);
			XyFunction.Points.Add(points.Xy);
		}

		public void SetNewMousePoint(DataPoint pos)
		{
			var last = MxyFunction.Points.Last();
			if ((pos.X != last.X) || (pos.Y != last.Y))
				MxyFunction.Points.Add(pos);
			else
				MxyFunction.Points.Add(new DataPoint(0, 0));
		}

		private void SetStartingPoints((DataPoint Yz, DataPoint Xz, DataPoint Xy) points)
		{
			YzStart.Points.Clear();
			XzStart.Points.Clear();
			XyStart.Points.Clear();
			MxyFunction.Points.Clear();
			YzStart.Points.Add(points.Yz);
			XzStart.Points.Add(points.Xz);
			XyStart.Points.Add(points.Xy);
			MxyFunction.Points.Add(new DataPoint(0, 0));
		}

		private void SetEndingPoints((DataPoint Yz, DataPoint Xz, DataPoint Xy) points)
		{
			YzEnding.Points.Clear();
			XzEnding.Points.Clear();
			XyEnding.Points.Clear();
			YzEnding.Points.Add(points.Yz);
			XzEnding.Points.Add(points.Xz);
			XyEnding.Points.Add(points.Xy);
		}

		private void MenuItem_Checked(object sender, RoutedEventArgs e) => _showCommands = true;
		private void MenuItem_Checked_1(object sender, RoutedEventArgs e) => _showButtons = true;
		private void MenuItem_Unchecked(object sender, RoutedEventArgs e) => _showCommands = false;
		private void MenuItem_Unchecked_1(object sender, RoutedEventArgs e) => _showButtons = false;

		private void Speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_isPaused = e.NewValue == 0;
			_sleepFactor = e.NewValue;
		}

		private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
		{
			if (!_isPlaying)
			{
				var ofd = new OpenFileDialog
				{
					Filter = "Demo Files (*.dem)|*.dem*",
					CheckFileExists = true
				};

				if (ofd.ShowDialog() == true)
				{
					_ds = new DemoSimulation(ofd.FileName);
					SetStartingPoints(_ds.GetStartingPoints());
					SetEndingPoints(_ds.GetEndingPoints());
				}
			}
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key != Key.Enter)
				return;
			if ((int.TryParse(GotoTick.Text, out var tick)) && tick <= (_ds.Demo.PlaybackTicks))
			{
				_jumpTick = tick;
				_jump = true;
				return;
			}
			GotoTick.Text = "0";
		}
	}
}