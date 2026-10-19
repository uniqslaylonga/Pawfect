using MyApp.Controls;

namespace MyApp;

/// <summary>A read-only table in the app's popup style.</summary>
internal sealed class ListDialog : ThemedDialog
{
    public ListDialog(string title, string subtitle, string[] columns, int[] widths, IEnumerable<string[]> rows)
        : base(title, subtitle, 640)
    {
        var lv = new ListView
        {
            View = View.Details,
            FullRowSelect = true,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
            Font = Theme.Face(9f),
            BorderStyle = BorderStyle.FixedSingle,
        };
        for (int i = 0; i < columns.Length; i++)
            lv.Columns.Add(columns[i], widths[i]);

        int n = 0;
        foreach (var r in rows) { lv.Items.Add(new ListViewItem(r)); n++; }
        if (n == 0) lv.Items.Add(new ListViewItem("Nothing to show."));

        AddCustom(null, lv, 340);
        AddButtons("Close", (_, _) => DialogResult = DialogResult.OK);
    }
}