# ItemtypeDatEditorEO

**ItemtypeDatEditorEO** is a modern Windows tool for viewing, editing, exporting, and syncing `itemtype.dat` files from Eudemons Online private server with MySQL databases. Designed for power users, private server admins, and database managers, it offers batch editing, import/export, and MySQL integration in one package.

---

## 🚀 Features

- **Open & Edit `itemtype.dat`:**
  - Load and display all item records in a sortable, searchable table.
  - Add, edit, delete, copy, paste rows with ease.
  - All columns are mapped to database fields for direct data syncing.
  - Handles `req_sex` and `req_level` (auto converts 256/512 to male/female).

- **Search & Filter:**
  - Search by any field or select a specific field (combo box).
  - Instant row navigation with looping search (press search multiple times to go to next match).

- **Import/Export:**
  - Export all or selected rows to CSV (compatible with Excel/Notepad).
  - Safe CSV export (handles commas and quotes inside descriptions).
  - Import from CSV (skips duplicates based on `ID`).
  - Automatically checks for duplicate IDs on import/save.

- **Context Menu (Right-click):**
  - Add row (`Ctrl+N`), copy (`Ctrl+C`), paste (`Ctrl+V`), export selected (`Ctrl+E`), delete selected (`Del`).
  - Multi-row select and operation supported.

- **Live Status:**
  - Shows total items loaded, number of selected rows, status after load/save/export/import.
  - Error and success messages shown in the status bar.

- **Auto-clean Row:**
  - When adding a new row, if the row is left empty and focus moves away, it auto-deletes to prevent invalid data.

- **Sorting:**
  - Click any column to sort ascending/descending.
  - Save/export uses current sort order.

- **MySQL Integration:**
  - Connects to MySQL (v4/v5) with configurable host, port, user, password, and database.
  - **Add All:** Adds all new items (by ID) that do not exist in the database.
  - **Update Name:** Updates all item names for existing IDs.
  - **Update All:** Updates all fields for existing IDs only (confirmation required).
  - All actions are safe and skip duplicates where required.

- **User Friendly:**
  - All major functions (load, save, export, import, add, update) are accessible with one click.
  - Keyboard shortcuts for common tasks.

---

## 🖥️ Requirements

- .NET Framework 4.7.2 or higher (or .NET 6+ for modern Visual Studio)
- MySql.Data.dll (add as reference, required for MySQL features)
- Windows 7/8/10/11

---

## 📦 How to Use

1. **Load itemtype.dat**  
   Click "Load DAT" and select your `itemtype.dat` file. All data will be loaded in a grid.

2. **Edit Data**  
   Double-click any cell to edit. Right-click for context menu options.

3. **Search**  
   Enter keyword, choose a field, and press Search. Repeat to jump to the next match.

4. **Add/Delete/Copy/Paste Rows**  
   Use right-click or keyboard shortcuts (`Ctrl+N`, `Ctrl+C`, `Ctrl+V`, `Del`).

5. **Export/Import CSV**  
   Use Export/Import buttons for backup or batch edit in Excel.

6. **MySQL Connect**  
   Fill in connection info (host, port, user, pass, db) and click Connect.  
   - Once connected, you can Add All, Update Name, or Update All.

7. **Save to .dat**  
   After edits, click Save. Duplicates by `ID` are auto-removed on save.

---

## ⚠️ Notes

- **All ID must be unique!** The editor will remove duplicates when saving.
- **Descriptions** are not synced to MySQL (database does not have this column).
- **Data safety:** Always backup your files and database before using batch import/export/update.
- **No .NET installer provided:** Just build and run, all dependencies are open source.

---

## 📚 Shortcuts

| Action              | Shortcut      |
|---------------------|--------------|
| Add Row             | Ctrl+N       |
| Copy                | Ctrl+C       |
| Paste               | Ctrl+V       |
| Export Selected     | Ctrl+E       |
| Delete Selected     | Del          |

---

---

## 📬 Feedback & Issues

Report bugs or feature requests via GitHub Issues or contact me at [Facebook]([https://facebook.com/duaselipar](https://www.facebook.com/profile.php?id=61554036273018)).

---

Enjoy making EO private server management easier!  
