using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcesaArchivos.clases
{
    class Horarios : FileReader
    {
        IWorkbook archivo;

        static string[] headersA = {"NOMBRE DEL CURSO", "CLAVE", "GRUPO", "HORAS"};
        static string[] noEcom = {"No.ECON.", "NO.ECON.", "NO.ECON", "NOECON", "NOECON." };
        static string[] profesores = {"PROFESOR POSIBLE","PROFESOR", "PROFESOR (ES)" };
        static string[] headersB = {"LUNES", "MARTES", "MIÉRCOLES", "JUEVES", "VIERNES"};
        static string[] max = { "MAX.", "CUPO MAX.", "CUPO\nMAX." };

        public override List<ICell> getHeaders(int pos)
        {
            List<ICell> headers = new List<ICell>();
            bool flag = false;
            var sheet = getSheetAt(pos);

            var nombreHoja = sheet.SheetName;


            for (int i = sheet.FirstRowNum;i<sheet.LastRowNum;i++ )
            {
                var iR = sheet.GetRow(i);
                for(int j = iR.FirstCellNum; j<iR.LastCellNum;j++)
                {
                    ICell celda = iR.GetCell(j);
                    if (celda == null) continue;
                    if (celda.CellType == CellType.Blank) continue;

                    if (celda.CellType == CellType.String && (
                        headersA.Contains(celda.StringCellValue.Trim()) || 
                        profesores.Contains(celda.StringCellValue.Trim()) ||
                        noEcom.Contains(celda.StringCellValue.Trim().Replace(" ","")) ||
                        max.Contains(celda.StringCellValue.Trim())
                        ))
                    {
                        headers.Add(celda);
                    }
                    if (celda.CellType == CellType.String && (headersB.Contains(celda.StringCellValue.Trim() ) ) )
                    {
                        headers.Add(celda);
                        flag = true;
                    }
                }
                if (flag)
                    break;
            }

            return headers;
        }



        public List<Materia> readMaterias()
        {
            List<Materia> materias = new List<Materia>();

            for(int i = 0; i<getNumSheets(); i++)
            {
                List<ICell> headers = getHeaders(i);
                if (headers.Count == 12) //Faltan encabezados, no es hoja valida
                {
                    ICell pivote1 = headers[0];
                    var fila = pivote1.RowIndex;
                    var columna = pivote1.ColumnIndex;

                    ISheet hoja = getSheetAt(i);
                    while( ++fila< hoja.LastRowNum )
                    {
                        IRow fil = hoja.GetRow(fila);
                        if (fil == null) { continue; }
                        ICell actual = fil.GetCell(columna);
                        if (actual == null) continue;
                        if (actual.CellType == CellType.Blank || 
                            ( actual.CellType == CellType.String && headersA.Contains(actual.StringCellValue.Trim()) ) 
                            )
                        {
                            continue;
                        }

                        Materia m = new Materia();
                        m.nombre = actual.StringCellValue;
                        m.clave = getStringValue( hoja.GetRow(fila).GetCell(headers[1].ColumnIndex ) );
                        m.grupo = getStringValue(hoja.GetRow(fila).GetCell(headers[2].ColumnIndex));
                        m.cupo = getIntValue(hoja.GetRow(fila).GetCell(headers[6].ColumnIndex));
                        m.numeroHoras = getIntValue(hoja.GetRow(fila).GetCell(headers[5].ColumnIndex));

                        m.setHorario("LUNES", getStringValue(hoja.GetRow(fila).GetCell(headers[7].ColumnIndex)));
                        m.setHorario("MARTES", getStringValue(hoja.GetRow(fila).GetCell(headers[8].ColumnIndex)));
                        m.setHorario("MIERCOLES", getStringValue(hoja.GetRow(fila).GetCell(headers[9].ColumnIndex)));
                        m.setHorario("JUEVES", getStringValue(hoja.GetRow(fila).GetCell(headers[10].ColumnIndex)));
                        m.setHorario("VIERNES", getStringValue(hoja.GetRow(fila).GetCell(headers[11].ColumnIndex)));

                        materias.Add(m);
                    }
                }
            }
            return materias;
        }

    }
}
