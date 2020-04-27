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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.current_connections = new System.Windows.Forms.ListBox();
            this.add_device = new System.Windows.Forms.Button();
            this.delete_device = new System.Windows.Forms.Button();
            this.white_list = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.deny_connections = new System.Windows.Forms.Button();
            this.allow_connections = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(964, 24);
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
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(94, 39);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 29);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "e-mail:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(308, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "список подключенных устройств";
            // 
            // current_connections
            // 
            this.current_connections.FormattingEnabled = true;
            this.current_connections.ItemHeight = 24;
            this.current_connections.Location = new System.Drawing.Point(21, 144);
            this.current_connections.Name = "current_connections";
            this.current_connections.Size = new System.Drawing.Size(559, 76);
            this.current_connections.TabIndex = 4;
            this.current_connections.SelectedIndexChanged += new System.EventHandler(this.current_connections_SelectedIndexChanged);
            // 
            // add_device
            // 
            this.add_device.Location = new System.Drawing.Point(21, 366);
            this.add_device.Name = "add_device";
            this.add_device.Size = new System.Drawing.Size(135, 62);
            this.add_device.TabIndex = 5;
            this.add_device.Text = "Добавить устройство";
            this.add_device.UseVisualStyleBackColor = true;
            this.add_device.Click += new System.EventHandler(this.add_device_Click);
            // 
            // delete_device
            // 
            this.delete_device.Location = new System.Drawing.Point(190, 366);
            this.delete_device.Name = "delete_device";
            this.delete_device.Size = new System.Drawing.Size(135, 62);
            this.delete_device.TabIndex = 6;
            this.delete_device.Text = "Удалить устройство";
            this.delete_device.UseVisualStyleBackColor = true;
            // 
            // white_list
            // 
            this.white_list.FormattingEnabled = true;
            this.white_list.ItemHeight = 24;
            this.white_list.Location = new System.Drawing.Point(21, 269);
            this.white_list.Name = "white_list";
            this.white_list.Size = new System.Drawing.Size(559, 76);
            this.white_list.TabIndex = 7;
            this.white_list.SelectedIndexChanged += new System.EventHandler(this.white_list_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(287, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "список доверенных устройств";
            // 
            // deny_connections
            // 
            this.deny_connections.Location = new System.Drawing.Point(614, 144);
            this.deny_connections.Name = "deny_connections";
            this.deny_connections.Size = new System.Drawing.Size(156, 56);
            this.deny_connections.TabIndex = 9;
            this.deny_connections.Text = "Запретить подключения";
            this.deny_connections.UseVisualStyleBackColor = true;
            this.deny_connections.Click += new System.EventHandler(this.deny_connections_Click);
            // 
            // allow_connections
            // 
            this.allow_connections.Location = new System.Drawing.Point(614, 269);
            this.allow_connections.Name = "allow_connections";
            this.allow_connections.Size = new System.Drawing.Size(156, 60);
            this.allow_connections.TabIndex = 10;
            this.allow_connections.Text = "Разрешить подключения";
            this.allow_connections.UseVisualStyleBackColor = true;
            this.allow_connections.Click += new System.EventHandler(this.allow_connections_Click);
            // 
            // main_window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 461);
            this.Controls.Add(this.allow_connections);
            this.Controls.Add(this.deny_connections);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.white_list);
            this.Controls.Add(this.delete_device);
            this.Controls.Add(this.add_device);
            this.Controls.Add(this.current_connections);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "main_window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "USB Control";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox current_connections;
        private System.Windows.Forms.Button add_device;
        private System.Windows.Forms.Button delete_device;
        private System.Windows.Forms.ListBox white_list;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button deny_connections;
        private System.Windows.Forms.Button allow_connections;
    }
}

