<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="KMWeb.Account.UserDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <br />
    <br />
                <asp:GridView ID="gvPreferiraneKategorije" runat="server" 
                    DataKeyNames = "IdKategorijaClanaka" AutoGenerateColumns="False" >
                     <Columns>
                        <asp:BoundField DataField="IdKategorijaClanaka" HeaderText="IdKategorijaClanaka" Visible="true"/>
                        <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="Preferira" runat="server"  Checked='<%# Bind("Preferira") %>' />
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NazivKategorije" HeaderText="NazivKategorije" />    
                      </Columns>
                </asp:GridView>

                <br />
    <asp:Button ID="Button1" runat="server" Text="Spremi podatke" 
        onclick="Button1_Click" />

                <br />
</asp:Content>
