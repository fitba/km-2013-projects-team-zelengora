<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QAQuestionShow.aspx.cs" Inherits="KMWeb.QA.QAQuestionShow" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
   <ItemTemplate>
 <div id="all"  >

   <div id="naslov" style="float:left; margin-right:20px; background-color:Gray; font-size:x-large; color:White; width:100% ">   
       <%# Eval("NaslovPitanja")%>
   </div> 
   
    <div id="pitanje" style="float:left; margin-right:20px; background-color:InactiveBorder; font-size:small; color:Black; width:100% ">
       <%# Eval("Pitanje")%>
    </div>
    <br />
       
   <div id="Ostalo">
       <div id="datum" style="float:left; margin-right:20px" >
       Postavljeno dana:
         <%# Eval("Datum")%>
         </div>

         <div id="KorisnickoIme" style="float:left;  margin-right:20px">
         Korisničko ime:
         <%# Eval("KorisnickoIme")%>
         </div>

         <div id="Pregleda" style="float:left;  margin-right:20px">
         Broj pregleda:
          <%# Eval("Pregleda")%>
         </div>      
         <br />

         <div id="Div1" style="float:left;  margin-right:20px">
         Ukupna Ocjena:
          <%# Eval("UkupnaOcjena")%>
         </div>      
         <asp:LinkButton ID="UP" runat="server" Text="UP" CommandName="Up" CommandArgument='<%# Eval("ID") + ";" +Eval("UkupnaOcjena") %>'  ></asp:LinkButton>
         <asp:LinkButton ID="Down" runat="server" Text="DOWN" CommandName="Down" CommandArgument='<%# Eval("ID") + ";" +Eval("UkupnaOcjena") %>'  ></asp:LinkButton>
         <br />
        
          
         <div id="Div2" style="float:left;  margin-right:20px">
         Broj odgovora:
          <%# Eval("BrojOdgovora")%>
         </div>      
         <br />
         
    </div>
          <asp:LinkButton ID="LinkButton1" runat="server" Text="Uredi pitanje" CommandName="Review" CommandArgument='<%# Eval("ID") + ";" +Eval("UkupnaOcjena") %>'  ></asp:LinkButton>
</div>

    <br /><br />

   </ItemTemplate>
</asp:Repeater>

          Ključne rijeći:     
        <asp:Repeater ID="Rep" runat="server">
               <ItemTemplate>           
                   <a href="#"><%# Eval("Key")%></a>
              </ItemTemplate>
           </asp:Repeater>
           <br />

    <asp:Label ID="Label1" runat="server" Text="" style="float:left; margin-right:20px; margin-top:40px; background-color:Orange; font-size:x-large; color:White; width:100% ">ODGOVORI:</asp:Label>

    <br />
    <br />

<asp:Repeater ID="RepeaterAnswers" runat="server" OnItemCommand="RepeaterAnswers_ItemCommand">
   <ItemTemplate>
       <div id="all" style="float:left; margin-top:30px; background-color:InactiveBorder;  font-size:medium; color:Black; width:100%"  >
       <h1>Odgovor</h1>
       <%# Eval("Id")%>
       <div id="odgovor" >
           <%# Eval("Odgovor")%>
      </div>
    <br />
       
   <div id="Ostalo"  style="float:left; margin-right:0px; margin-top:1px; background-color:; font-size:medium; color:Black; width:100% " >
       <div id="datum" style="float:left; margin-right:20px" >
       Postavljeno dana:
         <%# Eval("Datum")%>
         </div>

         <div id="KorisnickoIme" style="float:left;  margin-right:20px">
         Korisničko ime:
         <%# Eval("KorisnickoIme")%>
         </div>      

         <div id="Div3" style="float:left;  margin-right:20px">
         Ukupna ocjena:
           <%# Eval("UkupnaOcjena")%>
         </div>  

    </div>
         <asp:LinkButton ID="UPAnswer" runat="server" Text="UP" CommandName="UPAnswer" CommandArgument='<%# Eval("ID") + ";" +Eval("UkupnaOcjena") %>' ></asp:LinkButton>
         <asp:LinkButton ID="DownAnswer" runat="server" Text="DOWN" CommandName="DownAnswer"  CommandArgument='<%# Eval("ID") + ";" +Eval("UkupnaOcjena")  %>'></asp:LinkButton>
       </div>
<br />

    <br /><br />

   </ItemTemplate>
</asp:Repeater>

    <br />
    <br />
    <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="Medium" 
        Text="Dodaj svoj odgovor:"></asp:Label>
    <br />
    <br />
    <asp:TextBox ID="txtUserAnswer" runat="server" Height="139px" 
        TextMode="MultiLine" Width="375px" Visible="false"></asp:TextBox>
    <br />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Pošalji odgovor" />

    <br />
    <br />
    <CKEditor:CKEditorControl ID="CKEditor1" runat="server"></CKEditor:CKEditorControl>
</asp:Content>
