$(document).keydown(function (event) {
    if (event.keyCode == 123) { // Prevent F12
        return false;
    } else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) { // Prevent Ctrl+Shift+I        
        return false;
    } else if (event.ctrlKey && event.shiftKey && event.keyCode == 74) { // Prevent Ctrl+Shift+J        
        return false;
    } else if (event.ctrlKey && event.keyCode == 85) { // Prevent Ctrl+U        
        return false;
    } else if (event.ctrlKey && event.keyCode == 78) { // Prevent Ctrl+N       
        return false;
    }
});

$(document).on("contextmenu", function (e) {
    e.preventDefault();
});

function checkKeyCode(evt)
{
	var evt = (evt) ? evt : ((event) ? event : null);
	var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
	if(event.keyCode==116) {
		evt.keyCode=0;
		return false
	}
}
document.onkeydown=checkKeyCode;