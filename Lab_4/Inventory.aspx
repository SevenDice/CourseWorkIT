﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="Lab_4.Inventory1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Инвентарь</title>
    <style>
        .floatableDiv:after {
            content: "";
            display: table;
            clear: both;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
        <div>
            <h1 style="font-family: Impact; background-color: greenyellow; text-align: center;">Просмотр инвентаря предприятия</h1>
            <span style="display: block; margin: 10px"><a href="/Main.aspx">Назад</a></span>
            <div>
                <asp:GridView HorizontalAlign="Center"  ID="InventoryTable" runat="server" DataKeyNames="IdI" AutoGenerateColumns="False" AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" AllowSorting="True" OnSorting="InventoryTable_Sorting">
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <AlternatingRowStyle BackColor="White" />
                    <HeaderStyle BackColor="#28a4fb" Font-Bold="True" ForeColor="White" CssClass="Header" />
                    <Columns>
                        <asp:BoundField DataField="IdI" HeaderText="IdI" InsertVisible="False" ReadOnly="True" SortExpression="IdI" Visible="False" />
                        <asp:BoundField DataField="IName" HeaderText="Наименование" SortExpression="IName" />
                        <asp:BoundField DataField="Count" HeaderText="Количество" SortExpression="Count" />
                        <asp:BoundField DataField="Price" HeaderText="Цена за единицу" SortExpression="Price" />
                        <asp:BoundField DataField="MPrice" HeaderText="Стоимость обслуживания" SortExpression="MPrice" />
                    </Columns>
                </asp:GridView>
                
                <div class="floatableDiv" style="margin: 15px auto 10px; width: 800px">
                    <div style="float: left; margin-right: 15px; border: 1px black dashed;">
                        <table>
                        <tr>
                            <td>Наименование:</td>
                            <td>
                                <asp:TextBox ID="TextBox5" runat="server" Width="160px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Количество:</td>
                            <td>
                                <asp:TextBox ID="TextBox2" runat="server" Width="160px"></asp:TextBox></td>
                        </tr>
                        <tr> 
                            <td>Цена за eдиницу:</td>
                            <td>
                                <asp:TextBox ID="TextBox3" runat="server" Width="160px"></asp:TextBox></td>
                        </tr>
                           <tr>
                            <td>Стоимость обслуживания:</td>
                            <td>
                                <asp:TextBox ID="TextBox4" runat="server" Width="160px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center">
                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Добавить" /></td>
                        </tr>

                    </table>
                    </div>
                    <div style="float: left; border: 1px black dashed;">
                        <table>
                            <tr>
                                <td>Стоимость обслуживания всего инвентаря за месяц:</td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server" Width="160px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center">
                                    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Рассчитать" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
