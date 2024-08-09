namespace GallerySwiper
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox = new PictureBox();
            buttonOpenFolder = new Button();
            listboxCategories = new ListBox();
            textboxCategory = new TextBox();
            textboxShortcut = new TextBox();
            labelNewCategory = new Label();
            labelShortcut = new Label();
            buttonSubmit = new Button();
            buttonRemove = new Button();
            buttonProcess = new Button();
            labelCurrentFolder = new Label();
            buttonHelp = new Button();
            galleryFolderDialog = new FolderBrowserDialog();
            textboxLog = new RichTextBox();
            outputFolderDialog = new FolderBrowserDialog();
            checkboxShouldMove = new CheckBox();
            checkboxSpecialCats = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox.BackColor = SystemColors.ButtonShadow;
            pictureBox.Location = new Point(159, 12);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(679, 426);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.Click += ResetFocus;
            // 
            // buttonOpenFolder
            // 
            buttonOpenFolder.Location = new Point(12, 12);
            buttonOpenFolder.Name = "buttonOpenFolder";
            buttonOpenFolder.Size = new Size(141, 23);
            buttonOpenFolder.TabIndex = 1;
            buttonOpenFolder.Text = "Open Folder...";
            buttonOpenFolder.UseVisualStyleBackColor = true;
            buttonOpenFolder.Click += buttonOpenFolder_Click;
            buttonOpenFolder.KeyDown += Any_KeyDown;
            // 
            // listboxCategories
            // 
            listboxCategories.FormattingEnabled = true;
            listboxCategories.ItemHeight = 15;
            listboxCategories.Items.AddRange(new object[] { "Delete\tRight", "Keep\tLeft" });
            listboxCategories.Location = new Point(12, 80);
            listboxCategories.Name = "listboxCategories";
            listboxCategories.Size = new Size(141, 94);
            listboxCategories.TabIndex = 2;
            listboxCategories.SelectedIndexChanged += listboxCategories_SelectedIndexChanged;
            listboxCategories.KeyDown += Any_KeyDown;
            // 
            // textboxCategory
            // 
            textboxCategory.Location = new Point(12, 195);
            textboxCategory.Name = "textboxCategory";
            textboxCategory.Size = new Size(141, 23);
            textboxCategory.TabIndex = 3;
            // 
            // textboxShortcut
            // 
            textboxShortcut.Location = new Point(12, 239);
            textboxShortcut.Name = "textboxShortcut";
            textboxShortcut.ReadOnly = true;
            textboxShortcut.Size = new Size(141, 23);
            textboxShortcut.TabIndex = 4;
            textboxShortcut.KeyDown += Shortcut_KeyDown;
            // 
            // labelNewCategory
            // 
            labelNewCategory.AutoSize = true;
            labelNewCategory.Location = new Point(12, 177);
            labelNewCategory.Name = "labelNewCategory";
            labelNewCategory.Size = new Size(90, 15);
            labelNewCategory.TabIndex = 5;
            labelNewCategory.Text = "Category Name";
            // 
            // labelShortcut
            // 
            labelShortcut.AutoSize = true;
            labelShortcut.Location = new Point(12, 221);
            labelShortcut.Name = "labelShortcut";
            labelShortcut.Size = new Size(52, 15);
            labelShortcut.TabIndex = 6;
            labelShortcut.Text = "Shortcut";
            // 
            // buttonSubmit
            // 
            buttonSubmit.Location = new Point(12, 268);
            buttonSubmit.Name = "buttonSubmit";
            buttonSubmit.Size = new Size(62, 23);
            buttonSubmit.TabIndex = 5;
            buttonSubmit.Text = "Add";
            buttonSubmit.UseVisualStyleBackColor = true;
            buttonSubmit.Click += buttonSubmit_Click;
            buttonSubmit.KeyDown += Any_KeyDown;
            // 
            // buttonRemove
            // 
            buttonRemove.Enabled = false;
            buttonRemove.Location = new Point(80, 268);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new Size(73, 23);
            buttonRemove.TabIndex = 6;
            buttonRemove.Text = "Remove";
            buttonRemove.UseVisualStyleBackColor = true;
            buttonRemove.Click += buttonRemove_Click;
            buttonRemove.KeyDown += Any_KeyDown;
            // 
            // buttonProcess
            // 
            buttonProcess.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonProcess.Location = new Point(12, 415);
            buttonProcess.Name = "buttonProcess";
            buttonProcess.Size = new Size(141, 23);
            buttonProcess.TabIndex = 8;
            buttonProcess.Text = "Process";
            buttonProcess.UseVisualStyleBackColor = true;
            buttonProcess.Click += buttonProcess_Click;
            buttonProcess.KeyDown += Any_KeyDown;
            // 
            // labelCurrentFolder
            // 
            labelCurrentFolder.AutoEllipsis = true;
            labelCurrentFolder.AutoSize = true;
            labelCurrentFolder.Location = new Point(12, 38);
            labelCurrentFolder.MaximumSize = new Size(141, 30);
            labelCurrentFolder.Name = "labelCurrentFolder";
            labelCurrentFolder.Size = new Size(86, 30);
            labelCurrentFolder.TabIndex = 10;
            labelCurrentFolder.Text = "Current Folder:\r\nNone";
            // 
            // buttonHelp
            // 
            buttonHelp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonHelp.Location = new Point(12, 386);
            buttonHelp.Name = "buttonHelp";
            buttonHelp.Size = new Size(141, 23);
            buttonHelp.TabIndex = 7;
            buttonHelp.Text = "Help";
            buttonHelp.UseVisualStyleBackColor = true;
            buttonHelp.Click += buttonHelp_Click;
            buttonHelp.KeyDown += Any_KeyDown;
            // 
            // galleryFolderDialog
            // 
            galleryFolderDialog.AddToRecent = false;
            galleryFolderDialog.Description = "Select Gallery Location";
            galleryFolderDialog.OkRequiresInteraction = true;
            galleryFolderDialog.RootFolder = Environment.SpecialFolder.CommonPictures;
            galleryFolderDialog.UseDescriptionForTitle = true;
            // 
            // textboxLog
            // 
            textboxLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            textboxLog.Location = new Point(844, 12);
            textboxLog.Name = "textboxLog";
            textboxLog.ReadOnly = true;
            textboxLog.Size = new Size(239, 426);
            textboxLog.TabIndex = 11;
            textboxLog.Text = "Progress will be shown here!";
            textboxLog.KeyDown += Any_KeyDown;
            // 
            // outputFolderDialog
            // 
            outputFolderDialog.AddToRecent = false;
            outputFolderDialog.Description = "Select Output Folder";
            outputFolderDialog.OkRequiresInteraction = true;
            outputFolderDialog.RootFolder = Environment.SpecialFolder.MyPictures;
            // 
            // checkboxShouldMove
            // 
            checkboxShouldMove.AutoSize = true;
            checkboxShouldMove.Location = new Point(12, 297);
            checkboxShouldMove.Name = "checkboxShouldMove";
            checkboxShouldMove.Size = new Size(82, 19);
            checkboxShouldMove.TabIndex = 12;
            checkboxShouldMove.Text = "Move Files";
            checkboxShouldMove.UseVisualStyleBackColor = true;
            checkboxShouldMove.CheckedChanged += checkboxShouldMove_CheckedChanged;
            // 
            // checkboxSpecialCats
            // 
            checkboxSpecialCats.AutoSize = true;
            checkboxSpecialCats.Enabled = false;
            checkboxSpecialCats.Location = new Point(12, 322);
            checkboxSpecialCats.Name = "checkboxSpecialCats";
            checkboxSpecialCats.Size = new Size(122, 19);
            checkboxSpecialCats.TabIndex = 13;
            checkboxSpecialCats.Text = "Special Categories";
            checkboxSpecialCats.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1095, 450);
            Controls.Add(checkboxSpecialCats);
            Controls.Add(checkboxShouldMove);
            Controls.Add(textboxLog);
            Controls.Add(buttonHelp);
            Controls.Add(labelCurrentFolder);
            Controls.Add(buttonProcess);
            Controls.Add(buttonRemove);
            Controls.Add(buttonSubmit);
            Controls.Add(labelShortcut);
            Controls.Add(labelNewCategory);
            Controls.Add(textboxShortcut);
            Controls.Add(textboxCategory);
            Controls.Add(listboxCategories);
            Controls.Add(buttonOpenFolder);
            Controls.Add(pictureBox);
            MinimumSize = new Size(700, 450);
            Name = "MainForm";
            Text = "GallerySwiper";
            Click += ResetFocus;
            KeyDown += Any_KeyDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox;
        private Button buttonOpenFolder;
        private ListBox listboxCategories;
        private TextBox textboxCategory;
        private TextBox textboxShortcut;
        private Label labelNewCategory;
        private Label labelShortcut;
        private Button buttonSubmit;
        private Button buttonRemove;
        private Button buttonProcess;
        private Label labelCurrentFolder;
        private Button buttonHelp;
        private FolderBrowserDialog galleryFolderDialog;
        private RichTextBox textboxLog;
        private FolderBrowserDialog outputFolderDialog;
        private CheckBox checkboxShouldMove;
        private CheckBox checkboxSpecialCats;
    }
}
