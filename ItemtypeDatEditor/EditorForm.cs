using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient; // Import MySQL .dll


namespace ItemtypeDatEditor
{
    public partial class EditorForm : Form
    {

        public static DataTable ToDataTable<T>(List<T> items)
        {
            var dt = new DataTable(typeof(T).Name);
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                    values[i] = props[i].GetValue(item, null) ?? DBNull.Value;
                dt.Rows.Add(values);
            }
            return dt;
        }
        public EditorForm()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            InitializeComponent();

            txtHost.Text = "localhost";
            txtPort.Text = "3306";
            txtUser.Text = "root";
            txtPass.Text = "test";
            txtDb.Text = "yourdb";
            btnConnect.Text = "Connect";

            SetMySqlControlsEnabled(false); // Disable masa mula2 buka

            dataGridView1.AllowUserToResizeColumns = false; // Lock column width
            dataGridView1.AllowUserToResizeRows = false;    // Lock row height
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing; // Lock row header width
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; // Lock column header height

            btnSave.Enabled = false;
            btnExport.Enabled = false;
            btnImport.Enabled = false;
            txtSearch.Enabled = false;
            btnSearch.Enabled = false;
            cmbSearch.Enabled = false;



            cmbSearch.DropDownStyle = ComboBoxStyle.DropDownList;


            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;

            var menu = new ContextMenuStrip();
            var addItem = new ToolStripMenuItem("Add Row");
            addItem.ShortcutKeys = Keys.Control | Keys.N;
            addItem.ShortcutKeyDisplayString = "Ctrl+N";
            addItem.Click += (s, ea) => AddRow();
            menu.Items.Add(addItem);

            var copyItem = new ToolStripMenuItem("Copy");
            copyItem.ShortcutKeys = Keys.Control | Keys.C;
            copyItem.ShortcutKeyDisplayString = "Ctrl+C";
            copyItem.Click += (s, ea) => CopyRow();
            menu.Items.Add(copyItem);

            var pasteItem = new ToolStripMenuItem("Paste");
            pasteItem.ShortcutKeys = Keys.Control | Keys.V;
            pasteItem.ShortcutKeyDisplayString = "Ctrl+V";
            pasteItem.Click += (s, ea) => PasteRow();
            menu.Items.Add(pasteItem);

            var exportItem = new ToolStripMenuItem("Export Selected");
            exportItem.ShortcutKeys = Keys.Control | Keys.E;
            exportItem.ShortcutKeyDisplayString = "Ctrl+E";
            exportItem.Click += (s, ea) => ExportSelected();
            menu.Items.Add(exportItem);

            var delItem = new ToolStripMenuItem("Delete Selected");
            delItem.ShortcutKeys = Keys.Delete;
            delItem.ShortcutKeyDisplayString = "Del";
            delItem.Click += (s, ea) => DeleteSelected();
            menu.Items.Add(delItem);


            dataGridView1.ContextMenuStrip = menu;
            dataGridView1.RowLeave += dataGridView1_RowLeave;
            dataGridView1.DataError += dataGridView1_DataError;

