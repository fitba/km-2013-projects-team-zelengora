<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewQuestion.aspx.cs" Inherits="KMWeb.QA.NewQuestion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblNaslov" runat="server" Text="Naslov:  "></asp:Label>
    <asp:TextBox ID="txtNaslovPitanja" runat="server" Width="398px"></asp:TextBox>
    <br />
    <br />
    <asp:TextBox ID="txtPitanjeold" runat="server" Height="51px" TextMode="MultiLine" 
        Width="449px" Visible="false"></asp:TextBox>
     <CKEditor:CKEditorControl ID="txtPitanje" runat="server"></CKEditor:CKEditorControl>
    <br />
    <br />
    <asp:Label ID="Label6" runat="server" Text="Ključne riječi"></asp:Label>
    <br />
    <asp:TextBox ID="txtKljucnaRijec1" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec2" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec3" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec4" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtKljucnaRijec5" runat="server"></asp:TextBox>
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Postavi pitanje" 
    onclick="Button1_Click" />
    <br />
    <br />
</asp:Content>
