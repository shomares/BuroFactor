<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="aspnetViewver.aspx.cs" Inherits="BuroFactor.AspNetForms.aspnetViewver" %>

<![CDATA[<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>]]>
    <div>
        <form id="form1" runat="server">
            <CR:CrystalReportViewer ID="Visor"  runat="server" AutoDataBind="False" OnInit="Visor_Init" Navigate="Visor_Navigate" HasCrystalLogo="False"
			 GroupTreeStyle-BorderColor="Transparent" GroupTreeStyle-BorderStyle="Ridge" GroupTreeStyle-BorderWidth="2px" HasToggleGroupTreeButton="False"
			 BorderColor="Transparent" />
        </form>
        <script src="../Scripts/Controller/VisorController.js"></script>
    </div>
