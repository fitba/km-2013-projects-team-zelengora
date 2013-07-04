<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchResults.aspx.cs" Inherits="KMWeb.SearchResults" %>
<%@ Import Namespace="System.Text.RegularExpressions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
    .style1
    {
        width: 604px;
    }
        .style4
        {
            width: 326px;
        }
    </style>

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
               <div id="all">     
               <div id="Ostalo" style="float:left; text-decoration:none; margin-right:20px; background-color:; font-size:small; margin-bottom:30px; color:Black; width:100% ">
        
                   <a href="#"  ><%# Eval("articleTitle")%></a>
               </div>
                   <br />
                    
                    <%#Regex.Replace(DataBinder.Eval(Container.DataItem, "articleText").ToString(), "<(.|\n)*?>", String.Empty).ToString()%>
                    <br />
                    </div>
              </ItemTemplate>
           </asp:Repeater>

</asp:Content>
