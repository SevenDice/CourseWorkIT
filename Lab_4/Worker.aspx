<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Worker.aspx.cs" Inherits="Lab_4.Worker" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Добавление коммивояжера<br />
        <br />
        Фамилия<asp:TextBox ID="Surname" runat="server" style="margin-left: 50px"></asp:TextBox>
        <br />
        Имя<asp:TextBox ID="Name" runat="server" style="margin-left: 85px"></asp:TextBox>
        <br />
        Отчество<asp:TextBox ID="TName" runat="server" style="margin-left: 48px"></asp:TextBox>
        <br />
        <br />
        Пол<asp:RadioButtonList ID="Gen" runat="server" OnSelectedIndexChanged="Gender_SelectedIndexChanged">
            <asp:ListItem>Мужской</asp:ListItem>
            <asp:ListItem>Женский</asp:ListItem>
        </asp:RadioButtonList>
        <br />
        <br />
        <br />
    
    </div>
        Дата рождения <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="104px" NextPrevFormat="FullMonth" OnSelectionChanged="Calendar1_SelectionChanged" style="margin-left: 120px" Width="223px">
            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
            <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
            <OtherMonthDayStyle ForeColor="#999999" />
            <SelectedDayStyle BackColor="#333399" ForeColor="White" />
            <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
            <TodayDayStyle BackColor="#CCCCCC" />
        </asp:Calendar>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Добавить" />
        <br />
        <br />
    </form>
</body>
</html>
