﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdministratorHome.aspx.cs" Inherits="KMWeb.Administration.AdministratorHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="Label2" runat="server" 
        Text="Prijedlozi za reviziju clanaka. Odabrati prijedlog."></asp:Label>
    <br />
    <asp:GridView ID="gvPrijedloziRevizije" runat="server" 
    AutoGenerateSelectButton="True" 
    onselectedindexchanged="gvPrijedloziRevizije_SelectedIndexChanged" 
        PageSize="6">
</asp:GridView>
<br/><br/>
    <asp:Label ID="Label3" runat="server" 
        Text="Prijedlozi za reviziju pitanja. Odabrati prijedlog."></asp:Label>
    <br/><br/>
<asp:GridView ID="gvQAPrijedloziRevizije" runat="server" 
    AutoGenerateSelectButton="True" 
        PageSize="6" 
        onselectedindexchanged="gvQAPrijedloziRevizije_SelectedIndexChanged">
</asp:GridView>
</asp:Content>
