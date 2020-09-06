using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ClosedXML.Excel;
using JiraWorklogsApp.Common.Attributes;
using JiraWorklogsApp.Common.Extensions;

namespace JiraWorklogsApp.Common.Helpers
{
    public static class ExcelHelper
    {
        internal static XLWorkbook CollectionToWorkbook<T>(IEnumerable<T> collection, string workSheetName)
        {
            XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add(workSheetName);

            var props = GetExcelProperties(typeof(T)).ToList();

            var list = collection.ToList();

            foreach (var pi in props)
            {
                ExportToExcelAttribute attribute = pi.Value.GetCustomAttribute<ExportToExcelAttribute>();

                var headerCell = worksheet.Cell(1, attribute.Column + 1).SetValue(attribute.Title);
                headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                headerCell.Style.Font.Bold = true;

                for (int i = 0; i < list.Count; i++)
                {
                    T game = list[i];

                    var propValue = game.GetPropValue(pi.Key);
                    worksheet.Cell(i + 2, attribute.Column + 1).SetValue(propValue);
                }
            }

            return workbook;
        }

        private static Dictionary<string, PropertyInfo> GetExcelProperties(Type t, string parent = null, Dictionary<string, PropertyInfo> pis = null)
        {
            if (pis == null)
            {
                pis = new Dictionary<string, PropertyInfo>();
            }

            foreach (var prp in t.GetProperties(BindingFlags.Public|BindingFlags.Instance))
            {
                if (Attribute.IsDefined(prp, typeof(ExportToExcelAttribute)))
                {
                    if (String.IsNullOrWhiteSpace(parent))
                    {
                        pis.Add(prp.Name, prp); 
                    }
                    else
                    {
                        pis.Add(parent + "." + prp.Name, prp); 
                    }
                }
                if (!prp.PropertyType.IsPrimitive && prp.PropertyType != typeof(string) && prp.PropertyType.IsClass)
                {
                    GetExcelProperties(prp.PropertyType, prp.Name, pis);
                }
            }

            return pis;
        }

        public static byte[] GetExportFile<T>(IEnumerable<T> collection, string workSheetName)
        {
            byte[] content;

            XLWorkbook workbook = CollectionToWorkbook(collection, workSheetName);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                content = memoryStream.ToArray();
                memoryStream.Close();
            }
            return content;
        }
    }
}
