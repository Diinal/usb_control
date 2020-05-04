namespace USB_Control
{
    partial class main_window
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main_window));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.current_connections = new System.Windows.Forms.ListBox();
            this.add_device = new System.Windows.Forms.Button();
            this.delete_device = new System.Windows.Forms.Button();
            this.white_list = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SourceField = new System.Windows.Forms.ListBox();
            this.labelSource = new System.Windows.Forms.Label();
            this.openFileButtonSource = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.encryptButton = new System.Windows.Forms.Button();
            this.DestinationField = new System.Windows.Forms.ListBox();
            this.destinationLabel = new System.Windows.Forms.Label();
            this.DecryptButton = new System.Windows.Forms.Button();
            this.openFolderButtonDestination = new System.Windows.Forms.Button();
            this.KeyTextBox = new System.Windows.Forms.TextBox();
            this.KeyLabel = new System.Windows.Forms.Label();
            this.buttonClearSource = new System.Windows.Forms.Button();
            this.buttonClearDestinations = new System.Windows.Forms.Button();
            this.radioButtonAllow = new System.Windows.Forms.RadioButton();
            this.radioButtonDeny = new System.Windows.Forms.RadioButton();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1069, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(254, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Подключенные устройства";
            // 
            // current_connections
            // 
            this.current_connections.FormattingEnabled = true;
            this.current_connections.ItemHeight = 24;
            this.current_connections.Location = new System.Drawing.Point(12, 73);
            this.current_connections.Name = "current_connections";
            this.current_connections.Size = new System.Drawing.Size(758, 76);
            this.current_connections.TabIndex = 4;
            this.current_connections.SelectedIndexChanged += new System.EventHandler(this.current_connections_SelectedIndexChanged);
            // 
            // add_device
            // 
            this.add_device.Location = new System.Drawing.Point(282, 162);
            this.add_device.Name = "add_device";
            this.add_device.Size = new System.Drawing.Size(233, 31);
            this.add_device.TabIndex = 5;
            this.add_device.Text = "Добавить устройство";
            this.add_device.UseVisualStyleBackColor = true;
            this.add_device.Click += new System.EventHandler(this.add_device_Click);
            // 
            // delete_device
            // 
            this.delete_device.Location = new System.Drawing.Point(543, 162);
            this.delete_device.Name = "delete_device";
            this.delete_device.Size = new System.Drawing.Size(227, 31);
            this.delete_device.TabIndex = 6;
            this.delete_device.Text = "Удалить устройство";
            this.delete_device.UseVisualStyleBackColor = true;
            this.delete_device.Click += new System.EventHandler(this.delete_device_Click);
            // 
            // white_list
            // 
            this.white_list.FormattingEnabled = true;
            this.white_list.ItemHeight = 24;
            this.white_list.Location = new System.Drawing.Point(12, 206);
            this.white_list.Name = "white_list";
            this.white_list.Size = new System.Drawing.Size(758, 76);
            this.white_list.TabIndex = 7;
            this.white_list.SelectedIndexChanged += new System.EventHandler(this.white_list_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Белый список";
            // 
            // SourceField
            // 
            this.SourceField.AllowDrop = true;
            this.SourceField.FormattingEnabled = true;
            this.SourceField.ItemHeight = 24;
            this.SourceField.Location = new System.Drawing.Point(12, 328);
            this.SourceField.Name = "SourceField";
            this.SourceField.Size = new System.Drawing.Size(758, 100);
            this.SourceField.TabIndex = 11;
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new System.Drawing.Point(8, 292);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(368, 24);
            this.labelSource.TabIndex = 12;
            this.labelSource.Text = "Файлы для шифрования/дешифрования";
            // 
            // openFileButtonSource
            // 
            this.openFileButtonSource.Location = new System.Drawing.Point(785, 328);
            this.openFileButtonSource.Name = "openFileButtonSource";
            this.openFileButtonSource.Size = new System.Drawing.Size(161, 36);
            this.openFileButtonSource.TabIndex = 13;
            this.openFileButtonSource.Text = "Выбрать файл";
            this.openFileButtonSource.UseVisualStyleBackColor = true;
            this.openFileButtonSource.Click += new System.EventHandler(this.openFileButtonSource_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Multiselect = true;
            // 
            // encryptButton
            // 
            this.encryptButton.Location = new System.Drawing.Point(310, 588);
            this.encryptButton.Name = "encryptButton";
            this.encryptButton.Size = new System.Drawing.Size(161, 36);
            this.encryptButton.TabIndex = 15;
            this.encryptButton.Text = "Зашифровать";
            this.encryptButton.UseVisualStyleBackColor = true;
            this.encryptButton.Click += new System.EventHandler(this.encryptButton_Click);
            // 
            // DestinationField
            // 
            this.DestinationField.FormattingEnabled = true;
            this.DestinationField.ItemHeight = 24;
            this.DestinationField.Location = new System.Drawing.Point(12, 481);
            this.DestinationField.Name = "DestinationField";
            this.DestinationField.Size = new System.Drawing.Size(758, 100);
            this.DestinationField.TabIndex = 16;
            // 
            // destinationLabel
            // 
            this.destinationLabel.AutoSize = true;
            this.destinationLabel.Location = new System.Drawing.Point(12, 445);
            this.destinationLabel.Name = "destinationLabel";
            this.destinationLabel.Size = new System.Drawing.Size(459, 24);
            this.destinationLabel.TabIndex = 17;
            this.destinationLabel.Text = "Папка для шифрованных/дешифрованных файлов";
            // 
            // DecryptButton
            // 
            this.DecryptButton.Location = new System.Drawing.Point(496, 588);
            this.DecryptButton.Name = "DecryptButton";
            this.DecryptButton.Size = new System.Drawing.Size(161, 36);
            this.DecryptButton.TabIndex = 18;
            this.DecryptButton.Text = "Дешифровать";
            this.DecryptButton.UseVisualStyleBackColor = true;
            this.DecryptButton.Click += new System.EventHandler(this.DecryptButton_Click);
            // 
            // openFolderButtonDestination
            // 
            this.openFolderButtonDestination.Location = new System.Drawing.Point(785, 481);
            this.openFolderButtonDestination.Name = "openFolderButtonDestination";
            this.openFolderButtonDestination.Size = new System.Drawing.Size(161, 36);
            this.openFolderButtonDestination.TabIndex = 20;
            this.openFolderButtonDestination.Text = "Выбрать папку";
            this.openFolderButtonDestination.UseVisualStyleBackColor = true;
            this.openFolderButtonDestination.Click += new System.EventHandler(this.openFolderButtonDestination_Click);
            // 
            // KeyTextBox
            // 
            this.KeyTextBox.Location = new System.Drawing.Point(88, 591);
            this.KeyTextBox.Name = "KeyTextBox";
            this.KeyTextBox.Size = new System.Drawing.Size(188, 29);
            this.KeyTextBox.TabIndex = 21;
            this.KeyTextBox.Enter += new System.EventHandler(this.KeyTextBox_Enter);
            // 
            // KeyLabel
            // 
            this.KeyLabel.AutoSize = true;
            this.KeyLabel.Location = new System.Drawing.Point(12, 594);
            this.KeyLabel.Name = "KeyLabel";
            this.KeyLabel.Size = new System.Drawing.Size(61, 24);
            this.KeyLabel.TabIndex = 22;
            this.KeyLabel.Text = "Ключ:";
            // 
            // buttonClearSource
            // 
            this.buttonClearSource.Location = new System.Drawing.Point(785, 392);
            this.buttonClearSource.Name = "buttonClearSource";
            this.buttonClearSource.Size = new System.Drawing.Size(161, 36);
            this.buttonClearSource.TabIndex = 23;
            this.buttonClearSource.Text = "Очистить";
            this.buttonClearSource.UseVisualStyleBackColor = true;
            this.buttonClearSource.Click += new System.EventHandler(this.buttonClearSource_Click);
            // 
            // buttonClearDestinations
            // 
            this.buttonClearDestinations.Location = new System.Drawing.Point(785, 545);
            this.buttonClearDestinations.Name = "buttonClearDestinations";
            this.buttonClearDestinations.Size = new System.Drawing.Size(161, 36);
            this.buttonClearDestinations.TabIndex = 24;
            this.buttonClearDestinations.Text = "Очистить";
            this.buttonClearDestinations.UseVisualStyleBackColor = true;
            this.buttonClearDestinations.Click += new System.EventHandler(this.buttonClearDestinations_Click);
            // 
            // radioButtonAllow
            // 
            this.radioButtonAllow.AutoSize = true;
            this.radioButtonAllow.Location = new System.Drawing.Point(785, 73);
            this.radioButtonAllow.Name = "radioButtonAllow";
            this.radioButtonAllow.Size = new System.Drawing.Size(253, 28);
            this.radioButtonAllow.TabIndex = 25;
            this.radioButtonAllow.TabStop = true;
            this.radioButtonAllow.Text = "Разрешить подключения";
            this.radioButtonAllow.UseVisualStyleBackColor = true;
            this.radioButtonAllow.CheckedChanged += new System.EventHandler(this.radioButtonConnections_CheckedChanged);
            // 
            // radioButtonDeny
            // 
            this.radioButtonDeny.AutoSize = true;
            this.radioButtonDeny.Location = new System.Drawing.Point(785, 107);
            this.radioButtonDeny.Name = "radioButtonDeny";
            this.radioButtonDeny.Size = new System.Drawing.Size(251, 28);
            this.radioButtonDeny.TabIndex = 26;
            this.radioButtonDeny.TabStop = true;
            this.radioButtonDeny.Text = "Запретить подключения";
            this.radioButtonDeny.UseVisualStyleBackColor = true;
            this.radioButtonDeny.CheckedChanged += new System.EventHandler(this.radioButtonConnections_CheckedChanged);
            // 
            // main_window
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 635);
            this.Controls.Add(this.radioButtonDeny);
            this.Controls.Add(this.radioButtonAllow);
            this.Controls.Add(this.buttonClearDestinations);
            this.Controls.Add(this.buttonClearSource);
            this.Controls.Add(this.KeyLabel);
            this.Controls.Add(this.KeyTextBox);
            this.Controls.Add(this.openFolderButtonDestination);
            this.Controls.Add(this.DecryptButton);
            this.Controls.Add(this.destinationLabel);
            this.Controls.Add(this.DestinationField);
            this.Controls.Add(this.encryptButton);
            this.Controls.Add(this.openFileButtonSource);
            this.Controls.Add(this.labelSource);
            this.Controls.Add(this.SourceField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.white_list);
            this.Controls.Add(this.delete_device);
            this.Controls.Add(this.add_device);
            this.Controls.Add(this.current_connections);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "main_window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "USB Control";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.main_window_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox current_connections;
        private System.Windows.Forms.Button add_device;
        private System.Windows.Forms.Button delete_device;
        private System.Windows.Forms.ListBox white_list;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox SourceField;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Button openFileButtonSource;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button encryptButton;
        private System.Windows.Forms.ListBox DestinationField;
        private System.Windows.Forms.Label destinationLabel;
        private System.Windows.Forms.Button DecryptButton;
        private System.Windows.Forms.Button openFolderButtonDestination;
        private System.Windows.Forms.TextBox KeyTextBox;
        private System.Windows.Forms.Label KeyLabel;
        private System.Windows.Forms.Button buttonClearSource;
        private System.Windows.Forms.Button buttonClearDestinations;
        private System.Windows.Forms.RadioButton radioButtonAllow;
        private System.Windows.Forms.RadioButton radioButtonDeny;
    }
}

