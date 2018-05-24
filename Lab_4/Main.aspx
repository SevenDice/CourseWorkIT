<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Lab_4.Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Сельхозпредприятие</title>
    <style type="text/css">
        .auto-style1 {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1 style="font-family: Impact; background-color: greenyellow; text-align: center;">Система управления деятельностью сельхозпредприятия</h1>
            <div class="auto-style1">
                <div class="auto-style1">
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="Логин"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Пароль<br />
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    <br />
                <asp:Button ID="Button6" runat="server" Text="Войти" Width="254px" OnClick="Button6_Click" style="text-align: center; margin-bottom: 0px" />
                    <br />
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="Неверные значения логина и пароля"></asp:Label>
                    <br />
                    <br />
                </div>
            <div class="auto-style1">
                <asp:Button ID="Button1" runat="server" PostBackUrl="/Workers.aspx" Text="Просмотреть работников" Width="345px" />
                <br />
            </div>
            <div class="auto-style1">
                <asp:Button ID="Button2" runat="server" PostBackUrl="/Inventory.aspx" Text="Просмотреть инвентарь" Width="345px" />
                <br />
            </div>
            <div class="auto-style1">
                <asp:Button ID="Button3" runat="server" PostBackUrl="/Places.aspx" Text="Просмотреть рабочие отделы" Width="345px" />
                <br />
            </div>
            <div class="auto-style1">
                <asp:Button ID="Button4" runat="server" PostBackUrl="/Cultures.aspx" Text="Просмотреть выращиваемые культуры" Width="345px" />
                <br />
            </div>
            <div style="text-align: center">
                <asp:Button ID="Button5" runat="server" PostBackUrl="/Animals.aspx" Text="Просмотреть разводимые породы животных" Width="345px" style="text-align: center; margin-bottom: 0px" />
                <br />
                <br />
            </div>
                <br />
            </div>
        </div>
    </form>
</body>
</html>
