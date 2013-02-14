<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewArticle.aspx.cs" Inherits="KMWeb.Administration.NewArticle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    <asp:Label ID="Label1" runat="server" Text="Unos novog clanka"></asp:Label>
</p>
<p>
    <asp:Label ID="Label2" runat="server" Text="Naslov:"></asp:Label>
    <asp:TextBox ID="txtNaslov" runat="server" MaxLength="50" Width="395px"></asp:TextBox>
</p>
<p>
    <asp:Label ID="Label3" runat="server" Text="Sadržaj:"></asp:Label>
    <asp:TextBox ID="txtSadrzaj" runat="server" Height="180px" MaxLength="5000" 
        TextMode="MultiLine" Width="395px"></asp:TextBox>
</p>
<p>
    <asp:Label ID="Label4" runat="server" Text="Kategorija:"></asp:Label>
    <asp:DropDownList ID="DropDownCategory" runat="server" 
        DataTextField="NazivKategorije" DataValueField="Id" Height="19px" 
        onload="DropDownList1_Load" 
        onselectedindexchanged="DropDownCategory_SelectedIndexChanged" Width="151px">
    </asp:DropDownList>
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click1" 
        style="height: 26px" Text="Dopuni kategorije" />
</p>
<p>
    <asp:Label ID="Label5" runat="server" Text="Ključne riječi:"></asp:Label>
</p>
    <p>
    <asp:TextBox ID="txtKljucnaRijec1" runat="server" Width="108px"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec2" runat="server" Width="108px"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec3" runat="server" Width="108px"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec4" runat="server" Width="108px"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec5" runat="server" Width="108px"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec6" runat="server" Width="108px"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec7" runat="server" Width="108px"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec8" runat="server" Width="108px"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec9" runat="server" Width="108px"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec10" runat="server" Width="108px"></asp:TextBox>
        <asp:Button ID="btnKljucneRijeci" runat="server" onclick="Button3_Click1" 
            Text="UnesiKLjucneRijeci" />
</p>
<p>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Spremi" />
</p>
</asp:Content>
