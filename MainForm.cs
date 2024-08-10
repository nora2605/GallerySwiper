using System.Collections.ObjectModel;

namespace GallerySwiper
{
    public partial class MainForm : Form
    {
        string helpText = @"Welcome to Gallery Swiper!

In this app, you can press single keys to sort photos! Just add your categories with corresponding shortcuts and tap the keys while the images show up on the right!
You can always go back with the reserved Shortcut Ctrl+Z and nothing gets moved before you click ""Process"" at the bottom!
To get started open a folder and either use the defaults or edit/remove them!

You cannot remove a category in the middle of the process (yet) if it contains items so be careful!

The special categories checkbox enables actions to be performed automatically after! (They are ""Delete"" and ""Ignore"")

Thanks for using Gallery Swiper
- Nora J.F.";

        // Category Name, Shortcut
        ObservableCollection<(string, Keys)> categories = [];
        // From, To
        List<(string, string)> sorted = [];
        // index via sorted.Count
        string[] filesToProcess = [];

        // no idea if the picturebox actually supports them
        IEnumerable<string> allowedFileExtensions = [
            ".png",
            ".jpg",
            ".jpeg",
            ".jfif",
            ".gif",
            ".bmp"
        ];

        bool noWarning = true;

        Keys currentShortcut = Keys.None;

        public MainForm()
        {
            InitializeComponent();

            categories.CollectionChanged += UpdateListBox;
            // Default
            categories.Add(("Keep", Keys.Right));
            categories.Add(("Delete", Keys.Left));
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(helpText, "Help", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }

        private void Any_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z && e.Control)
            {
                if (sorted.Count > 0)
                {
                    sorted.RemoveAt(sorted.Count - 1);
                }
            }
            else if (filesToProcess.Length > 0)
            {
                string cat = categories.FirstOrDefault((a) => a.Item2 == (e.Modifiers | e.KeyCode)).Item1;
                if (string.IsNullOrEmpty(cat)) return;
                sorted.Add((filesToProcess[sorted.Count], cat));
            }

            LoadCurrentImage();
            if (sorted.Count == filesToProcess.Length) MessageBox.Show("You're done! You can now press the \"Process\" Button or Undo with Ctrl+Z");
            e.SuppressKeyPress = true;
        }

        private void UpdateListBox(object? sender, EventArgs e)
        {
            listboxCategories.Items.Clear();
            // Needs to match indexes
            for (int i = 0; i < categories.Count; i++)
            {
                var category = categories[i];
                listboxCategories.Items.Add($"{category.Item1}\t{category.Item2.ToString()}");
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (textboxCategory.Text == "") return;
            if (textboxShortcut.Text == "" || currentShortcut == Keys.None) return;

            bool sel = listboxCategories.SelectedIndex != -1;

            textboxCategory.Text = textboxCategory.Text.Trim();

            if (Path.GetInvalidFileNameChars().Any(textboxCategory.Text.Contains) || textboxCategory.Text.ToLower() == "con")
            {
                MessageBox.Show("The Category name must be a valid directory name", "Invalid Category Name");
                return;
            }
            if (categories.Any(a => !(sel && a.Equals(categories[listboxCategories.SelectedIndex])) && a.Item2 == currentShortcut)
            )
            {
                MessageBox.Show("You cannot have the same shortcut for multiple categories!", "Duplicate Shortcut");
                return;
            }
            if (categories.Any(a =>
                !(sel && a.Equals(categories[listboxCategories.SelectedIndex]))
                && a.Item1.Equals(textboxCategory.Text, StringComparison.CurrentCultureIgnoreCase))
            )
            {
                MessageBox.Show("You cannot have the same category name for multiple categories!", "Duplicate Category Name");
                return;
            }

            if (sel)
            {
                if (sorted.Count > 0 && sorted.Any(a => a.Item2 == categories[listboxCategories.SelectedIndex].Item1))
                {
                    // bad idea! :/
                    sorted = sorted
                        .Select(a => a.Item2 == categories[listboxCategories.SelectedIndex].Item1 ? (a.Item1, textboxCategory.Text) : a)
                        .ToList();
                }
                categories[listboxCategories.SelectedIndex] = (textboxCategory.Text, currentShortcut);
            }
            else
            {
                categories.Add((textboxCategory.Text, currentShortcut));
            }
            textboxCategory.Text = "";
            textboxShortcut.Text = "";
            currentShortcut = Keys.None;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listboxCategories.SelectedIndex == -1) return; // how
            if (sorted.Count > 0 && sorted.Any(a => a.Item2 == categories[listboxCategories.SelectedIndex].Item1))
            {
                MessageBox.Show("You can't remove an already populated category!", "No!");
                return;
            }
            categories.RemoveAt(listboxCategories.SelectedIndex);
            textboxCategory.Text = "";
            textboxShortcut.Text = "";
            currentShortcut = Keys.None;
        }

