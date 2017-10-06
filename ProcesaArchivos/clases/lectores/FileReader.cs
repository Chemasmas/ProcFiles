using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcesaArchivos.clases
{
    abstract class FileReader
    {
        IWorkbook archivo;

        public abstract List<ICell> getHeaders(int pos);

        public IWorkbook readFile(string path)
        {
                archivo = WorkbookFactory.Create(path);
                return archivo;
        }

        public int getNumSheets()
        {
            return archivo.NumberOfSheets;
        }

        public ISheet getSheetAt(int sheet)
        {
            if (sheet > getNumSheets() || sheet < 0)
                throw new IndexOutOfRangeException("No existe ese numero de hojas");
            return archivo.GetSheetAt(sheet);
        }

        public string getStringValue(ICell cell)
        {
            if (cell == null) return "";
            if (cell.CellType == CellType.String)
                return cell.StringCellValue.Replace("\n","-").Replace(",","-");
            else if (cell.CellType == CellType.Numeric)
                return System.Convert.ToString(cell.NumericCellValue).Replace("\n", "-").Replace(",", "-");
            else
                return ""; //CAso de error
        }

        public int getIntValue(ICell cell)
        {
            if (cell == null) return -1;
            if (cell.CellType == CellType.Numeric)
                return System.Convert.ToInt32(cell.NumericCellValue);
            else if (cell.CellType == CellType.Numeric)
                return System.Convert.ToInt32(cell.StringCellValue);
            else
                return -1; //Caso de error
        }
    }
}
