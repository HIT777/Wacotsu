namespace WacotsuTest
{
	partial class Form1
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.liveIdTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.reserveButton = new System.Windows.Forms.Button();
			this.userSessionLabel = new System.Windows.Forms.Label();
			this.logTextBox = new System.Windows.Forms.TextBox();
			this.liveInfoListBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// liveIdTextBox
			// 
			this.liveIdTextBox.Location = new System.Drawing.Point(130, 72);
			this.liveIdTextBox.Name = "liveIdTextBox";
			this.liveIdTextBox.Size = new System.Drawing.Size(244, 19);
			this.liveIdTextBox.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(84, 78);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "liveId";
			// 
			// reserveButton
			// 
			this.reserveButton.Location = new System.Drawing.Point(12, 103);
			this.reserveButton.Name = "reserveButton";
			this.reserveButton.Size = new System.Drawing.Size(362, 23);
			this.reserveButton.TabIndex = 2;
			this.reserveButton.Text = "Reserve";
			this.reserveButton.UseVisualStyleBackColor = true;
			this.reserveButton.Click += new System.EventHandler(this.reserveButton_Click);
			// 
			// userSessionLabel
			// 
			this.userSessionLabel.AutoSize = true;
			this.userSessionLabel.Location = new System.Drawing.Point(36, 24);
			this.userSessionLabel.Name = "userSessionLabel";
			this.userSessionLabel.Size = new System.Drawing.Size(76, 12);
			this.userSessionLabel.TabIndex = 3;
			this.userSessionLabel.Text = "user_session=";
			// 
			// logTextBox
			// 
			this.logTextBox.Location = new System.Drawing.Point(12, 142);
			this.logTextBox.Multiline = true;
			this.logTextBox.Name = "logTextBox";
			this.logTextBox.Size = new System.Drawing.Size(362, 107);
			this.logTextBox.TabIndex = 4;
			// 
			// liveInfoListBox
			// 
			this.liveInfoListBox.FormattingEnabled = true;
			this.liveInfoListBox.ItemHeight = 12;
			this.liveInfoListBox.Location = new System.Drawing.Point(12, 255);
			this.liveInfoListBox.Name = "liveInfoListBox";
			this.liveInfoListBox.Size = new System.Drawing.Size(362, 244);
			this.liveInfoListBox.TabIndex = 5;
			this.liveInfoListBox.SelectedIndexChanged += new System.EventHandler(this.liveInfoListBox_SelectedIndexChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(386, 512);
			this.Controls.Add(this.liveInfoListBox);
			this.Controls.Add(this.logTextBox);
			this.Controls.Add(this.userSessionLabel);
			this.Controls.Add(this.reserveButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.liveIdTextBox);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox liveIdTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button reserveButton;
		private System.Windows.Forms.Label userSessionLabel;
		private System.Windows.Forms.TextBox logTextBox;
		private System.Windows.Forms.ListBox liveInfoListBox;
	}
}

