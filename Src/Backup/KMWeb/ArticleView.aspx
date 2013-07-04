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
    }
    .style4
    {
        width: 219px;
    }
    .style5
    {
        width: 220px;
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%; background-color:transparent;">
        <tr>
            <td class="style1" colspan="8">
       
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="8">


    <asp:Repeater ID="Repeater1" runat="server" >
   <ItemTemplate>
    <div id="naslov" style="float:left; margin-right:20px; background-color:Gray; font-size:large; color:White; width:100% " >
         <%# Eval("Naslov")%>
         </div>
         <div id="sadrzaj" style="float:left; margin-right:20px; background-color:InactiveBorder; width:100%" >
         <%# Eval("Sadrzaj")%>
         </div>
   </ItemTemplate>
   </asp:Repeater>
                <br />
    	
            </td>
        </tr>
        <tr>
            <td class="style1" bgcolor="#CCFFFF">
                <asp:Label ID="Label6" runat="server" Text="Članak kreirao:"></asp:Label>
&nbsp;<asp:Label ID="lblAutor" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="style4" colspan="2" bgcolor="#CCFFFF" valign="top">
                <asp:Label ID="Label2" runat="server" Text="Datum kreiranja:"></asp:Label>
<asp:TextBox ID="txtDate" runat="server" Enabled="False" Width="120px"></asp:TextBox>
            </td>
            <td class="style5" align="right" colspan="4">
                &nbsp;
                <asp:ImageButton runat ="server" ID="btnLikeButton"  ImageUrl="~/img/like.jpg" 
                    Height="28px" onclick="btnLikeButton_Click" Width="24px" />
                <asp:ImageButton runat ="server" ID="btnDisLikeButton" 
                    ImageUrl="~/img/dislike.jpg" Height="25px" onclick="btnDisLikeButton_Click" 
                    Width="25px"/>
                <asp:Label runat="server" ID="lblLikeStatus" Text=""></asp:Label>
                </td>
            <td class="style1">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1" colspan="3">
                <asp:Label ID="Label9" runat="server" Text="Kljucne rijeci: "></asp:Label>
                <asp:Label ID="lblKljucneRijeci" runat="server"></asp:Label>
            </td>
            <td class="style1" colspan="2">
                &nbsp;</td>
            <td class="style1" colspan="3" align="right">
    	
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
    	
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="4">
                &nbsp;</td>
            <td class="style1" align="right">
                <asp:Label ID="Label4" runat="server" Text="Ocjenjeno: "></asp:Label>
                <asp:Label ID="lblBrojOcjena" runat="server" Text="-"></asp:Label>
                <asp:Label ID="Label5" runat="server" Text=" puta"></asp:Label>
            </td>
            <td class="style1" colspan="3" align="right">
                <asp:Label ID="Label3" runat="server" 
                    Text="Prosjecna ocjena: "></asp:Label>
                <asp:Label ID="lblOcjena" runat="server" Text="Nema ocjena"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="2" rowspan="0">
                <asp:Label ID="Label7" runat="server" Text="Broj pitanja: "></asp:Label>
                <asp:Label ID="lblBrojPitanja" runat="server" Text="-"></asp:Label>
            </td>
            <td class="style1" colspan="4">
                &nbsp;</td>
            <td class="style1" colspan="2" align="right">
                <asp:LinkButton ID="btnEditArticle" runat="server" 
                    onclick="btnEditArticle_Click">Uredi članak</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="8" bgcolor="#FFFFCC">
                &nbsp;</td>
        </tr>
    </table>
    	
    <br />
    <br />
    <br />
    	
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
       <asp:BoundField DataField="Username" HeaderText="KorisnickoIme" />
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
                <br />
                <asp:Label ID="Label10" runat="server" Text="Ocjenjeno: "></asp:Label>
                <asp:Label ID="lblBrojOcjenaPitanjaOdgovora" runat="server" Text="- "></asp:Label>
                <asp:Label ID="Label11" runat="server" Text="Puta"></asp:Label>
                <br />
                <asp:Label ID="Label12" runat="server" Text="Prosjecna ocjena: "></asp:Label>
                <asp:Label ID="lblProsjecnaOcjena" runat="server" Text="-"></asp:Label>
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
