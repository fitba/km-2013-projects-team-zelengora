<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchResults.aspx.cs" Inherits="KMWeb.SearchResults" %>
<%@ Import Namespace="System.Text.RegularExpressions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="Label2" runat="server" Text="Rezultati pretrage za : "></asp:Label>
    <asp:Label ID="txtSearchWord" runat="server" Text="-"></asp:Label>
    <br />
    <asp:Label ID="lblNotFound" runat="server" 
        Text="Nema rezultata za vašu pretragu" Visible="False"></asp:Label>
    <br />
    <br />
    <asp:Panel id="MyPanel" runat="server" Visible="true" />
    <asp:Repeater ID="Rep" runat="server">
               <ItemTemplate>           
                   <a href="#"><%# Eval("articleTitle")%></a>
                   <br />
                    
                    <%#Regex.Replace(DataBinder.Eval(Container.DataItem, "articleText").ToString(), "<(.|\n)*?>", String.Empty).ToString()%>
                    <br />
              </ItemTemplate>
           </asp:Repeater>

</asp:Content>