        private void listboxCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listboxCategories.SelectedIndex != -1)
            {
                buttonSubmit.Text = "Submit";
                buttonRemove.Enabled = true;

                textboxCategory.Text = categories[listboxCategories.SelectedIndex].Item1;
                currentShortcut = categories[listboxCategories.SelectedIndex].Item2;
                textboxShortcut.Text = ToKeysString(currentShortcut);
            }
            else
            {
                buttonSubmit.Text = "Add";
                buttonRemove.Enabled = false;
            }
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            if (galleryFolderDialog.ShowDialog() == DialogResult.OK)
            {
                labelCurrentFolder.Text = "Current Folder:\n" + galleryFolderDialog.SelectedPath;
                this.Refresh();
                filesToProcess =
                    Directory
                        .EnumerateFiles(galleryFolderDialog.SelectedPath, "*", SearchOption.AllDirectories)
                        .Where((a, _) => allowedFileExtensions.Any(f =>
                            a.ToLower().EndsWith(f)
                        ))
                        .ToArray();
                sorted.Clear();
                if (filesToProcess.Length == 0)
                {
                    MessageBox.Show("No image files found under that path!", "Empty Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                LoadCurrentImage();
            }
        }

        private void LoadCurrentImage()
        {
            try
            {
                if (sorted.Count < filesToProcess.Length)
                    pictureBox.ImageLocation = filesToProcess[sorted.Count];
                else pictureBox.ImageLocation = "";
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(
                    "Below is the Stack trace of this Exception. You can close the Application now (Click Yes) and you probably should if you don't know what this means.\n\n" + ex.StackTrace,
                    ex.Message,
                    MessageBoxButtons.YesNo) == DialogResult.Yes
                )
                {
                    Application.Exit();
                }
            }
            UpdateLog();
        }

        private void UpdateLog()
        {
            textboxLog.Text = $@"Progress: {sorted.Count}/{filesToProcess.Length} ({(100.0d * sorted.Count / filesToProcess.Length):F2}%)
Last few actions:
{(sorted.Count > 0
    ? sorted
        .TakeLast(10)
        .Reverse()
        .Select(a => $"{Path.GetRelativePath(galleryFolderDialog.SelectedPath, a.Item1)} sorted into {a.Item2}")
        .Aggregate((a, b) => $"{a}\n{b}")
    : "None!")}";
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            if (outputFolderDialog.ShowDialog() == DialogResult.OK)
            {
                ProcessFiles(outputFolderDialog.SelectedPath);
            }
        }

        private void ProcessFiles(string root)
        {
            Graphics g = pictureBox.CreateGraphics();
            textboxLog.Text = "Progress:\n-------- Initialize --------";
            foreach (var category in categories)
            {
                Directory.CreateDirectory(Path.Join(root, category.Item1));
                textboxLog.Text += $"\nCreated Directory {category.Item1}";
                textboxLog.Refresh();
            }
            textboxLog.Text += checkboxShouldMove.Checked ? "\n-------- Moving --------" : "\n-------- Copying --------";
            textboxLog.Refresh();
            foreach (var sortTuple in sorted)
            {
                if (checkboxSpecialCats.Checked && sortTuple.Item2 == "Ignore") continue;
                string relName = Path.GetRelativePath(galleryFolderDialog.SelectedPath, sortTuple.Item1);
                string catFolder = Path.Join(root, sortTuple.Item2);
                textboxLog.Text += $"\n{
                    (checkboxShouldMove.Checked ? "Moving" : "Copying")
                } {
                    Path.GetRelativePath(galleryFolderDialog.SelectedPath, sortTuple.Item1)
                } to {
                    sortTuple.Item2
                }...";
                textboxLog.Refresh();
                if (checkboxShouldMove.Checked) File.Move(sortTuple.Item1, Path.Join(catFolder, relName), true);
                else File.Copy(sortTuple.Item1, Path.Join(catFolder, relName), true);
            }
            if (!checkboxSpecialCats.Checked)
            {
                MessageBox.Show("Done!", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            textboxLog.Text += "\n-------- Special Actions --------";
            if (categories.Any(a => a.Item1 == "Delete"))
            {
                textboxLog.Text += "\nDeleting the \"Delete\" Directory...";
                Directory.Delete(Path.Join(root, "Delete"), true);
            }
        }

        private void Shortcut_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Menu) return;
            if (e.KeyCode == Keys.Escape)
            {
                currentShortcut = Keys.None;
                textboxShortcut.Text = "";
            }
            textboxShortcut.Text = ToKeysString(e.Modifiers | e.KeyCode);
            currentShortcut = e.Modifiers | e.KeyCode;
        }

        private void ResetFocus(object sender, EventArgs e)
        {
            this.ActiveControl = textboxLog;
            listboxCategories.SelectedIndex = -1;

            textboxCategory.Text = "";
            textboxShortcut.Text = "";
            currentShortcut = Keys.None;
        }

        private string ToKeysString(Keys k)
        {
            Keys m = k & Keys.Modifiers;
            bool ctrl = m.HasFlag(Keys.Control);
            bool shift = m.HasFlag(Keys.Shift);
            bool alt = m.HasFlag(Keys.Alt);
            return (ctrl ? "Ctrl + " : "") + (shift ? "Shift + " : "") + (alt ? "Alt + " : "") + ((Keys)(k - m)).ToString();
        }

        private void checkboxShouldMove_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxShouldMove.Checked && noWarning)
            {
                if (MessageBox.Show("There is absolutely no warranty if data gets lost when moving instead of copying the files!", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    checkboxShouldMove.Checked = false;
                }
                else noWarning = false;
            }

            if (checkboxShouldMove.Checked)
                checkboxSpecialCats.Enabled = true;
            else
                checkboxSpecialCats.Enabled = false;
        }
    }
}
