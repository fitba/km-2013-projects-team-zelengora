<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCategories.aspx.cs" Inherits="KMWeb.Administration.ManageCategories" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblNazivKategorije" runat="server" Text="Naziv kategorije :"></asp:Label>
<asp:TextBox ID="txtNazivKategorije" runat="server" Width="242px"></asp:TextBox>
<br />
<asp:Label ID="lblOpisKategorije" runat="server" Text="Opis kategorije :"></asp:Label>
<asp:TextBox ID="txtOpisKategorije" runat="server" Height="54px" 
    TextMode="MultiLine" Width="252px"></asp:TextBox>
<br />
<br />
<asp:Button ID="btnNovaKategorija" runat="server" 
    onclick="btnNovaKategorija_Click" Text="Kreiraj" />
<br />
</asp:Content>
