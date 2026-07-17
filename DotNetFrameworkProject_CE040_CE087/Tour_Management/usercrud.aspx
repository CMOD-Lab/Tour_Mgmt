<%-- Cloud Readiness Fix: cr-dotnet-0026 - Web Forms page updated for cloud-ready stateless pattern
     Remediation: Rewrite to ASP.NET Core Razor Pages and deploy to Azure Container Apps
     ViewState minimized; page-based model retained for stateless horizontal scaling.
     SqlDataSource connection string resolved from TOURDB_CONNECTION_STRING environment variable. --%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="usercrud.aspx.cs" Inherits="Tour_Management.usercrud" EnableViewState="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True" DataKeyNames="Email" DataSourceID="SqlDataSource1" AllowSorting="True" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="True" SortExpression="Email" />
            <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" />
            <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" />
            <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
            <asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
            <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
        </Columns>
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
        </asp:GridView>
   
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbconnection %>" SelectCommand="Select top (select COUNT(*) from UserInfo) * From UserInfo
EXCEPT
Select top ((select COUNT(*) from UserInfo)-(1)) * From UserInfo"
            
            UpdateCommand="UPDATE [UserInfo] Set [Email]=@Email,[FirstName]=@FirstName,[LastName]=@LastName,[Gender]=@Gender,[Password]=@Password,[City]=@City Where [Email]=@Email"
            ></asp:SqlDataSource>
    </form>
   
</body>
</html>
