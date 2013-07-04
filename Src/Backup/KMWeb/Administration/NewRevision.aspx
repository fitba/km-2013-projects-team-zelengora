<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewRevision.aspx.cs" Inherits="KMWeb.Administration.NewRevision" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%;">
    <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Originalni naslov"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtNaslov" runat="server" Enabled="False" Width="318px"></asp:TextBox>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="Prijedlog izmjene naslova"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPrijedlogNaslova" runat="server" Width="318px"></asp:TextBox>
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
            <asp:Label ID="Label4" runat="server" Text="Originalni sadržaj"></asp:Label>
        </td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            <asp:TextBox ID="txtOriginialSadrzaj" runat="server" Enabled="False" 
                Height="313px" TextMode="MultiLine" Width="481px" Visible="false"></asp:TextBox>
                <asp:Repeater ID="Repeater1" runat="server" >
   <ItemTemplate>
    <div id="naslov" style="float:left; margin-right:20px; background-color:Gray; font-size:large; color:White; width:95% " >
         <%# Eval("Naslov")%>
         </div>
         <div id="sadrzaj" style="float:left; margin-right:20px" >
         <%# Eval("Sadrzaj")%>
         </div>
   </ItemTemplate>
   </asp:Repeater>
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
            <asp:Label ID="Label5" runat="server" Text="Prijedlog izmjene sadržaja"></asp:Label>
        </td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            <asp:TextBox ID="txtPrijedlogSadrzajaOld" runat="server" Height="313px" 
                TextMode="MultiLine" Width="481px" Visible="false"></asp:TextBox>
                <CKEditor:CKEditorControl ID="txtPrijedlogSadrzaja" runat="server"></CKEditor:CKEditorControl>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td align="right">
            <asp:Button ID="btnPrijedlog" runat="server" onclick="btnPrijedlog_Click" 
                Text="Predloži izmjene" />
        </td>
        <td>
            &nbsp;</td>
    </tr>
</table>
</asp:Content>
