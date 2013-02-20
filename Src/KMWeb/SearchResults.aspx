<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchResults.aspx.cs" Inherits="KMWeb.SearchResults" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="Label2" runat="server" Text="Rezultati pretrage za : "></asp:Label>
    <asp:Label ID="txtSearchWord" runat="server" Text="-"></asp:Label>
    <br />
    <asp:Label ID="lblNotFound" runat="server" 
        Text="Nema rezultata za vašu pretragu" Visible="False"></asp:Label>
    <br />
    <br />
    <asp:Panel id="MyPanel" runat="server" Visible="true" />

</asp:Content>
