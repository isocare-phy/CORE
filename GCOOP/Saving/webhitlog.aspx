<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="webhitlog.aspx.cs"
    Inherits="Saving.webhitlog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" name="form1">
    <div id="output">
    </div>
    <input type="text" name="hgcallback" id="hgcallback" value=""  />
    <input type="text" name="hgws" id="hgws" value="0"  />
    <input type="text" name="hgcururl" id="hgcururl" value="0"  />
    <input type="text" name="hghtmldata" id="hghtmldata" value=""  />
    <input type="text" name="hgfilename" id="hgfilename" value="" />
    <input type="text" name="hghit_date" id="hghit_date" value=""  />
    <input type="text" name="hgsession_id" id="hgsession_id" value=""  />
    <input type="text" name="hgusername" id="hgusername" value=""  />
    <input type="submit" name="hgsaveBtn" id="hgsaveBtn" value="Save"  />
    <div>
    </div>
    </form>
</body>
</html>
