<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArticleStatistic.aspx.cs" Inherits="KMWeb.ArticleStatistic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%;">
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" 
                    Text="Zadnji clanci iz omiljenih kategorija:"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" 
                    Text="Najbolje ocijenjeni clanci iz om. kategorija:"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" 
                    Text="Clanci koje su pregledali drugi korisnici iz iste grupe"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
