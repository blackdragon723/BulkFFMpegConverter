namespace CleanPhone
{
    partial class Form1
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
            this.startButton = new System.Windows.Forms.Button();
            this.inputBrowse = new System.Windows.Forms.Button();
            this.inputFolderTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.recursiveCheckbox = new System.Windows.Forms.CheckBox();
            this.qualityComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.outputFolderTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.outputBrowse = new System.Windows.Forms.Button();
            this.haveOutputFolderCheckbox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.overwriteCheckbox = new System.Windows.Forms.CheckBox();
            this.removeWhenCompleteCheckbox = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFolderWhenCompleteCheckbox = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(125, 135);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // inputBrowse
            // 
            this.inputBrowse.Location = new System.Drawing.Point(328, 4);
            this.inputBrowse.Name = "inputBrowse";
            this.inputBrowse.Size = new System.Drawing.Size(75, 23);
            this.inputBrowse.TabIndex = 1;
            this.inputBrowse.Text = "Browse...";
            this.inputBrowse.UseVisualStyleBackColor = true;
            this.inputBrowse.Click += new System.EventHandler(this.button2_Click);
            // 
            // inputFolderTextBox
            // 
            this.inputFolderTextBox.Location = new System.Drawing.Point(92, 6);
            this.inputFolderTextBox.Name = "inputFolderTextBox";
            this.inputFolderTextBox.Size = new System.Drawing.Size(230, 20);
            this.inputFolderTextBox.TabIndex = 2;
            this.inputFolderTextBox.TextChanged += new System.EventHandler(this.inputFolderTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Input Folder:";
            // 
            // recursiveCheckbox
            // 
            this.recursiveCheckbox.AutoSize = true;
            this.recursiveCheckbox.Checked = true;
            this.recursiveCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.recursiveCheckbox.Location = new System.Drawing.Point(15, 59);
            this.recursiveCheckbox.Name = "recursiveCheckbox";
            this.recursiveCheckbox.Size = new System.Drawing.Size(74, 17);
            this.recursiveCheckbox.TabIndex = 4;
            this.recursiveCheckbox.Text = "Recursive";
            this.recursiveCheckbox.UseVisualStyleBackColor = true;
            this.recursiveCheckbox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // qualityComboBox
            // 
            this.qualityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.qualityComboBox.FormattingEnabled = true;
            this.qualityComboBox.Location = new System.Drawing.Point(60, 80);
            this.qualityComboBox.Name = "qualityComboBox";
            this.qualityComboBox.Size = new System.Drawing.Size(127, 21);
            this.qualityComboBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Quality:";
            // 
            // outputFolderTextBox
            // 
            this.outputFolderTextBox.Location = new System.Drawing.Point(92, 33);
            this.outputFolderTextBox.Name = "outputFolderTextBox";
            this.outputFolderTextBox.Size = new System.Drawing.Size(230, 20);
            this.outputFolderTextBox.TabIndex = 7;
            this.outputFolderTextBox.TextChanged += new System.EventHandler(this.outputFolderTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Output Folder:";
            // 
            // outputBrowse
            // 
            this.outputBrowse.Location = new System.Drawing.Point(328, 31);
            this.outputBrowse.Name = "outputBrowse";
            this.outputBrowse.Size = new System.Drawing.Size(75, 23);
            this.outputBrowse.TabIndex = 9;
            this.outputBrowse.Text = "Browse...";
            this.outputBrowse.UseVisualStyleBackColor = true;
            this.outputBrowse.Click += new System.EventHandler(this.outputBrowse_Click);
            // 
            // haveOutputFolderCheckbox
            // 
            this.haveOutputFolderCheckbox.AutoSize = true;
            this.haveOutputFolderCheckbox.Location = new System.Drawing.Point(92, 59);
            this.haveOutputFolderCheckbox.Name = "haveOutputFolderCheckbox";
            this.haveOutputFolderCheckbox.Size = new System.Drawing.Size(136, 17);
            this.haveOutputFolderCheckbox.TabIndex = 10;
            this.haveOutputFolderCheckbox.Text = "Separate Output Folder";
            this.haveOutputFolderCheckbox.UseVisualStyleBackColor = true;
            this.haveOutputFolderCheckbox.CheckedChanged += new System.EventHandler(this.haveOutputFolderCheckbox_CheckedChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Enabled = false;
            this.cancelButton.Location = new System.Drawing.Point(205, 135);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 23);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 165);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(391, 23);
            this.progressBar1.TabIndex = 12;
            // 
            // overwriteCheckbox
            // 
            this.overwriteCheckbox.AutoSize = true;
            this.overwriteCheckbox.Checked = true;
            this.overwriteCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.overwriteCheckbox.Location = new System.Drawing.Point(234, 59);
            this.overwriteCheckbox.Name = "overwriteCheckbox";
            this.overwriteCheckbox.Size = new System.Drawing.Size(109, 17);
            this.overwriteCheckbox.TabIndex = 13;
            this.overwriteCheckbox.Text = "Overwrite if Exists";
            this.overwriteCheckbox.UseVisualStyleBackColor = true;
            this.overwriteCheckbox.CheckedChanged += new System.EventHandler(this.overwriteCheckbox_CheckedChanged);
            // 
            // removeWhenCompleteCheckbox
            // 
            this.removeWhenCompleteCheckbox.AutoSize = true;
            this.removeWhenCompleteCheckbox.Checked = true;
            this.removeWhenCompleteCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.removeWhenCompleteCheckbox.Location = new System.Drawing.Point(193, 82);
            this.removeWhenCompleteCheckbox.Name = "removeWhenCompleteCheckbox";
            this.removeWhenCompleteCheckbox.Size = new System.Drawing.Size(145, 17);
            this.removeWhenCompleteCheckbox.TabIndex = 14;
            this.removeWhenCompleteCheckbox.Text = "Remove When Complete";
            this.removeWhenCompleteCheckbox.UseVisualStyleBackColor = true;
            this.removeWhenCompleteCheckbox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 193);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(413, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 17);
            // 
            // openFolderWhenCompleteCheckbox
            // 
            this.openFolderWhenCompleteCheckbox.AutoSize = true;
            this.openFolderWhenCompleteCheckbox.Location = new System.Drawing.Point(15, 109);
            this.openFolderWhenCompleteCheckbox.Name = "openFolderWhenCompleteCheckbox";
            this.openFolderWhenCompleteCheckbox.Size = new System.Drawing.Size(163, 17);
            this.openFolderWhenCompleteCheckbox.TabIndex = 16;
            this.openFolderWhenCompleteCheckbox.Text = "Open Folder When Complete";
            this.openFolderWhenCompleteCheckbox.UseVisualStyleBackColor = true;
            this.openFolderWhenCompleteCheckbox.CheckedChanged += new System.EventHandler(this.openFolderWhenCompleteCheckbox_CheckedChanged);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 215);
            this.Controls.Add(this.openFolderWhenCompleteCheckbox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.removeWhenCompleteCheckbox);
            this.Controls.Add(this.overwriteCheckbox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.haveOutputFolderCheckbox);
            this.Controls.Add(this.outputBrowse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.outputFolderTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.qualityComboBox);
            this.Controls.Add(this.recursiveCheckbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputFolderTextBox);
            this.Controls.Add(this.inputBrowse);
            this.Controls.Add(this.startButton);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Flac To MP3 Converter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button inputBrowse;
        private System.Windows.Forms.TextBox inputFolderTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox recursiveCheckbox;
        private System.Windows.Forms.ComboBox qualityComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox outputFolderTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button outputBrowse;
        private System.Windows.Forms.CheckBox haveOutputFolderCheckbox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox overwriteCheckbox;
        private System.Windows.Forms.CheckBox removeWhenCompleteCheckbox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.CheckBox openFolderWhenCompleteCheckbox;
    }
}

