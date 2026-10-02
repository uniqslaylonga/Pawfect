using System.Globalization;
using System.Net;
using System.Text;

namespace Pawfect.Services;

public record ExportSection(string Heading, string[] Headers, List<string[]> Rows);

// Builds CSV text (opens in Excel) and a print-friendly HTML page (use "Save as PDF" in the print dialog).
// No extra NuGet packages needed.
public static class ExportHelper
{
    public static string Csv(string[] headers, IEnumerable<string[]> rows)
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", headers.Select(Quote)));
        foreach (var r in rows) sb.AppendLine(string.Join(",", r.Select(Quote)));
        return sb.ToString();
    }

    private static string Quote(string? v)
    {
        v ??= "";
        // stop spreadsheet formula injection from user-entered text (plain negative numbers are fine)
        if (v.Length > 0 && !decimal.TryParse(v, NumberStyles.Number, CultureInfo.InvariantCulture, out _) && (v[0] == '=' || v[0] == '@' || v[0] == '+' || v[0] == '-'))
            v = "'" + v;
        return "\"" + v.Replace("\"", "\"\"") + "\"";
    }

    public static string Html(string title, string subtitle, IEnumerable<ExportSection> sections)
    {
        static string E(string? s) => WebUtility.HtmlEncode(s ?? "");
        var sb = new StringBuilder();
        sb.Append("<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>").Append(E(title)).Append("</title><style>")
          .Append("body{font-family:Segoe UI,Arial,sans-serif;color:#1F3350;margin:28px;font-size:12px}")
          .Append("h1{font-size:20px;margin:0}.sub{color:#6B7A90;margin:4px 0 18px}")
          .Append("h2{font-size:14px;margin:22px 0 8px}")
          .Append("table{width:100%;border-collapse:collapse;margin-bottom:6px}")
          .Append("th{background:#F0F4FA;text-align:left;padding:6px 8px;border:1px solid #D5DFEC;font-size:11px}")
          .Append("td{padding:5px 8px;border:1px solid #E3EAF4;vertical-align:top}")
          .Append("tr{page-break-inside:avoid}.empty{color:#8A97AA;text-align:center}")
          .Append("@media print{body{margin:12px}}")
          .Append("</style></head><body>");
        sb.Append("<h1>").Append(E(title)).Append("</h1><div class=\"sub\">").Append(E(subtitle)).Append("</div>");
        foreach (var s in sections)
        {
            sb.Append("<h2>").Append(E(s.Heading)).Append("</h2><table><thead><tr>");
            foreach (var h in s.Headers) sb.Append("<th>").Append(E(h)).Append("</th>");
            sb.Append("</tr></thead><tbody>");
            if (s.Rows.Count == 0)
                sb.Append("<tr><td class=\"empty\" colspan=\"").Append(s.Headers.Length).Append("\">No records</td></tr>");
            foreach (var r in s.Rows)
            {
                sb.Append("<tr>");
                foreach (var c in r) sb.Append("<td>").Append(E(c)).Append("</td>");
                sb.Append("</tr>");
            }
            sb.Append("</tbody></table>");
        }
        sb.Append("</body></html>");
        return sb.ToString();
    }
}
