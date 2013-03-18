<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewQuestionRevision.aspx.cs" Inherits="KMWeb.QA.NewQuestionRevision" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%;">
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="ORIGINAL PITANJE"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
 <asp:Repeater ID="Repeater1" runat="server" >
   <ItemTemplate>
        <div id="naslov" style="float:left; margin-right:20px; background-color:Gray; font-size:large; color:White; width:95% " >
         <%# Eval("NaslovPitanja")%>
         </div>
         <div id="sadrzaj" style="float:left; margin-right:20px" >
         <%# Eval("Pitanje")%>
         </div>
   </ItemTemplate>
   </asp:Repeater>
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
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="PRIJEDLOG IZMJENE"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" Text="Naslov"></asp:Label>
                <asp:TextBox ID="txtPrijedlogNaslova" runat="server" Width="534px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <CKEditor:CKEditorControl ID="txtPrijedlogPitanja" runat="server"></CKEditor:CKEditorControl></td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Predloži" 
                    onclick="Button1_Click" />
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