            // Keydown shortcut
            dataGridView1.KeyDown += DataGridView1_KeyDown;
            dataGridView1.MultiSelect = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        }

        public class ItemType
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int req_profession { get; set; }
            public int req_level { get; set; }
            public int req_sex { get; set; }      // 0: default, 1: male, 2: female
            public int req_force { get; set; }    // Str
            public int req_dex { get; set; }      // Dexterity (baru)
            public int req_health { get; set; }   // Vital
            public int req_soul { get; set; }     // Spirit
            public int monopoly { get; set; }
            public int price { get; set; }
            public int id_action { get; set; }
            public int attack_max { get; set; }
            public int attack_min { get; set; }
            public int defense { get; set; }
            public int magic_atk_min { get; set; }
            public int dodge { get; set; }
            public int life { get; set; }
            public int amount { get; set; }
            public int amount_limit { get; set; }
            public int magic_atk_max { get; set; }
            public int magic_def { get; set; }
            public int atk_range { get; set; }
            public int atk_speed { get; set; }
            public int hitrate { get; set; }
            public int target { get; set; }
            public int emoney { get; set; }
            public int official1 { get; set; }
            public int official2 { get; set; }
            public int official3 { get; set; }
            public int official4 { get; set; }
            public int official5 { get; set; }
            public int official6 { get; set; }
            public int official7 { get; set; }
            public int official8 { get; set; }
            public int official9 { get; set; }
            public int official10 { get; set; }
            public int official11 { get; set; }
            public string Desc { get; set; }
        }



        private void btnLoad_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "itemtype.dat|itemtype.dat|All files|*.*";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            var items = new List<ItemType>();
            using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            {
                int count = br.ReadInt32();
                long idTableOffset = 4;
                long dataOffset = idTableOffset + count * 4;

                for (int i = 0; i < count; i++)
                {
                    long rowStart = dataOffset + i * 244;
                    fs.Position = rowStart;
                    byte[] b = br.ReadBytes(244);

                    if (b.Length < 244)
                    {
                        MessageBox.Show($"Row {i + 1} size kurang: {b.Length} bytes (Expected 244)");
                        break;
                    }

                    int id = BitConverter.ToInt32(b, 0);
                    string name = Encoding.GetEncoding("GB2312").GetString(b, 4, 16).TrimEnd('\0');
                    int req_profession = BitConverter.ToInt16(b, 20);
                    int req_level = BitConverter.ToInt16(b, 22);

                    int req_force = BitConverter.ToInt16(b, 24);   // Str
                    int req_dex = BitConverter.ToInt16(b, 26);     // Dexterity (baru, offset 26-27)
                    int req_health = BitConverter.ToInt16(b, 28);  // Vital
                    int req_soul = BitConverter.ToInt16(b, 30);    // Spirit

                    int monopoly = BitConverter.ToInt32(b, 32);
                    int price = BitConverter.ToInt32(b, 36);
                    int id_action = BitConverter.ToInt32(b, 40);
                    int attack_max = BitConverter.ToInt16(b, 44);
                    int attack_min = BitConverter.ToInt16(b, 46);
                    int defense = BitConverter.ToInt16(b, 48);
                    int magic_atk_min = BitConverter.ToInt16(b, 50);
                    int dodge = BitConverter.ToInt16(b, 52);
                    int life = BitConverter.ToInt32(b, 54);
                    int amount = BitConverter.ToUInt16(b, 58);
                    int amount_limit = BitConverter.ToUInt16(b, 60);
                    int magic_atk_max = BitConverter.ToInt16(b, 68);
                    int magic_def = BitConverter.ToInt16(b, 70);
                    int atk_range = BitConverter.ToInt16(b, 72);
                    int atk_speed = BitConverter.ToInt16(b, 74);
                    int hitrate = BitConverter.ToInt32(b, 76);
                    int target = BitConverter.ToInt32(b, 80);
                    int emoney = BitConverter.ToInt16(b, 84);

                    // Official fields
                    int official1 = BitConverter.ToInt16(b, 88);
                    int official2 = BitConverter.ToInt16(b, 90);
                    int official3 = BitConverter.ToInt16(b, 92);
                    int official4 = BitConverter.ToInt16(b, 94);
                    int official5 = BitConverter.ToInt16(b, 96);
                    int official6 = BitConverter.ToInt16(b, 98);
                    int official7 = BitConverter.ToInt16(b, 100);
                    int official8 = BitConverter.ToInt16(b, 102);
                    int official9 = BitConverter.ToInt16(b, 104);
                    int official10 = BitConverter.ToInt16(b, 106);
                    int official11 = BitConverter.ToInt16(b, 108);

                    string desc = Encoding.GetEncoding("GB2312").GetString(b, 116, 128).TrimEnd('\0');

                    int req_sex = 0;
                    if (req_level == 256)
                    {
                        req_sex = 1;
                        req_level = 0;
                    }
                    else if (req_level == 512)
                    {
                        req_sex = 2;
                        req_level = 0;
                    }

                    items.Add(new ItemType
                    {
                        ID = id,
                        Name = name,
                        req_profession = req_profession,
                        req_level = req_level,
                        req_sex = req_sex,
                        req_force = req_force,
                        req_dex = req_dex,
                        req_health = req_health,
                        req_soul = req_soul,
                        monopoly = monopoly,
                        price = price,
                        id_action = id_action,
                        attack_max = attack_max,
                        attack_min = attack_min,
                        defense = defense,
                        magic_atk_min = magic_atk_min,
                        dodge = dodge,
                        life = life,
                        amount = amount,
                        amount_limit = amount_limit,
                        magic_atk_max = magic_atk_max,
                        magic_def = magic_def,
                        atk_range = atk_range,
                        atk_speed = atk_speed,
                        hitrate = hitrate,
                        target = target,
                        emoney = emoney,
                        official1 = official1,
                        official2 = official2,
                        official3 = official3,
                        official4 = official4,
                        official5 = official5,
                        official6 = official6,
                        official7 = official7,
                        official8 = official8,
                        official9 = official9,
                        official10 = official10,
                        official11 = official11,
                        Desc = desc
                    });
                }

            }

            // Convert ke DataTable untuk enable sorting
            DataTable dt = ToDataTable(items);
            // Set DataTable as datasource
            dataGridView1.DataSource = dt;

            // Enable sorting
            foreach (DataGridViewColumn col in dataGridView1.Columns)
                col.SortMode = DataGridViewColumnSortMode.Automatic;

            if (dataGridView1.Columns.Contains("req_level"))
                dataGridView1.Columns["req_level"].DefaultCellStyle.NullValue = 0;

            HookRowCountUpdater();

            // Set form title with total items
            this.Text = $"Itemtype.dat Editor by DuaSelipar - Total: {dt.Rows.Count} items";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
                col.SortMode = DataGridViewColumnSortMode.Automatic;

            if (dataGridView1.Columns.Contains("req_level"))
                dataGridView1.Columns["req_level"].DefaultCellStyle.NullValue = 0;

            PopulateSearchFields();
            lblStatus.Text = $"{dataGridView1.Rows.Count} items loaded. Ready.";
            btnSave.Enabled = true;
            btnExport.Enabled = true;
            btnImport.Enabled = true;
            txtSearch.Enabled = true;
            btnSearch.Enabled = true;
            cmbSearch.Enabled = true;

            SetMySqlControlsEnabled(true); // Enable semua lepas load file

        }





        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Guna nama column yang betul
            if (dataGridView1.Columns[e.ColumnIndex].Name == "req_level")
            {
                if (e.Value == null || e.Value.ToString() == "")
                {
                    e.Value = 0;
                    e.FormattingApplied = true;
                }
            }
        }








        // Event handler untuk btnSearch
        // Field global untuk track hasil search terakhir
        private int lastSearchRow = -1;

        // Search pertama (btnSearch)
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                dataGridView1.ClearSelection();
                lastSearchRow = -1;
                return;
            }

            FindNext(keyword);
        }


        private void PopulateSearchFields()
        {
            cmbSearch.Items.Clear();
            cmbSearch.Items.Add("All");
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                cmbSearch.Items.Add(col.HeaderText);
            }
            cmbSearch.SelectedIndex = 0; // default to All
        }

        private void FindNext(string keyword)
        {
            int totalRows = dataGridView1.Rows.Count;
            if (totalRows == 0) return;

            int startRow = lastSearchRow + 1;
            string selectedField = cmbSearch.SelectedItem?.ToString();

            // Search from current position to end
            for (int i = startRow; i < totalRows; i++)
            {
                if (IsMatch(i, keyword, selectedField))
                {
                    SelectRow(i);
                    return;
                }
            }
            // Wrap around, search from 0 to lastSearchRow
            for (int i = 0; i < startRow; i++)
            {
                if (IsMatch(i, keyword, selectedField))
                {
                    SelectRow(i);
                    return;
                }
            }
            MessageBox.Show("No more results.");
            lastSearchRow = -1;
        }

        private bool IsMatch(int rowIdx, string keyword, string field)
        {
            if (field == "All")
            {
                foreach (DataGridViewCell cell in dataGridView1.Rows[rowIdx].Cells)
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(keyword))
                        return true;
            }
            else
            {
                var cell = dataGridView1.Rows[rowIdx].Cells[field];
                if (cell != null && cell.Value != null && cell.Value.ToString().ToLower().Contains(keyword))
                    return true;
            }
            return false;
        }

        private void SelectRow(int rowIdx)
        {
            dataGridView1.ClearSelection();
            dataGridView1.Rows[rowIdx].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[rowIdx].Cells[0];
            dataGridView1.FirstDisplayedScrollingRowIndex = rowIdx;
            lastSearchRow = rowIdx;
        }



        // // Usage: Call FindNext(yourKeyword); for both Search and Next button.
        // // Example: FindNext(txtSearch.Text.Trim().ToLower());


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("No data to save.");
                return;
            }

            var dt = (DataTable)dataGridView1.DataSource;

            // Remove duplicate ID (keep first)
            var seen = new HashSet<int>();
            var toRemove = new List<DataRow>();
            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["ID"]);
                if (seen.Contains(id))
                    toRemove.Add(row);
                else
                    seen.Add(id);
            }
            foreach (var row in toRemove)
                dt.Rows.Remove(row);

            var sfd = new SaveFileDialog();
            sfd.Filter = "itemtype.dat|itemtype.dat|All files|*.*";
            if (sfd.ShowDialog() != DialogResult.OK) return;

            // Auto-fill null
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    if (row.IsNull(col))
                    {
                        if (col.DataType == typeof(string))
                            row[col] = "";
                        else
                            row[col] = 0;
                    }
                }
            }

            using (var fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
            using (var bw = new BinaryWriter(fs))
            {
                int count = dt.Rows.Count;
                bw.Write(count);

                foreach (DataRow row in dt.Rows)
                    bw.Write(Convert.ToInt32(row["ID"]));

                int rowNum = 0;
                foreach (DataRow row in dt.Rows)
                {
                    rowNum++;
                    byte[] b = new byte[244];
                    BitConverter.GetBytes(Convert.ToInt32(row["ID"])).CopyTo(b, 0);
                    var nameBytes = Encoding.GetEncoding("GB2312").GetBytes(row["Name"].ToString());
                    Array.Clear(b, 4, 16);
                    Array.Copy(nameBytes, 0, b, 4, Math.Min(nameBytes.Length, 16));
                    BitConverter.GetBytes(Convert.ToInt16(row["req_profession"])).CopyTo(b, 20);
                    // Convert req_sex back to legacy req_level (itemtype.dat format)
                    int save_req_level = Convert.ToInt16(row["req_level"]);
                    if (Convert.ToInt16(row["req_sex"]) == 1)
                        save_req_level = 256;
                    else if (Convert.ToInt16(row["req_sex"]) == 2)
                        save_req_level = 512;
                    BitConverter.GetBytes(save_req_level).CopyTo(b, 22);
                    BitConverter.GetBytes(Convert.ToInt16(row["req_force"])).CopyTo(b, 24);
                    BitConverter.GetBytes(Convert.ToInt16(row["req_dex"])).CopyTo(b, 26);
                    BitConverter.GetBytes(Convert.ToInt16(row["req_health"])).CopyTo(b, 28);
                    BitConverter.GetBytes(Convert.ToInt16(row["req_soul"])).CopyTo(b, 30);
                    BitConverter.GetBytes(Convert.ToInt32(row["monopoly"])).CopyTo(b, 32);
                    BitConverter.GetBytes(Convert.ToInt32(row["price"])).CopyTo(b, 36);
                    BitConverter.GetBytes(Convert.ToInt32(row["id_action"])).CopyTo(b, 40);
                    BitConverter.GetBytes(Convert.ToInt16(row["attack_max"])).CopyTo(b, 44);
                    BitConverter.GetBytes(Convert.ToInt16(row["attack_min"])).CopyTo(b, 46);
                    BitConverter.GetBytes(Convert.ToInt16(row["defense"])).CopyTo(b, 48);
                    BitConverter.GetBytes(Convert.ToInt16(row["magic_atk_min"])).CopyTo(b, 50);
                    BitConverter.GetBytes(Convert.ToInt16(row["dodge"])).CopyTo(b, 52);
                    BitConverter.GetBytes(Convert.ToInt32(row["life"])).CopyTo(b, 54);
                    BitConverter.GetBytes(Convert.ToUInt16(row["amount"])).CopyTo(b, 58);
                    BitConverter.GetBytes(Convert.ToUInt16(row["amount_limit"])).CopyTo(b, 60);
                    for (int j = 62; j <= 67; j++) b[j] = 0;
                    BitConverter.GetBytes(Convert.ToInt16(row["magic_atk_max"])).CopyTo(b, 68);
                    BitConverter.GetBytes(Convert.ToInt16(row["magic_def"])).CopyTo(b, 70);
                    BitConverter.GetBytes(Convert.ToInt16(row["atk_range"])).CopyTo(b, 72);
                    BitConverter.GetBytes(Convert.ToInt16(row["atk_speed"])).CopyTo(b, 74);
                    BitConverter.GetBytes(Convert.ToInt32(row["hitrate"])).CopyTo(b, 76);
                    BitConverter.GetBytes(Convert.ToInt32(row["target"])).CopyTo(b, 80);
                    BitConverter.GetBytes(Convert.ToInt16(row["emoney"])).CopyTo(b, 84);
                    b[86] = b[87] = 0;
                    BitConverter.GetBytes(Convert.ToInt16(row["official1"])).CopyTo(b, 88);
                    BitConverter.GetBytes(Convert.ToInt16(row["official2"])).CopyTo(b, 90);
                    BitConverter.GetBytes(Convert.ToInt16(row["official3"])).CopyTo(b, 92);
                    BitConverter.GetBytes(Convert.ToInt16(row["official4"])).CopyTo(b, 94);
                    BitConverter.GetBytes(Convert.ToInt16(row["official5"])).CopyTo(b, 96);
                    BitConverter.GetBytes(Convert.ToInt16(row["official6"])).CopyTo(b, 98);
                    BitConverter.GetBytes(Convert.ToInt16(row["official7"])).CopyTo(b, 100);
                    BitConverter.GetBytes(Convert.ToInt16(row["official8"])).CopyTo(b, 102);
                    BitConverter.GetBytes(Convert.ToInt16(row["official9"])).CopyTo(b, 104);
                    BitConverter.GetBytes(Convert.ToInt16(row["official10"])).CopyTo(b, 106);
                    BitConverter.GetBytes(Convert.ToInt16(row["official11"])).CopyTo(b, 108);
                    for (int j = 110; j <= 115; j++) b[j] = 0;
                    var descBytes = Encoding.GetEncoding("GB2312").GetBytes(row["Desc"].ToString());
                    Array.Clear(b, 116, 128);
                    Array.Copy(descBytes, 0, b, 116, Math.Min(descBytes.Length, 128));

                    bw.Write(b);
                }
            }

            MessageBox.Show("Save completed! Duplicates removed: " + toRemove.Count);
        }



        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            var sfd = new SaveFileDialog();
            sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            sfd.FileName = "itemtype_export.csv";
            if (sfd.ShowDialog() != DialogResult.OK) return;

            var dt = (DataTable)dataGridView1.DataSource;
            var sb = new StringBuilder();

            // Header
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.Append($"\"{dt.Columns[i]}\"");
                if (i < dt.Columns.Count - 1)
                    sb.Append(",");
            }
            sb.AppendLine();

            // Rows
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string val = row[i]?.ToString() ?? "";
                    // Escape " with ""
                    val = val.Replace("\"", "\"\"");
                    sb.Append($"\"{val}\"");
                    if (i < dt.Columns.Count - 1)
                        sb.Append(",");
                }
                sb.AppendLine();
            }

            File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("Export complete!");
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            ImportCsv();
        }

        private void ImportCsv()
        {
            if (dataGridView1.DataSource is DataTable dt)
            {
                var ofd = new OpenFileDialog();
                ofd.Filter = "CSV File|*.csv|All files|*.*";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                int added = 0, skipped = 0;
                var lines = File.ReadAllLines(ofd.FileName);

                // Option: skip header kalau ada
                int startLine = (lines.Length > 0 && lines[0].ToLower().Contains("id,")) ? 1 : 0;

                for (int idx = startLine; idx < lines.Length; idx++)
                {
                    var fields = ParseCsvLine(lines[idx], dt.Columns.Count);
                    if (fields.Count == 0) continue;

                    // Cari ID, kalau ada skip
                    string idStr = fields[0];
                    if (!int.TryParse(idStr, out int id)) continue;

                    bool exist = false;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToInt32(row["ID"]) == id)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (exist)
                    {
                        skipped++;
                        continue;
                    }

                    // Add new row
                    var newRow = dt.NewRow();
                    for (int col = 0; col < dt.Columns.Count && col < fields.Count; col++)
                    {
                        var colType = dt.Columns[col].DataType;
                        var val = fields[col];

                        if (colType == typeof(int))
                        {
                            if (int.TryParse(val, out int v))
                                newRow[col] = v;
                            else
                                newRow[col] = 0;
                        }
                        else
                        {
                            newRow[col] = val;
                        }
                    }
                    dt.Rows.Add(newRow);
                    added++;
                }

                MessageBox.Show($"Import completed!\nAdded: {added}\nSkipped (duplicate ID): {skipped}");
                // Optionally update window title here if needed
            }
        }


        // Simple CSV parser (support petik dua & koma & escaped quotes)
        private List<string> ParseCsvLine(string line)
        {
            var result = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (inQuotes)
                {
                    if (c == '\"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '\"')
                        {
                            sb.Append('\"');
                            i++; // skip escaped quote
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                else
                {
                    if (c == '\"')
                    {
                        inQuotes = true;
                    }
                    else if (c == ',')
                    {
                        result.Add(sb.ToString());
                        sb.Clear();
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }
            result.Add(sb.ToString());
            return result;
        }






        private DataObject clipboardRow = null;

        private void AddRow()
        {
            if (dataGridView1.DataSource is DataTable dt)
            {
                var newRow = dt.NewRow();
                // Semua int & string = DBNull (supaya kosong dalam grid, bukan 0)
                foreach (DataColumn col in dt.Columns)
                {
                    newRow[col.ColumnName] = DBNull.Value;
                }
                dt.Rows.Add(newRow);

                // Focus ke row terakhir & select
                int lastRow = dataGridView1.Rows.Count - 1;
                if (lastRow >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[lastRow].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[lastRow].Cells[0];
                    dataGridView1.FirstDisplayedScrollingRowIndex = lastRow;
                }
            }
            lblStatus.Text = $"New row added. Total: {dataGridView1.Rows.Count}";
        }



        private void CopyRow()
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            var sb = new StringBuilder();

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                var values = new List<string>();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    string val = cell.Value?.ToString() ?? "";
                    // Escape quotes: ganti " dengan ""
                    val = val.Replace("\"", "\"\"");
                    // Add quotes around field
                    values.Add($"\"{val}\"");
                }
                sb.AppendLine(string.Join(",", values));
            }

            Clipboard.SetText(sb.ToString());
        }


        private void PasteRow()
        {
            if (dataGridView1.DataSource is DataTable dt)
            {
                // Ambil clipboard text, asingkan baris
                var lines = Clipboard.GetText().Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length == 0) return;

                int startRow = dataGridView1.CurrentRow?.Index ?? 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (startRow + i >= dt.Rows.Count) // Auto add kalau tak cukup row
                        dt.Rows.Add(dt.NewRow());

                    var fields = ParseCsvLine(lines[i], dt.Columns.Count);

                    for (int col = 0; col < dt.Columns.Count && col < fields.Count; col++)
                    {
                        var colType = dt.Columns[col].DataType;
                        var val = fields[col];

                        if (colType == typeof(int))
                        {
                            if (int.TryParse(val, out int v))
                                dt.Rows[startRow + i][col] = v;
                            else
                                dt.Rows[startRow + i][col] = 0;
                        }
                        else
                        {
                            dt.Rows[startRow + i][col] = val;
                        }
                    }
                }
            }
        }

        // Parser untuk CSV dengan quote & escape ""
        private List<string> ParseCsvLine(string line, int expectCols)
        {
            var list = new List<string>();
            int i = 0;
            while (i < line.Length)
            {
                if (line[i] == '"')
                {
                    // quoted field
                    int start = ++i;
                    var sb = new StringBuilder();
                    while (i < line.Length)
                    {
                        if (line[i] == '"' && i + 1 < line.Length && line[i + 1] == '"')
                        {
                            sb.Append('"');
                            i += 2;
                        }
                        else if (line[i] == '"')
                        {
                            i++;
                            break;
                        }
                        else
                        {
                            sb.Append(line[i++]);
                        }
                    }
                    list.Add(sb.ToString());
                    // skip comma
                    if (i < line.Length && line[i] == ',') i++;
                }
                else
                {
                    // unquoted (should not happen, just fallback)
                    int start = i;
                    while (i < line.Length && line[i] != ',') i++;
                    list.Add(line.Substring(start, i - start));
                    if (i < line.Length && line[i] == ',') i++;
                }
            }
            // pad if less column
            while (list.Count < expectCols)
                list.Add("");
            return list;
        }

        private void ExportSelected()
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            var sfd = new SaveFileDialog { Filter = "CSV|*.csv" };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            using (var sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
            {
                // Header
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (i > 0) sw.Write(",");
                    sw.Write($"\"{dataGridView1.Columns[i].HeaderText}\"");
                }
                sw.WriteLine();

                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        if (i > 0) sw.Write(",");
                        var v = row.Cells[i].Value?.ToString() ?? "";
                        sw.Write($"\"{v.Replace("\"", "\"\"")}\"");
                    }
                    sw.WriteLine();
                }
            }
            MessageBox.Show("Selected rows exported!");
            int selectedCount = dataGridView1.SelectedRows.Count;
            lblStatus.Text = $"Exported {selectedCount} selected row(s).";
        }
        private void DeleteSelected()
        {
            int before = dataGridView1.Rows.Count;
            var dt = dataGridView1.DataSource as DataTable;
            if (dt == null) return;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (!row.IsNewRow)
                    dataGridView1.Rows.Remove(row);
            }
            int after = dataGridView1.Rows.Count;
            lblStatus.Text = $"Deleted {before - after} row(s). {after} remaining.";
        }

        // Keyboard shortcut
        private void DataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C) { CopyRow(); e.Handled = true; }
            else if (e.Control && e.KeyCode == Keys.V) { PasteRow(); e.Handled = true; }
            else if (e.Control && e.KeyCode == Keys.N) { AddRow(); e.Handled = true; }
            else if (e.Control && e.KeyCode == Keys.E) { ExportSelected(); e.Handled = true; }
            else if (e.KeyCode == Keys.Delete) { DeleteSelected(); e.Handled = true; }
        }


        // Panggil lepas dataGridView1.DataSource = dt;
        private void HookRowCountUpdater()
        {
            var dt = dataGridView1.DataSource as DataTable;
            if (dt == null) return;

            dt.RowChanged -= Dt_RowChanged;
            dt.RowDeleted -= Dt_RowChanged; // Handler sama, update bila row delete juga

            dt.RowChanged += Dt_RowChanged;
            dt.RowDeleted += Dt_RowChanged;

            UpdateTotalItemTitle();
        }

        private void Dt_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            UpdateTotalItemTitle();
        }

        private void UpdateTotalItemTitle()
        {
            var dt = dataGridView1.DataSource as DataTable;
            int total = dt?.Rows.Count ?? 0;
            this.Text = $"Itemtype.dat Editor by DuaSelipar - Total: {total} items";
        }

        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.DataSource is DataTable dt)
            {
                int rowIndex = e.RowIndex;
                if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
                {
                    var row = dataGridView1.Rows[rowIndex];
                    bool isEmpty = true;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        var v = cell.Value;
                        if (v != null && v.ToString() != "" && v.ToString() != "0")
                        {
                            isEmpty = false;
                            break;
                        }
                    }
                    if (isEmpty)
                    {
                        // Remove dari DataTable
                        if (rowIndex < dt.Rows.Count)
                        {
                            dt.Rows.RemoveAt(rowIndex);
                        }
                    }
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Just ignore format errors caused by empty/null values on int columns
            e.Cancel = true;
        }

        MySqlConnection conn;
        bool connected = false;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!connected)
            {
                // Try connect
                string connStr = $"Server={txtHost.Text};Port={txtPort.Text};Database={txtDb.Text};Uid={txtUser.Text};Pwd={txtPass.Text};";
                try
                {
                    conn = new MySqlConnection(connStr);
                    conn.Open();
                    lblStatus.Text = "Connected!";
                    btnConnect.Text = "Disconnect";
                    connected = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection failed!\n" + ex.Message);
                    lblStatus.Text = "Not Connected";
                }
            }
            else
            {
                // Disconnect
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                    lblStatus.Text = "Disconnected!";
                }
                btnConnect.Text = "Connect";
                connected = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (conn == null || conn.State != ConnectionState.Open)
            {
                MessageBox.Show("Not connected to database.");
                return;
            }
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("No data loaded.");
                return;
            }

            var dt = (DataTable)dataGridView1.DataSource;

            // Ambil semua id sedia ada dari DB (sekali je, laju & selamat)
            var existIds = new HashSet<int>();
            using (var cmd = new MySqlCommand("SELECT id FROM cq_itemtype", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                    existIds.Add(Convert.ToInt32(rdr[0]));
            }

            int addCount = 0, skipCount = 0;
            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["ID"]);
                if (existIds.Contains(id))
                {
                    skipCount++;
                    continue; // dah ada, skip
                }

                try
                {
                    using (var cmd = new MySqlCommand(
                        @"INSERT INTO cq_itemtype 
                (id, name, req_profession, req_level, req_sex, req_force, req_dex, req_health, req_soul, monopoly, price, id_action, attack_max, attack_min, defense, magic_atk_min, dodge, life, amount, amount_limit, magic_atk_max, magic_def, atk_range, atk_speed, hitrate, target, emoney, official1, official2, official3, official4, official5, official6, official7, official8, official9, official10, official11) 
                VALUES
                (@id, @name, @req_profession, @req_level, @req_sex, @req_force, @req_dex, @req_health, @req_soul, @monopoly, @price, @id_action, @attack_max, @attack_min, @defense, @magic_atk_min, @dodge, @life, @amount, @amount_limit, @magic_atk_max, @magic_def, @atk_range, @atk_speed, @hitrate, @target, @emoney, @official1, @official2, @official3, @official4, @official5, @official6, @official7, @official8, @official9, @official10, @official11)", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@name", row["Name"].ToString());
                        cmd.Parameters.AddWithValue("@req_profession", row["req_profession"]);
                        cmd.Parameters.AddWithValue("@req_level", row["req_level"]);
                        cmd.Parameters.AddWithValue("@req_sex", row["req_sex"]);
                        cmd.Parameters.AddWithValue("@req_force", row["req_force"]);
                        cmd.Parameters.AddWithValue("@req_dex", row["req_dex"]);
                        cmd.Parameters.AddWithValue("@req_health", row["req_health"]);
                        cmd.Parameters.AddWithValue("@req_soul", row["req_soul"]);
                        cmd.Parameters.AddWithValue("@monopoly", row["monopoly"]);
                        cmd.Parameters.AddWithValue("@price", row["price"]);
                        cmd.Parameters.AddWithValue("@id_action", row["id_action"]);
                        cmd.Parameters.AddWithValue("@attack_max", row["attack_max"]);
                        cmd.Parameters.AddWithValue("@attack_min", row["attack_min"]);
                        cmd.Parameters.AddWithValue("@defense", row["defense"]);
                        cmd.Parameters.AddWithValue("@magic_atk_min", row["magic_atk_min"]);
                        cmd.Parameters.AddWithValue("@dodge", row["dodge"]);
                        cmd.Parameters.AddWithValue("@life", row["life"]);
                        cmd.Parameters.AddWithValue("@amount", row["amount"]);
                        cmd.Parameters.AddWithValue("@amount_limit", row["amount_limit"]);
                        cmd.Parameters.AddWithValue("@magic_atk_max", row["magic_atk_max"]);
                        cmd.Parameters.AddWithValue("@magic_def", row["magic_def"]);
                        cmd.Parameters.AddWithValue("@atk_range", row["atk_range"]);
                        cmd.Parameters.AddWithValue("@atk_speed", row["atk_speed"]);
                        cmd.Parameters.AddWithValue("@hitrate", row["hitrate"]);
                        cmd.Parameters.AddWithValue("@target", row["target"]);
                        cmd.Parameters.AddWithValue("@emoney", row["emoney"]);
                        cmd.Parameters.AddWithValue("@official1", row["official1"]);
                        cmd.Parameters.AddWithValue("@official2", row["official2"]);
                        cmd.Parameters.AddWithValue("@official3", row["official3"]);
                        cmd.Parameters.AddWithValue("@official4", row["official4"]);
                        cmd.Parameters.AddWithValue("@official5", row["official5"]);
                        cmd.Parameters.AddWithValue("@official6", row["official6"]);
                        cmd.Parameters.AddWithValue("@official7", row["official7"]);
                        cmd.Parameters.AddWithValue("@official8", row["official8"]);
                        cmd.Parameters.AddWithValue("@official9", row["official9"]);
                        cmd.Parameters.AddWithValue("@official10", row["official10"]);
                        cmd.Parameters.AddWithValue("@official11", row["official11"]);

                        cmd.ExecuteNonQuery();
                        addCount++;
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    if (ex.Number == 1062) // duplicate
                    {
                        skipCount++;
                        continue;
                    }
                    else
                    {
                        MessageBox.Show("DB Error: " + ex.Message);
                        break;
                    }
                }
            }

            MessageBox.Show($"Add completed.\nInserted: {addCount}\nSkipped: {skipCount} (already exists)");
        }


        private void SetMySqlControlsEnabled(bool enabled)
        {
            txtHost.Enabled = enabled;
            txtPort.Enabled = enabled;
            txtUser.Enabled = enabled;
            txtPass.Enabled = enabled;
            txtDb.Enabled = enabled;
            btnConnect.Enabled = enabled;
            btnAdd.Enabled = enabled;
            btnUptname.Enabled = enabled;
            btnUptall.Enabled = enabled;

        }

        private void btnUptname_Click(object sender, EventArgs e)
        {
            if (conn == null || conn.State != ConnectionState.Open)
            {
                MessageBox.Show("Please connect to the database first!");
                return;
            }
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("Please load .dat file first!");
                return;
            }

            var dt = (DataTable)dataGridView1.DataSource;
            int updateCount = 0;

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "UPDATE cq_itemtype SET name=@name WHERE id=@id";
                cmd.Parameters.Add("@name", MySqlDbType.VarChar, 15);
                cmd.Parameters.Add("@id", MySqlDbType.Int32);

                foreach (DataRow row in dt.Rows)
                {
                    cmd.Parameters["@name"].Value = row["Name"].ToString();
                    cmd.Parameters["@id"].Value = Convert.ToInt32(row["ID"]);
                    int affected = cmd.ExecuteNonQuery();
                    if (affected > 0)
                        updateCount++;
                }
            }

            MessageBox.Show($"Updated {updateCount} names to database!");
        }

        private void btnUptall_Click(object sender, EventArgs e)
        {
            if (conn == null || conn.State != ConnectionState.Open)
            {
                MessageBox.Show("Please connect to the database first!");
                return;
            }
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("Please load .dat file first!");
                return;
            }

            var dt = (DataTable)dataGridView1.DataSource;
            int total = dt.Rows.Count;

            // Confirmation popup
            if (MessageBox.Show($"Are you sure to update all {total} items? This will overwrite existing records.",
                "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            int updated = 0;

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE cq_itemtype SET
                name=@name,
                req_profession=@req_profession,
                req_level=@req_level,
                req_sex=@req_sex,
                req_force=@req_force,
                req_dex=@req_dex,
                req_health=@req_health,
                req_soul=@req_soul,
                monopoly=@monopoly,
                price=@price,
                id_action=@id_action,
                attack_max=@attack_max,
                attack_min=@attack_min,
                defense=@defense,
                magic_atk_min=@magic_atk_min,
                dodge=@dodge,
                life=@life,
                amount=@amount,
                amount_limit=@amount_limit,
                magic_atk_max=@magic_atk_max,
                magic_def=@magic_def,
                atk_range=@atk_range,
                atk_speed=@atk_speed,
                hitrate=@hitrate,
                target=@target,
                emoney=@emoney,
                official1=@official1,
                official2=@official2,
                official3=@official3,
                official4=@official4,
                official5=@official5,
                official6=@official6,
                official7=@official7,
                official8=@official8,
                official9=@official9,
                official10=@official10,
                official11=@official11
            WHERE id=@id";
                // Add parameters
                cmd.Parameters.Add("@name", MySqlDbType.VarChar, 15);
                cmd.Parameters.Add("@req_profession", MySqlDbType.Int16);
                cmd.Parameters.Add("@req_level", MySqlDbType.Int16);
                cmd.Parameters.Add("@req_sex", MySqlDbType.Int16);
                cmd.Parameters.Add("@req_force", MySqlDbType.Int16);
                cmd.Parameters.Add("@req_dex", MySqlDbType.Int16);
                cmd.Parameters.Add("@req_health", MySqlDbType.Int16);
                cmd.Parameters.Add("@req_soul", MySqlDbType.Int16);
                cmd.Parameters.Add("@monopoly", MySqlDbType.Int32);
                cmd.Parameters.Add("@price", MySqlDbType.Int32);
                cmd.Parameters.Add("@id_action", MySqlDbType.Int32);
                cmd.Parameters.Add("@attack_max", MySqlDbType.Int16);
                cmd.Parameters.Add("@attack_min", MySqlDbType.Int16);
                cmd.Parameters.Add("@defense", MySqlDbType.Int16);
                cmd.Parameters.Add("@magic_atk_min", MySqlDbType.Int16);
                cmd.Parameters.Add("@dodge", MySqlDbType.Int16);
                cmd.Parameters.Add("@life", MySqlDbType.Int32);
                cmd.Parameters.Add("@amount", MySqlDbType.UInt16);
                cmd.Parameters.Add("@amount_limit", MySqlDbType.UInt16);
                cmd.Parameters.Add("@magic_atk_max", MySqlDbType.Int16);
                cmd.Parameters.Add("@magic_def", MySqlDbType.Int16);
                cmd.Parameters.Add("@atk_range", MySqlDbType.Int16);
                cmd.Parameters.Add("@atk_speed", MySqlDbType.Int16);
                cmd.Parameters.Add("@hitrate", MySqlDbType.Int32);
                cmd.Parameters.Add("@target", MySqlDbType.Int32);
                cmd.Parameters.Add("@emoney", MySqlDbType.Int16);
                cmd.Parameters.Add("@official1", MySqlDbType.Int16);
                cmd.Parameters.Add("@official2", MySqlDbType.Int16);
                cmd.Parameters.Add("@official3", MySqlDbType.Int16);
                cmd.Parameters.Add("@official4", MySqlDbType.Int16);
                cmd.Parameters.Add("@official5", MySqlDbType.Int16);
                cmd.Parameters.Add("@official6", MySqlDbType.Int16);
                cmd.Parameters.Add("@official7", MySqlDbType.Int16);
                cmd.Parameters.Add("@official8", MySqlDbType.Int16);
                cmd.Parameters.Add("@official9", MySqlDbType.Int16);
                cmd.Parameters.Add("@official10", MySqlDbType.Int16);
                cmd.Parameters.Add("@official11", MySqlDbType.Int16);
                cmd.Parameters.Add("@id", MySqlDbType.Int32);

                foreach (DataRow row in dt.Rows)
                {
                    cmd.Parameters["@name"].Value = row["Name"].ToString();
                    cmd.Parameters["@req_profession"].Value = row["req_profession"];
                    cmd.Parameters["@req_level"].Value = row["req_level"];
                    cmd.Parameters["@req_sex"].Value = row["req_sex"];
                    cmd.Parameters["@req_force"].Value = row["req_force"];
                    cmd.Parameters["@req_dex"].Value = row["req_dex"];
                    cmd.Parameters["@req_health"].Value = row["req_health"];
                    cmd.Parameters["@req_soul"].Value = row["req_soul"];
                    cmd.Parameters["@monopoly"].Value = row["monopoly"];
                    cmd.Parameters["@price"].Value = row["price"];
                    cmd.Parameters["@id_action"].Value = row["id_action"];
                    cmd.Parameters["@attack_max"].Value = row["attack_max"];
                    cmd.Parameters["@attack_min"].Value = row["attack_min"];
                    cmd.Parameters["@defense"].Value = row["defense"];
                    cmd.Parameters["@magic_atk_min"].Value = row["magic_atk_min"];
                    cmd.Parameters["@dodge"].Value = row["dodge"];
                    cmd.Parameters["@life"].Value = row["life"];
                    cmd.Parameters["@amount"].Value = row["amount"];
                    cmd.Parameters["@amount_limit"].Value = row["amount_limit"];
                    cmd.Parameters["@magic_atk_max"].Value = row["magic_atk_max"];
                    cmd.Parameters["@magic_def"].Value = row["magic_def"];
                    cmd.Parameters["@atk_range"].Value = row["atk_range"];
                    cmd.Parameters["@atk_speed"].Value = row["atk_speed"];
                    cmd.Parameters["@hitrate"].Value = row["hitrate"];
                    cmd.Parameters["@target"].Value = row["target"];
                    cmd.Parameters["@emoney"].Value = row["emoney"];
                    cmd.Parameters["@official1"].Value = row["official1"];
                    cmd.Parameters["@official2"].Value = row["official2"];
                    cmd.Parameters["@official3"].Value = row["official3"];
                    cmd.Parameters["@official4"].Value = row["official4"];
                    cmd.Parameters["@official5"].Value = row["official5"];
                    cmd.Parameters["@official6"].Value = row["official6"];
                    cmd.Parameters["@official7"].Value = row["official7"];
                    cmd.Parameters["@official8"].Value = row["official8"];
                    cmd.Parameters["@official9"].Value = row["official9"];
                    cmd.Parameters["@official10"].Value = row["official10"];
                    cmd.Parameters["@official11"].Value = row["official11"];
                    cmd.Parameters["@id"].Value = row["ID"];

                    int affected = cmd.ExecuteNonQuery();
                    if (affected > 0) updated++;
                }
            }

            MessageBox.Show($"Update completed! {updated} item(s) updated.", "Done");
        }

    }
}
