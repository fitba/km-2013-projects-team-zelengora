<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserAdministration.aspx.cs" Inherits="KMWeb.Administration.UserAdministration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 286px;
        }
        .style2
        {
            width: 29px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%;">
        <tr>
            <td class="style1">
                Korisnici</td>
            <td class="style2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                <asp:GridView ID="gvKorisnici" runat="server" AutoGenerateSelectButton="True" 
                    DataKeyNames = "Id" AutoGenerateColumns="False" 
                    onselectedindexchanged="gvKorisnici_SelectedIndexChanged">
                   <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" Visible="true"/>
                        <asp:BoundField DataField="KorisnickoIme" HeaderText="KorisnickoIme" />   
                        <asp:BoundField DataField="Ime" HeaderText="Ime" /> 
                        <asp:BoundField DataField="Prezime" HeaderText="Prezime" /> 
                        <asp:BoundField DataField="Grupa" HeaderText="Grupa" /> 
                        <asp:BoundField DataField="IdGrupe" HeaderText="IdGrupa" />
                      </Columns>
                </asp:GridView>
            </td>
            <td class="style2">
                &nbsp;</td>
            <td valign="top">
                <table style="width:100%;">
                    <tr>
                        <td>
    <asp:Label ID="lblGrupaKorisnika" runat="server" Text="Grupa korisnika: "></asp:Label>
                        </td>
                        <td align="right">
                <asp:DropDownList ID="DropDownListKorisnici" runat="server" Height="20px" Width="138px">
                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                <asp:Label ID="Label3" runat="server" Text="Ime korisnika: "></asp:Label>
                        </td>
                        <td align="right">
                <asp:TextBox ID="txtImeKorisnika" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                <asp:Label ID="Label4" runat="server" Text="Prezime korisnika: "></asp:Label>
                        </td>
                        <td align="right">
                <asp:TextBox ID="txtPrezimeKorisnika" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                <asp:Label ID="Label5" runat="server" Text="Korisničko ime: "></asp:Label>
                        </td>
                        <td align="right">
                <asp:TextBox ID="txtKorisnickoIme" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="style1">
                <asp:TextBox ID="txtIdKorisnika" runat="server" Visible="False"></asp:TextBox>
            </td>
            <td class="style2">
                &nbsp;</td>
            <td>
                <asp:Button ID="txtSpremi" runat="server" onclick="txtSpremi_Click" 
                    Text="Spremi" />
            </td>
        </tr>
    </table>
</asp:Content>
