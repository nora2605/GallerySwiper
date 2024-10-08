using System.Collections.ObjectModel;
using System.Text;

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

        //bool noWarning = true;

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

            if (filesToProcess.Length != 0)
            {
                LoadCurrentImage();
                if (sorted.Count == filesToProcess.Length)
                    MessageBox.Show("You're done! You can now press the \"Process\" Button or Undo with Ctrl+Z");
            }
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
                buttonSubmit.Text = "Add";
                buttonRemove.Enabled = false;
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
            if (galleryFolderDialog.ShowDialog() != DialogResult.OK) return;
            LoadFiles();
        }

        private void LoadFiles()
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

        private void LoadCurrentImage()
        {
            if (sorted.Count < filesToProcess.Length)
            {
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                pictureBox.Image = pictureBox.InitialImage;
                pictureBox.Refresh();
                pictureBox.ImageLocation = filesToProcess[sorted.Count];
                try { pictureBox.Load(); }
                catch {
                    pictureBox.Image = pictureBox.ErrorImage;
                }
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else pictureBox.ImageLocation = "";
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
            if (filesToProcess.Length != sorted.Count && !checkboxShouldMove.Checked)
            {
                DialogResult dr = MessageBox.Show("You didn't categorize all files! It may be easier to manage moving partial results instead of copying them! Do you want to switch to moving?", "Warning", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                    checkboxShouldMove.Checked = true;
                else if (dr != DialogResult.No)
                    return;
            }
            textboxLog.Text = "Progress:\n-------- Initialize --------";
            foreach (var category in categories)
            {
                if (checkboxSpecialCats.Checked && category.Item1 == "Ignore") continue;
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
                textboxLog.Text += $"\n{(checkboxShouldMove.Checked ? "Moving" : "Copying")} {Path.GetRelativePath(galleryFolderDialog.SelectedPath, sortTuple.Item1)} to {sortTuple.Item2}...";
                textboxLog.Refresh();
                textboxLog.SelectionStart = textboxLog.TextLength - 1;
                textboxLog.ScrollToCaret();
                string fileDest = Path.Join(catFolder, relName);
                Directory.CreateDirectory(Path.GetDirectoryName(fileDest) ?? "Ignore");
                try
                {
                    if (checkboxShouldMove.Checked) File.Move(sortTuple.Item1, fileDest, true);
                    else File.Copy(sortTuple.Item1, fileDest, true);
                }
                // ignore when a source file might not be present anymore; happens when sort data is outdated etc.
                catch (FileNotFoundException) { continue; }
                catch (Exception e)
                {
                    DialogResult dr = MessageBox.Show($"Error while transferring file: {e.Message}\nIf you're getting more of these you can abort the process by clicking Yes.", "Error", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                        return;
                }
            }
            if (checkboxSpecialCats.Checked)
            {
                textboxLog.Text += "\n-------- Special Actions --------";
                if (categories.Any(a => a.Item1 == "Delete"))
                {
                    textboxLog.Text += "\nDeleting the \"Delete\" Directory...";
                    textboxLog.Refresh();
                    textboxLog.SelectionStart = textboxLog.TextLength - 1;
                    textboxLog.ScrollToCaret();
                    Directory.Delete(Path.Join(root, "Delete"), true);
                }
            }
            if (checkboxShouldMove.Checked) LoadFiles();
            MessageBox.Show("Done!", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            //if (checkboxShouldMove.Checked && noWarning)
            //{
            //    if (MessageBox.Show("There is absolutely no warranty if data gets lost when moving instead of copying the files!", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
            //    {
            //        checkboxShouldMove.Checked = false;
            //    }
            //    else noWarning = false;
            //}

            if (checkboxShouldMove.Checked)
                checkboxSpecialCats.Enabled = true;
            else
                checkboxSpecialCats.Enabled = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(galleryFolderDialog.SelectedPath)) return;

            string outs = $"{galleryFolderDialog.SelectedPath}\n---Cat---";
            foreach (var (n, s) in categories)
            {
                outs += $"\n{n}><{(int)s}";
            }
            outs += "\n---Sorted---";
            foreach (var (p, d) in sorted)
            {
                outs += $"\n{p}><{d}";
            }
            var res = new SaveFileDialog()
            {
                Title = "Save Progress",
                Filter = "TXT Files (*.txt)|*.txt",
                FileName = "sorted.txt",
                ValidateNames = true,
                AddToRecent = false
            };
            if (res.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(res.FileName, outs, Encoding.UTF8);
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            var res = new OpenFileDialog()
            {
                Title = "Load Progress",
                Filter = "TXT Files (*.txt)|*.txt",
                FileName = "sorted.txt",
                ValidateNames = true,
                AddToRecent = false
            };
            if (res.ShowDialog() != DialogResult.OK) return;

            string[] ins = File.ReadAllLines(res.FileName, Encoding.UTF8);
            galleryFolderDialog.SelectedPath = ins[0];
            LoadFiles();
            categories.Clear();
            sorted.Clear();
            bool catDone = false;

            foreach (var l in ins[1..])
            {
                if (l == "---Cat---")
                {
                    catDone = false;
                    continue;
                }
                if (l == "---Sorted---")
                {
                    catDone = true;
                    continue;
                }
                string[] pd = l.Split("><");
                if (pd.Length != 2)
                {
                    MessageBox.Show("Invalid Progress file");
                    return;
                }
                if (catDone)
                    sorted.Add((pd[0], pd[1]));
                else
                {
                    bool v = int.TryParse(pd[1], out int key);
                    if (!v)
                    {
                        MessageBox.Show("Invalid Progress file");
                        return;
                    }
                    categories.Add((pd[0], (Keys)key));
                }
            }
            // bring already sorted entries to the front

            // remove all entries in sorted that aren't present,
            // then append all of sorted to the front to match the indices and enable correct ctrl+z functionality
            List<string> tmp = [.. filesToProcess];
            foreach (var (s, _) in sorted)
            {
                int i = tmp.IndexOf(s);
                if (i == -1) sorted.RemoveAll(t => t.Item1 == s);
                else tmp.RemoveAll(t => t == s);
            }
            filesToProcess = [.. sorted.Select(t => t.Item1), .. tmp];

            LoadCurrentImage();
        }
    }
}
