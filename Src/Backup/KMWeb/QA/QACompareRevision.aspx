<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QACompareRevision.aspx.cs" Inherits="KMWeb.QA.QACompareRevision" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 57px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

           <br/> 
    <table style="width:100%;">
        <tr>
            <td class="style1">
                <asp:Label ID="Label6" runat="server" Text="ORIGINAL PITANJE"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
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
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                <asp:Label ID="Label7" runat="server" Text="PRIJEDLOG IZMJENE"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td>

           <asp:Repeater ID="Repeater2" runat="server" onitemcommand="Repeater2_ItemCommand"  >
              <ItemTemplate>
                  <div id="naslov" style="float:left; margin-right:20px; background-color:Gray; font-size:large; color:White; width:95% " >
                    <%# Eval("NaslovPitanja")%>
                  </div>
                  <div id="sadrzaj" style="float:left; margin-right:20px" >
                    <%# Eval("Pitanje")%>
                 </div>
                 <br /><br /><br /><br /><br />
                 <div>
                 <br /><br /><br /><br />
                    <asp:LinkButton ID="btnPrihvatiSve1" runat="server" Text="Prihvati sve" CommandName="btnPrihvatiSve1" CommandArgument='<%# Eval("NaslovPitanja") + ";" +Eval("Pitanje") %>' ></asp:LinkButton>
                    <asp:LinkButton ID="btnPrihvatiSadrzaj" runat="server" Text="Prihvati Sadrzaj" CommandName="btnPrihvatiSadrzaj" CommandArgument='<%# Eval("NaslovPitanja") + ";" +Eval("Pitanje") %>' ></asp:LinkButton>
                    <asp:LinkButton ID="btnPrihvatiNaslov" runat="server" Text="Prihvati Naslov" CommandName="btnPrihvatiNaslov" CommandArgument='<%# Eval("NaslovPitanja") + ";" +Eval("Pitanje") %>' ></asp:LinkButton>
                    <asp:LinkButton ID="btnOdbijsve" runat="server" Text="Odbij sve" CommandName="btnOdbijsve" CommandArgument='<%# Eval("NaslovPitanja") + ";" +Eval("Pitanje") %>' ></asp:LinkButton>
                 
                    </div>
              </ItemTemplate>
           </asp:Repeater>
            </td>
        </tr>
    </table>
    <br/> <br/>

           </asp:Content>
