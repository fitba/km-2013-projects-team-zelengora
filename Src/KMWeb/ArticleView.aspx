<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArticleView.aspx.cs" Inherits="KMWeb.ArticleView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
  .SelectedRowStyle
{
    background-color: Yellow;
}
.EditRowStyle
{
    background-color: Yellow;
}
      .style1
      {
          width: 475px;
      }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 102%;">
        <tr>
            <td class="style1">
    <asp:TextBox ID="txtNaslov" runat="server" Width="450px" Enabled="False" 
                    ontextchanged="txtNaslov_TextChanged"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
<asp:TextBox ID="txtSadrzaj" runat="server" Enabled="False" Height="353px" 
    TextMode="MultiLine" Width="450px" style="margin-right: 0px"></asp:TextBox>
    	
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Ocjenjeno: "></asp:Label>
                <asp:Label ID="lblBrojOcjena" runat="server" Text="-"></asp:Label>
&nbsp;
                <asp:Label ID="Label5" runat="server" Text=" puta"></asp:Label>
                <br />
                <asp:Label ID="Label3" runat="server" 
                    Text="Prosjecna ocjena: "></asp:Label>
                <asp:Label ID="lblOcjena" runat="server" Text="Još nije ocjenjen"></asp:Label>
                <br />
                <br />
                <asp:DropDownList ID="DropDownListVoteArticle" runat="server" 
                    Width="64px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem Selected="True">5</asp:ListItem>
                </asp:DropDownList>
               
                <asp:Button ID="btnVoteArticle" runat="server" onclick="Button1_Click" 
                    Text="Ocjeni Clanak" Width="95px" />
                <br />
                <br />
                <br />
                <br />
                <asp:Label ID="Label7" runat="server" Text="Broj pitanja: "></asp:Label>
                <asp:Label ID="lblBrojPitanja" runat="server" Text="-"></asp:Label>
                <br />
                <br />
                <br />
                <asp:LinkButton ID="btnEditArticle" runat="server" 
                    onclick="btnEditArticle_Click">Uredi članak</asp:LinkButton>
                <br />
                <br />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                <asp:Label ID="Label6" runat="server" Text="Članak kreirao:"></asp:Label>
&nbsp;<asp:Label ID="lblAutor" runat="server" Text="Label"></asp:Label>
                <asp:Label ID="Label2" runat="server" Text="Datum kreiranja:"></asp:Label>
<asp:TextBox ID="txtDate" runat="server" Enabled="False" Width="105px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                <asp:Label ID="Label9" runat="server" Text="Kljucne rijeci: "></asp:Label>
                <asp:Label ID="lblKljucneRijeci" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    	
    <br />
    <table style="width:100%;">
        <tr>
            <td>
    <asp:GridView ID="gvPitanjaOdgovori" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="Id" AutoGenerateSelectButton="True" 
        onselectedindexchanged="gvPitanjaOdgovori_SelectedIndexChanged"        >
        <EditRowStyle CssClass="EditRowStyle" />
        <EditRowStyle CssClass="selectedRowStyle" />
     <Columns>
       <asp:BoundField DataField="Id" HeaderText="Id" Visible="true"/>
       <asp:BoundField DataField="Question" HeaderText="Question" />
       <asp:BoundField DataField="IdType" HeaderText="TypeId"  Visible="true" />
       <asp:BoundField DataField="Type" HeaderText="Type" />
       <asp:BoundField DataField="Date" HeaderText="Datum" />
       
     </Columns>
    </asp:GridView>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListVote" runat="server" Enabled="False" 
                    Width="64px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem Selected="True">5</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Button ID="btnVote" runat="server" onclick="btnVote_Click" 
                    Text="Ocjeni Pit ili Odg" Enabled="False" Width="115px" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
    <asp:TextBox ID="txtSelectedIndex" runat="server" Width="173px" Visible="False"></asp:TextBox>
    <asp:TextBox ID="txtSelectedType" runat="server" Width="173px" Visible="False"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtTempOdgovor" runat="server" Visible="False"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" 
                    Text="Ovdje unijeti vaše pitanje (ili odgovor):"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtPitanje" runat="server" Height="100px" TextMode="MultiLine" 
                    Width="448px"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnPitanje" runat="server" onclick="btnPitanje_Click" 
                    Text="Unesi pitanje" Width="104px" />
                <br />
                <br />
                <asp:ImageButton ID="btnUndo" runat="server" Enabled="False" 
                    ImageUrl="~/images.jpg" onclick="ImageButton1_Click" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    	
<div ID="PitanjaIOdgovori" runat="server" class="PitanjaIOdgovori">
    </div>
<asp:Panel id="MyPanel" runat="server" Visible="false" />
</asp:Content>
