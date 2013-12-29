namespace Raycaster
{
    partial class MainForm
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
            this.pbMain = new System.Windows.Forms.PictureBox();
            this.tmrTick = new System.Windows.Forms.Timer(this.components);
            this.pbMap = new System.Windows.Forms.PictureBox();
            this.radMovePlayer = new System.Windows.Forms.RadioButton();
            this.radDrawWalls = new System.Windows.Forms.RadioButton();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.chkTextures = new System.Windows.Forms.CheckBox();
            this.btnClearWalls = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMap)).BeginInit();
            this.SuspendLayout();
            // 
            // pbMain
            // 
            this.pbMain.BackColor = System.Drawing.Color.White;
            this.pbMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbMain.Location = new System.Drawing.Point(12, 12);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(320, 240);
            this.pbMain.TabIndex = 0;
            this.pbMain.TabStop = false;
            this.pbMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pbMain_Paint);
            // 
            // tmrTick
            // 
            this.tmrTick.Enabled = true;
            this.tmrTick.Tick += new System.EventHandler(this.tmrTick_Tick);
            // 
            // pbMap
            // 
            this.pbMap.BackColor = System.Drawing.Color.White;
            this.pbMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbMap.Location = new System.Drawing.Point(338, 12);
            this.pbMap.Name = "pbMap";
            this.pbMap.Size = new System.Drawing.Size(240, 240);
            this.pbMap.TabIndex = 0;
            this.pbMap.TabStop = false;
            this.pbMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pbMap_Paint);
            this.pbMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbMap_MouseDown);
            this.pbMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbMap_MouseMove);
            this.pbMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbMap_MouseUp);
            // 
            // radMovePlayer
            // 
            this.radMovePlayer.AutoSize = true;
            this.radMovePlayer.Checked = true;
            this.radMovePlayer.Location = new System.Drawing.Point(338, 259);
            this.radMovePlayer.Name = "radMovePlayer";
            this.radMovePlayer.Size = new System.Drawing.Size(87, 17);
            this.radMovePlayer.TabIndex = 1;
            this.radMovePlayer.TabStop = true;
            this.radMovePlayer.Text = "Move / Look";
            this.radMovePlayer.UseVisualStyleBackColor = true;
            // 
            // radDrawWalls
            // 
            this.radDrawWalls.AutoSize = true;
            this.radDrawWalls.Location = new System.Drawing.Point(483, 259);
            this.radDrawWalls.Name = "radDrawWalls";
            this.radDrawWalls.Size = new System.Drawing.Size(95, 17);
            this.radDrawWalls.TabIndex = 1;
            this.radDrawWalls.Text = "Build / Destroy";
            this.radDrawWalls.UseVisualStyleBackColor = true;
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(13, 259);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(319, 138);
            this.txtInfo.TabIndex = 2;
            // 
            // chkTextures
            // 
            this.chkTextures.AutoSize = true;
            this.chkTextures.Checked = true;
            this.chkTextures.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTextures.Location = new System.Drawing.Point(338, 282);
            this.chkTextures.Name = "chkTextures";
            this.chkTextures.Size = new System.Drawing.Size(58, 17);
            this.chkTextures.TabIndex = 3;
            this.chkTextures.Text = "Bricks!";
            this.chkTextures.UseVisualStyleBackColor = true;
            // 
            // btnClearWalls
            // 
            this.btnClearWalls.Location = new System.Drawing.Point(501, 282);
            this.btnClearWalls.Name = "btnClearWalls";
            this.btnClearWalls.Size = new System.Drawing.Size(75, 23);
            this.btnClearWalls.TabIndex = 4;
            this.btnClearWalls.Text = "Nuke";
            this.btnClearWalls.UseVisualStyleBackColor = true;
            this.btnClearWalls.Click += new System.EventHandler(this.btnClearWalls_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 409);
            this.Controls.Add(this.btnClearWalls);
            this.Controls.Add(this.chkTextures);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.radDrawWalls);
            this.Controls.Add(this.radMovePlayer);
            this.Controls.Add(this.pbMap);
            this.Controls.Add(this.pbMain);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbMain;
        private System.Windows.Forms.Timer tmrTick;
        private System.Windows.Forms.PictureBox pbMap;
        private System.Windows.Forms.RadioButton radMovePlayer;
        private System.Windows.Forms.RadioButton radDrawWalls;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.CheckBox chkTextures;
        private System.Windows.Forms.Button btnClearWalls;
    }
}

