<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuestionList.aspx.cs" Inherits="KMWeb.QA.QuestionList" %>
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
    <asp:GridView ID="gvQAPitanja" runat="server" AutoGenerateSelectButton="True" 
    onselectedindexchanged="gvQAPitanja_SelectedIndexChanged" DataKeyNames = "IdPitanje" AutoGenerateColumns="False" Visible="False">
    
                     <Columns>
                        <asp:BoundField DataField="IdPitanje" HeaderText="Id" Visible="true"/>
                        <asp:BoundField DataField="NaslovPitanja" HeaderText="NaslovPitanja" Visible="true"/>
                        <asp:BoundField DataField="Pitanje" HeaderText="Pitanje" />   
                        <asp:BoundField DataField="KorisnickoIme" HeaderText="KorisnickoIme" />   
                        <asp:BoundField DataField="Datum" HeaderText="Datum" />   
                        <asp:BoundField DataField="Pregleda" HeaderText="Pregleda" />  

                      </Columns>
</asp:GridView>
    <table style="width:100%;">
        <tr>
            <td>
<asp:Repeater ID="Repeater1" runat="server" onitemdatabound="Repeater1_ItemDataBound1">
   <ItemTemplate>
   
 <div id="all"  >

   <div id="naslov" style="float:left; text-decoration:none; margin-right:20px; background-color:InactiveBorder; font-size:large; color:White; width:100% " >   
       <a href="QAQuestionShow.aspx?IdQuestion=<%# Eval("Id")%>"><%# Eval("NaslovPitanja")%></a>
   </div> 
   <br />
    <div id="pitanje">
      
    </div>
    <br />
       
   <div id="Ostalo" style="float:left; text-decoration:none; margin-right:20px; background-color:; font-size:small; margin-bottom:30px; color:Black; width:100% ">
       <div id="datum" style="float:left; margin-right:20px" >
       Postavljeno dana:
         <%# Eval("Datum")%>
         </div>

         <div id="KorisnickoIme" style="float:left;  margin-right:20px">
         Korisničko ime:
         <%# Eval("KorisnickoIme")%> 
         
         </div>
  
         <br />

         Ključne rijeći:     
        <asp:Repeater ID="Rep" runat="server">
               <ItemTemplate>           
                   <a href="#"><%# Eval("Key")%></a>
              </ItemTemplate>
           </asp:Repeater>
          
          <br />
           <div id="Div1" style="float:left;  margin-right:20px">
           Broj Odgovora:
            <%# Eval("BrojOdgovora")%>
          
         </div>  
         
         <div id="Pregleda" style="float:left;  margin-right:20px">
             Broj pregleda:
          <%# Eval("Pregleda")%>
         </div>

         <div id="Div2" style="float:left;  margin-right:20px">
             Ocjena:
          <%# Eval("UkupnaOcjena")%>
         </div>
         <br />


    </div>

</div>

    <br /><br />
    
   </ItemTemplate>
</asp:Repeater>
            </td>
            <td valign="top" align="right">
    <asp:LinkButton ID="LinkButton4" runat="server" onclick="LinkButton4_Click">Postavi pitanje</asp:LinkButton>
            </td>
        </tr>
</table>
</asp:Content>
