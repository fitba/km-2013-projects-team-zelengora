<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CompareRevision.aspx.cs" Inherits="KMWeb.Administration.CompareRevision" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table style="width: 100%;">
    <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Originalni naslov"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtNaslov" runat="server" Enabled="False" Width="557px"></asp:TextBox>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="Prijedlog izmjene naslova"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPrijedlogNaslova" runat="server" Width="556px" 
                Enabled="False"></asp:TextBox>
            <asp:Button ID="btnPrihvatiNaslov" runat="server" Text="Prihvati naslov" 
                onclick="btnPrihvatiNaslov_Click" Visible="False" />
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
            <asp:Label ID="Label4" runat="server" Text="Originalni sadržaj" Visible="true"></asp:Label>
            
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
                Height="313px" TextMode="MultiLine" Width="481px" Visible="False"></asp:TextBox>
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
            <asp:TextBox ID="txtPrijedlogSadrzaja" runat="server" Height="313px" 
                TextMode="MultiLine" Width="481px" Enabled="false" Visible="False"></asp:TextBox>
              <asp:Repeater ID="Repeater2" runat="server" OnItemCommand="RepeaterPrijedlog_ItemCommand" >
              <ItemTemplate>
                  <div id="naslov" style="float:left; margin-right:20px; background-color:Gray; font-size:large; color:White; width:95% " >
                    <%# Eval("Naslov")%>
                  </div>
                  <div id="sadrzaj" style="float:left; margin-right:20px" >
                    <%# Eval("Sadrzaj")%>
                 </div>
                 <br /><br /><br /><br /><br />
                 <div>
                 <br /><br /><br /><br />
                    <asp:LinkButton ID="btnPrihvatiSve1" runat="server" Text="Prihvati sve" CommandName="btnPrihvatiSve1" CommandArgument='<%# Eval("Naslov") + ";" +Eval("Sadrzaj") %>' ></asp:LinkButton>
                    <asp:LinkButton ID="btnPrihvatiSadrzaj" runat="server" Text="Prihvati Sadrzaj" CommandName="btnPrihvatiSadrzaj" CommandArgument='<%# Eval("Naslov") + ";" +Eval("Sadrzaj") %>' ></asp:LinkButton>
                    <asp:LinkButton ID="btnPrihvatiNaslov" runat="server" Text="Prihvati Naslov" CommandName="btnPrihvatiNaslov" CommandArgument='<%# Eval("Naslov") + ";" +Eval("Sadrzaj") %>' ></asp:LinkButton>
                    <asp:LinkButton ID="btnOdbijsve" runat="server" Text="Odbij sve" CommandName="btnOdbijsve" CommandArgument='<%# Eval("Naslov") + ";" +Eval("Sadrzaj") %>' ></asp:LinkButton>
                 
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
            &nbsp;</td>
        <td align="right">
            <asp:Button ID="btnPrihvatiSadrzaj" runat="server" 
                Text="Prihvati Sadrzaj" onclick="btnPrihvatiSadrzaj_Click" 
                Visible="False" />
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td align="right">
            <asp:Button ID="btnPrihvatiSve" runat="server" Text="Prihvati sve" 
                Width="200px" onclick="btnPrihvatiSve_Click" 
                CommandArgument='<%# Eval("Naslov") + "kplj" +Eval("Sadrzaj") %>' 
                Visible="False" />
            <asp:Button ID="btnOdbijSve" runat="server" onclick="btnOdbijSve_Click" 
                Text="Odbij Sve" Visible="False" />
        </td>
        <td>
            &nbsp;</td>
    </tr>
</table>
</asp:Content>
