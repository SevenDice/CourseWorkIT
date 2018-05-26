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
            <div>
                
                <div runat="server" ID="LoginForm" style="border: 1px black dashed; width: 300px; margin: 10px auto 10px">
                    <table style="width: 100%">
                        <tr>
                            <td>Логин:</td>
                            <td><asp:TextBox ID="LoginTbx" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Пароль:</td>
                            <td><asp:TextBox ID="PwdTbx" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center"><asp:Button ID="Button6" runat="server" Text="Войти" OnClick="Button6_Click" /></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="color: red; text-align: center"><asp:Label ID="Label2" runat="server" Text="Неверные значения логина и пароля"></asp:Label></td>
                        </tr>
                    </table>
                </div>
                
                <div style="width: 300px; margin: 0 auto 10px">
                    <asp:Button ID="Button1" runat="server" PostBackUrl="/Workers.aspx" Text="Просмотреть работников" Width="100%" />
                    <asp:Button ID="Button2" runat="server" PostBackUrl="/Inventory.aspx" Text="Просмотреть инвентарь" Width="100%" />
                    <asp:Button ID="Button3" runat="server" PostBackUrl="/Places.aspx" Text="Просмотреть рабочие отделы" Width="100%" />
                    <asp:Button ID="Button4" runat="server" PostBackUrl="/Cultures.aspx" Text="Просмотреть выращиваемые культуры" Width="100%" />
                    <asp:Button ID="Button5" runat="server" PostBackUrl="/Animals.aspx" Text="Просмотреть разводимые породы животных" Width="100%" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
