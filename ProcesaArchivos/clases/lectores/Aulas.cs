using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;

namespace ProcesaArchivos.clases
{
    class Aulas : FileReader
    {

        static string[] headersA = { "Piso", "No. en plano","Nombre", "Capacidad" };

        public override List<ICell> getHeaders(int pos)
        {
            List<ICell> headers = new List<ICell>();
            bool flag = false;
            var sheet = getSheetAt(pos);
            for (int i = sheet.FirstRowNum; i < sheet.LastRowNum; i++)
            {
                var iR = sheet.GetRow(i);
                if (iR == null) continue;
                for (int j = iR.FirstCellNum; j < iR.LastCellNum; j++)
                {
                    ICell celda = iR.GetCell(j);
                    if (celda == null || celda.CellType == CellType.Blank) continue;
                    if (celda.CellType == CellType.String && (
                        headersA.Contains(celda.StringCellValue.Trim())
                        ))
                    {
                        headers.Add(celda);
                        flag = true;
                    }
                    /*
                    if (celda.CellType == CellType.String && headersB.Contains(celda.StringCellValue.Trim()))
                    {
                        headers.Add(celda);
                        flag = true;
                    }
                    */
                }
                if (flag)
                    break;
            }

            return headers;
        }

        public List<Salon> readAulas()
        {
            List<Salon> salones = new List<Salon>();
            //HashSet<Salon> salones = new HashSet<Salon>(new Salon());
            
            for (int i = 0; i < getNumSheets(); i++)
            {
                List<ICell> headers = getHeaders(i);
                //headers[0] // Piso
                //headers[1] // No en plano
                //headers[2] // Nombre
                //headers[3] // Capacidad
                if (headers.Count == 4)
                {
                    ICell pivote1 = headers[0];
                    var fila = pivote1.RowIndex;
                    var columna = pivote1.ColumnIndex;

                    ISheet hoja = getSheetAt(i);
                    while (++fila < hoja.LastRowNum)
                    {
                        IRow fil = hoja.GetRow(fila);
                        if (fil == null) { continue; }
                        ICell actual = fil.GetCell(columna);
                        if (actual == null) continue;
                        if (actual.CellType == CellType.Blank ||
                            (actual.CellType == CellType.String && headersA.Contains(actual.StringCellValue.Trim()))
                            )
                        {
                            continue;
                        }

                        Salon s1 = new Salon();
                        s1.nombre = getStringValue(hoja.GetRow(fila).GetCell(headers[2].ColumnIndex)).Trim();
                        s1.capacidad = getIntValue(hoja.GetRow(fila).GetCell(headers[3].ColumnIndex));


                        //Salon s2 = new Salon();
                        //s2.nombre = getStringValue(hoja.GetRow(fila).GetCell(headers[1].ColumnIndex)).Trim();
                        //s2.capacidad = getIntValue(hoja.GetRow(fila).GetCell(headers[2].ColumnIndex));

                        salones.Add(s1);
                        //salones.Add(s2);
                    }

                    break;
                }
                       
            }
            return salones;
        }
    }
}
