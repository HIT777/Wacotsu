namespace Wacotsu
{
	partial class WacotsuForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WacotsuForm));
			this.statusBar = new System.Windows.Forms.StatusStrip();
			this.statusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.availableLiveListView = new System.Windows.Forms.ListView();
			this.thumbnailImageList = new System.Windows.Forms.ImageList(this.components);
			this.reservedLiveListView = new System.Windows.Forms.ListView();
			this.gotLiveListView = new System.Windows.Forms.ListView();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.taskTrayEnabledCheckBox = new System.Windows.Forms.CheckBox();
			this.ballonTipEnabledCheckBox = new System.Windows.Forms.CheckBox();
			this.clockLabel = new System.Windows.Forms.Label();
			this.clockTimer = new System.Windows.Forms.Timer(this.components);
			this.statusBar.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusBar
			// 
			this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarLabel});
			this.statusBar.Location = new System.Drawing.Point(0, 540);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(584, 22);
			this.statusBar.SizingGrip = false;
			this.statusBar.TabIndex = 0;
			this.statusBar.Text = "statusStrip1";
			// 
			// statusBarLabel
			// 
			this.statusBarLabel.Name = "statusBarLabel";
			this.statusBarLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.availableLiveListView);
			this.groupBox1.Location = new System.Drawing.Point(12, 58);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(273, 479);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "予約可能な放送（Wクリック: 予約, 右クリック: ゲート）";
			// 
			// availableLiveListView
			// 
			this.availableLiveListView.LargeImageList = this.thumbnailImageList;
			this.availableLiveListView.Location = new System.Drawing.Point(6, 18);
			this.availableLiveListView.Name = "availableLiveListView";
			this.availableLiveListView.Size = new System.Drawing.Size(260, 452);
			this.availableLiveListView.TabIndex = 0;
			this.availableLiveListView.TileSize = new System.Drawing.Size(230, 44);
			this.availableLiveListView.UseCompatibleStateImageBehavior = false;
			this.availableLiveListView.View = System.Windows.Forms.View.Tile;
			this.availableLiveListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseClick);
			this.availableLiveListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
			// 
			// thumbnailImageList
			// 
			this.thumbnailImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.thumbnailImageList.ImageSize = new System.Drawing.Size(40, 40);
			this.thumbnailImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// reservedLiveListView
			// 
			this.reservedLiveListView.LargeImageList = this.thumbnailImageList;
			this.reservedLiveListView.Location = new System.Drawing.Point(6, 18);
			this.reservedLiveListView.Name = "reservedLiveListView";
			this.reservedLiveListView.Size = new System.Drawing.Size(260, 224);
			this.reservedLiveListView.TabIndex = 2;
			this.reservedLiveListView.TileSize = new System.Drawing.Size(230, 44);
			this.reservedLiveListView.UseCompatibleStateImageBehavior = false;
			this.reservedLiveListView.View = System.Windows.Forms.View.Tile;
			this.reservedLiveListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseClick);
			this.reservedLiveListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
			// 
			// gotLiveListView
			// 
			this.gotLiveListView.LargeImageList = this.thumbnailImageList;
			this.gotLiveListView.Location = new System.Drawing.Point(6, 18);
			this.gotLiveListView.Name = "gotLiveListView";
			this.gotLiveListView.Size = new System.Drawing.Size(260, 198);
			this.gotLiveListView.TabIndex = 3;
			this.gotLiveListView.TileSize = new System.Drawing.Size(230, 44);
			this.gotLiveListView.UseCompatibleStateImageBehavior = false;
			this.gotLiveListView.View = System.Windows.Forms.View.Tile;
			this.gotLiveListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseClick);
			this.gotLiveListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.reservedLiveListView);
			this.groupBox2.Location = new System.Drawing.Point(291, 58);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(275, 248);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "予約済の放送（Wクリック: 取消）";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.gotLiveListView);
			this.groupBox3.Location = new System.Drawing.Point(291, 312);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(275, 225);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "座席確保済の放送（Wクリック: ブラウザで開く）";
			// 
			// notifyIcon
			// 
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "Wacotsu";
			this.notifyIcon.Visible = true;
			this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.taskTrayEnabledCheckBox);
			this.groupBox4.Controls.Add(this.ballonTipEnabledCheckBox);
			this.groupBox4.Location = new System.Drawing.Point(12, 12);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(356, 40);
			this.groupBox4.TabIndex = 8;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "設定";
			// 
			// taskTrayEnabledCheckBox
			// 
			this.taskTrayEnabledCheckBox.AutoSize = true;
			this.taskTrayEnabledCheckBox.Checked = global::Wacotsu.Properties.Settings.Default.TaskTrayEnabled;
			this.taskTrayEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.taskTrayEnabledCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Wacotsu.Properties.Settings.Default, "TaskTrayEnabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.taskTrayEnabledCheckBox.Location = new System.Drawing.Point(6, 18);
			this.taskTrayEnabledCheckBox.Name = "taskTrayEnabledCheckBox";
			this.taskTrayEnabledCheckBox.Size = new System.Drawing.Size(156, 16);
			this.taskTrayEnabledCheckBox.TabIndex = 6;
			this.taskTrayEnabledCheckBox.Text = "最小化時タスクトレイに格納";
			this.taskTrayEnabledCheckBox.UseVisualStyleBackColor = true;
			// 
			// ballonTipEnabledCheckBox
			// 
			this.ballonTipEnabledCheckBox.AutoSize = true;
			this.ballonTipEnabledCheckBox.Checked = global::Wacotsu.Properties.Settings.Default.BallonTipEnabled;
			this.ballonTipEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ballonTipEnabledCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Wacotsu.Properties.Settings.Default, "BallonTipEnabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.ballonTipEnabledCheckBox.Location = new System.Drawing.Point(168, 18);
			this.ballonTipEnabledCheckBox.Name = "ballonTipEnabledCheckBox";
			this.ballonTipEnabledCheckBox.Size = new System.Drawing.Size(177, 16);
			this.ballonTipEnabledCheckBox.TabIndex = 7;
			this.ballonTipEnabledCheckBox.Text = "座席確保時バルーンと音で通知";
			this.ballonTipEnabledCheckBox.UseVisualStyleBackColor = true;
			// 
			// clockLabel
			// 
			this.clockLabel.AutoSize = true;
			this.clockLabel.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.clockLabel.Location = new System.Drawing.Point(374, 25);
			this.clockLabel.Name = "clockLabel";
			this.clockLabel.Size = new System.Drawing.Size(192, 19);
			this.clockLabel.TabIndex = 9;
			this.clockLabel.Text = "0000/00/00（月）00:00";
			// 
			// clockTimer
			// 
			this.clockTimer.Enabled = true;
			this.clockTimer.Interval = 60000;
			this.clockTimer.Tick += new System.EventHandler(this.clockTimer_Tick);
			// 
			// WacotsuForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 562);
			this.Controls.Add(this.clockLabel);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.statusBar);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "WacotsuForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Wacotsu 0.2";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WacotsuForm_FormClosing);
			this.Load += new System.EventHandler(this.WacotsuForm_Load);
			this.ClientSizeChanged += new System.EventHandler(this.WacotsuForm_ClientSizeChanged);
			this.statusBar.ResumeLayout(false);
			this.statusBar.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusBar;
		private System.Windows.Forms.ToolStripStatusLabel statusBarLabel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView availableLiveListView;
		private System.Windows.Forms.ImageList thumbnailImageList;
		private System.Windows.Forms.ListView reservedLiveListView;
		private System.Windows.Forms.ListView gotLiveListView;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.CheckBox taskTrayEnabledCheckBox;
		private System.Windows.Forms.CheckBox ballonTipEnabledCheckBox;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label clockLabel;
		private System.Windows.Forms.Timer clockTimer;

	}
}