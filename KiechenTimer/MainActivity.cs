using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System;
using System.Threading;
using Android.Media;

namespace KiechenTimer
{
    [Activity(Label = "KiechenTimer", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
		//残り時間
		private int _remainingMilliSec = 0;
		//カウントダウン中かどうかを示すフラグ
		private bool _isStart = false;
		//タイマー
		private Timer _timer;

		//スタートボタン
		private Button _startButton;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_main);

			var add10MinButton = FindViewById<Button>(Resource.Id.Add10MinButton);
			add10MinButton.Click += Add10MinButton_Click;
			var add1MinButton = FindViewById<Button>(Resource.Id.Add1MinButton);
			add10MinButton.Click += (s, e) =>
			{
				_remainingMilliSec += 60 * 1000;
				ShowRemainingTime();
			};
			//+10秒ボタンのClickイベント(指定秒だけ加算するメソッドを作成し、さらに簡潔にラムダ式で記述)
			var add10SecButton = FindViewById<Button>(Resource.Id.Add10SecButton);
			add10SecButton.Click += (s, e) => { AddRemainingTime(10); };
			//+1秒ボタンのClickイベント(指定秒だけ加算するメソッドを作成し、さらに簡潔にラムダ式で記述)
			var add1SecButton = FindViewById<Button>(Resource.Id.
			Add1SecButton);
			add1SecButton.Click += (s, e) => {AddRemainingTime(1); };

			//クリアボタンのClickイベント
			var clearButton = FindViewById<Button>(Resource.Id.ClearButton);
			clearButton.Click += (s, e) =>
			{
				// タイマーを止め、残り0秒にする
				_isStart = false;
				_remainingMilliSec = 0;
				//SetStartButtonTitle();
				ShowRemainingTime();
			};

			_startButton = FindViewById<Button>(Resource.Id.StartButton);
			_startButton.Click += StartButton_Click;
			_timer = new Timer(Timer_OnTick, null, 0, 100);
		
        }

		private void StartButton_Click(object sender, EventArgs e)
		{
			_isStart = !_isStart;
			if (_isStart)
			{
				_startButton.Text = "ストップ";
			}
			else
			{
				_startButton.Text = "スタート";
			}
		}

		private void Add10MinButton_Click(object sender, EventArgs e)
		{
			_remainingMilliSec += 600 * 100;
			ShowRemainingTime();
		}

		private void ShowRemainingTime() 
		{
			var sec = _remainingMilliSec / 100;
			FindViewById<TextView>(Resource.Id.RemainingTimeTextView).Text
				= string.Format("{0:f0}:{1:d2}",
				sec / 60,
				sec % 60);
		}

		private void Timer_OnTick (object state)
		{
			if (!_isStart) 
			{
				return;
			}

			RunOnUiThread(() =>
			{
			_remainingMilliSec -= 100;
			if (_remainingMilliSec <= 0)
			{
				//0ミリ秒になった
				_isStart = false;
				_remainingMilliSec = 0;
				_startButton.Text = "スタート";
				//アラームから鳴らす
				var toneGenerator = new ToneGenerator(Stream.System, 50);
					toneGenerator.StartTone(Tone.PropBeep);
				}
				ShowRemainingTime();
			});

		}

		private void AddRemainingTime(int seconds)
		{
			_remainingMilliSec += seconds * 1000;
			ShowRemainingTime();
		}

		class MyClass 
		{
			private string _myProp;

			public string MyProp 
			{
				get { return _myProp; }
				set { _myProp = value; }
			}
	
		}
	}
}

