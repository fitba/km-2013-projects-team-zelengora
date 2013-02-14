<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="KMWeb._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <table style="width:100%;">
        <tr>
            <td>
                &nbsp;</td>
            <td align="center">
                <asp:Label ID="Label2" runat="server" 
                    Text="KNOWLEDGE MANAGEMENT GROUP ZELENGORA"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="center">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="center">
      <asp:TextBox ID="txtSearchBox" runat="server" Height="31px" Width="467px"></asp:TextBox>
            </td>
            <td>
      <asp:Button ID="btnSearchBox" runat="server" Text="Search" onclick="btnSearchBox_Click" />
            </td>
        </tr>
    </table>
    <br />
    <br />
<br />
    <br />
    <table style="width:100%;">
        <tr>
            <td>
                <asp:HyperLink ID="HyperLinkKM" runat="server">KNOWLEDGE MANAGEMENT</asp:HyperLink>
            </td>
            <td>
                <asp:HyperLink ID="HyperLinkWEBP" runat="server">WEB TEHNOLOGIJE&amp;PROGRAMIRANJE</asp:HyperLink>
            </td>
            <td>
                <asp:HyperLink ID="HyperLinkStatistika" runat="server">STATISTIKA</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="HyperLinkNoCat" runat="server">NEKATEGORIZOVANO</asp:HyperLink>
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
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <br />
<br />
    <br />


    
</asp:Content>
