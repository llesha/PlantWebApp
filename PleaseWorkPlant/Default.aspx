<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PleaseWorkPlant._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Button ID="CreateNewPlant" runat="server" OnClick="CreateNewPlant_Click" Text="Create plant" />
    <asp:Button ID ="DownloadPlant" runat="server" OnClick ="DownloadPlant_Click" Text ="Download Plant.svg" />
    <asp:ImageMap ID="ImageContainer" runat="server"/>

</asp:Content>
