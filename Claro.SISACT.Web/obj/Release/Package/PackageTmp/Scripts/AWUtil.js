function setScrollGrid(xgrid)
{
   // alert();
    var tmpH=xgrid.getRowHeight(0);
	var tmpT=xgrid.getRowCount()*tmpH;
	tmpT+=xgrid.getFooterHeight();
	tmpT+=xgrid.getHeaderHeight();
	xgrid.setScrollHeight(tmpT);
}

var r_TextRangeGrid
function SelectCellText(grid)
{
	var row = grid.getCurrentRow();
	var col = grid.getCurrentColumn();
	var e = grid.getCellTemplate(col, row).getContent("box/text").element();
	var n_text = grid.getCellTemplate(col, row).getContent("box/text").element();
	r_TextRangeGrid = document.body.createTextRange();
	r_TextRangeGrid.moveToElementText(e);
	setTimeout("eval('r_TextRangeGrid.select()')", 1);
}

//var grilla_Global;
function SetNextCell2(grid, col) {
    grid.setCurrentColumn(col);
    grid.setSelectedColumns([col]);
    //var row = grid.getCurrentRow();
    //grilla_Global = grid;
    //setTimeout("eval('grilla_Global.getCellTemplate(" + col + "," + row + ").element().focus()')", 10)
    //grid.getCellTemplate(col, row).element().focus();
}

function SetNextCell(grid, col)
{								
	grid.setCurrentColumn(col); 
	grid.setSelectedColumns([col]);		
	grid.raiseEvent("editCurrentCell");		
	SelectCellText(grid);
}
function SetPreviusRow(grid)
{
	// Al presionar enter establece la fila que esta debajo de la 
	// actual como la nueva fila activa y entra e modo de edición.
	// Para ser llamado desde el evento: 
	// obj.onCellEditEnded = function(text, column, row){ };
				
	var row = grid.getCurrentRow();	
	
	var value = grid.getRowIndices();
	var nextIndex = 0; 
																																	
	// Si las filas no se han ordenado.
	if (value.length == 0)
	{														
		nextIndex = --row;																																																																																							
	}
	else
	{														
		for (var i = 0; i < value.length; i++)
		{
			if (value[i] == row)
			{
				nextIndex = value[i - 1];
				break;															
			}																																																																	
		}																																																																																																		
	}																																																
																																			
	grid.setCurrentRow(nextIndex);
	grid.setSelectedRows([nextIndex]);	
	
	// Es necesario en algunos casos para que se habilite el modo de edición, pero solo
	// cuando se presiona enter para no alterar la seleción de filas al hacer click.	
	//if (setEditMode)
	//{
	//	if (window.event.keyCode == 13) grid.raiseEvent("editCurrentCell");				
	//}
}
function SetNextRow(grid)
{	
	SetNextRow(grid, false);
}

function SetNextRow(grid, setEditMode)
{
	// Al presionar enter establece la fila que esta debajo de la 
	// actual como la nueva fila activa y entra e modo de edición.
	// Para ser llamado desde el evento: 
	// obj.onCellEditEnded = function(text, column, row){ };
				
	var row = grid.getCurrentRow();	
	
	var value = grid.getRowIndices();
	var nextIndex = 0; 
																																	
	// Si las filas no se han ordenado.
	if (value.length == 0)
	{														
		nextIndex = ++row;																																																																																							
	}
	else
	{														
		for (var i = 0; i < value.length; i++)
		{
			if (value[i] == row)
			{
				nextIndex = value[i + 1];
				break;															
			}																																																																	
		}																																																																																																		
	}																																																
																																			
	grid.setCurrentRow(nextIndex);
	grid.setSelectedRows([nextIndex]);	
	
	// Es necesario en algunos casos para que se habilite el modo de edición, pero solo
	// cuando se presiona enter para no alterar la seleción de filas al hacer click.	
	if (setEditMode)
	{
		if (window.event.keyCode == 13) grid.raiseEvent("editCurrentCell");				
	}
}

/*
	Version 1.0

	objBin.setAction("click", function(src) { 
		var currRow = objBin.getSelectionProperty("index");																		
		var nextRow = src.getRowProperty("index");
	
		...
	}
*/
/*Arreglar problema de sort en formato currency ($ 0.00)*/
function SetGridSortCurrent_Util(objGrid_Param,ColumnIndex_List)
{
    for(var col=0;col<ColumnIndex_List.length;col++)
    {
        var CurCol=ColumnIndex_List[col];
        objGrid_Param.setCellValue(function(CurCol, row){ 
           return parseFloat(atixReplace(atixRemoveCommas(this.getCellText(CurCol,row)),'$',''));//parseFloat(obj[row][col].replace(/[^0-9.]/m, "")); 
        }, col); 
    }
}
/*Asigna el formato numerico para las columnas pasadas como arreglo*/
function SetGridFormatNumeric_Util(objGrid_Param,ColumnIndex_List)
{
    var numberFormat = new AW.Formats.Number;	
    for(var col=0;col<ColumnIndex_List.length;col++)
    {  
        objGrid_Param.setCellFormat(numberFormat,ColumnIndex_List[col]);	
    }
}
/*Define y evalua el formato "red" cuando el valor es menor a cero*/
function SetGridFormatNegative_Util(objGrid_Param,ColumnIndex_List,strColor)
{
    objGrid_Param.defineCellProperty
	("colorNegative", function(col, row)
	{ 			
        var tmpVal=parseFloat(atixReplace(atixRemoveCommas(this.getCellText(col,row)),"$",""));
        if(tmpVal<0) return strColor;			       																			
	}
	); 
	for(var col=0;col<ColumnIndex_List.length;col++)
    {  
        var CurCol=ColumnIndex_List[col];
	    objGrid_Param.getCellTemplate(CurCol).setStyle
	    ("color", function()
	        { 
		        return this.getControlProperty("colorNegative"); 
	        }
	    ); 
	}
}
/*Valida que haya seleccionado al menos un item del la grilla (chk)*/
function Validate_ChkGrid_Util(objGrid,ColChk,msg)
{
    for(var row=0;row<objGrid.getRowCount();row++)
    {
        if(objGrid.getCellValue(ColChk,row)) return false;
    }
    alert(msg);
    return true;
} 
 
