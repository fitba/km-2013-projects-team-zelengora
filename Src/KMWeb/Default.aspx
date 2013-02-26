<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="KMWeb._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 553px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <table style="width:100%;">
        <tr>
            <td>
                &nbsp;</td>
            <td align="center" class="style1">
                <asp:Label ID="Label2" runat="server" 
                    Text="KNOWLEDGE MANAGEMENT GROUP ZELENGORA"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="center" class="style1">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="right" class="style1">
      <asp:TextBox ID="txtSearchBox" runat="server" Height="31px" Width="467px" ></asp:TextBox>
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
                <asp:HyperLink ID="HyperLinkKM" runat="server" Visible="false">KNOWLEDGE MANAGEMENT</asp:HyperLink>
            </td>
            <td>
                <asp:HyperLink ID="HyperLinkWEBP" runat="server" Visible="false">WEB TEHNOLOGIJE&amp;PROGRAMIRANJE</asp:HyperLink>
            </td>
            <td>
                <asp:HyperLink ID="HyperLinkStatistika" runat="server" Visible="false">STATISTIKA</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="HyperLinkNoCat" runat="server" Visible="false">NEKATEGORIZOVANO</asp:HyperLink>
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
                <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Button" 
                    Visible="False" />
            </td>
            <td>
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                    Text="Synchro db-&gt;solr" />
                <asp:Button ID="Button3" runat="server" onclick="Button3_Click" 
                    Text="Delete solr index" />
            </td>
        </tr>
    </table>
    <br />
<br />
    <br />


    
</asp:Content>
